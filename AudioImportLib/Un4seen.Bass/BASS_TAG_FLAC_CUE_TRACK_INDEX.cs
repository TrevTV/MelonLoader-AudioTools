using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class BASS_TAG_FLAC_CUE_TRACK_INDEX
	{
		private long _offset;

		private int _number;

		public long Offset => _offset;

		public long Number => _number;

		private BASS_TAG_FLAC_CUE_TRACK_INDEX()
		{
		}
	}
}
