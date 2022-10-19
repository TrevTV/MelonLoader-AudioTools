using System;

namespace Un4seen.Bass
{
	[Flags]
	public enum BASSInput
	{
		BASS_INPUT_NONE = 0x0,
		BASS_INPUT_OFF = 0x10000,
		BASS_INPUT_ON = 0x20000
	}
}
