using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct BASS_TAG_CART
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		private byte[] version;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		private byte[] title;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		private byte[] artist;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		private byte[] cutID;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		private byte[] clientID;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		private byte[] category;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		private byte[] classification;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		private byte[] outCue;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
		private byte[] startDate;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		private byte[] startTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
		private byte[] endDate;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		private byte[] endTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		private byte[] producerAppID;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		private byte[] producerAppVersion;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		private byte[] userDef;

		private int dwLevelReference;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		private byte[] timer1Usage;

		private int timer1Value;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		private byte[] timer2Usage;

		private int timer2Value;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		private byte[] timer3Usage;

		private int timer3Value;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		private byte[] timer4Usage;

		private int timer4Value;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		private byte[] timer5Usage;

		private int timer5Value;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		private byte[] timer6Usage;

		private int timer6Value;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		private byte[] timer7Usage;

		private int timer7Value;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		private byte[] timer8Usage;

		private int timer8Value;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 276)]
		public byte[] Reserved;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
		private byte[] url;

		public string Version
		{
			get
			{
				if (version == null)
				{
					return "0000";
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(version, 0, version.Length).TrimEnd(default(char));
				}
				return Encoding.ASCII.GetString(version, 0, version.Length).TrimEnd(default(char));
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					if (BassNet.UseBrokenLatin1Behavior)
					{
						version = Encoding.Default.GetBytes("0000");
					}
					else
					{
						version = Encoding.ASCII.GetBytes("0000");
					}
					return;
				}
				version = new byte[4];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value + "0000", 0, 4, version, 0);
				}
				else
				{
					Encoding.ASCII.GetBytes(value + "0000", 0, 4, version, 0);
				}
			}
		}

		public string Title
		{
			get
			{
				if (title == null)
				{
					return string.Empty;
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(title, 0, title.Length).TrimEnd(default(char));
				}
				return Encoding.ASCII.GetString(title, 0, title.Length).TrimEnd(default(char));
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					title = new byte[64];
					return;
				}
				title = new byte[64];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 64) ? 64 : value.Length, title, 0);
				}
				else
				{
					Encoding.ASCII.GetBytes(value, 0, (value.Length > 64) ? 64 : value.Length, title, 0);
				}
			}
		}

		public string Artist
		{
			get
			{
				if (artist == null)
				{
					return string.Empty;
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(artist, 0, artist.Length).TrimEnd(default(char));
				}
				return Encoding.ASCII.GetString(artist, 0, artist.Length).TrimEnd(default(char));
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					artist = new byte[64];
					return;
				}
				artist = new byte[64];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 64) ? 64 : value.Length, artist, 0);
				}
				else
				{
					Encoding.ASCII.GetBytes(value, 0, (value.Length > 64) ? 64 : value.Length, artist, 0);
				}
			}
		}

		public string CutID
		{
			get
			{
				if (cutID == null)
				{
					return string.Empty;
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(cutID, 0, cutID.Length).TrimEnd(default(char));
				}
				return Encoding.ASCII.GetString(cutID, 0, cutID.Length).TrimEnd(default(char));
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					cutID = new byte[64];
					return;
				}
				cutID = new byte[64];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 64) ? 64 : value.Length, cutID, 0);
				}
				else
				{
					Encoding.ASCII.GetBytes(value, 0, (value.Length > 64) ? 64 : value.Length, cutID, 0);
				}
			}
		}

		public string ClientID
		{
			get
			{
				if (clientID == null)
				{
					return string.Empty;
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(clientID, 0, clientID.Length).TrimEnd(default(char));
				}
				return Encoding.ASCII.GetString(clientID, 0, clientID.Length).TrimEnd(default(char));
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					clientID = new byte[64];
					return;
				}
				clientID = new byte[64];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 64) ? 64 : value.Length, clientID, 0);
				}
				else
				{
					Encoding.ASCII.GetBytes(value, 0, (value.Length > 64) ? 64 : value.Length, clientID, 0);
				}
			}
		}

		public string Category
		{
			get
			{
				if (category == null)
				{
					return string.Empty;
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(category, 0, category.Length).TrimEnd(default(char));
				}
				return Encoding.ASCII.GetString(category, 0, category.Length).TrimEnd(default(char));
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					category = new byte[64];
					return;
				}
				category = new byte[64];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 64) ? 64 : value.Length, category, 0);
				}
				else
				{
					Encoding.ASCII.GetBytes(value, 0, (value.Length > 64) ? 64 : value.Length, category, 0);
				}
			}
		}

		public string Classification
		{
			get
			{
				if (classification == null)
				{
					return string.Empty;
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(classification, 0, classification.Length).TrimEnd(default(char));
				}
				return Encoding.ASCII.GetString(classification, 0, classification.Length).TrimEnd(default(char));
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					classification = new byte[64];
					return;
				}
				classification = new byte[64];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 64) ? 64 : value.Length, classification, 0);
				}
				else
				{
					Encoding.ASCII.GetBytes(value, 0, (value.Length > 64) ? 64 : value.Length, classification, 0);
				}
			}
		}

		public string OutCue
		{
			get
			{
				if (outCue == null)
				{
					return string.Empty;
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(outCue, 0, outCue.Length).TrimEnd(default(char));
				}
				return Encoding.ASCII.GetString(outCue, 0, outCue.Length).TrimEnd(default(char));
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					outCue = new byte[64];
					return;
				}
				outCue = new byte[64];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 64) ? 64 : value.Length, outCue, 0);
				}
				else
				{
					Encoding.ASCII.GetBytes(value, 0, (value.Length > 64) ? 64 : value.Length, outCue, 0);
				}
			}
		}

		public string StartDate
		{
			get
			{
				if (startDate == null)
				{
					return "1900-01-01";
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(startDate, 0, startDate.Length).TrimEnd(default(char)).Replace(' ', '-')
						.Replace(':', '-')
						.Replace('.', '-')
						.Replace('_', '-');
				}
				return Encoding.ASCII.GetString(startDate, 0, startDate.Length).TrimEnd(default(char)).Replace(' ', '-')
					.Replace(':', '-')
					.Replace('.', '-')
					.Replace('_', '-');
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					startDate = Encoding.ASCII.GetBytes("1900-01-01");
					return;
				}
				startDate = new byte[10];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 10) ? 10 : value.Length, startDate, 0);
				}
				else
				{
					Encoding.ASCII.GetBytes(value, 0, (value.Length > 10) ? 10 : value.Length, startDate, 0);
				}
			}
		}

		public string StartTime
		{
			get
			{
				if (startTime == null)
				{
					return "00:00:00";
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(startTime, 0, startTime.Length).TrimEnd(default(char)).Replace(' ', ':')
						.Replace('-', ':')
						.Replace('.', ':')
						.Replace('_', ':');
				}
				return Encoding.ASCII.GetString(startTime, 0, startTime.Length).TrimEnd(default(char)).Replace(' ', ':')
					.Replace('-', ':')
					.Replace('.', ':')
					.Replace('_', ':');
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					startTime = Encoding.ASCII.GetBytes("00:00:00");
					return;
				}
				startTime = new byte[8];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 8) ? 8 : value.Length, startTime, 0);
				}
				else
				{
					Encoding.ASCII.GetBytes(value, 0, (value.Length > 8) ? 8 : value.Length, startTime, 0);
				}
			}
		}

		public string EndDate
		{
			get
			{
				if (endDate == null)
				{
					return "9999-12-31";
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(endDate, 0, endDate.Length).TrimEnd(default(char)).Replace(' ', '-')
						.Replace(':', '-')
						.Replace('.', '-')
						.Replace('_', '-');
				}
				return Encoding.ASCII.GetString(endDate, 0, endDate.Length).TrimEnd(default(char)).Replace(' ', '-')
					.Replace(':', '-')
					.Replace('.', '-')
					.Replace('_', '-');
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					endDate = Encoding.ASCII.GetBytes("9999-12-31");
					return;
				}
				endDate = new byte[10];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 10) ? 10 : value.Length, endDate, 0);
				}
				else
				{
					Encoding.ASCII.GetBytes(value, 0, (value.Length > 10) ? 10 : value.Length, endDate, 0);
				}
			}
		}

		public string EndTime
		{
			get
			{
				if (endTime == null)
				{
					return "23:59:59";
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(endTime, 0, startTime.Length).TrimEnd(default(char)).Replace(' ', ':')
						.Replace('-', ':')
						.Replace('.', ':')
						.Replace('_', ':');
				}
				return Encoding.ASCII.GetString(endTime, 0, startTime.Length).TrimEnd(default(char)).Replace(' ', ':')
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
						endTime = Encoding.Default.GetBytes("23:59:59");
					}
					else
					{
						endTime = Encoding.ASCII.GetBytes("23:59:59");
					}
					return;
				}
				endTime = new byte[8];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 8) ? 8 : value.Length, endTime, 0);
				}
				else
				{
					Encoding.ASCII.GetBytes(value, 0, (value.Length > 8) ? 8 : value.Length, endTime, 0);
				}
			}
		}

		public string ProducerAppID
		{
			get
			{
				if (producerAppID == null)
				{
					return string.Empty;
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(producerAppID, 0, producerAppID.Length).TrimEnd(default(char));
				}
				return Encoding.ASCII.GetString(producerAppID, 0, producerAppID.Length).TrimEnd(default(char));
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					producerAppID = new byte[64];
					return;
				}
				producerAppID = new byte[64];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 64) ? 64 : value.Length, producerAppID, 0);
				}
				else
				{
					Encoding.ASCII.GetBytes(value, 0, (value.Length > 64) ? 64 : value.Length, producerAppID, 0);
				}
			}
		}

		public string ProducerAppVersion
		{
			get
			{
				if (producerAppVersion == null)
				{
					return string.Empty;
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(producerAppVersion, 0, producerAppVersion.Length).TrimEnd(default(char));
				}
				return Encoding.ASCII.GetString(producerAppVersion, 0, producerAppVersion.Length).TrimEnd(default(char));
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					producerAppVersion = new byte[64];
					return;
				}
				producerAppVersion = new byte[64];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 64) ? 64 : value.Length, producerAppVersion, 0);
				}
				else
				{
					Encoding.ASCII.GetBytes(value, 0, (value.Length > 64) ? 64 : value.Length, producerAppVersion, 0);
				}
			}
		}

		public string UserDef
		{
			get
			{
				if (userDef == null)
				{
					return string.Empty;
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(userDef, 0, userDef.Length).TrimEnd(default(char));
				}
				return Encoding.ASCII.GetString(userDef, 0, userDef.Length).TrimEnd(default(char));
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					userDef = new byte[64];
					return;
				}
				userDef = new byte[64];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 64) ? 64 : value.Length, userDef, 0);
				}
				else
				{
					Encoding.ASCII.GetBytes(value, 0, (value.Length > 64) ? 64 : value.Length, userDef, 0);
				}
			}
		}

		public int LevelReference
		{
			get
			{
				return dwLevelReference;
			}
			set
			{
				dwLevelReference = value;
			}
		}

		public string Timer1Usage
		{
			get
			{
				if (timer1Usage == null)
				{
					return string.Empty;
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(timer1Usage, 0, timer1Usage.Length).TrimEnd(default(char));
				}
				return Encoding.ASCII.GetString(timer1Usage, 0, timer1Usage.Length).TrimEnd(default(char));
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					timer1Usage = new byte[4];
					return;
				}
				timer1Usage = new byte[4];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 4) ? 4 : value.Length, timer1Usage, 0);
				}
				else
				{
					Encoding.ASCII.GetBytes(value, 0, (value.Length > 4) ? 4 : value.Length, timer1Usage, 0);
				}
			}
		}

		public int Timer1Value
		{
			get
			{
				return timer1Value;
			}
			set
			{
				timer1Value = value;
			}
		}

		public string Timer2Usage
		{
			get
			{
				if (timer2Usage == null)
				{
					return string.Empty;
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(timer2Usage, 0, timer2Usage.Length).TrimEnd(default(char));
				}
				return Encoding.ASCII.GetString(timer2Usage, 0, timer2Usage.Length).TrimEnd(default(char));
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					timer2Usage = new byte[4];
					return;
				}
				timer2Usage = new byte[4];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 4) ? 4 : value.Length, timer2Usage, 0);
				}
				else
				{
					Encoding.ASCII.GetBytes(value, 0, (value.Length > 4) ? 4 : value.Length, timer2Usage, 0);
				}
			}
		}

		public int Timer2Value
		{
			get
			{
				return timer2Value;
			}
			set
			{
				timer2Value = value;
			}
		}

		public string Timer3Usage
		{
			get
			{
				if (timer3Usage == null)
				{
					return string.Empty;
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(timer3Usage, 0, timer3Usage.Length).TrimEnd(default(char));
				}
				return Encoding.ASCII.GetString(timer3Usage, 0, timer3Usage.Length).TrimEnd(default(char));
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					timer3Usage = new byte[4];
					return;
				}
				timer3Usage = new byte[4];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 4) ? 4 : value.Length, timer3Usage, 0);
				}
				else
				{
					Encoding.ASCII.GetBytes(value, 0, (value.Length > 4) ? 4 : value.Length, timer3Usage, 0);
				}
			}
		}

		public int Timer3Value
		{
			get
			{
				return timer3Value;
			}
			set
			{
				timer3Value = value;
			}
		}

		public string Timer4Usage
		{
			get
			{
				if (timer4Usage == null)
				{
					return string.Empty;
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(timer4Usage, 0, timer4Usage.Length).TrimEnd(default(char));
				}
				return Encoding.ASCII.GetString(timer4Usage, 0, timer4Usage.Length).TrimEnd(default(char));
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					timer4Usage = new byte[4];
					return;
				}
				timer4Usage = new byte[4];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 4) ? 4 : value.Length, timer4Usage, 0);
				}
				else
				{
					Encoding.ASCII.GetBytes(value, 0, (value.Length > 4) ? 4 : value.Length, timer4Usage, 0);
				}
			}
		}

		public int Timer4Value
		{
			get
			{
				return timer4Value;
			}
			set
			{
				timer4Value = value;
			}
		}

		public string Timer5Usage
		{
			get
			{
				if (timer5Usage == null)
				{
					return string.Empty;
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(timer5Usage, 0, timer5Usage.Length).TrimEnd(default(char));
				}
				return Encoding.ASCII.GetString(timer5Usage, 0, timer5Usage.Length).TrimEnd(default(char));
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					timer5Usage = new byte[4];
					return;
				}
				timer5Usage = new byte[4];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 4) ? 4 : value.Length, timer5Usage, 0);
				}
				else
				{
					Encoding.ASCII.GetBytes(value, 0, (value.Length > 4) ? 4 : value.Length, timer5Usage, 0);
				}
			}
		}

		public int Timer5Value
		{
			get
			{
				return timer5Value;
			}
			set
			{
				timer5Value = value;
			}
		}

		public string Timer6Usage
		{
			get
			{
				if (timer6Usage == null)
				{
					return string.Empty;
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(timer6Usage, 0, timer6Usage.Length).TrimEnd(default(char));
				}
				return Encoding.ASCII.GetString(timer6Usage, 0, timer6Usage.Length).TrimEnd(default(char));
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					timer6Usage = new byte[4];
					return;
				}
				timer6Usage = new byte[4];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 4) ? 4 : value.Length, timer6Usage, 0);
				}
				else
				{
					Encoding.ASCII.GetBytes(value, 0, (value.Length > 4) ? 4 : value.Length, timer6Usage, 0);
				}
			}
		}

		public int Timer6Value
		{
			get
			{
				return timer6Value;
			}
			set
			{
				timer6Value = value;
			}
		}

		public string Timer7Usage
		{
			get
			{
				if (timer7Usage == null)
				{
					return string.Empty;
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(timer7Usage, 0, timer7Usage.Length).TrimEnd(default(char));
				}
				return Encoding.ASCII.GetString(timer7Usage, 0, timer7Usage.Length).TrimEnd(default(char));
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					timer7Usage = new byte[4];
					return;
				}
				timer7Usage = new byte[4];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 4) ? 4 : value.Length, timer7Usage, 0);
				}
				else
				{
					Encoding.ASCII.GetBytes(value, 0, (value.Length > 4) ? 4 : value.Length, timer7Usage, 0);
				}
			}
		}

		public int Timer7Value
		{
			get
			{
				return timer7Value;
			}
			set
			{
				timer7Value = value;
			}
		}

		public string Timer8Usage
		{
			get
			{
				if (timer8Usage == null)
				{
					return string.Empty;
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(timer8Usage, 0, timer8Usage.Length).TrimEnd(default(char));
				}
				return Encoding.ASCII.GetString(timer8Usage, 0, timer8Usage.Length).TrimEnd(default(char));
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					timer8Usage = new byte[4];
					return;
				}
				timer8Usage = new byte[4];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 4) ? 4 : value.Length, timer8Usage, 0);
				}
				else
				{
					Encoding.ASCII.GetBytes(value, 0, (value.Length > 4) ? 4 : value.Length, timer8Usage, 0);
				}
			}
		}

		public int Timer8Value
		{
			get
			{
				return timer8Value;
			}
			set
			{
				timer8Value = value;
			}
		}

		public string URL
		{
			get
			{
				if (url == null)
				{
					return string.Empty;
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(url, 0, url.Length).TrimEnd(default(char));
				}
				return Encoding.ASCII.GetString(url, 0, url.Length).TrimEnd(default(char));
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					url = new byte[1024];
					return;
				}
				url = new byte[1024];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 1024) ? 1024 : value.Length, url, 0);
				}
				else
				{
					Encoding.ASCII.GetBytes(value, 0, (value.Length > 1024) ? 1024 : value.Length, url, 0);
				}
			}
		}

		public override string ToString()
		{
			return Artist + " - " + Title;
		}

		public unsafe string GetTagText(IntPtr tag)
		{
			if (tag == IntPtr.Zero)
			{
				return null;
			}
			int len;
			return Utils.IntPtrAsStringLatin1(new IntPtr((byte*)tag.ToPointer() + 2048), out len);
		}

		public byte[] AsByteArray(string tagText)
		{
			if (string.IsNullOrEmpty(tagText))
			{
				tagText = new string('\0', 256);
			}
			else
			{
				if (!tagText.EndsWith("\0"))
				{
					tagText += "\0";
				}
				if (tagText.Length % 2 == 1)
				{
					tagText += "\0";
				}
				int num = tagText.Length % 256;
				if (num > 0)
				{
					tagText += new string('\0', num);
				}
			}
			byte[] array = BassNet.UseBrokenLatin1Behavior ? Encoding.Default.GetBytes(tagText) : Encoding.ASCII.GetBytes(tagText);
			int num2 = Marshal.SizeOf(typeof(BASS_TAG_CART));
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
