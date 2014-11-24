/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

using System;
using System.Threading;

namespace Sharpen.Lang
{
	public class Thread : IRunnable
	{
		private IRunnable _target;

		private string _name;

		private System.Threading.Thread _thread;

		private bool _isDaemon;

		public Thread()
		{
			_target = this;
		}

		public Thread(IRunnable target, string name)
		{
			_target = target;
			SetName(name);
		}

		public Thread(IRunnable target)
		{
			_target = target;
		}

		public Thread(System.Threading.Thread thread)
		{
			_thread = thread;
		}

		public static Thread CurrentThread()
		{
			return new Thread(System.Threading.Thread.CurrentThread);
		}

		public virtual void Run()
		{
		}

		public void SetName(string name)
		{
			_name = name;
#if !CF
			if (_thread != null && name != null)
			{
				try
				{
					_thread.Name = _name;
				}
				catch
				{
				}
			}
#endif
		}

		public string GetName()
		{
#if !CF
			return _thread != null ? _thread.Name : _name;
#else
			return "";
#endif
		}

		public static void Sleep(long milliseconds)
		{
			System.Threading.Thread.Sleep((int)milliseconds);
		}

		public void Start()
		{
			_thread = new System.Threading.Thread(EntryPoint);
			_thread.IsBackground = _isDaemon;
			if (_name != null)
			{
				SetName(_name);
			}
			_thread.Start();
		}

		public void Join() 
		{
			if (_thread == null)
				return;
			_thread.Join();
		}

		public void Join(int millisecondsTimeout)
		{
			if (_thread == null)
				return;
			_thread.Join(millisecondsTimeout);
		}
		
		public void SetDaemon(bool isDaemon)
		{
			_isDaemon = isDaemon;
		}

		public override bool Equals(object obj)
		{
			Thread other = (obj as Thread);
			if (other == null)
				return false;
			if (other == this)
				return true;
			if (_thread == null)
				return false;
			return _thread == other._thread;
		}

		public override int GetHashCode()
		{
			return _thread == null ? 37 : _thread.GetHashCode();
		}

		private void EntryPoint()
		{
			try
			{
				_target.Run();
			}
			catch (Exception e)
			{
				// don't let an unhandled exception bring
				// the process down
				Runtime.PrintStackTrace(e);
			}
		}

		public bool IsDaemon()
		{
			return _thread != null
				? _thread.IsBackground
				: _isDaemon;
		}
	}
}
