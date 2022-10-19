using System;

namespace Un4seen.Bass
{
	[Flags]
	public enum BASSRecordInfo
	{
		DSCAPS_NONE = 0x0,
		DSCAPS_EMULDRIVER = 0x20,
		DSCAPS_CERTIFIED = 0x40
	}
}
