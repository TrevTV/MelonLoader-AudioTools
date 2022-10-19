using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_FILEPROCS
	{
		public FILECLOSEPROC close;

		public FILELENPROC length;

		public FILEREADPROC read;

		public FILESEEKPROC seek;

		public BASS_FILEPROCS(FILECLOSEPROC closeCallback, FILELENPROC lengthCallback, FILEREADPROC readCallback, FILESEEKPROC seekCallback)
		{
			close = closeCallback;
			length = lengthCallback;
			read = readCallback;
			seek = seekCallback;
		}
	}
}
