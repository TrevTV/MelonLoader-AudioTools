using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[return: MarshalAs(UnmanagedType.Bool)]
	public delegate bool RECORDPROC(int handle, IntPtr buffer, int length, IntPtr user);
}
