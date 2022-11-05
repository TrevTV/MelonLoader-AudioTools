using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CSCore;
using CSCore.Codecs;
using NLayer;

namespace CSCore.Codecs.MP3
{
    public sealed class MP3Source : ISampleSource
    {
        private readonly Stream _stream;
        private MpegFile _mp3Reader;

        private readonly WaveFormat _waveFormat;
        private bool _disposed;

        public MP3Source(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (!stream.CanRead)
                throw new ArgumentException("Stream is not readable.", "stream");
            _stream = stream;
            _mp3Reader = new MpegFile(stream);
            _waveFormat = new WaveFormat(_mp3Reader.SampleRate, 32, _mp3Reader.Channels, AudioEncoding.IeeeFloat);
        }

        public bool CanSeek => _mp3Reader.CanSeek;

        public WaveFormat WaveFormat => _waveFormat;

        public long Length => _mp3Reader.Length;

        public long Position
        {
            get => _mp3Reader.Position;

            set
            {
                if (!CanSeek)
                    throw new InvalidOperationException("Source is not seekable.");
                if (value < 0 || value > Length)
                    throw new ArgumentOutOfRangeException("value");

                _mp3Reader.Position = value;
            }
        }

        public int Read(float[] buffer, int offset, int count) => _mp3Reader.ReadSamples(buffer, offset, count);

        public void Dispose()
        {
            if (!_disposed)
            {
                _mp3Reader.Dispose();
                _disposed = true;
            }
        }
    }
}
