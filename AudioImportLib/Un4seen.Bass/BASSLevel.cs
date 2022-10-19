using System;

namespace Un4seen.Bass
{
	[Flags]
	public enum BASSLevel
	{
		BASS_LEVEL_ALL = 0x0,
		BASS_LEVEL_MONO = 0x1,
		BASS_LEVEL_STEREO = 0x2,
		BASS_LEVEL_RMS = 0x4,
		BASS_LEVEL_VOLPAN = 0x8
	}
}
