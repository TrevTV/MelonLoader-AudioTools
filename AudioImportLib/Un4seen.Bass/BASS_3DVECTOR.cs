using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_3DVECTOR
	{
		public float x;

		public float y;

		public float z;

		public BASS_3DVECTOR()
		{
		}

		public BASS_3DVECTOR(float X, float Y, float Z)
		{
			x = X;
			y = Y;
			z = Z;
		}

		public override string ToString()
		{
			return $"X={x}, Y={y}, Z={z}";
		}
	}
}
