using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_PLUGINFORM
	{
		public BASSChannelType ctype;

		[MarshalAs(UnmanagedType.LPStr)]
		public string name = string.Empty;

		[MarshalAs(UnmanagedType.LPStr)]
		public string exts = string.Empty;

		public BASS_PLUGINFORM()
		{
		}

		public BASS_PLUGINFORM(string Name, string Extensions, BASSChannelType ChannelType)
		{
			ctype = ChannelType;
			name = Name;
			exts = Extensions;
		}

		public override string ToString()
		{
			return $"{name}|{exts}";
		}
	}
}
