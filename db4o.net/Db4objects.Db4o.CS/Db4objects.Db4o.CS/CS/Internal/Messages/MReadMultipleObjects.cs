/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.CS.Internal.Objectexchange;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	public class MReadMultipleObjects : MsgD, IMessageWithResponse
	{
		public Msg ReplyFromServer()
		{
			int prefetchDepth = ReadInt();
			int prefetchCount = ReadInt();
			IIntIterator4 ids = new _FixedSizeIntIterator4Base_14(this, prefetchCount);
			ByteArrayBuffer buffer = MarshallObjects(prefetchDepth, prefetchCount, ids);
			return Msg.ReadMultipleObjects.GetWriterForBuffer(Transaction(), buffer);
		}

		private sealed class _FixedSizeIntIterator4Base_14 : FixedSizeIntIterator4Base
		{
			public _FixedSizeIntIterator4Base_14(MReadMultipleObjects _enclosing, int baseArg1
				) : base(baseArg1)
			{
				this._enclosing = _enclosing;
			}

			protected override int NextInt()
			{
				return this._enclosing.ReadInt();
			}

			private readonly MReadMultipleObjects _enclosing;
		}

		private ByteArrayBuffer MarshallObjects(int prefetchDepth, int prefetchCount, IIntIterator4
			 ids)
		{
			lock (ContainerLock())
			{
				IObjectExchangeStrategy strategy = ObjectExchangeStrategyFactory.ForConfig(new ObjectExchangeConfiguration
					(prefetchDepth, prefetchCount));
				return strategy.Marshall((LocalTransaction)Transaction(), ids, prefetchCount);
			}
		}
	}
}
