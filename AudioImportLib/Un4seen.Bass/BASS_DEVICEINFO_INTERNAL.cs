using System;

namespace Un4seen.Bass
{
	[Serializable]
	internal struct BASS_DEVICEINFO_INTERNAL
	{
		public IntPtr name;

		public IntPtr driver;

		public BASSDeviceInfo flags;
	}
}
