using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_SAMPLE
	{
		public int freq = 44100;

		public float volume = 1f;

		public float pan;

		public BASSFlag flags;

		public int length;

		public int max = 1;

		public int origres;

		public int chans = 2;

		public int mingap;

		public BASS3DMode mode3d;

		public float mindist;

		public float maxdist;

		public int iangle;

		public int oangle;

		public float outvol = 1f;

		public BASSVam vam = BASSVam.BASS_VAM_HARDWARE;

		public int priority;

		public int origresValue => origres & 0xFFFF;

		public bool origresIsFloat => (origres & 0x10000) == 65536;

		public BASS_SAMPLE()
		{
		}

		public BASS_SAMPLE(int Freq, float Volume, float Pan, BASSFlag Flags, int Length, int Max, int OrigRes, int Chans, int MinGap, BASS3DMode Flag3D, float MinDist, float MaxDist, int IAngle, int OAngle, float OutVol, BASSVam FlagsVam, int Priority)
		{
			freq = Freq;
			volume = Volume;
			pan = Pan;
			flags = Flags;
			length = Length;
			max = Max;
			origres = OrigRes;
			chans = Chans;
			mingap = MinGap;
			mode3d = Flag3D;
			mindist = MinDist;
			maxdist = MaxDist;
			iangle = IAngle;
			oangle = OAngle;
			outvol = OutVol;
			vam = FlagsVam;
			priority = Priority;
		}

		public override string ToString()
		{
			return $"Frequency={freq}, Volume={volume}, Pan={pan}";
		}
	}
}
