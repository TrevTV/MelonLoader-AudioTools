using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_DX8_PARAMEQ
	{
		public float fCenter = 100f;

		public float fBandwidth = 18f;

		public float fGain;

		public BASS_DX8_PARAMEQ()
		{
		}

		public BASS_DX8_PARAMEQ(float Center, float Bandwidth, float Gain)
		{
			fCenter = Center;
			fBandwidth = Bandwidth;
			fGain = Gain;
		}

		public void Preset_Default()
		{
			fCenter = 100f;
			fBandwidth = 18f;
			fGain = 0f;
		}

		public void Preset_Low()
		{
			fCenter = 125f;
			fBandwidth = 18f;
			fGain = 0f;
		}

		public void Preset_Mid()
		{
			fCenter = 1000f;
			fBandwidth = 18f;
			fGain = 0f;
		}

		public void Preset_High()
		{
			fCenter = 8000f;
			fBandwidth = 18f;
			fGain = 0f;
		}
	}
}
