using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_DX8_DISTORTION
	{
		public float fGain;

		public float fEdge = 50f;

		public float fPostEQCenterFrequency = 4000f;

		public float fPostEQBandwidth = 4000f;

		public float fPreLowpassCutoff = 4000f;

		public BASS_DX8_DISTORTION()
		{
		}

		public BASS_DX8_DISTORTION(float Gain, float Edge, float PostEQCenterFrequency, float PostEQBandwidth, float PreLowpassCutoff)
		{
			fGain = Gain;
			fEdge = Edge;
			fPostEQCenterFrequency = PostEQCenterFrequency;
			fPostEQBandwidth = PostEQBandwidth;
			fPreLowpassCutoff = PreLowpassCutoff;
		}

		public void Preset_Default()
		{
			fGain = 0f;
			fEdge = 50f;
			fPostEQCenterFrequency = 4000f;
			fPostEQBandwidth = 4000f;
			fPreLowpassCutoff = 4000f;
		}
	}
}
