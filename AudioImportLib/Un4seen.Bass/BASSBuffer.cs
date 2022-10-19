using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Un4seen.Bass
{
	[SuppressUnmanagedCodeSecurity]
	public sealed class BASSBuffer : IDisposable
	{
		private bool disposed;

		private int _bufferlength = 352800;

		private int _bps = 2;

		private int _samplerate = 44100;

		private int _chans = 2;

		private byte[] _buffer;

		private int _bufferwritepos;

		private int _readers = 1;

		private int[] _bufferreadpos = new int[1];

		public int BufferLength => _bufferlength;

		public int Bps => _bps;

		public int SampleRate => _samplerate;

		public int NumChans => _chans;

		public int Readers
		{
			get
			{
				return _readers;
			}
			set
			{
				if (value <= 0 || value == _readers)
				{
					return;
				}
				lock (_buffer)
				{
					int[] array = new int[value];
					for (int i = 0; i < value; i++)
					{
						if (i < _readers)
						{
							array[i] = _bufferreadpos[i];
						}
						else
						{
							array[i] = _bufferreadpos[_readers - 1];
						}
					}
					_bufferreadpos = array;
					_readers = value;
				}
			}
		}

		public BASSBuffer()
		{
			Initialize();
		}

		public BASSBuffer(float seconds, int samplerate, int chans, int bps)
		{
			if (seconds <= 0f)
			{
				seconds = 2f;
			}
			_samplerate = samplerate;
			if (_samplerate <= 0)
			{
				_samplerate = 44100;
			}
			_chans = chans;
			if (_chans <= 0)
			{
				_chans = 2;
			}
			_bps = bps;
			if (_bps > 4)
			{
				switch (_bps)
				{
				case 32:
					_bps = 4;
					break;
				case 8:
					_bps = 1;
					break;
				default:
					_bps = 2;
					break;
				}
			}
			if (_bps <= 0 || _bps == 3)
			{
				_bps = 2;
			}
			_bufferlength = (int)Math.Ceiling((double)seconds * (double)_samplerate * (double)_chans * (double)_bps);
			if (_bufferlength % _bps > 0)
			{
				_bufferlength -= _bufferlength % _bps;
			}
			Initialize();
		}

		private void Initialize()
		{
			_buffer = new byte[_bufferlength];
			Clear();
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (!disposed)
			{
			}
			disposed = true;
		}

		~BASSBuffer()
		{
			Dispose(disposing: false);
		}

		public void Clear()
		{
			lock (_buffer)
			{
				Array.Clear(_buffer, 0, _bufferlength);
				_bufferwritepos = 0;
				for (int i = 0; i < _readers; i++)
				{
					_bufferreadpos[i] = 0;
				}
			}
		}

		public void Resize(float factor)
		{
			if (factor <= 1f)
			{
				return;
			}
			lock (_buffer)
			{
				_bufferlength = (int)Math.Ceiling((double)factor * (double)_bufferlength);
				if (_bufferlength % _bps > 0)
				{
					_bufferlength -= _bufferlength % _bps;
				}
				byte[] array = new byte[_bufferlength];
				Array.Clear(array, 0, _bufferlength);
				Array.Copy(_buffer, array, _buffer.Length);
				_buffer = array;
			}
		}

		public int Space(int reader)
		{
			int num = _bufferlength;
			lock (_buffer)
			{
				if (reader < 0 || reader >= _readers)
				{
					int num2 = 0;
					for (int i = 0; i < _readers; i++)
					{
						num2 = _bufferlength - (_bufferwritepos - _bufferreadpos[reader]);
						if (num2 > _bufferlength)
						{
							num2 -= _bufferlength;
						}
						if (num2 < num)
						{
							num = num2;
						}
					}
					return num;
				}
				num = _bufferlength - (_bufferwritepos - _bufferreadpos[reader]);
				if (num > _bufferlength)
				{
					return num - _bufferlength;
				}
				return num;
			}
		}

		public int Count(int reader)
		{
			int num = -1;
			lock (_buffer)
			{
				if (reader < 0 || reader >= _readers)
				{
					int num2 = 0;
					for (int i = 0; i < _readers; i++)
					{
						num2 = _bufferwritepos - _bufferreadpos[i];
						if (num2 < 0)
						{
							num2 += _bufferlength;
						}
						if (num2 > num)
						{
							num = num2;
						}
					}
					return num;
				}
				num = _bufferwritepos - _bufferreadpos[reader];
				if (num < 0)
				{
					return num + _bufferlength;
				}
				return num;
			}
		}

		public unsafe int Write(IntPtr buffer, int length)
		{
			lock (_buffer)
			{
				if (length > _bufferlength)
				{
					length = _bufferlength;
				}
				int num = 0;
				int num2 = _bufferlength - _bufferwritepos;
				if (length >= num2)
				{
					Marshal.Copy(buffer, _buffer, _bufferwritepos, num2);
					num += num2;
					buffer = new IntPtr((byte*)buffer.ToPointer() + num2);
					length -= num2;
					_bufferwritepos = 0;
				}
				Marshal.Copy(buffer, _buffer, _bufferwritepos, length);
				num += length;
				_bufferwritepos += length;
				return num;
			}
		}

		public int Write(byte[] buffer, int length)
		{
			lock (_buffer)
			{
				if (length > _bufferlength)
				{
					length = _bufferlength;
				}
				int num = 0;
				int num2 = _bufferlength - _bufferwritepos;
				if (length >= num2)
				{
					Array.Copy(buffer, num, _buffer, _bufferwritepos, num2);
					num += num2;
					length -= num2;
					_bufferwritepos = 0;
				}
				Array.Copy(buffer, num, _buffer, _bufferwritepos, length);
				num += length;
				_bufferwritepos += length;
				return num;
			}
		}

		public unsafe int Read(IntPtr buffer, int length, int reader)
		{
			lock (_buffer)
			{
				int num = 0;
				if (reader < 0 || reader >= _readers)
				{
					reader = 0;
				}
				int num2 = _bufferwritepos - _bufferreadpos[reader];
				if (num2 < 0)
				{
					num2 += _bufferlength;
				}
				if (length > num2)
				{
					length = num2;
				}
				num2 = _bufferlength - _bufferreadpos[reader];
				if (length >= num2)
				{
					Marshal.Copy(_buffer, _bufferreadpos[reader], buffer, num2);
					num += num2;
					buffer = new IntPtr((byte*)buffer.ToPointer() + num2);
					length -= num2;
					_bufferreadpos[reader] = 0;
				}
				Marshal.Copy(_buffer, _bufferreadpos[reader], buffer, length);
				_bufferreadpos[reader] += length;
				return num + length;
			}
		}

		public int Read(byte[] buffer, int length, int reader)
		{
			lock (_buffer)
			{
				int num = 0;
				if (reader < 0 || reader >= _readers)
				{
					reader = 0;
				}
				int num2 = _bufferwritepos - _bufferreadpos[reader];
				if (num2 < 0)
				{
					num2 += _bufferlength;
				}
				if (length > num2)
				{
					length = num2;
				}
				num2 = _bufferlength - _bufferreadpos[reader];
				if (length >= num2)
				{
					Array.Copy(_buffer, _bufferreadpos[reader], buffer, num, num2);
					num += num2;
					length -= num2;
					_bufferreadpos[reader] = 0;
				}
				Array.Copy(_buffer, _bufferreadpos[reader], buffer, num, length);
				_bufferreadpos[reader] += length;
				return num + length;
			}
		}
	}
}
