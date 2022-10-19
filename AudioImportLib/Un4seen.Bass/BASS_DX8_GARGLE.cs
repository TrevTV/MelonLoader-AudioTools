using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_DX8_GARGLE
	{
		public int dwRateHz = 500;

		public int dwWaveShape = 1;

		public BASS_DX8_GARGLE()
		{
		}

		public BASS_DX8_GARGLE(int RateHz, int WaveShape)
		{
			dwRateHz = RateHz;
			dwWaveShape = WaveShape;
		}

		public void Preset_Default()
		{
			dwRateHz = 100;
			dwWaveShape = 1;
		}
	}
}
