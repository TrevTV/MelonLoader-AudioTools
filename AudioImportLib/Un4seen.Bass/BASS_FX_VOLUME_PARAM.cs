using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_FX_VOLUME_PARAM
	{
		public float fTarget = 1f;

		public float fCurrent = 1f;

		public float fTime;

		public int lCurve;

		public BASS_FX_VOLUME_PARAM()
		{
		}

		public BASS_FX_VOLUME_PARAM(float target, float current, float time, int curve)
		{
			fTarget = target;
			fCurrent = current;
			fTime = time;
			lCurve = curve;
		}
	}
}
