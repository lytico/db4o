/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Sharpen;

namespace Db4objects.Db4o.Foundation
{
	/// <exclude></exclude>
	public class TimeStampIdGenerator
	{
		public const int BitsReservedForCounter = 15;

		public const int CounterLimit = 64;

		private long _counter;

		private long _lastTime;

		public static long IdToMilliseconds(long id)
		{
			return id >> BitsReservedForCounter;
		}

		public static long MillisecondsToId(long milliseconds)
		{
			return milliseconds << BitsReservedForCounter;
		}

		public TimeStampIdGenerator(long minimumNext)
		{
			InternalSetMinimumNext(minimumNext);
		}

		public TimeStampIdGenerator() : this(0)
		{
		}

		public virtual long Generate()
		{
			long t = Now();
			if (t > _lastTime)
			{
				_lastTime = t;
				_counter = 0;
				return MillisecondsToId(t);
			}
			UpdateTimeOnCounterLimitOverflow();
			_counter++;
			UpdateTimeOnCounterLimitOverflow();
			return Last();
		}

		protected virtual long Now()
		{
			return Runtime.CurrentTimeMillis();
		}

		private void UpdateTimeOnCounterLimitOverflow()
		{
			if (_counter < CounterLimit)
			{
				return;
			}
			long timeIncrement = _counter / CounterLimit;
			_lastTime += timeIncrement;
			_counter -= (timeIncrement * CounterLimit);
		}

		public virtual long Last()
		{
			return MillisecondsToId(_lastTime) + _counter;
		}

		public virtual bool SetMinimumNext(long newMinimum)
		{
			if (newMinimum <= Last())
			{
				return false;
			}
			InternalSetMinimumNext(newMinimum);
			return true;
		}

		private void InternalSetMinimumNext(long newNext)
		{
			_lastTime = IdToMilliseconds(newNext);
			long timePart = MillisecondsToId(_lastTime);
			_counter = newNext - timePart;
			UpdateTimeOnCounterLimitOverflow();
		}
	}
}
