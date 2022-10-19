using System;

namespace Un4seen.Bass
{
	[Serializable]
	public sealed class BASS_CHANNELINFO
	{
		internal BASS_CHANNELINFO_INTERNAL _internal;

		public int freq;

		public int chans;

		public BASSFlag flags;

		public BASSChannelType ctype;

		public int origres;

		public int plugin;

		public int sample;

		public string filename = string.Empty;

		public bool origresIsFloat;

		public bool IsDecodingChannel => (flags & BASSFlag.BASS_STREAM_DECODE) != 0;

		public bool Is32bit => (flags & BASSFlag.BASS_SAMPLE_FLOAT) != 0;

		public bool Is8bit => (flags & BASSFlag.BASS_SAMPLE_8BITS) != 0;

		public override string ToString()
		{
			return string.Format("{0}, {1}Hz, {2}, {3}bit{4}", Utils.BASSChannelTypeToString(ctype), freq, Utils.ChannelNumberToString(chans), (origres != 0) ? origres : (Is32bit ? 32 : (Is8bit ? 8 : 16)), origresIsFloat ? "-float" : "");
		}
	}
}
