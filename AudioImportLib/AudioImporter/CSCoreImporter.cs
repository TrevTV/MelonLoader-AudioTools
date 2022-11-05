using CSCore;
using CSCore.Codecs;

namespace AudioImportLib
{
    internal class CSCoreImporter : DecoderImporter
    {
        private IWaveSource reader;
        private ISampleSource sampleProvider;

        protected override void Initialize()
        {
            reader = CodecFactory.Instance.GetCodec(uri.LocalPath);
            sampleProvider = reader.ToSampleSource();
        }

        protected override void Cleanup()
        {
            if (reader != null)
                reader.Dispose();

            reader = null;
            sampleProvider = null;
        }

        protected override AudioInfo GetInfo()
        {
            int lengthSamples = (int)reader.Length / (reader.WaveFormat.BitsPerSample / 8);
            return new AudioInfo(lengthSamples, reader.WaveFormat.SampleRate, reader.WaveFormat.Channels);
        }

        protected override int GetSamples(float[] buffer, int offset, int count)
        {
            return sampleProvider.Read(buffer, offset, count);
        }
    }
}