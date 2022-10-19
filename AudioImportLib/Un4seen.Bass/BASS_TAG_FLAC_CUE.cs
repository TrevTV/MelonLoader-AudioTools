using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class BASS_TAG_FLAC_CUE
	{
		private IntPtr _catalog = IntPtr.Zero;

		private int _leadin;

		[MarshalAs(UnmanagedType.Bool)]
		private bool _iscd;

		private int _ntracks;

		private IntPtr _tracks = IntPtr.Zero;

		public string Catalog
		{
			get
			{
				if (_catalog == IntPtr.Zero)
				{
					return null;
				}
				return Utils.IntPtrAsStringAnsi(_catalog);
			}
		}

		public int LeadIn => _leadin;

		public bool IsCD => _iscd;

		public int NumTracks => _ntracks;

		public unsafe BASS_TAG_FLAC_CUE_TRACK[] Tracks
		{
			get
			{
				if (_ntracks > 0 && _tracks != IntPtr.Zero)
				{
					BASS_TAG_FLAC_CUE_TRACK[] array = new BASS_TAG_FLAC_CUE_TRACK[_ntracks];
					IntPtr ptr = _tracks;
					for (int i = 0; i < _ntracks; i++)
					{
						array[i] = (BASS_TAG_FLAC_CUE_TRACK)Marshal.PtrToStructure(ptr, typeof(BASS_TAG_FLAC_CUE_TRACK));
						ptr = new IntPtr((byte*)ptr.ToPointer() + Marshal.SizeOf((object)array[i]));
					}
					return array;
				}
				return null;
			}
		}

		private BASS_TAG_FLAC_CUE()
		{
		}

		public static BASS_TAG_FLAC_CUE GetTag(int handle)
		{
			IntPtr intPtr = Bass.BASS_ChannelGetTags(handle, BASSTag.BASS_TAG_FLAC_CUE);
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return (BASS_TAG_FLAC_CUE)Marshal.PtrToStructure(intPtr, typeof(BASS_TAG_FLAC_CUE));
		}

		public static BASS_TAG_FLAC_CUE FromIntPtr(IntPtr p)
		{
			if (p == IntPtr.Zero)
			{
				return null;
			}
			return (BASS_TAG_FLAC_CUE)Marshal.PtrToStructure(p, typeof(BASS_TAG_FLAC_CUE));
		}
	}
}
