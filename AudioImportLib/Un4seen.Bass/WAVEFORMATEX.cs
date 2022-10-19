using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public sealed class WAVEFORMATEX
	{
		public WAVEFormatTag wFormatTag = WAVEFormatTag.PCM;

		public short nChannels = 2;

		public int nSamplesPerSec = 44100;

		public int nAvgBytesPerSec = 176400;

		public short nBlockAlign = 4;

		public short wBitsPerSample = 16;

		public short cbSize;

		public WAVEFORMATEX()
		{
		}

		public WAVEFORMATEX(WAVEFormatTag format, short channels, int samplesPerSec, short bitsPerSample, short exSize)
		{
			wFormatTag = format;
			nChannels = channels;
			nSamplesPerSec = samplesPerSec;
			wBitsPerSample = bitsPerSample;
			nBlockAlign = (short)(nChannels * (wBitsPerSample / 8));
			nAvgBytesPerSec = nSamplesPerSec * nBlockAlign;
			cbSize = exSize;
		}

		public override string ToString()
		{
			string text = "Stereo";
			if (nChannels == 1)
			{
				text = "Mono";
			}
			else if (nChannels == 3)
			{
				text = "2.1";
			}
			else if (nChannels == 4)
			{
				text = "Quad";
			}
			else if (nChannels == 5)
			{
				text = "4.1";
			}
			else if (nChannels == 6)
			{
				text = "5.1";
			}
			else if (nChannels == 7)
			{
				text = "6.1";
			}
			else if (nChannels > 7)
			{
				text = nChannels + "chans";
			}
			string text2 = "16-bit";
			if (wBitsPerSample == 0)
			{
				text2 = $"{nAvgBytesPerSec * 8 / 1000} kbps";
			}
			else
			{
				text2 = wBitsPerSample + "-bit";
				if (nAvgBytesPerSec > 0)
				{
					text2 += $", {nAvgBytesPerSec * 8 / 1000} kbps";
				}
			}
			return string.Format(CultureInfo.InvariantCulture, "{0} {1}, {2:##0.0#}kHz {3}", wFormatTag, text2, (double)nSamplesPerSec / 1000.0, text);
		}
	}
}
