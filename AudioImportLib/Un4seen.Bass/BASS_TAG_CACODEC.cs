using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[Serializable]
	public sealed class BASS_TAG_CACODEC
	{
		[Serializable]
		private struct TAG_CA_CODEC
		{
			public int ftype;

			public int atype;

			public IntPtr name;
		}

		public int ftype;

		public int atype;

		public string name = string.Empty;

		public BASS_TAG_CACODEC()
		{
		}

		public BASS_TAG_CACODEC(IntPtr p)
		{
			try
			{
				TAG_CA_CODEC tAG_CA_CODEC = (TAG_CA_CODEC)Marshal.PtrToStructure(p, typeof(TAG_CA_CODEC));
				ftype = tAG_CA_CODEC.ftype;
				atype = tAG_CA_CODEC.atype;
				name = Utils.IntPtrAsStringAnsi(tAG_CA_CODEC.name);
			}
			catch
			{
			}
		}

		public override string ToString()
		{
			return name;
		}
	}
}
