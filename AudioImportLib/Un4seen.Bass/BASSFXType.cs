using System;

namespace Un4seen.Bass
{
	public enum BASSFXType
	{
		BASS_FX_DX8_CHORUS = 0,
		BASS_FX_DX8_COMPRESSOR = 1,
		BASS_FX_DX8_DISTORTION = 2,
		BASS_FX_DX8_ECHO = 3,
		BASS_FX_DX8_FLANGER = 4,
		BASS_FX_DX8_GARGLE = 5,
		BASS_FX_DX8_I3DL2REVERB = 6,
		BASS_FX_DX8_PARAMEQ = 7,
		BASS_FX_DX8_REVERB = 8,
		BASS_FX_VOLUME = 9,
		BASS_FX_BFX_ROTATE = 0x10000,
		[Obsolete("This effect is obsolete; use BASS_FX_BFX_ECHO4 instead")]
		BASS_FX_BFX_ECHO = 65537,
		[Obsolete("This effect is obsolete; use BASS_FX_BFX_CHORUS instead")]
		BASS_FX_BFX_FLANGER = 65538,
		BASS_FX_BFX_VOLUME = 65539,
		BASS_FX_BFX_PEAKEQ = 65540,
		[Obsolete("This effect is obsolete; use BASS_FX_BFX_FREEVERB instead")]
		BASS_FX_BFX_REVERB = 65541,
		[Obsolete("This effect is obsolete; use 2x BASS_FX_BFX_BQF with BASS_BFX_BQF_LOWPASS filter and appropriate fQ values instead")]
		BASS_FX_BFX_LPF = 65542,
		BASS_FX_BFX_MIX = 65543,
		BASS_FX_BFX_DAMP = 65544,
		BASS_FX_BFX_AUTOWAH = 65545,
		[Obsolete("This effect is obsolete; use BASS_FX_BFX_ECHO4 instead")]
		BASS_FX_BFX_ECHO2 = 65546,
		BASS_FX_BFX_PHASER = 65547,
		[Obsolete("This effect is obsolete; use BASS_FX_BFX_ECHO4 instead")]
		BASS_FX_BFX_ECHO3 = 65548,
		BASS_FX_BFX_CHORUS = 65549,
		[Obsolete("This effect is obsolete; use BASS_FX_BFX_BQF with BASS_BFX_BQF_ALLPASS filter instead")]
		BASS_FX_BFX_APF = 65550,
		[Obsolete("This effect is obsolete; use BASS_FX_BFX_COMPRESSOR2 instead")]
		BASS_FX_BFX_COMPRESSOR = 65551,
		BASS_FX_BFX_DISTORTION = 65552,
		BASS_FX_BFX_COMPRESSOR2 = 65553,
		BASS_FX_BFX_VOLUME_ENV = 65554,
		BASS_FX_BFX_BQF = 65555,
		BASS_FX_BFX_ECHO4 = 65556,
		BASS_FX_BFX_PITCHSHIFT = 65557,
		BASS_FX_BFX_FREEVERB = 65558
	}
}
