using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_DX8_COMPRESSOR
	{
		public float fGain;

		public float fAttack = 10f;

		public float fRelease = 200f;

		public float fThreshold = -20f;

		public float fRatio = 3f;

		public float fPredelay = 4f;

		public BASS_DX8_COMPRESSOR()
		{
		}

		public BASS_DX8_COMPRESSOR(float Gain, float Attack, float Release, float Threshold, float Ratio, float Predelay)
		{
			fGain = Gain;
			fAttack = Attack;
			fRelease = Release;
			fThreshold = Threshold;
			fRatio = Ratio;
			fPredelay = Predelay;
		}

		public void Preset_Default()
		{
			fGain = 0f;
			fAttack = 10f;
			fRelease = 200f;
			fThreshold = -20f;
			fRatio = 3f;
			fPredelay = 4f;
		}

		public void Preset_Soft()
		{
			fGain = 0f;
			fAttack = 12f;
			fRelease = 800f;
			fThreshold = -20f;
			fRatio = 3f;
			fPredelay = 4f;
		}

		public void Preset_Soft2()
		{
			fGain = 2f;
			fAttack = 20f;
			fRelease = 800f;
			fThreshold = -20f;
			fRatio = 4f;
			fPredelay = 4f;
		}

		public void Preset_Medium()
		{
			fGain = 4f;
			fAttack = 5f;
			fRelease = 600f;
			fThreshold = -20f;
			fRatio = 5f;
			fPredelay = 3f;
		}

		public void Preset_Hard()
		{
			fGain = 2f;
			fAttack = 2f;
			fRelease = 400f;
			fThreshold = -20f;
			fRatio = 8f;
			fPredelay = 2f;
		}

		public void Preset_Hard2()
		{
			fGain = 6f;
			fAttack = 2f;
			fRelease = 200f;
			fThreshold = -20f;
			fRatio = 10f;
			fPredelay = 2f;
		}

		public void Preset_HardCommercial()
		{
			fGain = 4f;
			fAttack = 5f;
			fRelease = 300f;
			fThreshold = -16f;
			fRatio = 9f;
			fPredelay = 2f;
		}
	}
}
