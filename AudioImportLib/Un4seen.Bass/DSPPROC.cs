using System;

namespace Un4seen.Bass
{
	public delegate void DSPPROC(int handle, int channel, IntPtr buffer, int length, IntPtr user);
}
