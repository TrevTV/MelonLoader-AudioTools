using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_DX8_I3DL2REVERB
	{
		public int lRoom = -1000;

		public int lRoomHF;

		public float flRoomRolloffFactor;

		public float flDecayTime = 1.49f;

		public float flDecayHFRatio = 0.83f;

		public int lReflections = -2602;

		public float flReflectionsDelay = 0.007f;

		public int lReverb = 200;

		public float flReverbDelay = 0.011f;

		public float flDiffusion = 100f;

		public float flDensity = 100f;

		public float flHFReference = 5000f;

		public BASS_DX8_I3DL2REVERB()
		{
		}

		public BASS_DX8_I3DL2REVERB(int Room, int RoomHF, float RoomRolloffFactor, float DecayTime, float DecayHFRatio, int Reflections, float ReflectionsDelay, int Reverb, float ReverbDelay, float Diffusion, float Density, float HFReference)
		{
			lRoom = Room;
			lRoomHF = RoomHF;
			flRoomRolloffFactor = RoomRolloffFactor;
			flDecayTime = DecayTime;
			flDecayHFRatio = DecayHFRatio;
			lReflections = Reflections;
			flReflectionsDelay = ReflectionsDelay;
			lReverb = Reverb;
			flReverbDelay = ReverbDelay;
			flDiffusion = Diffusion;
			flDensity = Density;
			flHFReference = HFReference;
		}

		public void Preset_Default()
		{
			lRoom = -1000;
			lRoomHF = 0;
			flRoomRolloffFactor = 0f;
			flDecayTime = 1.49f;
			flDecayHFRatio = 0.83f;
			lReflections = -2602;
			flReflectionsDelay = 0.007f;
			lReverb = 200;
			flReverbDelay = 0.011f;
			flDiffusion = 100f;
			flDensity = 100f;
			flHFReference = 5000f;
		}
	}
}
