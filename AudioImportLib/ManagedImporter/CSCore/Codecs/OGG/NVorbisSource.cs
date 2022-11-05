using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CSCore;
using CSCore.Codecs;
using NVorbis;

namespace CSCore.Codecs.OGG
{
    public sealed class NVorbisSource : ISampleSource
    {
        public int LowerBitrate => _vorbisReader?.LowerBitrate ?? 0;
        public int NominalBitrate => _vorbisReader?.NominalBitrate ?? 0;
        public int UpperBitrate => _vorbisReader?.UpperBitrate ?? 0;
        //public string[] Comments => _vorbisReader?.Tags?.All?.SelectMany(k => k.Value, (kvp, Item) => $"{kvp.Key}={Item}").ToArray();
        public string[] Comments => _vorbisReader?.Comments;

        private readonly Stream _stream;
        private readonly VorbisReader _vorbisReader;

        private readonly WaveFormat _waveFormat;
        private bool _disposed;

        public NVorbisSource(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (!stream.CanRead)
                throw new ArgumentException("Stream is not readable.", "stream");
            _stream = stream;
            _vorbisReader = new VorbisReader(stream, false);
            _waveFormat = new WaveFormat(_vorbisReader.SampleRate, 32, _vorbisReader.Channels, AudioEncoding.IeeeFloat);
        }

        public bool CanSeek
        {
            get { return _stream.CanSeek; }
        }

        public WaveFormat WaveFormat
        {
            get { return _waveFormat; }
        }

        //got fixed through workitem #17, thanks for reporting @rgodart.
        public long Length
        {
            get { return CanSeek ? _vorbisReader.TotalSamples * _vorbisReader.Channels : 0; }
        }

        //got fixed through workitem #17, thanks for reporting @rgodart.
        public long Position
        {
            get
            {
                return CanSeek ? _vorbisReader.DecodedPosition * _vorbisReader.Channels : 0;
            }
            set
            {
                if (!CanSeek)
                    throw new InvalidOperationException("NVorbisSource is not seekable.");
                if (value < 0 || value > Length)
                    throw new ArgumentOutOfRangeException("value");

                _vorbisReader.DecodedPosition = value / _vorbisReader.Channels;
            }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            return _vorbisReader.ReadSamples(buffer, offset, count);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _vorbisReader.Dispose();
                _disposed = true;
            }
        }
    }
}
