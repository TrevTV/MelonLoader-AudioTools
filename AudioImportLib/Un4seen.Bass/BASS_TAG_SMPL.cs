using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public class BASS_TAG_SMPL
	{
		private int _manufacturer;

		private int _product;

		private int _samplePeriod;

		private int _midiUnityNote;

		private int _midiPitchFraction;

		private int _smpteFormat;

		private int _smpteOffset;

		private int _nsampleLoops;

		private int _sampleData;

		private IntPtr _sampleLoops = IntPtr.Zero;

		public int Manufacturer => _manufacturer;

		public int Product => _product;

		public int SamplePeriod => _samplePeriod;

		public int MIDIUnityNote => _midiUnityNote;

		public int MIDIPitchFraction => _midiPitchFraction;

		public int SMPTEFormat => _smpteFormat;

		public int SMPTEOffset => _smpteOffset;

		public int NumSampleLoops => _nsampleLoops;

		public int SampleData => _sampleData;

		public unsafe BASS_TAG_SMPL_LOOP[] SampleLoops
		{
			get
			{
				if (_nsampleLoops > 0 && _sampleLoops != IntPtr.Zero)
				{
					BASS_TAG_SMPL_LOOP[] array = new BASS_TAG_SMPL_LOOP[_nsampleLoops];
					IntPtr ptr = _sampleLoops;
					for (int i = 0; i < _nsampleLoops; i++)
					{
						array[i] = (BASS_TAG_SMPL_LOOP)Marshal.PtrToStructure(ptr, typeof(BASS_TAG_SMPL_LOOP));
						ptr = new IntPtr((byte*)ptr.ToPointer() + Marshal.SizeOf((object)array[i]));
					}
					return array;
				}
				return null;
			}
		}

		private BASS_TAG_SMPL()
		{
		}

		public static BASS_TAG_SMPL GetTag(int handle)
		{
			IntPtr intPtr = Bass.BASS_ChannelGetTags(handle, BASSTag.BASS_TAG_RIFF_SMPL);
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return (BASS_TAG_SMPL)Marshal.PtrToStructure(intPtr, typeof(BASS_TAG_SMPL));
		}

		public static BASS_TAG_SMPL FromIntPtr(IntPtr p)
		{
			if (p == IntPtr.Zero)
			{
				return null;
			}
			return (BASS_TAG_SMPL)Marshal.PtrToStructure(p, typeof(BASS_TAG_SMPL));
		}
	}
}
