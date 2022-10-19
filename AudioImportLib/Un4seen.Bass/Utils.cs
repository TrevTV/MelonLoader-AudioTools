using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Un4seen.Bass
{
	[SuppressUnmanagedCodeSecurity]
	public sealed class Utils
	{
		private static Random _autoRandomizer = new Random();

		public static bool Is64Bit
		{
			get
			{
				if (IntPtr.Size != 8)
				{
					return false;
				}
				return true;
			}
		}

		private Utils()
		{
		}

		public static byte MakeByte(byte lowBits, byte highBits)
		{
			return (byte)((highBits << 4) | lowBits);
		}

		public static short MakeWord(byte lowByte, byte highByte)
		{
			return (short)((highByte << 8) | lowByte);
		}

		public static int MakeLong(short lowWord, short highWord)
		{
			return (highWord << 16) | (lowWord & 0xFFFF);
		}

		public static int MakeLong(int lowWord, int highWord)
		{
			return (highWord << 16) | (lowWord & 0xFFFF);
		}

		public static long MakeLong64(int lowWord, int highWord)
		{
			return ((long)highWord << 32) | (lowWord & uint.MaxValue);
		}

		public static long MakeLong64(long lowWord, long highWord)
		{
			return (highWord << 32) | (lowWord & uint.MaxValue);
		}

		public static short HighWord(int dWord)
		{
			return (short)((dWord >> 16) & 0xFFFF);
		}

		public static int HighWord32(int dWord)
		{
			return (dWord >> 16) & 0xFFFF;
		}

		public static int HighWord(long qWord)
		{
			return (int)((qWord >> 32) & uint.MaxValue);
		}

		public static short LowWord(int dWord)
		{
			return (short)(dWord & 0xFFFF);
		}

		public static int LowWord32(int dWord)
		{
			return dWord & 0xFFFF;
		}

		public static int LowWord(long qWord)
		{
			return (int)(qWord & uint.MaxValue);
		}

		public static double LevelToDB(int level, int maxLevel)
		{
			return 20.0 * Math.Log10((double)level / (double)maxLevel);
		}

		public static double LevelToDB(double level, double maxLevel)
		{
			return 20.0 * Math.Log10(level / maxLevel);
		}

		public static int DBToLevel(double dB, int maxLevel)
		{
			return (int)Math.Round((double)maxLevel * Math.Pow(10.0, dB / 20.0));
		}

		public static double DBToLevel(double dB, double maxLevel)
		{
			return maxLevel * Math.Pow(10.0, dB / 20.0);
		}

		public static string FixTimespan(double seconds)
		{
			return TimeSpan.FromSeconds(seconds).ToString();
		}

		public static string FixTimespan(double seconds, string format)
		{
			string result = string.Empty;
			try
			{
				DateTime dateTime = DateTime.Today.AddSeconds(seconds);
				switch (format)
				{
				case "HHMM":
					result = dateTime.ToString("HH:mm");
					return result;
				case "HHMMSS":
					result = dateTime.ToString("HH:mm:ss");
					return result;
				case "MMSS":
					result = dateTime.ToString("mm:ss");
					return result;
				case "MMSSFFF":
					result = dateTime.ToString("mm:ss.fff");
					return result;
				case "MMSSFF":
					result = dateTime.ToString("mm:ss.ff");
					return result;
				case "MMSSF":
					result = dateTime.ToString("mm:ss.f");
					return result;
				case "HHMMSSFFF":
					result = dateTime.ToString("HH:mm:ss.fff");
					return result;
				case "HHMMSSFF":
					result = dateTime.ToString("HH:mm:ss.ff");
					return result;
				case "HHMMSSF":
					result = dateTime.ToString("HH:mm:ss.f");
					return result;
				case "SMPTE":
					result = dateTime.ToString("HH.mm.ss.f");
					return result;
				case "SMPTE24":
					result = $"{dateTime.Hour}:{dateTime.Minute}:{dateTime.Second}:{Math.Round((double)dateTime.Millisecond / 41.666666666666664):F0}";
					return result;
				case "SMPTE25":
					result = $"{dateTime.Hour}:{dateTime.Minute}:{dateTime.Second}:{Math.Round((double)dateTime.Millisecond / 40.0):F0}";
					return result;
				case "SMPTE30":
					result = $"{dateTime.Hour}:{dateTime.Minute}:{dateTime.Second}:{Math.Round((double)dateTime.Millisecond / 33.333333333333336):F0}";
					return result;
				case "SMPTE50":
					result = $"{dateTime.Hour}:{dateTime.Minute}:{dateTime.Second}:{Math.Round((double)dateTime.Millisecond / 20.0):F0}";
					return result;
				case "SMPTE60":
					result = $"{dateTime.Hour}:{dateTime.Minute}:{dateTime.Second}:{Math.Round((double)dateTime.Millisecond / 16.666666666666668):F0}";
					return result;
				default:
					result = dateTime.ToString(format);
					return result;
				}
			}
			catch
			{
				return result;
			}
		}

		public static int FFTFrequency2Index(int frequency, int length, int samplerate)
		{
			int num = (int)Math.Round((double)length * (double)frequency / (double)samplerate);
			if (num > length / 2 - 1)
			{
				num = length / 2 - 1;
			}
			return num;
		}

		public static int FFTIndex2Frequency(int index, int length, int samplerate)
		{
			return (int)Math.Round((double)index * (double)samplerate / (double)length);
		}

		public static byte[] SampleTo8Bit(byte sample)
		{
			return new byte[1]
			{
				sample
			};
		}

		public static byte[] SampleTo8Bit(short sample)
		{
			byte[] array = new byte[1];
			int num = sample / 256 + 128;
			if (num > 255)
			{
				num = 255;
			}
			else if (num < 0)
			{
				num = 0;
			}
			array[0] = (byte)num;
			return array;
		}

		public static byte[] SampleTo8Bit(float sample)
		{
			byte[] array = new byte[1];
			int num = (int)(sample * 128f) + 128;
			if (num > 255)
			{
				num = 255;
			}
			else if (num < 0)
			{
				num = 0;
			}
			array[0] = (byte)num;
			return array;
		}

		public static byte[] SampleTo16Bit(byte sample)
		{
			byte[] array = new byte[2];
			int num = (sample - 128) * 256;
			if (num > 32767)
			{
				num = 32767;
			}
			else if (num < -32768)
			{
				num = -32768;
			}
			for (int i = 0; i < 2; i++)
			{
				array[i] = (byte)(num >> i * 8);
			}
			return array;
		}

		public static byte[] SampleTo16Bit(short sample)
		{
			byte[] array = new byte[2];
			for (int i = 0; i < 2; i++)
			{
				array[i] = (byte)(sample >> i * 8);
			}
			return array;
		}

		public static byte[] SampleTo16Bit(float sample)
		{
			byte[] array = new byte[2];
			int num = (int)(sample * 32768f);
			if (num > 32767)
			{
				num = 32767;
			}
			else if (num < -32768)
			{
				num = -32768;
			}
			for (int i = 0; i < 2; i++)
			{
				array[i] = (byte)(num >> i * 8);
			}
			return array;
		}

		public static byte[] SampleTo24Bit(byte sample)
		{
			byte[] array = new byte[3];
			int num = (sample - 128) * 65536;
			if (num > 8388607)
			{
				num = 8388607;
			}
			else if (num < -8388608)
			{
				num = -8388608;
			}
			for (int i = 0; i < 3; i++)
			{
				array[i] = (byte)(num >> i * 8);
			}
			return array;
		}

		public static byte[] SampleTo24Bit(short sample)
		{
			byte[] array = new byte[3];
			int num = sample * 256;
			if (num > 8388607)
			{
				num = 8388607;
			}
			else if (num < -8388608)
			{
				num = -8388608;
			}
			for (int i = 0; i < 3; i++)
			{
				array[i] = (byte)(num >> i * 8);
			}
			return array;
		}

		public static byte[] SampleTo24Bit(float sample)
		{
			byte[] array = new byte[3];
			int num = (int)(sample * 8388608f);
			if (num > 8388607)
			{
				num = 8388607;
			}
			else if (num < -8388608)
			{
				num = -8388608;
			}
			for (int i = 0; i < 3; i++)
			{
				array[i] = (byte)(num >> i * 8);
			}
			return array;
		}

		public static float SampleTo32Bit(byte sample)
		{
			return ((float)(int)sample - 128f) / 128f;
		}

		public static float SampleTo32Bit(short sample)
		{
			return (float)sample / 32768f;
		}

		public static float SampleTo32Bit(byte[] sample)
		{
			int num = sample.Length;
			if (num == 1)
			{
				return SampleTo32Bit(sample[0]);
			}
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				num2 |= sample[i] << i * 8;
			}
			if (sample[num - 1] > 127)
			{
				num2 -= (int)(Math.Pow(256.0, num) / 2.0);
				return -1f + (float)((double)num2 / (Math.Pow(256.0, num) / 2.0));
			}
			return (float)((double)num2 / (Math.Pow(256.0, num) / 2.0));
		}

		public static int SampleTo24Bit(byte[] sample)
		{
			int num = sample.Length;
			if (num < 3)
			{
				return 0;
			}
			int num2 = 0;
			int num3 = 0;
			for (int i = num - 3; i < num; i++)
			{
				num2 |= sample[i] << num3 * 8;
				num3++;
			}
			while (num2 > 8388607)
			{
				num2 -= 8388608;
			}
			return num2;
		}

		public static short SampleTo16Bit(byte[] sample)
		{
			int num = sample.Length;
			if (num < 2)
			{
				return 0;
			}
			int num2 = 0;
			int num3 = 0;
			for (int i = num - 2; i < num; i++)
			{
				num2 |= sample[i] << num3 * 8;
				num3++;
			}
			return (short)num2;
		}

		public static byte SampleTo8Bit(byte[] sample)
		{
			int num = sample.Length;
			if (num < 1)
			{
				return 0;
			}
			return sample[num - 1];
		}

		public static float Semitone2Samplerate(float origfreq, int semitones)
		{
			return origfreq * (float)Math.Pow(2.0, (float)semitones / 12f);
		}

		public static float BPM2Seconds(float bpm)
		{
			if (bpm != 0f)
			{
				return 60f / bpm;
			}
			return -1f;
		}

		public static float Seconds2BPM(double seconds)
		{
			if (seconds != 0.0)
			{
				return (float)(60.0 / seconds);
			}
			return -1f;
		}

		public static string ByteToHex(byte[] buffer)
		{
			if (buffer == null || buffer.Length == 0)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder(buffer.Length * 3);
			foreach (byte value in buffer)
			{
				stringBuilder.Append(Convert.ToString(value, 16).PadLeft(2, '0').PadRight(3, ' '));
			}
			return stringBuilder.ToString().ToUpper();
		}

		public static byte[] HexToByte(string hexString, int length)
		{
			if (string.IsNullOrEmpty(hexString))
			{
				return null;
			}
			hexString = hexString.Replace(" ", "");
			byte[] array = (length < 0) ? new byte[hexString.Length / 2] : new byte[length];
			for (int i = 0; i < hexString.Length; i += 2)
			{
				array[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
				if (i / 2 == length)
				{
					break;
				}
			}
			return array;
		}

		private static int IntPtrNullTermLength(IntPtr p)
		{
			int i;
			for (i = 0; Marshal.ReadByte(p, i) != 0; i++)
			{
			}
			return i;
		}

		public static string IntPtrAsStringAnsi(IntPtr ansiPtr)
		{
			int len;
			return IntPtrAsStringUtf8orLatin1(ansiPtr, out len);
		}

		public static string IntPtrAsStringAnsi(IntPtr ansiPtr, int len)
		{
			if (ansiPtr != IntPtr.Zero && len > 0)
			{
				byte[] array = new byte[len];
				Marshal.Copy(ansiPtr, array, 0, len);
				return Encoding.Default.GetString(array, 0, len);
			}
			return null;
		}

		public static string IntPtrAsStringUnicode(IntPtr unicodePtr)
		{
			if (unicodePtr != IntPtr.Zero)
			{
				return Marshal.PtrToStringUni(unicodePtr);
			}
			return null;
		}

		public static string IntPtrAsStringUnicode(IntPtr unicodePtr, int len)
		{
			if (unicodePtr != IntPtr.Zero)
			{
				return Marshal.PtrToStringUni(unicodePtr, len);
			}
			return null;
		}

		public static string IntPtrAsStringLatin1(IntPtr latin1Ptr, out int len)
		{
			len = 0;
			if (latin1Ptr != IntPtr.Zero)
			{
				len = IntPtrNullTermLength(latin1Ptr);
				if (len != 0)
				{
					byte[] array = new byte[len];
					Marshal.Copy(latin1Ptr, array, 0, len);
					if (!BassNet.UseBrokenLatin1Behavior)
					{
						return Encoding.GetEncoding("latin1").GetString(array, 0, len);
					}
					return Encoding.Default.GetString(array, 0, len);
				}
			}
			return null;
		}

		public static string IntPtrAsStringUtf8orLatin1(IntPtr utf8Ptr, out int len)
		{
			len = 0;
			if (utf8Ptr != IntPtr.Zero)
			{
				len = IntPtrNullTermLength(utf8Ptr);
				if (len != 0)
				{
					byte[] array = new byte[len];
					Marshal.Copy(utf8Ptr, array, 0, len);
					string text = BassNet.UseBrokenLatin1Behavior ? Encoding.Default.GetString(array, 0, len) : Encoding.GetEncoding("latin1").GetString(array, 0, len);
					try
					{
						string @string = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true).GetString(array, 0, len);
						if (@string.Length < text.Length)
						{
							return @string;
						}
						return text;
					}
					catch
					{
						return text;
					}
				}
			}
			return null;
		}

		public static string IntPtrAsStringUtf8(IntPtr utf8Ptr, out int len)
		{
			len = 0;
			if (utf8Ptr != IntPtr.Zero)
			{
				len = IntPtrNullTermLength(utf8Ptr);
				if (len != 0)
				{
					byte[] array = new byte[len];
					Marshal.Copy(utf8Ptr, array, 0, len);
					return Encoding.UTF8.GetString(array, 0, len);
				}
			}
			return null;
		}

		public static string IntPtrAsStringUtf8(IntPtr utf8Ptr)
		{
			if (utf8Ptr != IntPtr.Zero)
			{
				int num = IntPtrNullTermLength(utf8Ptr);
				if (num != 0)
				{
					byte[] array = new byte[num];
					Marshal.Copy(utf8Ptr, array, 0, num);
					return Encoding.UTF8.GetString(array, 0, num);
				}
			}
			return null;
		}

		public static object IntAsObject(IntPtr ptr, Type structureType)
		{
			return Marshal.PtrToStructure(ptr, structureType);
		}

		public static Version GetVersion()
		{
			return Assembly.GetExecutingAssembly().GetName().Version;
		}

		public unsafe static string[] IntPtrToArrayNullTermAnsi(IntPtr pointer)
		{
			if (pointer != IntPtr.Zero)
			{
				List<string> list = new List<string>();
				while (true)
				{
					string text = IntPtrAsStringAnsi(pointer);
					if (string.IsNullOrEmpty(text))
					{
						break;
					}
					list.Add(text);
					pointer = new IntPtr((byte*)pointer.ToPointer() + text.Length + 1);
				}
				if (list.Count > 0)
				{
					return list.ToArray();
				}
			}
			return null;
		}

		public unsafe static string[] IntPtrToArrayNullTermUtf8(IntPtr pointer)
		{
			if (pointer != IntPtr.Zero)
			{
				List<string> list = new List<string>();
				string empty = string.Empty;
				while (true)
				{
					int num = IntPtrNullTermLength(pointer);
					if (num <= 0)
					{
						break;
					}
					byte[] array = new byte[num];
					Marshal.Copy(pointer, array, 0, num);
					pointer = new IntPtr((byte*)pointer.ToPointer() + num + 1);
					empty = Encoding.UTF8.GetString(array, 0, num);
					list.Add(empty);
				}
				if (list.Count > 0)
				{
					return list.ToArray();
				}
			}
			return null;
		}

		public unsafe static string[] IntPtrToArrayNullTermUnicode(IntPtr pointer)
		{
			if (pointer != IntPtr.Zero)
			{
				List<string> list = new List<string>();
				while (true)
				{
					string text = Marshal.PtrToStringUni(pointer);
					if (text.Length == 0)
					{
						break;
					}
					list.Add(text);
					pointer = new IntPtr((byte*)pointer.ToPointer() + 2 * text.Length + 2);
				}
				if (list.Count > 0)
				{
					return list.ToArray();
				}
				return null;
			}
			return null;
		}

		public unsafe static int[] IntPtrToArrayNullTermInt32(IntPtr pointer)
		{
			if (pointer != IntPtr.Zero)
			{
				int i = 0;
				for (int* ptr = (int*)(void*)pointer; ptr[i] != 0; i++)
				{
				}
				if (i > 0)
				{
					int[] array = new int[i];
					Marshal.Copy(pointer, array, 0, i);
					return array;
				}
				return null;
			}
			return null;
		}

		public unsafe static short[] IntPtrToArrayNullTermInt16(IntPtr pointer)
		{
			if (pointer != IntPtr.Zero)
			{
				int i = 0;
				for (short* ptr = (short*)(void*)pointer; ptr[i] != 0; i++)
				{
				}
				if (i > 0)
				{
					short[] array = new short[i];
					Marshal.Copy(pointer, array, 0, i);
					return array;
				}
				return null;
			}
			return null;
		}

		public static void StringToNullTermAnsi(string text, IntPtr target)
		{
			if (target != IntPtr.Zero && text != null)
			{
				text += "\0";
				byte[] bytes = Encoding.Default.GetBytes(text);
				Marshal.Copy(bytes, 0, target, bytes.Length);
			}
		}

		public static void StringToNullTermUnicode(string text, IntPtr target)
		{
			if (target != IntPtr.Zero && text != null)
			{
				text += "\0";
				byte[] bytes = Encoding.Unicode.GetBytes(text);
				Marshal.Copy(bytes, 0, target, bytes.Length);
			}
		}

		public static void StringToNullTermUtf8(string text, IntPtr target)
		{
			if (target != IntPtr.Zero && text != null)
			{
				text += "\0";
				byte[] bytes = Encoding.UTF8.GetBytes(text);
				Marshal.Copy(bytes, 0, target, bytes.Length);
			}
		}

		public static void StringToNullTermAnsi(string[] text, IntPtr target, bool addCRLF)
		{
			if (!(target != IntPtr.Zero) || text == null)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string text2 in text)
			{
				if (!string.IsNullOrEmpty(text2))
				{
					stringBuilder.Append(text2);
					if (addCRLF && !text2.EndsWith("\r\n"))
					{
						stringBuilder.Append("\r\n");
					}
					stringBuilder.Append('\0');
				}
			}
			stringBuilder.Append('\0');
			byte[] bytes = Encoding.Default.GetBytes(stringBuilder.ToString());
			Marshal.Copy(bytes, 0, target, bytes.Length);
		}

		public static void StringToNullTermUnicode(string[] text, IntPtr target, bool addCRLF)
		{
			if (!(target != IntPtr.Zero) || text == null)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string text2 in text)
			{
				if (!string.IsNullOrEmpty(text2))
				{
					stringBuilder.Append(text2);
					if (addCRLF && !text2.EndsWith("\r\n"))
					{
						stringBuilder.Append("\r\n");
					}
					stringBuilder.Append('\0');
				}
			}
			stringBuilder.Append('\0');
			byte[] bytes = Encoding.Unicode.GetBytes(stringBuilder.ToString());
			Marshal.Copy(bytes, 0, target, bytes.Length);
		}

		public static void StringToNullTermUtf8(string[] text, IntPtr target, bool addCRLF)
		{
			if (!(target != IntPtr.Zero) || text == null)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string text2 in text)
			{
				if (!string.IsNullOrEmpty(text2))
				{
					stringBuilder.Append(text2);
					if (addCRLF && !text2.EndsWith("\r\n"))
					{
						stringBuilder.Append("\r\n");
					}
					stringBuilder.Append('\0');
				}
			}
			stringBuilder.Append('\0');
			byte[] bytes = Encoding.UTF8.GetBytes(stringBuilder.ToString());
			Marshal.Copy(bytes, 0, target, bytes.Length);
		}

		public static string ChannelNumberToString(int chans)
		{
			string result = chans.ToString();
			switch (chans)
			{
			case 1:
				result = "Mono";
				break;
			case 2:
				result = "Stereo";
				break;
			case 3:
				result = "2.1";
				break;
			case 4:
				result = "2.2";
				break;
			case 5:
				result = "4.1";
				break;
			case 6:
				result = "5.1";
				break;
			case 7:
				result = "5.2";
				break;
			case 8:
				result = "7.1";
				break;
			}
			return result;
		}

		public static string BASSChannelTypeToString(BASSChannelType ctype)
		{
			string result = "???";
			if ((ctype & BASSChannelType.BASS_CTYPE_STREAM_WAV) > BASSChannelType.BASS_CTYPE_UNKNOWN)
			{
				ctype = BASSChannelType.BASS_CTYPE_STREAM_WAV;
			}
			switch (ctype)
			{
			case BASSChannelType.BASS_CTYPE_SAMPLE:
				result = "Sample";
				break;
			case BASSChannelType.BASS_CTYPE_RECORD:
				result = "Recording";
				break;
			case BASSChannelType.BASS_CTYPE_MUSIC_MO3:
				result = "MO3";
				break;
			case BASSChannelType.BASS_CTYPE_STREAM:
				result = "Custom Stream";
				break;
			case BASSChannelType.BASS_CTYPE_STREAM_WAV:
			case BASSChannelType.BASS_CTYPE_STREAM_WAV_PCM:
			case BASSChannelType.BASS_CTYPE_STREAM_WAV_FLOAT:
				result = "WAV";
				break;
			case BASSChannelType.BASS_CTYPE_STREAM_MIXER:
				result = "Mixer";
				break;
			case BASSChannelType.BASS_CTYPE_STREAM_SPLIT:
				result = "Splitter";
				break;
			case BASSChannelType.BASS_CTYPE_STREAM_OGG:
				result = "OGG";
				break;
			case BASSChannelType.BASS_CTYPE_STREAM_MP1:
				result = "MP1";
				break;
			case BASSChannelType.BASS_CTYPE_STREAM_MP2:
				result = "MP2";
				break;
			case BASSChannelType.BASS_CTYPE_STREAM_MP3:
				result = "MP3";
				break;
			case BASSChannelType.BASS_CTYPE_STREAM_AIFF:
				result = "AIFF";
				break;
			case BASSChannelType.BASS_CTYPE_MUSIC_MOD:
				result = "MOD";
				break;
			case BASSChannelType.BASS_CTYPE_MUSIC_MTM:
				result = "MTM";
				break;
			case BASSChannelType.BASS_CTYPE_MUSIC_S3M:
				result = "S3M";
				break;
			case BASSChannelType.BASS_CTYPE_MUSIC_XM:
				result = "XM";
				break;
			case BASSChannelType.BASS_CTYPE_MUSIC_IT:
				result = "IT";
				break;
			case BASSChannelType.BASS_CTYPE_STREAM_WV:
				result = "Wavpack";
				break;
			case BASSChannelType.BASS_CTYPE_STREAM_WV_H:
				result = "Wavpack";
				break;
			case BASSChannelType.BASS_CTYPE_STREAM_WV_L:
				result = "Wavpack";
				break;
			case BASSChannelType.BASS_CTYPE_STREAM_WV_LH:
				result = "Wavpack";
				break;
			case BASSChannelType.BASS_CTYPE_STREAM_CD:
				result = "CDA";
				break;
			case BASSChannelType.BASS_CTYPE_STREAM_WMA:
				result = "WMA";
				break;
			case BASSChannelType.BASS_CTYPE_STREAM_WMA_MP3:
				result = "MP3";
				break;
			case BASSChannelType.BASS_CTYPE_STREAM_FLAC:
				result = "FLAC";
				break;
			case BASSChannelType.BASS_CTYPE_STREAM_OFR:
				result = "Optimfrog";
				break;
			case BASSChannelType.BASS_CTYPE_STREAM_APE:
				result = "APE";
				break;
			case BASSChannelType.BASS_CTYPE_STREAM_MPC:
				result = "MPC";
				break;
			case BASSChannelType.BASS_CTYPE_STREAM_AAC:
				result = "AAC";
				break;
			case BASSChannelType.BASS_CTYPE_STREAM_MP4:
				result = "MP4";
				break;
			case BASSChannelType.BASS_CTYPE_STREAM_MF:
				result = "MF";
				break;
			case BASSChannelType.BASS_CTYPE_STREAM_AM:
				result = "AM";
				break;
			case BASSChannelType.BASS_CTYPE_STREAM_SPX:
				result = "Speex";
				break;
			case BASSChannelType.BASS_CTYPE_STREAM_OPUS:
				result = "OPUS";
				break;
			case BASSChannelType.BASS_CTYPE_STREAM_ALAC:
				result = "ALAC";
				break;
			case BASSChannelType.BASS_CTYPE_STREAM_TTA:
				result = "TTA";
				break;
			case BASSChannelType.BASS_CTYPE_STREAM_AC3:
				result = "AC3";
				break;
			case BASSChannelType.BASS_CTYPE_STREAM_ADX:
				result = "ADX";
				break;
			case BASSChannelType.BASS_CTYPE_STREAM_AIX:
				result = "AIX";
				break;
			case BASSChannelType.BASS_CTYPE_STREAM_VIDEO:
				result = "Video";
				break;
			case BASSChannelType.BASS_CTYPE_STREAM_MIDI:
				result = "MIDI";
				break;
			case BASSChannelType.BASS_CTYPE_STREAM_WINAMP:
				result = "Winamp";
				break;
			case BASSChannelType.BASS_CTYPE_STREAM_CA:
				result = "CoreAudio";
				break;
			case BASSChannelType.BASS_CTYPE_STREAM_DSD:
				result = "DSD";
				break;
			case BASSChannelType.BASS_CTYPE_UNKNOWN:
				result = "Unknown";
				break;
			}
			return result;
		}

		public static string BASSTagTypeToString(BASSTag tagType)
		{
			string text = "???";
			switch (tagType)
			{
			case BASSTag.BASS_TAG_ID3:
				return "ID3v1";
			case BASSTag.BASS_TAG_ID3V2:
			case BASSTag.BASS_TAG_LYRICS3:
				return "ID3v2";
			case BASSTag.BASS_TAG_WMA:
				return "WMA";
			case BASSTag.BASS_TAG_OGG:
			case BASSTag.BASS_TAG_VENDOR:
				return "OGG";
			case BASSTag.BASS_TAG_MP4:
				return "MP4";
			case BASSTag.BASS_TAG_MF:
				return "MF";
			case BASSTag.BASS_TAG_APE:
			case BASSTag.BASS_TAG_APE_BINARY:
				return "APE";
			case BASSTag.BASS_TAG_WAVEFORMAT:
				return "WAV";
			case BASSTag.BASS_TAG_RIFF_BEXT:
				return "BWF";
			case BASSTag.BASS_TAG_RIFF_CART:
				return "RIFF CART";
			case BASSTag.BASS_TAG_RIFF_DISP:
				return "RIFF DISP";
			case BASSTag.BASS_TAG_RIFF_INFO:
				return "RIFF";
			case BASSTag.BASS_TAG_ADX_LOOP:
				return "ADX";
			case BASSTag.BASS_TAG_DSD_ARTIST:
				return "DSD Artist";
			case BASSTag.BASS_TAG_DSD_TITLE:
				return "DSD Title";
			case BASSTag.BASS_TAG_DSD_COMMENT:
				return "DSD Comment";
			case BASSTag.BASS_TAG_MIDI_TRACK:
				return "MIDI";
			case BASSTag.BASS_TAG_MUSIC_NAME:
			case BASSTag.BASS_TAG_MUSIC_MESSAGE:
			case BASSTag.BASS_TAG_MUSIC_INST:
			case BASSTag.BASS_TAG_MUSIC_SAMPLE:
				return "MUSIC";
			case BASSTag.BASS_TAG_HTTP:
			case BASSTag.BASS_TAG_ICY:
			case BASSTag.BASS_TAG_META:
			case BASSTag.BASS_TAG_WMA_META:
				return "META";
			case BASSTag.BASS_TAG_HLS_EXTINF:
				return "HLS";
			default:
				return "Unknown";
			}
		}

		public static string BASSAddOnGetSupportedFileExtensions(string file)
		{
			return string.Empty;
		}

		public static string BASSAddOnGetSupportedFileName(string file)
		{
			string result = string.Empty;
			return result;
		}

		public static string BASSAddOnGetSupportedFileExtensions(Dictionary<int, string> plugins, bool includeBASS)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (includeBASS)
			{
				stringBuilder.Append(BASSAddOnGetSupportedFileExtensions(null));
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(";");
				}
				stringBuilder.Append(BASSAddOnGetSupportedFileExtensions("music"));
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(";");
				}
			}
			if (plugins != null)
			{
				foreach (string value in plugins.Values)
				{
					stringBuilder.Append(BASSAddOnGetSupportedFileExtensions(value));
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(";");
					}
				}
			}
			if (stringBuilder.Length > 0 && stringBuilder[stringBuilder.Length - 1] == ';')
			{
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
			}
			if (stringBuilder.Length > 0 && stringBuilder[0] == ';')
			{
				stringBuilder.Remove(0, 1);
			}
			return stringBuilder.ToString();
		}

		public static string BASSAddOnGetSupportedFileFilter(Dictionary<int, string> plugins, string allFormatName)
		{
			return BASSAddOnGetSupportedFileFilter(plugins, allFormatName, includeBASS: true);
		}

		public static string BASSAddOnGetSupportedFileFilter(Dictionary<int, string> plugins, string allFormatName, bool includeBASS)
		{
			string empty = string.Empty;
			string empty2 = string.Empty;
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			if (includeBASS)
			{
				empty = BASSAddOnGetSupportedFileName(null);
				empty2 = BASSAddOnGetSupportedFileExtensions(null);
				if (!string.IsNullOrEmpty(empty) && !string.IsNullOrEmpty(empty2))
				{
					stringBuilder.Append(empty + " (" + empty2 + ")|" + empty2);
				}
				if (!string.IsNullOrEmpty(empty2))
				{
					stringBuilder2.Append(empty2);
				}
				empty = BASSAddOnGetSupportedFileName("music");
				empty2 = BASSAddOnGetSupportedFileExtensions("music");
				if (!string.IsNullOrEmpty(empty) && !string.IsNullOrEmpty(empty2))
				{
					stringBuilder.Append("|" + empty + " (" + empty2 + ")|" + empty2);
				}
				if (!string.IsNullOrEmpty(empty2))
				{
					stringBuilder2.Append(";" + empty2);
				}
			}
			if (plugins != null)
			{
				foreach (string value in plugins.Values)
				{
					empty = BASSAddOnGetSupportedFileName(value);
					empty2 = BASSAddOnGetSupportedFileExtensions(value);
					if (!string.IsNullOrEmpty(empty) && !string.IsNullOrEmpty(empty2))
					{
						stringBuilder.Append("|" + empty + " (" + empty2 + ")|" + empty2);
					}
					if (!string.IsNullOrEmpty(empty2))
					{
						stringBuilder2.Append(";" + empty2);
					}
				}
			}
			if (!string.IsNullOrEmpty(allFormatName) && stringBuilder2.Length > 0)
			{
				stringBuilder.Insert(0, allFormatName + "|" + stringBuilder2.ToString() + "|");
			}
			if (stringBuilder.Length > 0 && stringBuilder[0] == '|')
			{
				stringBuilder.Remove(0, 1);
			}
			return stringBuilder.ToString();
		}

		public static string BASSAddOnGetSupportedFileFilter(Dictionary<int, string> plugins, string allFormatName, bool includeBASS, Dictionary<string, string> extra)
		{
			string empty = string.Empty;
			string empty2 = string.Empty;
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			if (includeBASS)
			{
				empty = BASSAddOnGetSupportedFileName(null);
				empty2 = BASSAddOnGetSupportedFileExtensions(null);
				if (!string.IsNullOrEmpty(empty) && !string.IsNullOrEmpty(empty2))
				{
					stringBuilder.Append(empty + " (" + empty2 + ")|" + empty2);
				}
				if (!string.IsNullOrEmpty(empty2))
				{
					stringBuilder2.Append(empty2);
				}
				empty = BASSAddOnGetSupportedFileName("music");
				empty2 = BASSAddOnGetSupportedFileExtensions("music");
				if (!string.IsNullOrEmpty(empty) && !string.IsNullOrEmpty(empty2))
				{
					stringBuilder.Append("|" + empty + " (" + empty2 + ")|" + empty2);
				}
				if (!string.IsNullOrEmpty(empty2))
				{
					stringBuilder2.Append(";" + empty2);
				}
			}
			if (plugins != null)
			{
				foreach (string value in plugins.Values)
				{
					empty = BASSAddOnGetSupportedFileName(value);
					empty2 = BASSAddOnGetSupportedFileExtensions(value);
					if (!string.IsNullOrEmpty(empty) && !string.IsNullOrEmpty(empty2))
					{
						stringBuilder.Append("|" + empty + " (" + empty2 + ")|" + empty2);
					}
					if (!string.IsNullOrEmpty(empty2))
					{
						stringBuilder2.Append(";" + empty2);
					}
				}
			}
			if (extra != null)
			{
				foreach (KeyValuePair<string, string> item in extra)
				{
					empty = item.Key;
					empty2 = item.Value;
					if (!string.IsNullOrEmpty(empty) && !string.IsNullOrEmpty(empty2))
					{
						stringBuilder.Append("|" + empty + " (" + empty2 + ")|" + empty2);
					}
					if (!string.IsNullOrEmpty(empty2))
					{
						stringBuilder2.Append(";" + empty2);
					}
				}
			}
			if (!string.IsNullOrEmpty(allFormatName) && stringBuilder2.Length > 0)
			{
				stringBuilder.Insert(0, allFormatName + "|" + stringBuilder2.ToString() + "|");
			}
			if (stringBuilder.Length > 0 && stringBuilder[0] == '|')
			{
				stringBuilder.Remove(0, 1);
			}
			return stringBuilder.ToString();
		}

		public static string BASSAddOnGetPluginFileFilter(Dictionary<int, string> plugins, string allFormatName)
		{
			return BASSAddOnGetPluginFileFilter(plugins, allFormatName, includeBASS: true);
		}

		public static string BASSAddOnGetPluginFileFilter(Dictionary<int, string> plugins, string allFormatName, bool includeBASS)
		{
			string empty = string.Empty;
			string empty2 = string.Empty;
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			if (includeBASS)
			{
				BASS_PLUGINFORM[] formats = Bass.BASS_PluginGetInfo(0).formats;
				foreach (BASS_PLUGINFORM obj in formats)
				{
					empty = obj.name;
					empty2 = obj.exts;
					if (!string.IsNullOrEmpty(empty) && !string.IsNullOrEmpty(empty2))
					{
						stringBuilder.Append("|" + empty + "|" + empty2);
					}
					if (!string.IsNullOrEmpty(empty2))
					{
						stringBuilder2.Append(";" + empty2);
					}
				}
			}
			if (plugins != null)
			{
				foreach (int key in plugins.Keys)
				{
					BASS_PLUGINFORM[] formats = Bass.BASS_PluginGetInfo(key).formats;
					foreach (BASS_PLUGINFORM obj2 in formats)
					{
						empty = obj2.name;
						empty2 = obj2.exts;
						if (!string.IsNullOrEmpty(empty) && !string.IsNullOrEmpty(empty2))
						{
							stringBuilder.Append("|" + empty + "|" + empty2);
						}
						if (!string.IsNullOrEmpty(empty2))
						{
							stringBuilder2.Append(";" + empty2);
						}
					}
				}
			}
			if (stringBuilder2.Length > 0 && stringBuilder2[0] == ';')
			{
				stringBuilder2.Remove(0, 1);
			}
			if (stringBuilder.Length > 0 && stringBuilder[0] == '|')
			{
				stringBuilder.Remove(0, 1);
			}
			if (!string.IsNullOrEmpty(allFormatName) && stringBuilder2.Length > 0)
			{
				stringBuilder.Insert(0, allFormatName + "|" + stringBuilder2.ToString() + "|");
			}
			if (stringBuilder.Length > 0 && stringBuilder[0] == '|')
			{
				stringBuilder.Remove(0, 1);
			}
			return stringBuilder.ToString();
		}

		public static bool BASSAddOnIsFileSupported(Dictionary<int, string> plugins, string filename)
		{
			if (filename == null || filename == string.Empty)
			{
				return false;
			}
			string fileExt = Path.GetExtension(filename).ToLower();
			_ = string.Empty;
			if (MatchExtensions(BASSAddOnGetSupportedFileExtensions(null).ToLower(), fileExt))
			{
				return true;
			}
			if (MatchExtensions(BASSAddOnGetSupportedFileExtensions("music").ToLower(), fileExt))
			{
				return true;
			}
			bool result = false;
			if (plugins != null)
			{
				foreach (string value in plugins.Values)
				{
					if (MatchExtensions(BASSAddOnGetSupportedFileExtensions(value).ToLower(), fileExt))
					{
						return true;
					}
				}
				return result;
			}
			return result;
		}

		private static bool MatchExtensions(string addonExts, string fileExt)
		{
			bool result = false;
			string[] array = addonExts.Split(';');
			if (array != null && !string.IsNullOrEmpty(fileExt))
			{
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					if (array2[i].EndsWith(fileExt))
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}

		public static short AbsSignMax(short val1, short val2)
		{
			if (val1 == short.MinValue)
			{
				return val1;
			}
			if (val2 == short.MinValue)
			{
				return val2;
			}
			if (Math.Abs(val1) < Math.Abs(val2))
			{
				return val2;
			}
			return val1;
		}

		public static float AbsSignMax(float val1, float val2)
		{
			if (Math.Abs(val1) < Math.Abs(val2))
			{
				return val2;
			}
			return val1;
		}

		public static double SampleDither(double sample, double factor, double max)
		{
			return sample += (_autoRandomizer.NextDouble() - _autoRandomizer.NextDouble()) * factor / max;
		}

		public static int GetLevel(short[] buffer, int chans, int startIndex, int length)
		{
			if (buffer == null)
			{
				return 0;
			}
			int num = buffer.Length;
			if (startIndex > num - 1 || startIndex < 0)
			{
				startIndex = 0;
			}
			if (length > num || length < 0)
			{
				length = num;
			}
			if (startIndex + length > num)
			{
				length = num - startIndex;
			}
			short num2 = 0;
			short num3 = 0;
			int num4 = 0;
			num = startIndex + length;
			for (int i = startIndex; i < num; i++)
			{
				num4 = Math.Abs((int)buffer[i]);
				if (num4 > 32767)
				{
					num4 = 32767;
				}
				if (i % 2 == 0)
				{
					if (num4 > num2)
					{
						num2 = (short)num4;
					}
				}
				else if (num4 > num3)
				{
					num3 = (short)num4;
				}
			}
			if (chans == 1)
			{
				num3 = (num2 = Math.Max(num2, num3));
			}
			return MakeLong(num2, num3);
		}

		public static long GetLevel2(short[] buffer, int chans, int startIndex, int length)
		{
			if (buffer == null)
			{
				return 0L;
			}
			int num = buffer.Length;
			if (startIndex > num - 1 || startIndex < 0)
			{
				startIndex = 0;
			}
			if (length > num || length < 0)
			{
				length = num;
			}
			if (startIndex + length > num)
			{
				length = num - startIndex;
			}
			short num2 = short.MinValue;
			short num3 = short.MinValue;
			short num4 = short.MaxValue;
			short num5 = short.MaxValue;
			short num6 = 0;
			num = startIndex + length;
			for (int i = startIndex; i < num; i++)
			{
				num6 = buffer[i];
				if (i % 2 == 0)
				{
					if (num6 > num2)
					{
						num2 = num6;
					}
					if (num6 < num4)
					{
						num4 = num6;
					}
				}
				else
				{
					if (num6 > num3)
					{
						num3 = num6;
					}
					if (num6 < num5)
					{
						num5 = num6;
					}
				}
			}
			if (chans == 1)
			{
				num3 = (num2 = Math.Max(num2, num3));
				num5 = (num4 = Math.Min(num4, num5));
			}
			return MakeLong64(MakeLong(num4, num2), MakeLong(num5, num3));
		}

		public static int GetLevel(float[] buffer, int chans, int startIndex, int length)
		{
			if (buffer == null)
			{
				return 0;
			}
			int num = buffer.Length;
			if (startIndex > num - 1 || startIndex < 0)
			{
				startIndex = 0;
			}
			if (length > num || length < 0)
			{
				length = num;
			}
			if (startIndex + length > num)
			{
				length = num - startIndex;
			}
			short num2 = 0;
			short num3 = 0;
			int num4 = 0;
			num = startIndex + length;
			for (int i = startIndex; i < num; i++)
			{
				num4 = (int)Math.Round(Math.Abs(buffer[i] * 32768f));
				if (num4 > 32767)
				{
					num4 = 32767;
				}
				if (i % 2 == 0)
				{
					if (num4 > num2)
					{
						num2 = (short)num4;
					}
				}
				else if (num4 > num3)
				{
					num3 = (short)num4;
				}
			}
			if (chans == 1)
			{
				num3 = (num2 = Math.Max(num2, num3));
			}
			return MakeLong(num2, num3);
		}

		public static long GetLevel2(float[] buffer, int chans, int startIndex, int length)
		{
			if (buffer == null)
			{
				return 0L;
			}
			int num = buffer.Length;
			if (startIndex > num - 1 || startIndex < 0)
			{
				startIndex = 0;
			}
			if (length > num || length < 0)
			{
				length = num;
			}
			if (startIndex + length > num)
			{
				length = num - startIndex;
			}
			short num2 = short.MinValue;
			short num3 = short.MinValue;
			short num4 = short.MaxValue;
			short num5 = short.MaxValue;
			int num6 = 0;
			num = startIndex + length;
			for (int i = startIndex; i < num; i++)
			{
				num6 = (int)Math.Round((double)buffer[i] * 32768.0);
				if (num6 > 32767)
				{
					num6 = 32767;
				}
				else if (num6 < -32768)
				{
					num6 = -32768;
				}
				if (i % 2 == 0)
				{
					if (num6 > num2)
					{
						num2 = (short)num6;
					}
					if (num6 < num4)
					{
						num4 = (short)num6;
					}
				}
				else
				{
					if (num6 > num3)
					{
						num3 = (short)num6;
					}
					if (num6 < num5)
					{
						num5 = (short)num6;
					}
				}
			}
			if (chans == 1)
			{
				num3 = (num2 = Math.Max(num2, num3));
				num5 = (num4 = Math.Min(num4, num5));
			}
			return MakeLong64(MakeLong(num4, num2), MakeLong(num5, num3));
		}

		public static int GetLevel(byte[] buffer, int chans, int startIndex, int length)
		{
			if (buffer == null)
			{
				return 0;
			}
			int num = buffer.Length;
			if (startIndex > num - 1 || startIndex < 0)
			{
				startIndex = 0;
			}
			if (length > num || length < 0)
			{
				length = num;
			}
			if (startIndex + length > num)
			{
				length = num - startIndex;
			}
			short num2 = 0;
			short num3 = 0;
			int num4 = 0;
			num = startIndex + length;
			for (int i = startIndex; i < num; i++)
			{
				num4 = Math.Abs((buffer[i] - 128) * 256);
				if (num4 > 32767)
				{
					num4 = 32767;
				}
				if (i % 2 == 0)
				{
					if (num4 > num2)
					{
						num2 = (short)num4;
					}
				}
				else if (num4 > num3)
				{
					num3 = (short)num4;
				}
			}
			if (chans == 1)
			{
				num3 = (num2 = Math.Max(num2, num3));
			}
			return MakeLong(num2, num3);
		}

		public static long GetLevel2(byte[] buffer, int chans, int startIndex, int length)
		{
			if (buffer == null)
			{
				return 0L;
			}
			int num = buffer.Length;
			if (startIndex > num - 1 || startIndex < 0)
			{
				startIndex = 0;
			}
			if (length > num || length < 0)
			{
				length = num;
			}
			if (startIndex + length > num)
			{
				length = num - startIndex;
			}
			short num2 = short.MinValue;
			short num3 = short.MinValue;
			short num4 = short.MaxValue;
			short num5 = short.MaxValue;
			short num6 = 0;
			num = startIndex + length;
			for (int i = startIndex; i < num; i++)
			{
				num6 = (short)((buffer[i] - 128) * 256);
				if (i % 2 == 0)
				{
					if (num6 > num2)
					{
						num2 = num6;
					}
					if (num6 < num4)
					{
						num4 = num6;
					}
				}
				else
				{
					if (num6 > num3)
					{
						num3 = num6;
					}
					if (num6 < num5)
					{
						num5 = num6;
					}
				}
			}
			if (chans == 1)
			{
				num3 = (num2 = Math.Max(num2, num3));
				num5 = (num4 = Math.Min(num4, num5));
			}
			return MakeLong64(MakeLong(num4, num2), MakeLong(num5, num3));
		}

		public unsafe static int GetLevel(IntPtr buffer, int chans, int bps, int startIndex, int length)
		{
			if (buffer == IntPtr.Zero)
			{
				return 0;
			}
			if (bps == 16 || bps == 32 || bps == 8)
			{
				bps /= 8;
			}
			if (startIndex < 0)
			{
				startIndex = 0;
			}
			short num = 0;
			short num2 = 0;
			int num3 = 0;
			int num4 = startIndex + length;
			switch (bps)
			{
			case 2:
			{
				short* ptr2 = (short*)(void*)buffer;
				for (int j = startIndex; j < num4; j++)
				{
					num3 = Math.Abs((int)ptr2[j]);
					if (num3 > 32767)
					{
						num3 = 32767;
					}
					if (j % 2 == 0)
					{
						if (num3 > num)
						{
							num = (short)num3;
						}
					}
					else if (num3 > num2)
					{
						num2 = (short)num3;
					}
				}
				break;
			}
			case 4:
			{
				float* ptr3 = (float*)(void*)buffer;
				for (int k = startIndex; k < num4; k++)
				{
					num3 = (int)Math.Round(Math.Abs(ptr3[k] * 32768f));
					if (num3 > 32767)
					{
						num3 = 32767;
					}
					if (k % 2 == 0)
					{
						if (num3 > num)
						{
							num = (short)num3;
						}
					}
					else if (num3 > num2)
					{
						num2 = (short)num3;
					}
				}
				break;
			}
			default:
			{
				byte* ptr = (byte*)(void*)buffer;
				for (int i = startIndex; i < num4; i++)
				{
					num3 = Math.Abs((ptr[i] - 128) * 256);
					if (num3 > 32767)
					{
						num3 = 32767;
					}
					if (i % 2 == 0)
					{
						if (num3 > num)
						{
							num = (short)num3;
						}
					}
					else if (num3 > num2)
					{
						num2 = (short)num3;
					}
				}
				break;
			}
			}
			if (chans == 1)
			{
				num2 = (num = Math.Max(num, num2));
			}
			return MakeLong(num, num2);
		}

		public unsafe static long GetLevel2(IntPtr buffer, int chans, int bps, int startIndex, int length)
		{
			if (buffer == IntPtr.Zero)
			{
				return 0L;
			}
			if (bps == 16 || bps == 32 || bps == 8)
			{
				bps /= 8;
			}
			if (startIndex < 0)
			{
				startIndex = 0;
			}
			short num = short.MinValue;
			short num2 = short.MinValue;
			short num3 = short.MaxValue;
			short num4 = short.MaxValue;
			int num5 = 0;
			int num6 = startIndex + length;
			switch (bps)
			{
			case 2:
			{
				short* ptr2 = (short*)(void*)buffer;
				for (int j = startIndex; j < num6; j++)
				{
					num5 = ptr2[j];
					if (j % 2 == 0)
					{
						if (num5 > num)
						{
							num = (short)num5;
						}
						if (num5 < num3)
						{
							num3 = (short)num5;
						}
					}
					else
					{
						if (num5 > num2)
						{
							num2 = (short)num5;
						}
						if (num5 < num4)
						{
							num4 = (short)num5;
						}
					}
				}
				break;
			}
			case 4:
			{
				float* ptr3 = (float*)(void*)buffer;
				for (int k = startIndex; k < num6; k++)
				{
					num5 = (int)Math.Round(ptr3[k] * 32768f);
					if (num5 > 32767)
					{
						num5 = 32767;
					}
					else if (num5 < -32768)
					{
						num5 = -32768;
					}
					if (k % 2 == 0)
					{
						if (num5 > num)
						{
							num = (short)num5;
						}
						if (num5 < num3)
						{
							num3 = (short)num5;
						}
					}
					else
					{
						if (num5 > num2)
						{
							num2 = (short)num5;
						}
						if (num5 < num4)
						{
							num4 = (short)num5;
						}
					}
				}
				break;
			}
			default:
			{
				byte* ptr = (byte*)(void*)buffer;
				for (int i = startIndex; i < num6; i++)
				{
					num5 = (ptr[i] - 128) * 256;
					if (i % 2 == 0)
					{
						if (num5 > num)
						{
							num = (short)num5;
						}
						if (num5 < num3)
						{
							num3 = (short)num5;
						}
					}
					else
					{
						if (num5 > num2)
						{
							num2 = (short)num5;
						}
						if (num5 < num4)
						{
							num4 = (short)num5;
						}
					}
				}
				break;
			}
			}
			if (chans == 1)
			{
				num2 = (num = Math.Max(num, num2));
				num4 = (num3 = Math.Min(num3, num4));
			}
			return MakeLong64(MakeLong(num3, num), MakeLong(num4, num2));
		}

		public static long DecodeAllData(int channel, bool autoFree)
		{
			long num = 0L;
			byte[] buffer = new byte[131072];
			while (Bass.BASS_ChannelIsActive(channel) == BASSActive.BASS_ACTIVE_PLAYING)
			{
				int num2 = Bass.BASS_ChannelGetData(channel, buffer, 131072);
				if (num2 < 0)
				{
					break;
				}
				num += num2;
			}
			if (autoFree)
			{
				Bass.BASS_StreamFree(channel);
			}
			return num;
		}

		public static bool DetectCuePoints(string filename, float blockSize, ref double cueInPos, ref double cueOutPos, double dBIn, double dBOut, int findZeroCrossing)
		{
			int num = Bass.BASS_StreamCreateFile(filename, 0L, 0L, BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_PRESCAN | BASSFlag.BASS_STREAM_DECODE);
			bool result = DetectCuePoints(num, blockSize, ref cueInPos, ref cueOutPos, dBIn, dBOut, findZeroCrossing);
			if (num != 0)
			{
				Bass.BASS_StreamFree(num);
			}
			return result;
		}

		public static bool DetectCuePoints(int decodingStream, float blockSize, ref double cueInPos, ref double cueOutPos, double dBIn, double dBOut, int findZeroCrossing)
		{
			if (decodingStream == 0)
			{
				return false;
			}
			long pos = Bass.BASS_ChannelGetPosition(decodingStream);
			BASS_CHANNELINFO bASS_CHANNELINFO = new BASS_CHANNELINFO();
			if (!Bass.BASS_ChannelGetInfo(decodingStream, bASS_CHANNELINFO) || !bASS_CHANNELINFO.IsDecodingChannel)
			{
				return false;
			}
			if (dBIn > 0.0)
			{
				dBIn = 0.0;
			}
			else if (dBIn < -90.0)
			{
				dBIn = -90.0;
			}
			if (dBOut > 0.0)
			{
				dBOut = 0.0;
			}
			else if (dBOut < -90.0)
			{
				dBOut = -90.0;
			}
			if (blockSize > 30f)
			{
				blockSize = 30f;
			}
			else if (blockSize < 0.1f)
			{
				blockSize = 0.1f;
			}
			float num = (float)DBToLevel(dBIn, 1.0);
			float num2 = (float)DBToLevel(dBOut, 1.0);
			long num3 = Bass.BASS_ChannelGetLength(decodingStream);
			long num4 = 0L;
			long num5 = num3;
			int num6 = (int)Bass.BASS_ChannelSeconds2Bytes(decodingStream, blockSize);
			float[] buffer = new float[num6 / 4];
			int num7 = 0;
			int num8 = 0;
			long num9 = 0L;
			bool flag = false;
			while (!flag && num9 < num3)
			{
				num8 = Bass.BASS_ChannelGetData(decodingStream, buffer, num6);
				num4 = num9;
				num7 = 0;
				while (!flag && num7 < num8)
				{
					if (ScanSampleLevel(buffer, num7 / 4, bASS_CHANNELINFO.chans) < num)
					{
						num7 += 4 * bASS_CHANNELINFO.chans;
						continue;
					}
					flag = true;
					num4 = num9 + num7;
				}
				if (!flag)
				{
					num9 += num8;
					if (num8 <= 0)
					{
						num9 = num3;
						num4 = num9;
					}
				}
			}
			if (flag && num4 < num3)
			{
				if (findZeroCrossing == 1)
				{
					while (num7 > 0 && !IsZeroCrossingPos(buffer, num7 / 4, num7 / 4 - bASS_CHANNELINFO.chans, bASS_CHANNELINFO.chans))
					{
						num7 -= 4 * bASS_CHANNELINFO.chans;
						num4 -= 4 * bASS_CHANNELINFO.chans;
					}
					if (num4 < 0)
					{
						num4 = 0L;
					}
				}
				else if (findZeroCrossing == 2)
				{
					while (num7 > 0 && ScanSampleLevel(buffer, num7 / 4, bASS_CHANNELINFO.chans) > num / 2f)
					{
						num7 -= 4 * bASS_CHANNELINFO.chans;
						num4 -= 4 * bASS_CHANNELINFO.chans;
					}
					if (num4 < 0)
					{
						num4 = 0L;
					}
				}
			}
			else
			{
				num4 = 0L;
			}
			num8 = 0;
			num9 = num3;
			flag = false;
			while (!flag && num9 > 0)
			{
				Bass.BASS_ChannelSetPosition(decodingStream, (num9 - num6 >= 0) ? (num9 - num6) : 0);
				num8 = Bass.BASS_ChannelGetData(decodingStream, buffer, num6);
				num5 = num9;
				num7 = num8;
				while (!flag && num7 > 0)
				{
					if (ScanSampleLevel(buffer, num7 / 4 - bASS_CHANNELINFO.chans, bASS_CHANNELINFO.chans) < num2)
					{
						num7 -= 4 * bASS_CHANNELINFO.chans;
						continue;
					}
					flag = true;
					num5 = num9 - num8 + num7;
				}
				if (!flag)
				{
					num9 -= num8;
					if (num8 <= 0)
					{
						num9 = 0L;
						num5 = num3;
					}
				}
			}
			if (flag && num5 > 0)
			{
				if (findZeroCrossing == 1)
				{
					while (num7 < num8 && !IsZeroCrossingPos(buffer, num7 / 4, num7 / 4 + bASS_CHANNELINFO.chans, bASS_CHANNELINFO.chans))
					{
						num7 += 4 * bASS_CHANNELINFO.chans;
						num5 += 4 * bASS_CHANNELINFO.chans;
					}
					if (num5 > num3)
					{
						num5 = num3;
					}
				}
				else if (findZeroCrossing == 2)
				{
					while (num7 < num8 && ScanSampleLevel(buffer, num7 / 4, bASS_CHANNELINFO.chans) > num2 / 2f)
					{
						num7 += 4 * bASS_CHANNELINFO.chans;
						num5 += 4 * bASS_CHANNELINFO.chans;
					}
				}
			}
			else
			{
				num5 = num3;
			}
			cueInPos = Bass.BASS_ChannelBytes2Seconds(decodingStream, num4);
			cueOutPos = Bass.BASS_ChannelBytes2Seconds(decodingStream, num5);
			Bass.BASS_ChannelSetPosition(decodingStream, pos);
			return true;
		}

		public static double DetectNextLevel(int decodingStream, float blockSize, double startpos, double dB, bool reverse, bool findZeroCrossing)
		{
			if (decodingStream == 0)
			{
				return startpos;
			}
			BASS_CHANNELINFO bASS_CHANNELINFO = new BASS_CHANNELINFO();
			if (!Bass.BASS_ChannelGetInfo(decodingStream, bASS_CHANNELINFO) || !bASS_CHANNELINFO.IsDecodingChannel)
			{
				return startpos;
			}
			if (dB > 0.0)
			{
				dB = 0.0;
			}
			else if (dB < -90.0)
			{
				dB = -90.0;
			}
			if (blockSize > 30f)
			{
				blockSize = 30f;
			}
			else if (blockSize < 0.1f)
			{
				blockSize = 0.1f;
			}
			float num = (float)DBToLevel(dB, 1.0);
			long num2 = Bass.BASS_ChannelGetLength(decodingStream);
			int num3 = (int)Bass.BASS_ChannelSeconds2Bytes(decodingStream, blockSize);
			float[] buffer = new float[num3 / 4];
			int num4 = 0;
			int num5 = 0;
			long num6 = Bass.BASS_ChannelSeconds2Bytes(decodingStream, startpos);
			long num7 = num6;
			bool flag = false;
			if (reverse)
			{
				while (!flag && num6 > 0)
				{
					Bass.BASS_ChannelSetPosition(decodingStream, (num6 - num3 >= 0) ? (num6 - num3) : 0);
					num5 = Bass.BASS_ChannelGetData(decodingStream, buffer, num3 | 0x40000000);
					num7 = num6;
					num4 = num5;
					while (!flag && num4 > 0)
					{
						if (ScanSampleLevel(buffer, num4 / 4 - bASS_CHANNELINFO.chans, bASS_CHANNELINFO.chans) < num)
						{
							num4 -= 4 * bASS_CHANNELINFO.chans;
							continue;
						}
						flag = true;
						num7 = num6 - num5 + num4;
					}
					if (!flag)
					{
						num6 -= num5;
						if (num5 <= 0)
						{
							num6 = 0L;
							num7 = num6;
						}
					}
				}
				if (flag && num7 > 0)
				{
					if (findZeroCrossing)
					{
						while (num4 < num5 && !IsZeroCrossingPos(buffer, num4 / 4, num4 / 4 + bASS_CHANNELINFO.chans, bASS_CHANNELINFO.chans))
						{
							num4 += 4 * bASS_CHANNELINFO.chans;
							num7 += 4 * bASS_CHANNELINFO.chans;
						}
					}
				}
				else
				{
					num7 = Bass.BASS_ChannelSeconds2Bytes(decodingStream, startpos);
				}
			}
			else
			{
				while (!flag && num6 < num2)
				{
					num5 = Bass.BASS_ChannelGetData(decodingStream, buffer, num3 | 0x40000000);
					num7 = num6;
					num4 = 0;
					while (!flag && num4 < num5)
					{
						if (ScanSampleLevel(buffer, num4 / 4, bASS_CHANNELINFO.chans) < num)
						{
							num4 += 4 * bASS_CHANNELINFO.chans;
							continue;
						}
						flag = true;
						num7 = num6 + num4;
					}
					if (!flag)
					{
						num6 += num5;
						if (num5 <= 0)
						{
							num6 = num2;
							num7 = num6;
						}
					}
				}
				if (flag && num7 < num2)
				{
					if (findZeroCrossing)
					{
						while (num4 > 0 && !IsZeroCrossingPos(buffer, num4 / 4, num4 / 4 - bASS_CHANNELINFO.chans, bASS_CHANNELINFO.chans))
						{
							num4 -= 4 * bASS_CHANNELINFO.chans;
							startpos -= (double)(4 * bASS_CHANNELINFO.chans);
						}
						if (startpos < 0.0)
						{
							startpos = 0.0;
						}
					}
				}
				else
				{
					num7 = Bass.BASS_ChannelSeconds2Bytes(decodingStream, startpos);
				}
			}
			return Bass.BASS_ChannelBytes2Seconds(decodingStream, num7);
		}

		private static float ScanSampleLevel(float[] buffer, int idx, int chans)
		{
			float num = 0f;
			float num2 = 0f;
			for (int i = 0; i < chans; i++)
			{
				num2 = ((idx + i >= buffer.Length) ? 0f : Math.Abs(buffer[idx + i]));
				if (num2 > num)
				{
					num = num2;
				}
			}
			return num;
		}

		private static bool IsZeroCrossingPos(float[] buffer, int idx1, int idx2, int chans)
		{
			bool result = false;
			try
			{
				if (chans > 1)
				{
					float num = buffer[idx1];
					float num2 = buffer[idx1 + 1];
					float num3 = buffer[idx2];
					float num4 = buffer[idx2 + 1];
					if ((!(num >= 0f) || !(num3 <= 0f)) && (!(num2 >= 0f) || !(num4 <= 0f)) && (!(num < 0f) || !(num3 > 0f)))
					{
						if (!(num2 < 0f))
						{
							return result;
						}
						if (!(num4 > 0f))
						{
							return result;
						}
					}
					result = true;
					return result;
				}
				float num5 = buffer[idx1];
				float num6 = buffer[idx2];
				if (!(num5 >= 0f) || !(num6 <= 0f))
				{
					if (!(num5 >= 0f))
					{
						return result;
					}
					if (!(num6 <= 0f))
					{
						return result;
					}
				}
				result = true;
				return result;
			}
			catch
			{
				return result;
			}
		}

		public static float GetNormalizationGain(string filename, float blockSize, double startpos, double endpos, ref float peak)
		{
			int num = Bass.BASS_StreamCreateFile(filename, 0L, 0L, BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_DECODE);
			if (num == 0)
			{
				return -1f;
			}
			float result = 1f;
			long num2 = 0L;
			long num3 = Bass.BASS_ChannelGetLength(num);
			if (startpos < 0.0)
			{
				startpos = 0.0;
			}
			if (endpos < 0.0)
			{
				num3 = long.MaxValue;
			}
			if (endpos > startpos && endpos <= Bass.BASS_ChannelBytes2Seconds(num, num3))
			{
				num2 = Bass.BASS_ChannelSeconds2Bytes(num, startpos);
				num3 = Bass.BASS_ChannelSeconds2Bytes(num, endpos);
			}
			if (num2 > 0)
			{
				Bass.BASS_ChannelSetPosition(num, num2);
			}
			int num4 = (int)Bass.BASS_ChannelSeconds2Bytes(num, blockSize);
			float[] array = new float[num4 / 4];
			float num5 = 0f;
			float num6 = 0f;
			while (num2 < num3)
			{
				int num7 = Bass.BASS_ChannelGetData(num, array, num4 | 0x40000000);
				if (num7 < 0)
				{
					num2 = num3;
				}
				else
				{
					num2 += num7;
					num7 /= 4;
					for (int i = 0; i < num7; i++)
					{
						num5 = Math.Abs(array[i]);
						if (num5 > num6)
						{
							num6 = num5;
						}
						if (num6 >= 1f)
						{
							break;
						}
					}
				}
				if (num6 >= 1f)
				{
					break;
				}
			}
			if (num6 < 1f && num6 > 0f)
			{
				result = 1f / num6;
			}
			peak = num6;
			if (num != 0)
			{
				Bass.BASS_StreamFree(num);
			}
			return result;
		}

		[DllImport("kernel32.dll", EntryPoint = "CopyMemory")]
		private static extern void DMACopyMemory(IntPtr destination, IntPtr source, IntPtr length);

		public static void DMACopyMemory(IntPtr destination, IntPtr source, long length)
		{
			DMACopyMemory(destination, source, new IntPtr(length));
		}

		[DllImport("kernel32.dll", EntryPoint = "MoveMemory")]
		private static extern void DMAMoveMemory(IntPtr destination, IntPtr source, IntPtr length);

		public static void DMAMoveMemory(IntPtr destination, IntPtr source, long length)
		{
			DMAMoveMemory(destination, source, new IntPtr(length));
		}

		[DllImport("kernel32.dll", EntryPoint = "FillMemory")]
		private static extern void DMAFillMemory(IntPtr destination, IntPtr length, byte fill);

		public static void DMAFillMemory(IntPtr destination, long length, byte fill)
		{
			DMAFillMemory(destination, new IntPtr(length), fill);
		}

		[DllImport("kernel32.dll", EntryPoint = "ZeroMemory")]
		private static extern void DMAZeroMemory(IntPtr destination, IntPtr length);

		public static void DMAZeroMemory(IntPtr destination, long length)
		{
			DMAZeroMemory(destination, new IntPtr(length));
		}

		[DllImport("kernel32.dll", EntryPoint = "LoadLibrary")]
		public static extern int LIBLoadLibrary(string fileName);

		[DllImport("kernel32.dll", EntryPoint = "FreeLibrary")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool LIBFreeLibrary(int hModule);

		internal static bool LoadLib(string moduleName, ref int handle)
		{
			if (handle == 0)
			{
				handle = LIBLoadLibrary(moduleName);
			}
			return handle != 0;
		}

		internal static bool FreeLib(ref int handle)
		{
			if (handle != 0)
			{
				return LIBFreeLibrary(handle);
			}
			return true;
		}
	}
}
