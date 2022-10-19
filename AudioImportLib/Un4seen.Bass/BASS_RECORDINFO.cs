using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_RECORDINFO
	{
		public BASSRecordInfo flags;

		public int formats;

		public int inputs;

		public bool singlein;

		public int freq;

		public BASSRecordFormat WaveFormat => (BASSRecordFormat)(formats & 0xFFFFFF);

		public int Channels => formats >> 24;

		public bool SupportsDirectSound => (flags & BASSRecordInfo.DSCAPS_EMULDRIVER) == 0;

		public bool IsCertified => (flags & BASSRecordInfo.DSCAPS_CERTIFIED) != 0;

		public override string ToString()
		{
			return $"Inputs={inputs}, SingleIn={singlein}";
		}
	}
}
