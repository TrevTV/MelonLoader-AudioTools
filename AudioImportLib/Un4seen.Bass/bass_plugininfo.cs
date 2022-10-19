using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[Serializable]
	internal struct bass_plugininfo
	{
		public int version;

		public int formatc;

		public IntPtr formats;
	}
	[Serializable]
	public sealed class BASS_PLUGININFO
	{
		public int version;

		public int formatc;

		public BASS_PLUGINFORM[] formats;

		private BASS_PLUGININFO()
		{
		}

		internal BASS_PLUGININFO(int Version, BASS_PLUGINFORM[] Formats)
		{
			version = Version;
			formatc = Formats.Length;
			formats = Formats;
		}

		internal BASS_PLUGININFO(IntPtr pluginInfoPtr)
		{
			if (!(pluginInfoPtr == IntPtr.Zero))
			{
				bass_plugininfo bass_plugininfo = (bass_plugininfo)Marshal.PtrToStructure(pluginInfoPtr, typeof(bass_plugininfo));
				version = bass_plugininfo.version;
				formatc = bass_plugininfo.formatc;
				formats = new BASS_PLUGINFORM[formatc];
				ReadArrayStructure(formatc, bass_plugininfo.formats);
			}
		}

		internal BASS_PLUGININFO(int ver, int count, IntPtr fPtr)
		{
			version = ver;
			formatc = count;
			if (!(fPtr == IntPtr.Zero))
			{
				formats = new BASS_PLUGINFORM[count];
				ReadArrayStructure(formatc, fPtr);
			}
		}

		private unsafe void ReadArrayStructure(int count, IntPtr p)
		{
			for (int i = 0; i < count; i++)
			{
				try
				{
					formats[i] = (BASS_PLUGINFORM)Marshal.PtrToStructure(p, typeof(BASS_PLUGINFORM));
					p = new IntPtr((byte*)p.ToPointer() + Marshal.SizeOf((object)formats[i]));
				}
				catch
				{
					return;
				}
			}
		}

		public override string ToString()
		{
			return $"{version}, {formatc}";
		}
	}
}
