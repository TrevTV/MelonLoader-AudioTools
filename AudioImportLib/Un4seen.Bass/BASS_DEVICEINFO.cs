using System;

namespace Un4seen.Bass
{
	[Serializable]
	public sealed class BASS_DEVICEINFO
	{
		internal BASS_DEVICEINFO_INTERNAL _internal;

		public string name = string.Empty;

		public string driver = string.Empty;

		public string id;

		public BASSDeviceInfo flags;

		public BASSDeviceInfo status => flags & (BASSDeviceInfo)16777215;

		public BASSDeviceInfo type => flags & BASSDeviceInfo.BASS_DEVICE_TYPE_MASK;

		public bool IsEnabled => (flags & BASSDeviceInfo.BASS_DEVICE_ENABLED) != 0;

		public bool IsDefault => (flags & BASSDeviceInfo.BASS_DEVICE_DEFAULT) != 0;

		public bool IsInitialized => (flags & BASSDeviceInfo.BASS_DEVICE_INIT) != 0;

		public override string ToString()
		{
			return name;
		}
	}
}
