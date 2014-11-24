/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Bench.Delaying;
using Db4objects.Db4o.Bench.Timing;
using Db4objects.Db4o.IO;

namespace Db4objects.Db4o.Bench.Delaying
{
	public class DelayingStorage : StorageDecorator
	{
		private static Delays NoDelays = new Delays(0, 0, 0);

		private Delays _delays;

		public DelayingStorage(IStorage delegateAdapter) : this(delegateAdapter, NoDelays
			)
		{
		}

		public DelayingStorage(IStorage delegateAdapter, Delays delays) : base(delegateAdapter
			)
		{
			_delays = delays;
		}

		protected override IBin Decorate(BinConfiguration config, IBin bin)
		{
			return new DelayingStorage.DelayingBin(bin, _delays);
		}

		private class DelayingBin : BinDecorator
		{
			private TicksTiming _timing;

			private Delays _delays;

			/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
			public DelayingBin(IBin bin, Delays delays) : base(bin)
			{
				_delays = delays;
				_timing = new TicksTiming();
			}

			/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
			public override int Read(long pos, byte[] bytes, int length)
			{
				Delay(_delays.values[Delays.Read]);
				return _bin.Read(pos, bytes, length);
			}

			/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
			public override void Sync()
			{
				Delay(_delays.values[Delays.Sync]);
				_bin.Sync();
			}

			/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
			public override void Write(long pos, byte[] buffer, int length)
			{
				Delay(_delays.values[Delays.Write]);
				_bin.Write(pos, buffer, length);
			}

			private void Delay(long time)
			{
				_timing.WaitTicks(time);
			}
		}
	}
}
