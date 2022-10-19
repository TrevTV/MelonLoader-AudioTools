using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_INFO
	{
		public BASSInfo flags;

		public int hwsize;

		public int hwfree;

		public int freesam;

		public int free3d;

		public int minrate;

		public int maxrate;

		public bool eax;

		public int minbuf = 500;

		public int dsver;

		public int latency;

		public BASSInit initflags;

		public int speakers;

		public int freq;

		public bool SupportsContinuousRate => (flags & BASSInfo.DSCAPS_CONTINUOUSRATE) != 0;

		public bool SupportsDirectSound => (flags & BASSInfo.DSCAPS_EMULDRIVER) == 0;

		public bool IsCertified => (flags & BASSInfo.DSCAPS_CERTIFIED) != 0;

		public bool SupportsMonoSamples => (flags & BASSInfo.DSCAPS_SECONDARYMONO) != 0;

		public bool SupportsStereoSamples => (flags & BASSInfo.DSCAPS_SECONDARYSTEREO) != 0;

		public bool Supports8BitSamples => (flags & BASSInfo.DSCAPS_SECONDARY8BIT) != 0;

		public bool Supports16BitSamples => (flags & BASSInfo.DSCAPS_SECONDARY16BIT) != 0;

		public override string ToString()
		{
			return $"Speakers={speakers}, MinRate={minrate}, MaxRate={maxrate}, DX={dsver}, EAX={eax}";
		}
	}
}
