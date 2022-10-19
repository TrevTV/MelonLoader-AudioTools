using System;
using System.ComponentModel;
using System.Security;
using System.Threading;

namespace Un4seen.Bass
{
	[SuppressUnmanagedCodeSecurity]
	public sealed class BASSTimer : IDisposable
	{
		private bool disposed;

		private Timer _timer;

		private int _interval = 50;

		private TimerCallback _timerDelegate;

		private bool _enabled;

		public int Interval
		{
			get
			{
				return _interval;
			}
			set
			{
				if (value <= 0)
				{
					_interval = -1;
				}
				else
				{
					_interval = value;
				}
				if (Enabled)
				{
					lock (_timer)
					{
						_timer.Change(_interval, _interval);
					}
				}
			}
		}

		public bool Enabled
		{
			get
			{
				if (_enabled)
				{
					return _timer != null;
				}
				return false;
			}
			set
			{
				if (value == _enabled)
				{
					return;
				}
				if (value)
				{
					if (_timer != null)
					{
						lock (_timer)
						{
							_timer.Change(_interval, _interval);
							_enabled = true;
						}
					}
					else
					{
						Start();
					}
				}
				else if (_timer != null)
				{
					lock (_timer)
					{
						_timer.Change(-1, -1);
						_enabled = false;
					}
				}
				else
				{
					Stop();
				}
			}
		}

		public event EventHandler Tick;

		public BASSTimer()
		{
			_timerDelegate = timer_Tick;
		}

		public BASSTimer(int interval)
		{
			_interval = interval;
			_timerDelegate = timer_Tick;
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
				try
				{
					Stop();
				}
				catch
				{
				}
			}
			disposed = true;
		}

		~BASSTimer()
		{
			Dispose(disposing: false);
		}

		private void timer_Tick(object state)
		{
			if (this.Tick != null)
			{
				ProcessDelegate(this.Tick, this, EventArgs.Empty);
			}
		}

		private void ProcessDelegate(Delegate del, params object[] args)
		{
			if ((object)del == null || _timer == null)
			{
				return;
			}
			lock (_timer)
			{
				Delegate[] invocationList = del.GetInvocationList();
				foreach (Delegate del2 in invocationList)
				{
					InvokeDelegate(del2, args);
				}
			}
		}

		private void InvokeDelegate(Delegate del, object[] args)
		{
			ISynchronizeInvoke synchronizeInvoke = del.Target as ISynchronizeInvoke;
			if (synchronizeInvoke != null)
			{
				if (!synchronizeInvoke.InvokeRequired)
				{
					del.DynamicInvoke(args);
					return;
				}
				try
				{
					synchronizeInvoke.BeginInvoke(del, args);
				}
				catch
				{
				}
			}
			else
			{
				del.DynamicInvoke(args);
			}
		}

		public void Start()
		{
			if (_timer == null)
			{
				_timer = new Timer(_timerDelegate, null, _interval, _interval);
				_enabled = true;
				return;
			}
			lock (_timer)
			{
				_timer.Change(_interval, _interval);
				_enabled = true;
			}
		}

		public void Stop()
		{
			if (_timer != null)
			{
				lock (_timer)
				{
					_timer.Change(-1, -1);
					_timer.Dispose();
					_enabled = false;
				}
				_timer = null;
			}
			else
			{
				_enabled = false;
			}
		}
	}
}
