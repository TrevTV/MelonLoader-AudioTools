using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_DX8_REVERB
	{
		public float fInGain;

		public float fReverbMix;

		public float fReverbTime = 1000f;

		public float fHighFreqRTRatio = 0.001f;

		public BASS_DX8_REVERB()
		{
		}

		public BASS_DX8_REVERB(float InGain, float ReverbMix, float ReverbTime, float HighFreqRTRatio)
		{
			fInGain = InGain;
			fReverbMix = ReverbMix;
			fReverbTime = ReverbTime;
			fHighFreqRTRatio = HighFreqRTRatio;
		}

		public void Preset_Default()
		{
			fInGain = -3f;
			fReverbMix = -6f;
			fReverbTime = 1000f;
			fHighFreqRTRatio = 0.5f;
		}
	}
}
