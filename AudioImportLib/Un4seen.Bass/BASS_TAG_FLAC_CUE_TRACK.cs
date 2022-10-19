using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class BASS_TAG_FLAC_CUE_TRACK
	{
		[Flags]
		public enum CUESHEETTrackType
		{
			TAG_FLAC_CUE_TRACK_AUDIO = 0x0,
			TAG_FLAC_CUE_TRACK_DATA = 0x1,
			TAG_FLAC_CUE_TRACK_PRE = 0x2
		}

		private long _offset;

		private int _number;

		private IntPtr _isrc = IntPtr.Zero;

		private int _flags;

		private int _nindexes;

		private IntPtr _indexes = IntPtr.Zero;

		public long Offset => _offset;

		public int Number => _number;

		public string ISRC
		{
			get
			{
				if (_isrc == IntPtr.Zero)
				{
					return null;
				}
				return Utils.IntPtrAsStringAnsi(_isrc);
			}
		}

		public CUESHEETTrackType Flags => (CUESHEETTrackType)_flags;

		public int NumIndexes => _nindexes;

		public unsafe BASS_TAG_FLAC_CUE_TRACK_INDEX[] Indexes
		{
			get
			{
				if (_nindexes > 0 && _indexes != IntPtr.Zero)
				{
					BASS_TAG_FLAC_CUE_TRACK_INDEX[] array = new BASS_TAG_FLAC_CUE_TRACK_INDEX[_nindexes];
					IntPtr ptr = _indexes;
					for (int i = 0; i < _nindexes; i++)
					{
						array[i] = (BASS_TAG_FLAC_CUE_TRACK_INDEX)Marshal.PtrToStructure(ptr, typeof(BASS_TAG_FLAC_CUE_TRACK_INDEX));
						ptr = new IntPtr((byte*)ptr.ToPointer() + Marshal.SizeOf((object)array[i]));
					}
					return array;
				}
				return null;
			}
		}

		private BASS_TAG_FLAC_CUE_TRACK()
		{
		}
	}
}
