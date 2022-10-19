using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_DX8_ECHO
	{
		public float fWetDryMix;

		public float fFeedback;

		public float fLeftDelay = 333f;

		public float fRightDelay = 333f;

		[MarshalAs(UnmanagedType.Bool)]
		public bool lPanDelay;

		public BASS_DX8_ECHO()
		{
		}

		public BASS_DX8_ECHO(float WetDryMix, float Feedback, float LeftDelay, float RightDelay, bool PanDelay)
		{
			fWetDryMix = WetDryMix;
			fFeedback = Feedback;
			fLeftDelay = LeftDelay;
			fRightDelay = RightDelay;
			lPanDelay = PanDelay;
		}

		public void Preset_Default()
		{
			fWetDryMix = 50f;
			fFeedback = 0f;
			fLeftDelay = 333f;
			fRightDelay = 333f;
			lPanDelay = false;
		}

		public void Preset_Small()
		{
			fWetDryMix = 50f;
			fFeedback = 20f;
			fLeftDelay = 100f;
			fRightDelay = 100f;
			lPanDelay = false;
		}

		public void Preset_Long()
		{
			fWetDryMix = 50f;
			fFeedback = 20f;
			fLeftDelay = 700f;
			fRightDelay = 700f;
			lPanDelay = false;
		}
	}
}
