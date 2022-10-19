using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public class BASS_TAG_DSD_COMMENT
	{
		public short TimeStampYear;

		public byte TimeStampMonth;

		public byte TimeStampDay;

		public byte TimeStampHour;

		public byte TimeStampMinutes;

		public short CommentType;

		public short CommentRef;

		private int _commentCount;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
		private byte[] _commentText;

		public string CommentText
		{
			get
			{
				if (_commentCount == 0)
				{
					return string.Empty;
				}
				return Encoding.ASCII.GetString(_commentText, 0, _commentCount);
			}
		}

		public override string ToString()
		{
			return CommentText;
		}

		private BASS_TAG_DSD_COMMENT()
		{
		}

		public static BASS_TAG_DSD_COMMENT GetTag(int handle, int index)
		{
			return FromIntPtr(Bass.BASS_ChannelGetTags(handle, (BASSTag)(78080 + index)));
		}

		public unsafe static BASS_TAG_DSD_COMMENT FromIntPtr(IntPtr p)
		{
			if (p == IntPtr.Zero)
			{
				return null;
			}
			BASS_TAG_DSD_COMMENT bASS_TAG_DSD_COMMENT = (BASS_TAG_DSD_COMMENT)Marshal.PtrToStructure(p, typeof(BASS_TAG_DSD_COMMENT));
			if (bASS_TAG_DSD_COMMENT._commentCount > 0)
			{
				byte[] array = new byte[bASS_TAG_DSD_COMMENT._commentCount];
				Marshal.Copy(new IntPtr((byte*)p.ToPointer() + Marshal.OffsetOf(typeof(BASS_TAG_DSD_COMMENT), "_commentText").ToInt32()), array, 0, bASS_TAG_DSD_COMMENT._commentCount);
				Array.Resize(ref bASS_TAG_DSD_COMMENT._commentText, bASS_TAG_DSD_COMMENT._commentCount);
				Buffer.BlockCopy(array, 0, bASS_TAG_DSD_COMMENT._commentText, 0, bASS_TAG_DSD_COMMENT._commentCount);
			}
			return bASS_TAG_DSD_COMMENT;
		}
	}
}
