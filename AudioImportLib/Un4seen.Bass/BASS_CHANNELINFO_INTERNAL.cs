using System;

namespace Un4seen.Bass
{
	[Serializable]
	internal struct BASS_CHANNELINFO_INTERNAL
	{
		public int freq;

		public int chans;

		public BASSFlag flags;

		public BASSChannelType ctype;

		public int origres;

		public int plugin;

		public int sample;

		public IntPtr filename;
	}
}
