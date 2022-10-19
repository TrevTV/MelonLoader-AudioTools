using System;

namespace Un4seen.Bass
{
	[Flags]
	public enum BASSSync
	{
		BASS_SYNC_POS = 0x0,
		BASS_SYNC_MUSICINST = 0x1,
		BASS_SYNC_END = 0x2,
		BASS_SYNC_MUSICFX = 0x3,
		BASS_SYNC_META = 0x4,
		BASS_SYNC_SLIDE = 0x5,
		BASS_SYNC_STALL = 0x6,
		BASS_SYNC_DOWNLOAD = 0x7,
		BASS_SYNC_FREE = 0x8,
		BASS_SYNC_MUSICPOS = 0xA,
		BASS_SYNC_SETPOS = 0xB,
		BASS_SYNC_OGG_CHANGE = 0xC,
		BASS_SYNC_DEV_FAIL = 0xE,
		BASS_SYNC_DEV_FORMAT = 0xF,
		BASS_SYNC_MIXTIME = 0x40000000,
		BASS_SYNC_ONETIME = int.MinValue,
		BASS_SYNC_MIXER_ENVELOPE = 0x10200,
		BASS_SYNC_MIXER_ENVELOPE_NODE = 0x10201,
		BASS_SYNC_WMA_CHANGE = 0x10100,
		BASS_SYNC_WMA_META = 0x10101,
		BASS_SYNC_CD_ERROR = 0x3E8,
		BASS_SYNC_CD_SPEED = 0x3EA,
		BASS_WINAMP_SYNC_BITRATE = 0x64,
		BASS_SYNC_MIDI_MARKER = 0x10000,
		BASS_SYNC_MIDI_CUE = 0x10001,
		BASS_SYNC_MIDI_LYRIC = 0x10002,
		BASS_SYNC_MIDI_TEXT = 0x10003,
		BASS_SYNC_MIDI_EVENT = 0x10004,
		BASS_SYNC_MIDI_TICK = 0x10005,
		BASS_SYNC_MIDI_TIMESIG = 0x10006,
		BASS_SYNC_MIDI_KEYSIG = 0x10007,
		BASS_SYNC_HLS_SEGMENT = 0x10300
	}
}