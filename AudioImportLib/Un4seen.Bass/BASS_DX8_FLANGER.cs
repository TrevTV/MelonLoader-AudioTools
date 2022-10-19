using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_DX8_FLANGER
	{
		public float fWetDryMix;

		public float fDepth = 25f;

		public float fFeedback;

		public float fFrequency;

		public int lWaveform = 1;

		public float fDelay;

		public BASSFXPhase lPhase = BASSFXPhase.BASS_FX_PHASE_ZERO;

		public BASS_DX8_FLANGER()
		{
		}

		public BASS_DX8_FLANGER(float WetDryMix, float Depth, float Feedback, float Frequency, int Waveform, float Delay, BASSFXPhase Phase)
		{
			fWetDryMix = WetDryMix;
			fDepth = Depth;
			fFeedback = Feedback;
			fFrequency = Frequency;
			lWaveform = Waveform;
			fDelay = Delay;
			lPhase = Phase;
		}

		public void Preset_Default()
		{
			fWetDryMix = 50f;
			fDepth = 25f;
			fFeedback = 0f;
			fFrequency = 0f;
			lWaveform = 1;
			fDelay = 0f;
			lPhase = BASSFXPhase.BASS_FX_PHASE_ZERO;
		}

		public void Preset_A()
		{
			fWetDryMix = 60f;
			fDepth = 60f;
			fFeedback = 25f;
			fFrequency = 5f;
			lWaveform = 1;
			fDelay = 1f;
			lPhase = BASSFXPhase.BASS_FX_PHASE_90;
		}

		public void Preset_B()
		{
			fWetDryMix = 75f;
			fDepth = 80f;
			fFeedback = 50f;
			fFrequency = 7f;
			lWaveform = 0;
			fDelay = 3f;
			lPhase = BASSFXPhase.BASS_FX_PHASE_NEG_90;
		}
	}
}
