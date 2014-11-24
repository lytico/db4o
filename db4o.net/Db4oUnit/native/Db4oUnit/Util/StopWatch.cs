/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
namespace Db4oUnit.Util
{
	public class StopWatch
	{
		private System.DateTime _started;

		private System.DateTime _finished;

		public StopWatch()
		{
		}

		public virtual void Start()
		{
			_started = System.DateTime.Now;
		}

		public virtual void Stop()
		{
			_finished = System.DateTime.Now;
		}

		public virtual System.TimeSpan Elapsed()
		{
			return _finished - _started;
		}

		public override string ToString()
		{
			return Elapsed().ToString();
		}
	}
}
