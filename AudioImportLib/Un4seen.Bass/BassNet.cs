using System.Drawing;
using System.Reflection;
using System.Security;
using System.Threading;

namespace Un4seen.Bass
{
	[SuppressUnmanagedCodeSecurity]
	public sealed class BassNet
	{
		internal static string _eMail;

		internal static string _registrationKey;

		internal static string _internalName;

		public static bool OmitCheckVersion;

		private static bool _useBrokenLatin1;

		private static bool _useRiffInfoUTF8;

		public static string InternalName => _internalName;

		public static bool UseBrokenLatin1Behavior
		{
			get
			{
				return _useBrokenLatin1;
			}
			set
			{
				_useBrokenLatin1 = value;
			}
		}

		public static bool UseRiffInfoUTF8
		{
			get
			{
				return _useRiffInfoUTF8;
			}
			set
			{
				_useRiffInfoUTF8 = value;
			}
		}

		private BassNet()
		{
		}

		static BassNet()
		{
			_eMail = string.Empty;
			_registrationKey = string.Empty;
			_internalName = "BASS.NET";
			OmitCheckVersion = false;
			_useBrokenLatin1 = false;
			_useRiffInfoUTF8 = false;
			AssemblyName name = Assembly.GetExecutingAssembly().GetName();
			_internalName = $"{name.Name} v{name.Version}";
		}

		public static void Registration(string eMail, string registrationKey)
		{
			_eMail = eMail;
			_registrationKey = registrationKey;
		}
	}
}
