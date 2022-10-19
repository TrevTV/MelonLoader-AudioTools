using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;

namespace Un4seen.Bass
{
	[SuppressUnmanagedCodeSecurity]
	public sealed class Bass
	{
		private static object _mutex;

		private static bool _usesMF;

		private static bool _usesCA;

		public static string SupportedStreamExtensions;

		public static string SupportedStreamName;

		public static string SupportedMusicExtensions;

		internal static bool _configUTF8;

		public const int BASSVERSION = 516;

		public const int FALSE = 0;

		public const int TRUE = 1;

		public const int ERROR = -1;

		private static int _myModuleHandle;

		private const string _myModuleName = "bass";

		public static bool UsesMediaFoundation => _usesMF;

		public static bool UsesCoreAudio => _usesCA;

		static Bass()
		{
			_mutex = new object();
			_usesMF = false;
			_usesCA = false;
			SupportedStreamExtensions = "*.mp3;*.ogg;*.wav;*.mp2;*.mp1;*.aiff;*.m2a;*.mpa;*.m1a;*.mpg;*.mpeg;*.aif;*.mp3pro;*.bwf;*.mus";
			SupportedStreamName = "WAV/AIFF/MP3/MP2/MP1/OGG";
			SupportedMusicExtensions = "*.mod;*.mo3;*.s3m;*.xm;*.it;*.mtm;*.umx;*.mdz;*.s3z;*.itz;*.xmz";
			_configUTF8 = false;
			_myModuleHandle = 0;
			string text = string.Empty;
			try
			{
				text = new StackFrame(1).GetMethod().Name;
			}
			catch
			{
			}
			if (!text.Equals("LoadMe"))
			{
				InitBass();
			}
		}

		private static void InitBass()
		{
			if (!BassNet.OmitCheckVersion)
			{
				CheckVersion();
			}
			_configUTF8 = (BASS_SetConfig(BASSConfig.BASS_CONFIG_UNICODE, 1) || Environment.OSVersion.Platform > PlatformID.WinCE);
			try
			{
				if (BASS_GetVersion() >= 33818624 && Environment.OSVersion.Platform == PlatformID.MacOSX)
				{
					_usesCA = true;
					SupportedStreamExtensions += ";*.aac;*.adts;*.mp4;*.m4a;*.m4b;*.m4p;*.ac3;*.caf;*.mov";
					SupportedStreamName += "/AAC/MP4/AC3/CAF";
				}
			}
			catch
			{
			}
			try
			{
				if (BASS_GetVersion() >= 33818624 && Environment.OSVersion.Platform <= PlatformID.WinCE && File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "mfreadwrite.dll")))
				{
					_usesMF = true;
					SupportedStreamExtensions += ";*.wma;*.wmv;*.aac;*.adts;*.mp4;*.m4a;*.m4b;*.m4p";
					SupportedStreamName += "/WMA/AAC/MP4";
				}
				else if (BASS_GetVersion() >= 33818624 && Environment.OSVersion.Platform == PlatformID.MacOSX)
				{
					_usesCA = true;
					SupportedStreamExtensions += ";*.aac;*.adts;*.mp4;*.m4a;*.m4b;*.m4p;*.ac3;*.caf;*.mov";
					SupportedStreamName += "/AAC/MP4/AC3/CAF";
				}
			}
			catch
			{
			}
		}

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool BASS_Init(int device, int freq, BASSInit flags, IntPtr win, IntPtr clsid);

		public static bool BASS_Init(int device, int freq, BASSInit flags, IntPtr win)
		{
			return BASS_Init(device, freq, flags, win, IntPtr.Zero);
		}

		[DllImport("bass", EntryPoint = "BASS_Init")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool BASS_InitGuid(int device, int freq, BASSInit flags, IntPtr win, [MarshalAs(UnmanagedType.LPStruct)] Guid clsid);

		public static bool BASS_Init(int device, int freq, BASSInit flags, IntPtr win, Guid clsid)
		{
			if (clsid == Guid.Empty)
			{
				return BASS_Init(device, freq, flags, win, IntPtr.Zero);
			}
			return BASS_InitGuid(device, freq, flags, win, clsid);
		}

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_IsStarted();

		[DllImport("bass", EntryPoint = "BASS_GetDeviceInfo")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool BASS_GetDeviceInfoInternal([In] int device, [In] [Out] ref BASS_DEVICEINFO_INTERNAL info);

		public unsafe static bool BASS_GetDeviceInfo(int device, BASS_DEVICEINFO info)
		{
			bool flag = BASS_GetDeviceInfoInternal(device, ref info._internal);
			if (flag)
			{
				if (_configUTF8)
				{
					info.name = Utils.IntPtrAsStringUtf8(info._internal.name, out int len);
					info.driver = Utils.IntPtrAsStringUtf8(info._internal.driver, out len);
					if (len > 0 && BASS_GetVersion() > 33818624 && Environment.OSVersion.Platform < PlatformID.WinCE)
					{
						try
						{
							info.id = Utils.IntPtrAsStringUtf8(new IntPtr((byte*)info._internal.driver.ToPointer() + len + 1), out len);
						}
						catch
						{
						}
					}
				}
				else
				{
					info.name = Utils.IntPtrAsStringAnsi(info._internal.name);
					info.driver = Utils.IntPtrAsStringAnsi(info._internal.driver);
					if (!string.IsNullOrEmpty(info.driver) && BASS_GetVersion() > 33818624 && Environment.OSVersion.Platform < PlatformID.WinCE)
					{
						try
						{
							info.id = Utils.IntPtrAsStringAnsi(new IntPtr((byte*)info._internal.driver.ToPointer() + info.driver.Length + 1));
						}
						catch
						{
						}
					}
				}
				info.flags = info._internal.flags;
			}
			return flag;
		}

		public static BASS_DEVICEINFO BASS_GetDeviceInfo(int device)
		{
			BASS_DEVICEINFO bASS_DEVICEINFO = new BASS_DEVICEINFO();
			if (BASS_GetDeviceInfo(device, bASS_DEVICEINFO))
			{
				return bASS_DEVICEINFO;
			}
			return null;
		}

		public static BASS_DEVICEINFO[] BASS_GetDeviceInfos()
		{
			List<BASS_DEVICEINFO> list = new List<BASS_DEVICEINFO>();
			int num = 0;
			BASS_DEVICEINFO item;
			while ((item = BASS_GetDeviceInfo(num)) != null)
			{
				list.Add(item);
				num++;
			}
			BASS_GetCPU();
			return list.ToArray();
		}

		public static int BASS_GetDeviceCount()
		{
			BASS_DEVICEINFO info = new BASS_DEVICEINFO();
			int i;
			for (i = 0; BASS_GetDeviceInfo(i, info); i++)
			{
			}
			BASS_GetCPU();
			return i;
		}

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_GetInfo([In] [Out] BASS_INFO info);

		public static BASS_INFO BASS_GetInfo()
		{
			BASS_INFO bASS_INFO = new BASS_INFO();
			if (BASS_GetInfo(bASS_INFO))
			{
				return bASS_INFO;
			}
			return null;
		}

		[DllImport("bass")]
		public static extern BASSError BASS_ErrorGetCode();

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_Stop();

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_Free();

		[DllImport("bass")]
		public static extern int BASS_GetVersion();

		public static Version BASS_GetVersion(int fieldcount)
		{
			if (fieldcount < 1)
			{
				fieldcount = 1;
			}
			if (fieldcount > 4)
			{
				fieldcount = 4;
			}
			int num = BASS_GetVersion();
			Version result = new Version(2, 3);
			switch (fieldcount)
			{
			case 1:
				result = new Version((num >> 24) & 0xFF, 0);
				break;
			case 2:
				result = new Version((num >> 24) & 0xFF, (num >> 16) & 0xFF);
				break;
			case 3:
				result = new Version((num >> 24) & 0xFF, (num >> 16) & 0xFF, (num >> 8) & 0xFF);
				break;
			case 4:
				result = new Version((num >> 24) & 0xFF, (num >> 16) & 0xFF, (num >> 8) & 0xFF, num & 0xFF);
				break;
			}
			return result;
		}

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_SetDevice(int device);

		[DllImport("bass")]
		public static extern int BASS_GetDevice();

		[DllImport("bass")]
		public static extern IntPtr BASS_GetDSoundObject(int handle);

		[DllImport("bass")]
		public static extern IntPtr BASS_GetDSoundObject(BASSDirectSound dsobject);

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_Update(int length);

		[DllImport("bass")]
		public static extern float BASS_GetCPU();

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_Start();

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_Pause();

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_SetVolume(float volume);

		[DllImport("bass")]
		public static extern float BASS_GetVolume();

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_SetConfig(BASSConfig option, int newvalue);

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_SetConfig(BASSConfig option, [In] [MarshalAs(UnmanagedType.Bool)] bool newvalue);

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_SetConfigPtr(BASSConfig option, IntPtr newvalue);

		[DllImport("bass", EntryPoint = "BASS_SetConfigPtr")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool BASS_SetConfigStringUnicode(BASSConfig option, [In] [MarshalAs(UnmanagedType.LPWStr)] string newvalue);

		public static bool BASS_SetConfigString(BASSConfig option, string newvalue)
		{
			return BASS_SetConfigStringUnicode(option | (BASSConfig)(-2147483648), newvalue);
		}

		[DllImport("bass")]
		public static extern int BASS_GetConfig(BASSConfig option);

		[DllImport("bass", EntryPoint = "BASS_GetConfig")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_GetConfigBool(BASSConfig option);

		[DllImport("bass")]
		public static extern IntPtr BASS_GetConfigPtr(BASSConfig option);

		public static string BASS_GetConfigString(BASSConfig option)
		{
			return Utils.IntPtrAsStringUnicode(BASS_GetConfigPtr(option | (BASSConfig)(-2147483648)));
		}

		[DllImport("bass", EntryPoint = "BASS_PluginLoad")]
		private static extern int BASS_PluginLoadUnicode([In] [MarshalAs(UnmanagedType.LPWStr)] string file, BASSFlag flags);

		public static int BASS_PluginLoad(string file)
		{
			return BASS_PluginLoadUnicode(file, BASSFlag.BASS_UNICODE);
		}

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_PluginFree(int handle);

		public static Dictionary<int, string> BASS_PluginLoadDirectory(string dir)
		{
			return BASS_PluginLoadDirectory(dir, null);
		}

		public static Dictionary<int, string> BASS_PluginLoadDirectory(string dir, List<string> exclude)
		{
			Dictionary<int, string> dictionary = new Dictionary<int, string>();
			string[] files = Directory.GetFiles(dir, "bass*.dll");
			if (files == null || files.Length == 0)
			{
				files = Directory.GetFiles(dir, "libbass*.so");
			}
			if (files == null || files.Length == 0)
			{
				files = Directory.GetFiles(dir, "libbass*.dylib");
			}
			if (files != null)
			{
				string[] array = files;
				foreach (string text in array)
				{
					if (exclude != null)
					{
						string fn = Path.GetFileNameWithoutExtension(text);
						if (exclude.Find((string f) => f.ToLower().Contains(fn.ToLower())) != null)
						{
							continue;
						}
					}
					int num = BASS_PluginLoad(text);
					if (num != 0)
					{
						dictionary.Add(num, text);
					}
				}
			}
			BASS_GetCPU();
			if (dictionary.Count > 0)
			{
				return dictionary;
			}
			return null;
		}

		[DllImport("bass", EntryPoint = "BASS_PluginGetInfo")]
		private static extern IntPtr BASS_PluginGetInfoPtr(int handle);

		public static BASS_PLUGININFO BASS_PluginGetInfo(int handle)
		{
			if (handle != 0)
			{
				IntPtr intPtr = BASS_PluginGetInfoPtr(handle);
				if (intPtr != IntPtr.Zero)
				{
					bass_plugininfo bass_plugininfo = (bass_plugininfo)Marshal.PtrToStructure(intPtr, typeof(bass_plugininfo));
					return new BASS_PLUGININFO(bass_plugininfo.version, bass_plugininfo.formatc, bass_plugininfo.formats);
				}
				return null;
			}
			if (_usesMF)
			{
				BASS_PLUGINFORM[] formats = new BASS_PLUGINFORM[16]
				{
					new BASS_PLUGINFORM("WAVE Audio", "*.wav", BASSChannelType.BASS_CTYPE_STREAM_WAV),
					new BASS_PLUGINFORM("Ogg Vorbis", "*.ogg", BASSChannelType.BASS_CTYPE_STREAM_OGG),
					new BASS_PLUGINFORM("MPEG Layer 1", "*.mp1;*.m1a", BASSChannelType.BASS_CTYPE_STREAM_MP1),
					new BASS_PLUGINFORM("MPEG Layer 2", "*.mp2;*.m2a;*.mpa;*.mus", BASSChannelType.BASS_CTYPE_STREAM_MP2),
					new BASS_PLUGINFORM("MPEG Layer 3", "*.mp3;*.mpg;*.mpeg;*.mp3pro", BASSChannelType.BASS_CTYPE_STREAM_MP3),
					new BASS_PLUGINFORM("Audio IFF", "*.aif;*.aiff", BASSChannelType.BASS_CTYPE_STREAM_AIFF),
					new BASS_PLUGINFORM("Broadcast Wave", "*.bwf", BASSChannelType.BASS_CTYPE_STREAM_WAV),
					new BASS_PLUGINFORM("Windows Media Audio", "*.wma;*.wmv", BASSChannelType.BASS_CTYPE_STREAM_WMA),
					new BASS_PLUGINFORM("Advanced Audio Codec", "*.aac;*.adts", BASSChannelType.BASS_CTYPE_STREAM_AAC),
					new BASS_PLUGINFORM("MPEG 4 Audio", "*.mp4;*.m4a;*.m4b;*.m4p", BASSChannelType.BASS_CTYPE_STREAM_MP4),
					new BASS_PLUGINFORM("MOD Music", "*.mod;*.mdz", BASSChannelType.BASS_CTYPE_MUSIC_MOD),
					new BASS_PLUGINFORM("MO3 Music", "*.mo3", BASSChannelType.BASS_CTYPE_MUSIC_MO3),
					new BASS_PLUGINFORM("S3M Music", "*.s3m;*.s3z", BASSChannelType.BASS_CTYPE_MUSIC_S3M),
					new BASS_PLUGINFORM("XM Music", "*.xm;*.xmz", BASSChannelType.BASS_CTYPE_MUSIC_XM),
					new BASS_PLUGINFORM("IT Music", "*.it;*.itz;*.umx", BASSChannelType.BASS_CTYPE_MUSIC_IT),
					new BASS_PLUGINFORM("MTM Music", "*.mtm", BASSChannelType.BASS_CTYPE_MUSIC_MTM)
				};
				return new BASS_PLUGININFO(BASS_GetVersion(), formats);
			}
			if (_usesCA)
			{
				BASS_PLUGINFORM[] formats2 = new BASS_PLUGINFORM[18]
				{
					new BASS_PLUGINFORM("WAVE Audio", "*.wav", BASSChannelType.BASS_CTYPE_STREAM_WAV),
					new BASS_PLUGINFORM("Ogg Vorbis", "*.ogg", BASSChannelType.BASS_CTYPE_STREAM_OGG),
					new BASS_PLUGINFORM("MPEG Layer 1", "*.mp1;*.m1a", BASSChannelType.BASS_CTYPE_STREAM_MP1),
					new BASS_PLUGINFORM("MPEG Layer 2", "*.mp2;*.m2a;*.mpa;*.mus", BASSChannelType.BASS_CTYPE_STREAM_MP2),
					new BASS_PLUGINFORM("MPEG Layer 3", "*.mp3;*.mpg;*.mpeg;*.mp3pro", BASSChannelType.BASS_CTYPE_STREAM_MP3),
					new BASS_PLUGINFORM("Audio IFF", "*.aif;*.aiff", BASSChannelType.BASS_CTYPE_STREAM_AIFF),
					new BASS_PLUGINFORM("Broadcast Wave", "*.bwf", BASSChannelType.BASS_CTYPE_STREAM_WAV),
					new BASS_PLUGINFORM("Advanced Audio Codec", "*.aac;*.adts", BASSChannelType.BASS_CTYPE_STREAM_AAC),
					new BASS_PLUGINFORM("MPEG 4 Audio", "*.mp4;*.m4a;*.m4b;*.m4p", BASSChannelType.BASS_CTYPE_STREAM_MP4),
					new BASS_PLUGINFORM("AC-3 Dolby Digital", "*.ac3", BASSChannelType.BASS_CTYPE_STREAM_AC3),
					new BASS_PLUGINFORM("Apple Lossless Audio", "*.mp4;*.m4a;*.m4b;*.m4p;*.mov", BASSChannelType.BASS_CTYPE_STREAM_ALAC),
					new BASS_PLUGINFORM("Apple Core Audio", "*.caf", BASSChannelType.BASS_CTYPE_STREAM_CA),
					new BASS_PLUGINFORM("MOD Music", "*.mod;*.mdz", BASSChannelType.BASS_CTYPE_MUSIC_MOD),
					new BASS_PLUGINFORM("MO3 Music", "*.mo3", BASSChannelType.BASS_CTYPE_MUSIC_MO3),
					new BASS_PLUGINFORM("S3M Music", "*.s3m;*.s3z", BASSChannelType.BASS_CTYPE_MUSIC_S3M),
					new BASS_PLUGINFORM("XM Music", "*.xm;*.xmz", BASSChannelType.BASS_CTYPE_MUSIC_XM),
					new BASS_PLUGINFORM("IT Music", "*.it;*.itz;*.umx", BASSChannelType.BASS_CTYPE_MUSIC_IT),
					new BASS_PLUGINFORM("MTM Music", "*.mtm", BASSChannelType.BASS_CTYPE_MUSIC_MTM)
				};
				return new BASS_PLUGININFO(BASS_GetVersion(), formats2);
			}
			BASS_PLUGINFORM[] formats3 = new BASS_PLUGINFORM[13]
			{
				new BASS_PLUGINFORM("WAVE Audio", "*.wav", BASSChannelType.BASS_CTYPE_STREAM_WAV),
				new BASS_PLUGINFORM("Ogg Vorbis", "*.ogg", BASSChannelType.BASS_CTYPE_STREAM_OGG),
				new BASS_PLUGINFORM("MPEG Layer 1", "*.mp1;*.m1a", BASSChannelType.BASS_CTYPE_STREAM_MP1),
				new BASS_PLUGINFORM("MPEG Layer 2", "*.mp2;*.m2a;*.mpa;*.mus", BASSChannelType.BASS_CTYPE_STREAM_MP2),
				new BASS_PLUGINFORM("MPEG Layer 3", "*.mp3;*.mpg;*.mpeg;*.mp3pro", BASSChannelType.BASS_CTYPE_STREAM_MP3),
				new BASS_PLUGINFORM("Audio IFF", "*.aif;*.aiff", BASSChannelType.BASS_CTYPE_STREAM_AIFF),
				new BASS_PLUGINFORM("Broadcast Wave", "*.bwf", BASSChannelType.BASS_CTYPE_STREAM_WAV),
				new BASS_PLUGINFORM("MOD Music", "*.mod;*.mdz", BASSChannelType.BASS_CTYPE_MUSIC_MOD),
				new BASS_PLUGINFORM("MO3 Music", "*.mo3", BASSChannelType.BASS_CTYPE_MUSIC_MO3),
				new BASS_PLUGINFORM("S3M Music", "*.s3m;*.s3z", BASSChannelType.BASS_CTYPE_MUSIC_S3M),
				new BASS_PLUGINFORM("XM Music", "*.xm;*.xmz", BASSChannelType.BASS_CTYPE_MUSIC_XM),
				new BASS_PLUGINFORM("IT Music", "*.it;*.itz;*.umx", BASSChannelType.BASS_CTYPE_MUSIC_IT),
				new BASS_PLUGINFORM("MTM Music", "*.mtm", BASSChannelType.BASS_CTYPE_MUSIC_MTM)
			};
			return new BASS_PLUGININFO(BASS_GetVersion(), formats3);
		}

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_GetEAXParameters(ref EAXEnvironment env, ref float vol, ref float decay, ref float damp);

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_GetEAXParameters([In] [Out] [MarshalAs(UnmanagedType.AsAny)] object env, [In] [Out] [MarshalAs(UnmanagedType.AsAny)] object vol, [In] [Out] [MarshalAs(UnmanagedType.AsAny)] object decay, [In] [Out] [MarshalAs(UnmanagedType.AsAny)] object damp);

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_SetEAXParameters(EAXEnvironment env, float vol, float decay, float damp);

		public static bool BASS_SetEAXParameters(EAXPreset preset)
		{
			bool result = false;
			switch (preset)
			{
			case EAXPreset.EAX_PRESET_GENERIC:
				result = BASS_SetEAXParameters(EAXEnvironment.EAX_ENVIRONMENT_GENERIC, 0.5f, 1.493f, 0.5f);
				break;
			case EAXPreset.EAX_PRESET_PADDEDCELL:
				result = BASS_SetEAXParameters(EAXEnvironment.EAX_ENVIRONMENT_PADDEDCELL, 0.25f, 0.1f, 0f);
				break;
			case EAXPreset.EAX_PRESET_ROOM:
				result = BASS_SetEAXParameters(EAXEnvironment.EAX_ENVIRONMENT_ROOM, 0.417f, 0.4f, 0.666f);
				break;
			case EAXPreset.EAX_PRESET_BATHROOM:
				result = BASS_SetEAXParameters(EAXEnvironment.EAX_ENVIRONMENT_BATHROOM, 0.653f, 1.499f, 0.166f);
				break;
			case EAXPreset.EAX_PRESET_LIVINGROOM:
				result = BASS_SetEAXParameters(EAXEnvironment.EAX_ENVIRONMENT_LIVINGROOM, 0.208f, 0.478f, 0f);
				break;
			case EAXPreset.EAX_PRESET_STONEROOM:
				result = BASS_SetEAXParameters(EAXEnvironment.EAX_ENVIRONMENT_STONEROOM, 0.5f, 2.309f, 0.888f);
				break;
			case EAXPreset.EAX_PRESET_AUDITORIUM:
				result = BASS_SetEAXParameters(EAXEnvironment.EAX_ENVIRONMENT_AUDITORIUM, 0.403f, 4.279f, 0.5f);
				break;
			case EAXPreset.EAX_PRESET_CONCERTHALL:
				result = BASS_SetEAXParameters(EAXEnvironment.EAX_ENVIRONMENT_CONCERTHALL, 0.5f, 3.961f, 0.5f);
				break;
			case EAXPreset.EAX_PRESET_CAVE:
				result = BASS_SetEAXParameters(EAXEnvironment.EAX_ENVIRONMENT_CAVE, 0.5f, 2.886f, 1.304f);
				break;
			case EAXPreset.EAX_PRESET_ARENA:
				result = BASS_SetEAXParameters(EAXEnvironment.EAX_ENVIRONMENT_ARENA, 0.361f, 7.284f, 0.332f);
				break;
			case EAXPreset.EAX_PRESET_HANGAR:
				result = BASS_SetEAXParameters(EAXEnvironment.EAX_ENVIRONMENT_HANGAR, 0.5f, 10f, 0.3f);
				break;
			case EAXPreset.EAX_PRESET_CARPETEDHALLWAY:
				result = BASS_SetEAXParameters(EAXEnvironment.EAX_ENVIRONMENT_CARPETEDHALLWAY, 0.153f, 0.259f, 2f);
				break;
			case EAXPreset.EAX_PRESET_HALLWAY:
				result = BASS_SetEAXParameters(EAXEnvironment.EAX_ENVIRONMENT_HALLWAY, 0.361f, 1.493f, 0f);
				break;
			case EAXPreset.EAX_PRESET_STONECORRIDOR:
				result = BASS_SetEAXParameters(EAXEnvironment.EAX_ENVIRONMENT_STONECORRIDOR, 0.444f, 2.697f, 0.638f);
				break;
			case EAXPreset.EAX_PRESET_ALLEY:
				result = BASS_SetEAXParameters(EAXEnvironment.EAX_ENVIRONMENT_ALLEY, 0.25f, 1.752f, 0.776f);
				break;
			case EAXPreset.EAX_PRESET_FOREST:
				result = BASS_SetEAXParameters(EAXEnvironment.EAX_ENVIRONMENT_FOREST, 0.111f, 3.145f, 0.472f);
				break;
			case EAXPreset.EAX_PRESET_CITY:
				result = BASS_SetEAXParameters(EAXEnvironment.EAX_ENVIRONMENT_CITY, 0.111f, 2.767f, 0.224f);
				break;
			case EAXPreset.EAX_PRESET_MOUNTAINS:
				result = BASS_SetEAXParameters(EAXEnvironment.EAX_ENVIRONMENT_MOUNTAINS, 0.194f, 7.841f, 0.472f);
				break;
			case EAXPreset.EAX_PRESET_QUARRY:
				result = BASS_SetEAXParameters(EAXEnvironment.EAX_ENVIRONMENT_QUARRY, 1f, 1.499f, 0.5f);
				break;
			case EAXPreset.EAX_PRESET_PLAIN:
				result = BASS_SetEAXParameters(EAXEnvironment.EAX_ENVIRONMENT_PLAIN, 0.097f, 2.767f, 0.224f);
				break;
			case EAXPreset.EAX_PRESET_PARKINGLOT:
				result = BASS_SetEAXParameters(EAXEnvironment.EAX_ENVIRONMENT_PARKINGLOT, 0.208f, 1.652f, 1.5f);
				break;
			case EAXPreset.EAX_PRESET_SEWERPIPE:
				result = BASS_SetEAXParameters(EAXEnvironment.EAX_ENVIRONMENT_SEWERPIPE, 0.652f, 2.886f, 0.25f);
				break;
			case EAXPreset.EAX_PRESET_UNDERWATER:
				result = BASS_SetEAXParameters(EAXEnvironment.EAX_ENVIRONMENT_UNDERWATER, 1f, 1.499f, 0f);
				break;
			case EAXPreset.EAX_PRESET_DRUGGED:
				result = BASS_SetEAXParameters(EAXEnvironment.EAX_ENVIRONMENT_DRUGGED, 0.875f, 8.392f, 1.388f);
				break;
			case EAXPreset.EAX_PRESET_DIZZY:
				result = BASS_SetEAXParameters(EAXEnvironment.EAX_ENVIRONMENT_DIZZY, 0.139f, 17.234f, 0.666f);
				break;
			case EAXPreset.EAX_PRESET_PSYCHOTIC:
				result = BASS_SetEAXParameters(EAXEnvironment.EAX_ENVIRONMENT_PSYCHOTIC, 0.486f, 7.563f, 0.806f);
				break;
			}
			return result;
		}

		[DllImport("bass")]
		public static extern void BASS_Apply3D();

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_Set3DFactors(float distf, float rollf, float doppf);

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_Get3DFactors(ref float distf, ref float rollf, ref float doppf);

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_Get3DFactors([In] [Out] [MarshalAs(UnmanagedType.AsAny)] object distf, [In] [Out] [MarshalAs(UnmanagedType.AsAny)] object rollf, [In] [Out] [MarshalAs(UnmanagedType.AsAny)] object doppf);

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_Set3DPosition([In] BASS_3DVECTOR pos, [In] BASS_3DVECTOR vel, [In] BASS_3DVECTOR front, [In] BASS_3DVECTOR top);

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_Get3DPosition([In] [Out] BASS_3DVECTOR pos, [In] [Out] BASS_3DVECTOR vel, [In] [Out] BASS_3DVECTOR front, [In] [Out] BASS_3DVECTOR top);

		[DllImport("bass", EntryPoint = "BASS_SampleLoad")]
		private static extern int BASS_SampleLoadUnicode([MarshalAs(UnmanagedType.Bool)] bool mem, [In] [MarshalAs(UnmanagedType.LPWStr)] string file, long offset, int length, int max, BASSFlag flags);

		public static int BASS_SampleLoad(string file, long offset, int length, int max, BASSFlag flags)
		{
			flags |= BASSFlag.BASS_UNICODE;
			return BASS_SampleLoadUnicode(mem: false, file, offset, length, max, flags);
		}

		[DllImport("bass", EntryPoint = "BASS_SampleLoad")]
		private static extern int BASS_SampleLoadMemory([MarshalAs(UnmanagedType.Bool)] bool mem, IntPtr memory, long offset, int length, int max, BASSFlag flags);

		public static int BASS_SampleLoad(IntPtr memory, long offset, int length, int max, BASSFlag flags)
		{
			return BASS_SampleLoadMemory(mem: true, memory, offset, length, max, flags);
		}

		[DllImport("bass", EntryPoint = "BASS_SampleLoad")]
		private static extern int BASS_SampleLoadMemory([MarshalAs(UnmanagedType.Bool)] bool mem, byte[] memory, long offset, int length, int max, BASSFlag flags);

		public static int BASS_SampleLoad(byte[] memory, long offset, int length, int max, BASSFlag flags)
		{
			return BASS_SampleLoadMemory(mem: true, memory, offset, length, max, flags);
		}

		[DllImport("bass")]
		public static extern int BASS_SampleCreate(int length, int freq, int chans, int max, BASSFlag flags);

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_SampleSetData(int handle, IntPtr buffer);

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_SampleSetData(int handle, float[] buffer);

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_SampleSetData(int handle, int[] buffer);

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_SampleSetData(int handle, short[] buffer);

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_SampleSetData(int handle, byte[] buffer);

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_SampleGetData(int handle, IntPtr buffer);

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_SampleGetData(int handle, float[] buffer);

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_SampleGetData(int handle, int[] buffer);

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_SampleGetData(int handle, short[] buffer);

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_SampleGetData(int handle, byte[] buffer);

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_SampleFree(int handle);

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_SampleGetInfo(int handle, [In] [Out] BASS_SAMPLE info);

		public static BASS_SAMPLE BASS_SampleGetInfo(int handle)
		{
			BASS_SAMPLE bASS_SAMPLE = new BASS_SAMPLE();
			if (BASS_SampleGetInfo(handle, bASS_SAMPLE))
			{
				return bASS_SAMPLE;
			}
			return null;
		}

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_SampleSetInfo(int handle, [In] BASS_SAMPLE info);

		[DllImport("bass")]
		public static extern int BASS_SampleGetChannel(int handle, [MarshalAs(UnmanagedType.Bool)] bool onlynew);

		[DllImport("bass")]
		public static extern int BASS_SampleGetChannels(int handle, int[] channels);

		public static int[] BASS_SampleGetChannels(int handle)
		{
			int[] array = new int[BASS_SampleGetInfo(handle).max];
			int num = BASS_SampleGetChannels(handle, array);
			if (num >= 0)
			{
				int[] array2 = new int[num];
				Array.Copy(array, array2, num);
				return array2;
			}
			return null;
		}

		public static int BASS_SampleGetChannelCount(int handle)
		{
			return BASS_SampleGetChannels(handle, null);
		}

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_SampleStop(int handle);

		[DllImport("bass")]
		public static extern int BASS_StreamCreate(int freq, int chans, BASSFlag flags, STREAMPROC proc, IntPtr user);

		[DllImport("bass", EntryPoint = "BASS_StreamCreate")]
		private static extern int BASS_StreamCreatePtr(int freq, int chans, BASSFlag flags, IntPtr procPtr, IntPtr user);

		public static int BASS_StreamCreate(int freq, int chans, BASSFlag flags, BASSStreamProc proc)
		{
			return BASS_StreamCreatePtr(freq, chans, flags, new IntPtr((int)proc), IntPtr.Zero);
		}

		public static int BASS_StreamCreateDummy(int freq, int chans, BASSFlag flags, IntPtr user)
		{
			return BASS_StreamCreatePtr(freq, chans, flags, IntPtr.Zero, user);
		}

		public static int BASS_StreamCreatePush(int freq, int chans, BASSFlag flags, IntPtr user)
		{
			return BASS_StreamCreatePtr(freq, chans, flags, new IntPtr(-1), user);
		}

		public static int BASS_StreamCreateDevice(int freq, int chans, BASSFlag flags, IntPtr user)
		{
			return BASS_StreamCreatePtr(freq, chans, flags, new IntPtr(-2), user);
		}

		public static int BASS_StreamCreateDevice3D(int freq, int chans, BASSFlag flags, IntPtr user)
		{
			return BASS_StreamCreatePtr(freq, chans, flags, new IntPtr(-3), user);
		}

		[DllImport("bass")]
		public static extern int BASS_StreamCreateFileUser(BASSStreamSystem system, BASSFlag flags, BASS_FILEPROCS procs, IntPtr user);

		[DllImport("bass", EntryPoint = "BASS_StreamCreateFile")]
		private static extern int BASS_StreamCreateFileUnicode([MarshalAs(UnmanagedType.Bool)] bool mem, [In] [MarshalAs(UnmanagedType.LPWStr)] string file, long offset, long length, BASSFlag flags);

		public static int BASS_StreamCreateFile(string file, long offset, long length, BASSFlag flags)
		{
			flags |= BASSFlag.BASS_UNICODE;
			return BASS_StreamCreateFileUnicode(mem: false, file, offset, length, flags);
		}

		[DllImport("bass", EntryPoint = "BASS_StreamCreateFile")]
		private static extern int BASS_StreamCreateFileMemory([MarshalAs(UnmanagedType.Bool)] bool mem, IntPtr memory, long offset, long length, BASSFlag flags);

		public static int BASS_StreamCreateFile(IntPtr memory, long offset, long length, BASSFlag flags)
		{
			return BASS_StreamCreateFileMemory(mem: true, memory, offset, length, flags);
		}

		[DllImport("bass", EntryPoint = "BASS_StreamCreateURL")]
		private static extern int BASS_StreamCreateURLUnicode([In] [MarshalAs(UnmanagedType.LPWStr)] string url, int offset, BASSFlag flags, DOWNLOADPROC proc, IntPtr user);

		[DllImport("bass", CharSet = CharSet.Ansi, EntryPoint = "BASS_StreamCreateURL")]
		private static extern int BASS_StreamCreateURLAscii([In] [MarshalAs(UnmanagedType.LPStr)] string url, int offset, BASSFlag flags, DOWNLOADPROC proc, IntPtr user);

		public static int BASS_StreamCreateURL(string url, int offset, BASSFlag flags, DOWNLOADPROC proc, IntPtr user)
		{
			flags |= BASSFlag.BASS_UNICODE;
			int num = BASS_StreamCreateURLUnicode(url, offset, flags, proc, user);
			if (num == 0)
			{
				flags &= (BASSFlag.BASS_SAMPLE_8BITS | BASSFlag.BASS_SAMPLE_MONO | BASSFlag.BASS_SAMPLE_LOOP | BASSFlag.BASS_SAMPLE_3D | BASSFlag.BASS_SAMPLE_SOFTWARE | BASSFlag.BASS_SAMPLE_MUTEMAX | BASSFlag.BASS_SAMPLE_VAM | BASSFlag.BASS_SAMPLE_FX | BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_RECORD_PAUSE | BASSFlag.BASS_RECORD_ECHOCANCEL | BASSFlag.BASS_RECORD_AGC | BASSFlag.BASS_STREAM_PRESCAN | BASSFlag.BASS_STREAM_AUTOFREE | BASSFlag.BASS_STREAM_RESTRATE | BASSFlag.BASS_STREAM_BLOCK | BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_STREAM_STATUS | BASSFlag.BASS_SPEAKER_FRONT | BASSFlag.BASS_SPEAKER_REAR | BASSFlag.BASS_SPEAKER_REAR2 | BASSFlag.BASS_SPEAKER_LEFT | BASSFlag.BASS_SPEAKER_RIGHT | BASSFlag.BASS_SPEAKER_PAIR8 | BASSFlag.BASS_ASYNCFILE | BASSFlag.BASS_SAMPLE_OVER_VOL | BASSFlag.BASS_WV_STEREO | BASSFlag.BASS_AC3_DOWNMIX_2 | BASSFlag.BASS_AC3_DOWNMIX_4 | BASSFlag.BASS_AC3_DYNAMIC_RANGE | BASSFlag.BASS_AAC_FRAME960);
				num = BASS_StreamCreateURLAscii(url, offset, flags, proc, user);
			}
			return num;
		}

		[DllImport("bass")]
		public static extern long BASS_StreamGetFilePosition(int handle, BASSStreamFilePosition mode);

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_StreamFree(int handle);

		[DllImport("bass")]
		public static extern int BASS_StreamPutData(int handle, IntPtr buffer, int length);

		[DllImport("bass")]
		public static extern int BASS_StreamPutData(int handle, float[] buffer, int length);

		[DllImport("bass")]
		public static extern int BASS_StreamPutData(int handle, int[] buffer, int length);

		[DllImport("bass")]
		public static extern int BASS_StreamPutData(int handle, short[] buffer, int length);

		[DllImport("bass")]
		public static extern int BASS_StreamPutData(int handle, byte[] buffer, int length);

		public unsafe static int BASS_StreamPutData(int handle, byte[] buffer, int startIdx, int length)
		{
			fixed (byte* value = &buffer[startIdx])
			{
				return BASS_StreamPutData(handle, new IntPtr(value), length);
			}
		}

		[DllImport("bass")]
		public static extern int BASS_StreamPutFileData(int handle, IntPtr buffer, int length);

		[DllImport("bass")]
		public static extern int BASS_StreamPutFileData(int handle, float[] buffer, int length);

		[DllImport("bass")]
		public static extern int BASS_StreamPutFileData(int handle, int[] buffer, int length);

		[DllImport("bass")]
		public static extern int BASS_StreamPutFileData(int handle, short[] buffer, int length);

		[DllImport("bass")]
		public static extern int BASS_StreamPutFileData(int handle, byte[] buffer, int length);

		[DllImport("bass", EntryPoint = "BASS_MusicLoad")]
		private static extern int BASS_MusicLoadUnicode([MarshalAs(UnmanagedType.Bool)] bool mem, [In] [MarshalAs(UnmanagedType.LPWStr)] string file, long offset, int length, BASSFlag flags, int freq);

		public static int BASS_MusicLoad(string file, long offset, int length, BASSFlag flags, int freq)
		{
			flags |= BASSFlag.BASS_UNICODE;
			return BASS_MusicLoadUnicode(mem: false, file, offset, length, flags, freq);
		}

		[DllImport("bass", EntryPoint = "BASS_MusicLoad")]
		private static extern int BASS_MusicLoadMemory([MarshalAs(UnmanagedType.Bool)] bool mem, IntPtr memory, long offset, int length, BASSFlag flags, int freq);

		public static int BASS_MusicLoad(IntPtr memory, long offset, int length, BASSFlag flags, int freq)
		{
			return BASS_MusicLoadMemory(mem: true, memory, offset, length, flags, freq);
		}

		[DllImport("bass", EntryPoint = "BASS_MusicLoad")]
		private static extern int BASS_MusicLoadMemory([MarshalAs(UnmanagedType.Bool)] bool mem, byte[] memory, long offset, int length, BASSFlag flags, int freq);

		public static int BASS_MusicLoad(byte[] memory, long offset, int length, BASSFlag flags, int freq)
		{
			return BASS_MusicLoadMemory(mem: true, memory, offset, length, flags, freq);
		}

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_MusicFree(int handle);

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_RecordInit(int device);

		[DllImport("bass")]
		public static extern int BASS_RecordStart(int freq, int chans, BASSFlag flags, RECORDPROC proc, IntPtr user);

		public static int BASS_RecordStart(int freq, int chans, BASSFlag flags, int period, RECORDPROC proc, IntPtr user)
		{
			return BASS_RecordStart(freq, chans, (BASSFlag)Utils.MakeLong((int)flags, period), proc, user);
		}

		[DllImport("bass", EntryPoint = "BASS_RecordGetDeviceInfo")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool BASS_RecordGetDeviceInfoInternal([In] int device, [In] [Out] ref BASS_DEVICEINFO_INTERNAL info);

		public unsafe static bool BASS_RecordGetDeviceInfo(int device, BASS_DEVICEINFO info)
		{
			bool flag = BASS_RecordGetDeviceInfoInternal(device, ref info._internal);
			if (flag)
			{
				if (_configUTF8)
				{
					info.name = Utils.IntPtrAsStringUtf8(info._internal.name, out int len);
					info.driver = Utils.IntPtrAsStringUtf8(info._internal.driver, out len);
					if (len > 0 && BASS_GetVersion() > 33818624)
					{
						try
						{
							info.id = Utils.IntPtrAsStringUtf8(new IntPtr((byte*)info._internal.driver.ToPointer() + len + 1), out len);
						}
						catch
						{
						}
					}
				}
				else
				{
					info.name = Utils.IntPtrAsStringAnsi(info._internal.name);
					info.driver = Utils.IntPtrAsStringAnsi(info._internal.driver);
					if (!string.IsNullOrEmpty(info.driver) && BASS_GetVersion() > 33818624)
					{
						try
						{
							info.id = Utils.IntPtrAsStringAnsi(new IntPtr((byte*)info._internal.driver.ToPointer() + info.driver.Length + 1));
						}
						catch
						{
						}
					}
				}
				info.flags = info._internal.flags;
			}
			return flag;
		}

		public static BASS_DEVICEINFO BASS_RecordGetDeviceInfo(int device)
		{
			BASS_DEVICEINFO bASS_DEVICEINFO = new BASS_DEVICEINFO();
			if (BASS_RecordGetDeviceInfo(device, bASS_DEVICEINFO))
			{
				return bASS_DEVICEINFO;
			}
			return null;
		}

		public static BASS_DEVICEINFO[] BASS_RecordGetDeviceInfos()
		{
			List<BASS_DEVICEINFO> list = new List<BASS_DEVICEINFO>();
			int num = 0;
			BASS_DEVICEINFO item;
			while ((item = BASS_RecordGetDeviceInfo(num)) != null)
			{
				list.Add(item);
				num++;
			}
			BASS_GetCPU();
			return list.ToArray();
		}

		public static int BASS_RecordGetDeviceCount()
		{
			BASS_DEVICEINFO info = new BASS_DEVICEINFO();
			int i;
			for (i = 0; BASS_RecordGetDeviceInfo(i, info); i++)
			{
			}
			BASS_GetCPU();
			return i;
		}

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_RecordSetDevice(int device);

		[DllImport("bass")]
		public static extern int BASS_RecordGetDevice();

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_RecordGetInfo([In] [Out] BASS_RECORDINFO info);

		public static BASS_RECORDINFO BASS_RecordGetInfo()
		{
			BASS_RECORDINFO bASS_RECORDINFO = new BASS_RECORDINFO();
			if (BASS_RecordGetInfo(bASS_RECORDINFO))
			{
				return bASS_RECORDINFO;
			}
			return null;
		}

		[DllImport("bass", EntryPoint = "BASS_RecordGetInputName")]
		private static extern IntPtr BASS_RecordGetInputNamePtr(int input);

		public static string BASS_RecordGetInputName(int input)
		{
			IntPtr intPtr = BASS_RecordGetInputNamePtr(input);
			if (intPtr != IntPtr.Zero)
			{
				if (_configUTF8 && BASS_GetVersion() >= 33819150)
				{
					return Utils.IntPtrAsStringUtf8(intPtr);
				}
				return Utils.IntPtrAsStringAnsi(intPtr);
			}
			return null;
		}

		public static string[] BASS_RecordGetInputNames()
		{
			List<string> list = new List<string>();
			int num = 0;
			string item;
			while ((item = BASS_RecordGetInputName(num)) != null)
			{
				list.Add(item);
				num++;
			}
			BASS_GetCPU();
			return list.ToArray();
		}

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_RecordSetInput(int input, BASSInput setting, float volume);

		[DllImport("bass")]
		public static extern int BASS_RecordGetInput(int input, ref float volume);

		public static BASSInput BASS_RecordGetInput(int input)
		{
			int num = BASS_RecordGetInputPtr(input, IntPtr.Zero);
			if (num != -1)
			{
				return (BASSInput)(num & 0xFF0000);
			}
			return BASSInput.BASS_INPUT_NONE;
		}

		[DllImport("bass", EntryPoint = "BASS_RecordGetInput")]
		private static extern int BASS_RecordGetInputPtr(int input, IntPtr vol);

		public static BASSInputType BASS_RecordGetInputType(int input)
		{
			int num = BASS_RecordGetInputPtr(input, IntPtr.Zero);
			if (num != -1)
			{
				return (BASSInputType)(num & -16777216);
			}
			return BASSInputType.BASS_INPUT_TYPE_ERROR;
		}

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_RecordFree();

		[DllImport("bass", EntryPoint = "BASS_ChannelGetInfo")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool BASS_ChannelGetInfoInternal(int handle, [In] [Out] ref BASS_CHANNELINFO_INTERNAL info);

		public static bool BASS_ChannelGetInfo(int handle, BASS_CHANNELINFO info)
		{
			bool num = BASS_ChannelGetInfoInternal(handle, ref info._internal);
			if (num)
			{
				info.chans = info._internal.chans;
				info.ctype = info._internal.ctype;
				info.flags = info._internal.flags;
				info.freq = info._internal.freq;
				info.origres = (info._internal.origres & 0xFFFF);
				info.origresIsFloat = ((info._internal.origres & 0x10000) == 65536);
				info.plugin = info._internal.plugin;
				info.sample = info._internal.sample;
				if ((info.flags & BASSFlag.BASS_UNICODE) != 0)
				{
					info.filename = Marshal.PtrToStringUni(info._internal.filename);
					return num;
				}
				if (_configUTF8 || Environment.OSVersion.Platform > PlatformID.WinCE)
				{
					info.filename = Utils.IntPtrAsStringUtf8(info._internal.filename);
					return num;
				}
				info.filename = Utils.IntPtrAsStringAnsi(info._internal.filename);
			}
			return num;
		}

		public static BASS_CHANNELINFO BASS_ChannelGetInfo(int handle)
		{
			BASS_CHANNELINFO bASS_CHANNELINFO = new BASS_CHANNELINFO();
			if (BASS_ChannelGetInfo(handle, bASS_CHANNELINFO))
			{
				return bASS_CHANNELINFO;
			}
			return null;
		}

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_ChannelPlay(int handle, [MarshalAs(UnmanagedType.Bool)] bool restart);

		[DllImport("bass")]
		public static extern int BASS_ChannelSetDSP(int handle, DSPPROC proc, IntPtr user, int priority);

		[DllImport("bass")]
		public static extern int BASS_ChannelGetData(int handle, IntPtr buffer, int length);

		[DllImport("bass")]
		public static extern int BASS_ChannelGetData(int handle, [In] [Out] float[] buffer, int length);

		[DllImport("bass")]
		public static extern int BASS_ChannelGetData(int handle, [In] [Out] short[] buffer, int length);

		[DllImport("bass")]
		public static extern int BASS_ChannelGetData(int handle, [In] [Out] int[] buffer, int length);

		[DllImport("bass")]
		public static extern int BASS_ChannelGetData(int handle, [In] [Out] byte[] buffer, int length);

		[DllImport("bass")]
		public static extern long BASS_ChannelSeconds2Bytes(int handle, double pos);

		[DllImport("bass")]
		public static extern double BASS_ChannelBytes2Seconds(int handle, long pos);

		[DllImport("bass")]
		public static extern BASSActive BASS_ChannelIsActive(int handle);

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_ChannelLock(int handle, [MarshalAs(UnmanagedType.Bool)] bool state);

		[DllImport("bass")]
		public static extern long BASS_ChannelGetLength(int handle, BASSMode mode);

		public static long BASS_ChannelGetLength(int handle)
		{
			return BASS_ChannelGetLength(handle, BASSMode.BASS_POS_BYTE);
		}

		[DllImport("bass")]
		public static extern int BASS_ChannelSetSync(int handle, BASSSync type, long param, SYNCPROC proc, IntPtr user);

		[DllImport("bass")]
		public static extern int BASS_ChannelSetFX(int handle, BASSFXType type, int priority);

		[DllImport("bass")]
		public static extern int BASS_ChannelGetDevice(int handle);

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_ChannelSetDevice(int handle, int device);

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_ChannelStop(int handle);

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_ChannelPause(int handle);

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_ChannelSetAttribute(int handle, BASSAttribute attrib, float value);

		[DllImport("bass", EntryPoint = "BASS_ChannelSetAttributeEx")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_ChannelSetAttribute(int handle, BASSAttribute attrib, IntPtr value, int size);

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_ChannelGetAttribute(int handle, BASSAttribute attrib, ref float value);

		[DllImport("bass", EntryPoint = "BASS_ChannelGetAttributeEx")]
		public static extern int BASS_ChannelGetAttribute(int handle, BASSAttribute attrib, IntPtr value, int size);

		[DllImport("bass")]
		public static extern BASSFlag BASS_ChannelFlags(int handle, BASSFlag flags, BASSFlag mask);

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_ChannelUpdate(int handle, int length);

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_ChannelIsSliding(int handle, BASSAttribute attrib);

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_ChannelSlideAttribute(int handle, BASSAttribute attrib, float value, int time);

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_ChannelSet3DAttributes(int handle, BASS3DMode mode, float min, float max, int iangle, int oangle, int outvol);

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_ChannelGet3DAttributes(int handle, ref BASS3DMode mode, ref float min, ref float max, ref int iangle, ref int oangle, ref int outvol);

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_ChannelGet3DAttributes(int handle, [In] [Out] [MarshalAs(UnmanagedType.AsAny)] object mode, [In] [Out] [MarshalAs(UnmanagedType.AsAny)] object min, [In] [Out] [MarshalAs(UnmanagedType.AsAny)] object max, [In] [Out] [MarshalAs(UnmanagedType.AsAny)] object iangle, [In] [Out] [MarshalAs(UnmanagedType.AsAny)] object oangle, [In] [Out] [MarshalAs(UnmanagedType.AsAny)] object outvol);

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_ChannelSet3DPosition(int handle, [In] BASS_3DVECTOR pos, [In] BASS_3DVECTOR orient, [In] BASS_3DVECTOR vel);

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_ChannelGet3DPosition(int handle, [In] [Out] BASS_3DVECTOR pos, [In] [Out] BASS_3DVECTOR orient, [In] [Out] BASS_3DVECTOR vel);

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_ChannelSetPosition(int handle, long pos, BASSMode mode);

		public static bool BASS_ChannelSetPosition(int handle, long pos)
		{
			return BASS_ChannelSetPosition(handle, pos, BASSMode.BASS_POS_BYTE);
		}

		public static bool BASS_ChannelSetPosition(int handle, double seconds)
		{
			return BASS_ChannelSetPosition(handle, BASS_ChannelSeconds2Bytes(handle, seconds), BASSMode.BASS_POS_BYTE);
		}

		public static bool BASS_ChannelSetPosition(int handle, int order, int row)
		{
			return BASS_ChannelSetPosition(handle, Utils.MakeLong(order, row), BASSMode.BASS_POS_MUSIC_ORDERS);
		}

		[DllImport("bass")]
		public static extern long BASS_ChannelGetPosition(int handle, BASSMode mode);

		public static long BASS_ChannelGetPosition(int handle)
		{
			return BASS_ChannelGetPosition(handle, BASSMode.BASS_POS_BYTE);
		}

		[DllImport("bass")]
		public static extern int BASS_ChannelGetLevel(int handle);

		[DllImport("bass", EntryPoint = "BASS_ChannelGetLevelEx")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_ChannelGetLevel(int handle, [In] [Out] float[] levels, float length, BASSLevel flags);

		public static float[] BASS_ChannelGetLevels(int handle, float length = 0.02f, BASSLevel flags = BASSLevel.BASS_LEVEL_ALL)
		{
			BASS_CHANNELINFO bASS_CHANNELINFO = BASS_ChannelGetInfo(handle);
			if (bASS_CHANNELINFO != null)
			{
				int num = bASS_CHANNELINFO.chans;
				if ((flags & BASSLevel.BASS_LEVEL_MONO) == BASSLevel.BASS_LEVEL_MONO)
				{
					num = 1;
				}
				else if ((flags & BASSLevel.BASS_LEVEL_STEREO) == BASSLevel.BASS_LEVEL_STEREO)
				{
					num = 2;
				}
				float[] array = new float[num];
				if (BASS_ChannelGetLevel(handle, array, length, flags))
				{
					return array;
				}
				return null;
			}
			return null;
		}

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_ChannelRemoveSync(int handle, int sync);

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_ChannelRemoveDSP(int handle, int dsp);

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_ChannelRemoveFX(int handle, int fx);

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_ChannelSetLink(int handle, int chan);

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_ChannelRemoveLink(int handle, int chan);

		[DllImport("bass")]
		public static extern IntPtr BASS_ChannelGetTags(int handle, BASSTag tags);

		public static string[] BASS_ChannelGetTagsArrayNullTermAnsi(int handle, BASSTag format)
		{
			return Utils.IntPtrToArrayNullTermAnsi(BASS_ChannelGetTags(handle, format));
		}

		public static string[] BASS_ChannelGetTagsArrayNullTermUtf8(int handle, BASSTag format)
		{
			return Utils.IntPtrToArrayNullTermUtf8(BASS_ChannelGetTags(handle, format));
		}

		public static string[] BASS_ChannelGetTagsID3V1(int handle)
		{
			IntPtr intPtr = BASS_ChannelGetTags(handle, BASSTag.BASS_TAG_ID3);
			if (intPtr != IntPtr.Zero)
			{
				string[] array = new string[7];
				try
				{
					BASS_TAG_ID3 bASS_TAG_ID = (BASS_TAG_ID3)Marshal.PtrToStructure(intPtr, typeof(BASS_TAG_ID3));
					array[0] = bASS_TAG_ID.Title;
					array[1] = bASS_TAG_ID.Artist;
					array[2] = bASS_TAG_ID.Album;
					array[3] = bASS_TAG_ID.Year;
					array[4] = bASS_TAG_ID.Comment;
					array[5] = bASS_TAG_ID.Genre.ToString();
					if (bASS_TAG_ID.Dummy == 0)
					{
						array[6] = bASS_TAG_ID.Track.ToString();
						return array;
					}
					return array;
				}
				catch
				{
					return array;
				}
			}
			return null;
		}

		public static string[] BASS_ChannelGetTagsBWF(int handle)
		{
			IntPtr intPtr = BASS_ChannelGetTags(handle, BASSTag.BASS_TAG_RIFF_BEXT);
			if (intPtr != IntPtr.Zero)
			{
				string[] array = new string[9];
				try
				{
					BASS_TAG_BEXT bASS_TAG_BEXT = (BASS_TAG_BEXT)Marshal.PtrToStructure(intPtr, typeof(BASS_TAG_BEXT));
					array[0] = bASS_TAG_BEXT.Description;
					array[1] = bASS_TAG_BEXT.Originator;
					array[2] = bASS_TAG_BEXT.OriginatorReference;
					array[3] = bASS_TAG_BEXT.OriginationDate;
					array[4] = bASS_TAG_BEXT.OriginationTime;
					array[5] = bASS_TAG_BEXT.TimeReference.ToString();
					array[6] = bASS_TAG_BEXT.Version.ToString();
					array[7] = bASS_TAG_BEXT.UMID;
					array[8] = bASS_TAG_BEXT.GetCodingHistory(intPtr);
					return array;
				}
				catch
				{
					return array;
				}
			}
			return null;
		}

		public static BASS_TAG_CACODEC BASS_ChannelGetTagsCA(int handle)
		{
			IntPtr intPtr = BASS_ChannelGetTags(handle, BASSTag.BASS_TAG_WMA_META);
			if (intPtr != IntPtr.Zero)
			{
				return new BASS_TAG_CACODEC(intPtr);
			}
			return null;
		}

		public static string BASS_ChannelGetTagsDSDArtist(int handle)
		{
			return Utils.IntPtrAsStringAnsi(BASS_ChannelGetTags(handle, BASSTag.BASS_TAG_DSD_ARTIST));
		}

		public static string BASS_ChannelGetTagsDSDTitle(int handle)
		{
			return Utils.IntPtrAsStringAnsi(BASS_ChannelGetTags(handle, BASSTag.BASS_TAG_DSD_TITLE));
		}

		public static BASS_TAG_DSD_COMMENT[] BASS_ChannelGetTagsDSDComments(int handle)
		{
			List<BASS_TAG_DSD_COMMENT> list = new List<BASS_TAG_DSD_COMMENT>();
			int num = 0;
			BASS_TAG_DSD_COMMENT tag;
			while ((tag = BASS_TAG_DSD_COMMENT.GetTag(handle, num)) != null)
			{
				list.Add(tag);
				num++;
			}
			if (list.Count > 0)
			{
				return list.ToArray();
			}
			return null;
		}

		public static string[] BASS_ChannelGetTagsID3V2(int handle)
		{
			return null;
		}

		public static string[] BASS_ChannelGetTagsAPE(int handle)
		{
			return BASS_ChannelGetTagsArrayNullTermUtf8(handle, BASSTag.BASS_TAG_APE);
		}

		public static BASS_TAG_APE_BINARY[] BASS_ChannelGetTagsAPEBinary(int handle)
		{
			List<BASS_TAG_APE_BINARY> list = new List<BASS_TAG_APE_BINARY>();
			int num = 0;
			BASS_TAG_APE_BINARY tag;
			while ((tag = BASS_TAG_APE_BINARY.GetTag(handle, num)) != null)
			{
				list.Add(tag);
				num++;
			}
			if (list.Count > 0)
			{
				return list.ToArray();
			}
			return null;
		}

		public static string[] BASS_ChannelGetTagsWMA(int handle)
		{
			return BASS_ChannelGetTagsArrayNullTermUtf8(handle, BASSTag.BASS_TAG_WMA);
		}

		public static string[] BASS_ChannelGetTagsMP4(int handle)
		{
			return BASS_ChannelGetTagsArrayNullTermUtf8(handle, BASSTag.BASS_TAG_MP4);
		}

		public static string[] BASS_ChannelGetTagsMF(int handle)
		{
			return BASS_ChannelGetTagsArrayNullTermUtf8(handle, BASSTag.BASS_TAG_MF);
		}

		public static WAVEFORMATEXT BASS_ChannelGetTagsWAVEFORMAT(int handle)
		{
			IntPtr intPtr = BASS_ChannelGetTags(handle, BASSTag.BASS_TAG_WAVEFORMAT);
			if (intPtr != IntPtr.Zero)
			{
				return new WAVEFORMATEXT(intPtr);
			}
			return null;
		}

		public static BASS_TAG_FLAC_PICTURE[] BASS_ChannelGetTagsFLACPictures(int handle)
		{
			List<BASS_TAG_FLAC_PICTURE> list = new List<BASS_TAG_FLAC_PICTURE>();
			int num = 0;
			BASS_TAG_FLAC_PICTURE tag;
			while ((tag = BASS_TAG_FLAC_PICTURE.GetTag(handle, num)) != null)
			{
				list.Add(tag);
				num++;
			}
			if (list.Count > 0)
			{
				return list.ToArray();
			}
			return null;
		}

		public static BASS_TAG_FLAC_CUE BASS_ChannelGetTagsFLACCuesheet(int handle)
		{
			return BASS_TAG_FLAC_CUE.GetTag(handle);
		}

		public static string[] BASS_ChannelGetTagsHTTP(int handle)
		{
			return BASS_ChannelGetTagsArrayNullTermAnsi(handle, BASSTag.BASS_TAG_HTTP);
		}

		public static string[] BASS_ChannelGetTagsICY(int handle)
		{
			return BASS_ChannelGetTagsArrayNullTermAnsi(handle, BASSTag.BASS_TAG_ICY);
		}

		public static string[] BASS_ChannelGetTagsOGG(int handle)
		{
			return BASS_ChannelGetTagsArrayNullTermUtf8(handle, BASSTag.BASS_TAG_OGG);
		}

		public static string[] BASS_ChannelGetTagsRIFF(int handle)
		{
			return BASS_ChannelGetTagsArrayNullTermAnsi(handle, BASSTag.BASS_TAG_RIFF_INFO);
		}

		public static BASS_TAG_CUE BASS_ChannelGetTagsRIFFCUE(int handle)
		{
			return BASS_TAG_CUE.GetTag(handle);
		}

		public static string[] BASS_ChannelGetTagsMETA(int handle)
		{
			return Utils.IntPtrToArrayNullTermAnsi(BASS_ChannelGetTags(handle, BASSTag.BASS_TAG_META));
		}

		public static string BASS_ChannelGetTagLyrics3v2(int handle)
		{
			IntPtr intPtr = BASS_ChannelGetTags(handle, BASSTag.BASS_TAG_LYRICS3);
			if (intPtr != IntPtr.Zero)
			{
				return Utils.IntPtrAsStringAnsi(intPtr);
			}
			return null;
		}

		public static string BASS_ChannelGetMusicName(int handle)
		{
			IntPtr intPtr = BASS_ChannelGetTags(handle, BASSTag.BASS_TAG_MUSIC_NAME);
			if (intPtr != IntPtr.Zero)
			{
				return Utils.IntPtrAsStringAnsi(intPtr);
			}
			return null;
		}

		public static string BASS_ChannelGetMusicMessage(int handle)
		{
			IntPtr intPtr = BASS_ChannelGetTags(handle, BASSTag.BASS_TAG_MUSIC_MESSAGE);
			if (intPtr != IntPtr.Zero)
			{
				return Utils.IntPtrAsStringAnsi(intPtr);
			}
			return null;
		}

		public static string BASS_ChannelGetMusicInstrument(int handle, int instrument)
		{
			IntPtr intPtr = BASS_ChannelGetTags(handle, (BASSTag)(65792 + instrument));
			if (intPtr != IntPtr.Zero)
			{
				return Utils.IntPtrAsStringAnsi(intPtr);
			}
			return null;
		}

		public static string BASS_ChannelGetMusicSample(int handle, int sample)
		{
			IntPtr intPtr = BASS_ChannelGetTags(handle, (BASSTag)(66304 + sample));
			if (intPtr != IntPtr.Zero)
			{
				return Utils.IntPtrAsStringAnsi(intPtr);
			}
			return null;
		}

		public static string[] BASS_ChannelGetMidiTrackText(int handle, int track)
		{
			if (track >= 0)
			{
				return Utils.IntPtrToArrayNullTermAnsi(BASS_ChannelGetTags(handle, (BASSTag)(69632 + track)));
			}
			List<string> list = new List<string>();
			track = 0;
			while (true)
			{
				IntPtr intPtr = BASS_ChannelGetTags(handle, (BASSTag)(69632 + track));
				if (!(intPtr != IntPtr.Zero))
				{
					break;
				}
				string[] array = Utils.IntPtrToArrayNullTermAnsi(intPtr);
				if (array != null && array.Length != 0)
				{
					list.AddRange(array);
				}
				track++;
			}
			if (list.Count > 0)
			{
				return list.ToArray();
			}
			return null;
		}

		[DllImport("bass", EntryPoint = "BASS_FXSetParameters")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool BASS_FXSetParametersExt(int handle, [In] [MarshalAs(UnmanagedType.AsAny)] object par);

		public static bool BASS_FXSetParameters(int handle, object par)
		{
			return false;
		}

		[DllImport("bass", EntryPoint = "BASS_FXGetParameters")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool BASS_FXGetParametersExt(int handle, [In] [Out] float[] par);

		[DllImport("bass", EntryPoint = "BASS_FXGetParameters")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool BASS_FXGetParametersExt(int handle, [In] [Out] [MarshalAs(UnmanagedType.AsAny)] object par);

		public static bool BASS_FXGetParameters(int handle, object par)
		{
			return false;
		}

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_FXReset(int handle);

		[DllImport("bass")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_FXSetPriority(int handle, int priority);

		public static bool LoadMe()
		{
			bool num = Utils.LoadLib("bass", ref _myModuleHandle);
			if (num)
			{
				InitBass();
			}
			return num;
		}

		public static bool LoadMe(string path)
		{
			bool num = Utils.LoadLib(Path.Combine(path, "bass"), ref _myModuleHandle);
			if (num)
			{
				InitBass();
			}
			return num;
		}

		public static bool FreeMe()
		{
			return Utils.FreeLib(ref _myModuleHandle);
		}

		private static void CheckVersion()
		{
			try
			{
				if (Utils.HighWord(BASS_GetVersion()) == 516)
				{
					return;
				}
				Version version = BASS_GetVersion(2);
				Version version2 = new Version(2, 4);
				FileVersionInfo fileVersionInfo = null;
				ProcessModuleCollection modules = Process.GetCurrentProcess().Modules;
				for (int num = modules.Count - 1; num >= 0; num--)
				{
					ProcessModule processModule = modules[num];
					if (processModule.ModuleName.ToLower().Equals("bass".ToLower()))
					{
						fileVersionInfo = processModule.FileVersionInfo;
						break;
					}
				}
				if (fileVersionInfo != null)
				{
					Console.WriteLine(string.Format("An incorrect version of BASS was loaded!\r\n\r\nVersion loaded: {0}.{1}\r\nVersion expected: {2}.{3}\r\n\r\nFile: {4}\r\nFileVersion: {5}\r\nDescription: {6}\r\nCompany: {7}\r\nLanguage: {8}", version.Major, version.Minor, version2.Major, version2.Minor, fileVersionInfo.FileName, fileVersionInfo.FileVersion, fileVersionInfo.FileDescription, fileVersionInfo.CompanyName + " " + fileVersionInfo.LegalCopyright, fileVersionInfo.Language));
				}
				else
				{
					Console.WriteLine($"An incorrect version of BASS was loaded!\r\n\r\nBASS Version loaded: {version.Major}.{version.Minor}\r\nBASS Version expected: {version2.Major}.{version2.Minor}");
				}
			}
			catch
			{
			}
		}
	}
}
