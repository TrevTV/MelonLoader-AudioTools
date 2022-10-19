using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct BASS_TAG_BEXT
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		private byte[] description;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		private byte[] originator;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		private byte[] originatorReference;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
		private byte[] originationDate;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		private byte[] originationTime;

		public long TimeReference;

		public short Version;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		private byte[] umid;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 190)]
		public byte[] Reserved;

		public string Description
		{
			get
			{
				if (description == null)
				{
					return string.Empty;
				}
				Encoding encoding = Encoding.ASCII;
				if (BassNet.UseBrokenLatin1Behavior)
				{
					encoding = Encoding.Default;
				}
				string[] array = encoding.GetString(description, 0, description.Length).TrimEnd(default(char)).Split(new char[1], 2, StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = array[i].TrimEnd('\0', ' ');
				}
				return string.Join(";", array);
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					description = new byte[256];
					return;
				}
				string[] array = value.Split(new char[1]
				{
					';'
				}, 2, StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = array[i].Trim();
				}
				string empty = string.Empty;
				if (array.Length > 1)
				{
					if (array[0] != null && array[0].Length >= 64)
					{
						array[0] = array[0].Remove(63);
					}
					if (array[0] != null)
					{
						array[0] = array[0].PadRight(64, '\0');
						empty = array[0] + array[1];
					}
					else
					{
						empty = array[1];
					}
					if (empty.Length > 256)
					{
						empty = empty.Remove(256);
					}
				}
				else
				{
					empty = array[0];
					if (empty.Length > 256)
					{
						empty = empty.Remove(256);
					}
				}
				description = new byte[256];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(empty, 0, (empty.Length > 256) ? 256 : empty.Length, description, 0);
				}
				else
				{
					Encoding.ASCII.GetBytes(empty, 0, (empty.Length > 256) ? 256 : empty.Length, description, 0);
				}
			}
		}

		public string Originator
		{
			get
			{
				if (originator == null)
				{
					return string.Empty;
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(originator, 0, originator.Length).TrimEnd(default(char));
				}
				return Encoding.ASCII.GetString(originator, 0, originator.Length).TrimEnd(default(char));
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					originator = new byte[32];
					return;
				}
				originator = new byte[32];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 32) ? 32 : value.Length, originator, 0);
				}
				else
				{
					Encoding.ASCII.GetBytes(value, 0, (value.Length > 32) ? 32 : value.Length, originator, 0);
				}
			}
		}

		public string OriginatorReference
		{
			get
			{
				if (originatorReference == null)
				{
					return string.Empty;
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(originatorReference, 0, originatorReference.Length).TrimEnd(default(char));
				}
				return Encoding.ASCII.GetString(originatorReference, 0, originatorReference.Length).TrimEnd(default(char));
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					originatorReference = new byte[32];
					return;
				}
				originatorReference = new byte[32];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 32) ? 32 : value.Length, originatorReference, 0);
				}
				else
				{
					Encoding.ASCII.GetBytes(value, 0, (value.Length > 32) ? 32 : value.Length, originatorReference, 0);
				}
			}
		}

		public string OriginationDate
		{
			get
			{
				if (originationDate == null)
				{
					return "0000-01-01";
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(originationDate, 0, originationDate.Length).TrimEnd(default(char)).Replace(' ', '-')
						.Replace(':', '-')
						.Replace('.', '-')
						.Replace('_', '-');
				}
				return Encoding.ASCII.GetString(originationDate, 0, originationDate.Length).TrimEnd(default(char)).Replace(' ', '-')
					.Replace(':', '-')
					.Replace('.', '-')
					.Replace('_', '-');
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					if (BassNet.UseBrokenLatin1Behavior)
					{
						originationDate = Encoding.Default.GetBytes("0000-01-01");
					}
					else
					{
						originationDate = Encoding.ASCII.GetBytes("0000-01-01");
					}
					return;
				}
				originationDate = new byte[10];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 10) ? 10 : value.Length, originationDate, 0);
				}
				else
				{
					Encoding.ASCII.GetBytes(value, 0, (value.Length > 10) ? 10 : value.Length, originationDate, 0);
				}
			}
		}

		public string OriginationTime
		{
			get
			{
				if (originationTime == null)
				{
					return "00:00:00";
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(originationTime, 0, originationTime.Length).TrimEnd(default(char)).Replace(' ', ':')
						.Replace('-', ':')
						.Replace('.', ':')
						.Replace('_', ':');
				}
				return Encoding.ASCII.GetString(originationTime, 0, originationTime.Length).TrimEnd(default(char)).Replace(' ', ':')
					.Replace('-', ':')
					.Replace('.', ':')
					.Replace('_', ':');
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					if (BassNet.UseBrokenLatin1Behavior)
					{
						originationTime = Encoding.Default.GetBytes("00:00:00");
					}
					else
					{
						originationTime = Encoding.ASCII.GetBytes("00:00:00");
					}
					return;
				}
				originationTime = new byte[8];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 8) ? 8 : value.Length, originationTime, 0);
				}
				else
				{
					Encoding.ASCII.GetBytes(value, 0, (value.Length > 8) ? 8 : value.Length, originationTime, 0);
				}
			}
		}

		public string UMID
		{
			get
			{
				return Utils.ByteToHex(umid);
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					umid = new byte[64];
				}
				else
				{
					umid = Utils.HexToByte(value, 64);
				}
			}
		}

		public override string ToString()
		{
			return Description;
		}

		public unsafe string GetCodingHistory(IntPtr tag)
		{
			if (tag == IntPtr.Zero)
			{
				return null;
			}
			int len;
			return Utils.IntPtrAsStringLatin1(new IntPtr((byte*)tag.ToPointer() + 602), out len);
		}

		public byte[] AsByteArray(string codingHistory)
		{
			if (string.IsNullOrEmpty(codingHistory))
			{
				codingHistory = new string('\0', 256);
			}
			else
			{
				if (!codingHistory.EndsWith("\r\n"))
				{
					codingHistory += "\r\n";
				}
				if (!codingHistory.EndsWith("\0"))
				{
					codingHistory += "\0";
				}
				if (codingHistory.Length % 2 == 1)
				{
					codingHistory += "\0";
				}
				int num = codingHistory.Length % 256;
				if (num > 0)
				{
					codingHistory += new string('\0', num);
				}
			}
			byte[] array = BassNet.UseBrokenLatin1Behavior ? Encoding.Default.GetBytes(codingHistory) : Encoding.ASCII.GetBytes(codingHistory);
			int num2 = Marshal.SizeOf(typeof(BASS_TAG_BEXT));
			byte[] array2 = new byte[num2];
			GCHandle gCHandle = GCHandle.Alloc(array2, GCHandleType.Pinned);
			Marshal.StructureToPtr((object)this, gCHandle.AddrOfPinnedObject(), fDeleteOld: false);
			gCHandle.Free();
			byte[] array3 = new byte[num2 + array.Length];
			Array.Copy(array2, 0, array3, 0, num2);
			Array.Copy(array, 0, array3, num2, array.Length);
			return array3;
		}
	}
}
