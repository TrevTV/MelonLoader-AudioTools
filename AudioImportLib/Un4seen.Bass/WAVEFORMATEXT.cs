using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public sealed class WAVEFORMATEXT
	{
		public WAVEFORMATEX waveformatex;

		public byte[] extension;

		public int FormatLength
		{
			get
			{
				int num = 18;
				if (extension != null)
				{
					num += extension.Length;
				}
				return num;
			}
		}

		public WAVEFORMATEXT(int length)
		{
			waveformatex = new WAVEFORMATEX();
			waveformatex.cbSize = (short)(length - 18);
			if (waveformatex.cbSize >= 0)
			{
				extension = new byte[waveformatex.cbSize];
			}
		}

		public unsafe WAVEFORMATEXT(IntPtr codec)
		{
			waveformatex = (WAVEFORMATEX)Marshal.PtrToStructure(codec, typeof(WAVEFORMATEX));
			extension = new byte[waveformatex.cbSize];
			codec = new IntPtr((byte*)codec.ToPointer() + 18);
			Marshal.Copy(codec, extension, 0, waveformatex.cbSize);
		}

		public override string ToString()
		{
			return waveformatex.ToString();
		}
	}
}
