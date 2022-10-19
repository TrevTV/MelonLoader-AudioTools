using System;

namespace Un4seen.Bass
{
	[Flags]
	public enum BASSVam
	{
		BASS_VAM_HARDWARE = 0x1,
		BASS_VAM_SOFTWARE = 0x2,
		BASS_VAM_TERM_TIME = 0x4,
		BASS_VAM_TERM_DIST = 0x8,
		BASS_VAM_TERM_PRIO = 0x10
	}
}
