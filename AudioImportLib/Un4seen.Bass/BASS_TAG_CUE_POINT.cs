using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class BASS_TAG_CUE_POINT
	{
		private int _name;

		private int _position;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		private byte[] _fourCC;

		private int _chunkStart;

		private int _blockStart;

		private int _sampleOffset;

		public int Name => _name;

		public int Position => _position;

		public string FourCC
		{
			get
			{
				if (_fourCC == null)
				{
					return string.Empty;
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(_fourCC, 0, _fourCC.Length).TrimEnd(default(char));
				}
				return Encoding.ASCII.GetString(_fourCC, 0, _fourCC.Length).TrimEnd(default(char));
			}
		}

		public int ChunkStart => _chunkStart;

		public int BlockStart => _blockStart;

		public int SampleOffset => _sampleOffset;

		private BASS_TAG_CUE_POINT()
		{
		}
	}
}
