using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public class BASS_TAG_CUE
	{
		private int _ncuepoints;

		private IntPtr _cuepoints = IntPtr.Zero;

		public int NumCuePoints => _ncuepoints;

		public unsafe BASS_TAG_CUE_POINT[] CuePoints
		{
			get
			{
				if (_ncuepoints > 0 && _cuepoints != IntPtr.Zero)
				{
					BASS_TAG_CUE_POINT[] array = new BASS_TAG_CUE_POINT[_ncuepoints];
					IntPtr ptr = _cuepoints;
					for (int i = 0; i < _ncuepoints; i++)
					{
						array[i] = (BASS_TAG_CUE_POINT)Marshal.PtrToStructure(ptr, typeof(BASS_TAG_CUE_POINT));
						ptr = new IntPtr((byte*)ptr.ToPointer() + Marshal.SizeOf((object)array[i]));
					}
					return array;
				}
				return null;
			}
		}

		private BASS_TAG_CUE()
		{
		}

		public static BASS_TAG_CUE GetTag(int handle)
		{
			IntPtr intPtr = Bass.BASS_ChannelGetTags(handle, BASSTag.BASS_TAG_RIFF_CUE);
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return (BASS_TAG_CUE)Marshal.PtrToStructure(intPtr, typeof(BASS_TAG_CUE));
		}

		public static BASS_TAG_CUE FromIntPtr(IntPtr p)
		{
			if (p == IntPtr.Zero)
			{
				return null;
			}
			return (BASS_TAG_CUE)Marshal.PtrToStructure(p, typeof(BASS_TAG_CUE));
		}
	}
}
