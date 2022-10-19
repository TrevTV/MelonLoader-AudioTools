using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class BASS_TAG_SMPL_LOOP
	{
		private int _identifier;

		private int _type;

		private int _start;

		private int _end;

		private int _fraction;

		private int _playCount;

		public int Identifier => _identifier;

		public int Type => _type;

		public int Start => _start;

		public int End => _end;

		public int Fraction => _fraction;

		public int PlayCount => _playCount;

		private BASS_TAG_SMPL_LOOP()
		{
		}
	}
}
