using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class BASS_TAG_FLAC_PICTURE
	{
		private int _apic;

		private IntPtr _mime = IntPtr.Zero;

		private IntPtr _desc = IntPtr.Zero;

		private int _width;

		private int _height;

		private int _depth;

		private int _colors;

		private int _length;

		private IntPtr _data = IntPtr.Zero;

		public string Mime
		{
			get
			{
				if (_mime == IntPtr.Zero)
				{
					return null;
				}
				return Utils.IntPtrAsStringAnsi(_mime);
			}
		}

		public string Desc
		{
			get
			{
				if (_desc == IntPtr.Zero)
				{
					return null;
				}
				return Utils.IntPtrAsStringUtf8(_desc);
			}
		}

		public int Width => _width;

		public int Height => _height;

		public int Depth => _depth;

		public int Colors => _colors;

		public int Length => _length;

		public byte[] Data
		{
			get
			{
				if (_data == IntPtr.Zero || _length == 0)
				{
					return null;
				}
				byte[] array = new byte[_length];
				Marshal.Copy(_data, array, 0, _length);
				return array;
			}
		}

		public string ImageURL
		{
			get
			{
				if (Mime == "-->" && _data != IntPtr.Zero && _length > 0)
				{
					return Utils.IntPtrAsStringAnsi(_data);
				}
				return null;
			}
		}

		public override string ToString()
		{
			if (string.IsNullOrEmpty(Mime))
			{
				return Desc;
			}
			return $"{Desc} ({Mime})";
		}

		private BASS_TAG_FLAC_PICTURE()
		{
		}

		public static BASS_TAG_FLAC_PICTURE GetTag(int handle, int pictureIndex)
		{
			IntPtr intPtr = Bass.BASS_ChannelGetTags(handle, (BASSTag)(73728 + pictureIndex));
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return (BASS_TAG_FLAC_PICTURE)Marshal.PtrToStructure(intPtr, typeof(BASS_TAG_FLAC_PICTURE));
		}

		public static BASS_TAG_FLAC_PICTURE FromIntPtr(IntPtr p)
		{
			if (p == IntPtr.Zero)
			{
				return null;
			}
			return (BASS_TAG_FLAC_PICTURE)Marshal.PtrToStructure(p, typeof(BASS_TAG_FLAC_PICTURE));
		}
	}
}
