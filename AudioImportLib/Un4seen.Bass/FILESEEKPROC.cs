using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[return: MarshalAs(UnmanagedType.Bool)]
	public delegate bool FILESEEKPROC(long offset, IntPtr user);
}
