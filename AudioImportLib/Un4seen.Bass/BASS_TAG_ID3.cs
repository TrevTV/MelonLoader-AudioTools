using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct BASS_TAG_ID3
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		private byte[] id;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
		private byte[] title;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
		private byte[] artist;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
		private byte[] album;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		private byte[] year;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 28)]
		private byte[] comment;

		internal byte Dummy;

		public byte Track;

		public byte Genre;

		public string ID
		{
			get
			{
				if (id == null)
				{
					return string.Empty;
				}
				string @string = Encoding.Default.GetString(id, 0, id.Length);
				int num = @string.IndexOf('\0');
				if (num >= 0)
				{
					return @string.Substring(0, num).TrimEnd(null);
				}
				return @string.TrimEnd(null);
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					id = new byte[64];
					return;
				}
				id = new byte[64];
				Encoding.Default.GetBytes(value, 0, (value.Length > 3) ? 3 : value.Length, id, 0);
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
				string @string = Encoding.Default.GetString(title, 0, title.Length);
				int num = @string.IndexOf('\0');
				if (num >= 0)
				{
					return @string.Substring(0, num).TrimEnd(null);
				}
				return @string.TrimEnd(null);
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					title = new byte[30];
					return;
				}
				title = new byte[30];
				Encoding.Default.GetBytes(value, 0, (value.Length > 30) ? 30 : value.Length, title, 0);
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
				string @string = Encoding.Default.GetString(artist, 0, artist.Length);
				int num = @string.IndexOf('\0');
				if (num >= 0)
				{
					return @string.Substring(0, num).TrimEnd(null);
				}
				return @string.TrimEnd(null);
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					artist = new byte[30];
					return;
				}
				artist = new byte[30];
				Encoding.Default.GetBytes(value, 0, (value.Length > 30) ? 30 : value.Length, artist, 0);
			}
		}

		public string Album
		{
			get
			{
				if (album == null)
				{
					return string.Empty;
				}
				string @string = Encoding.Default.GetString(album, 0, album.Length);
				int num = @string.IndexOf('\0');
				if (num >= 0)
				{
					return @string.Substring(0, num).TrimEnd(null);
				}
				return @string.TrimEnd(null);
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					album = new byte[30];
					return;
				}
				album = new byte[30];
				Encoding.Default.GetBytes(value, 0, (value.Length > 30) ? 30 : value.Length, album, 0);
			}
		}

		public string Year
		{
			get
			{
				if (year == null)
				{
					return string.Empty;
				}
				string @string = Encoding.Default.GetString(year, 0, year.Length);
				int num = @string.IndexOf('\0');
				if (num >= 0)
				{
					return @string.Substring(0, num).TrimEnd(null);
				}
				return @string.TrimEnd(null);
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					year = new byte[4];
					return;
				}
				year = new byte[4];
				Encoding.Default.GetBytes(value, 0, (value.Length > 4) ? 4 : value.Length, year, 0);
			}
		}

		public string Comment
		{
			get
			{
				if (comment == null)
				{
					return string.Empty;
				}
				string @string = Encoding.Default.GetString(comment, 0, comment.Length);
				int num = @string.IndexOf('\0');
				if (num >= 0)
				{
					return @string.Substring(0, num).TrimEnd(null);
				}
				return @string.TrimEnd(null);
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					comment = new byte[28];
					return;
				}
				comment = new byte[28];
				Encoding.Default.GetBytes(value, 0, (value.Length > 28) ? 28 : value.Length, comment, 0);
			}
		}
	}
}
