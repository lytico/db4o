/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.IO;

namespace Db4objects.Db4o.Internal.IO
{
	public class BlockSizeImpl : IBlockSize
	{
		private readonly ListenerRegistry _listenerRegistry = ListenerRegistry.NewInstance
			();

		private int _value;

		public virtual void Register(IListener4 listener)
		{
			_listenerRegistry.Register(listener);
		}

		public virtual void Set(int newValue)
		{
			_value = newValue;
			_listenerRegistry.NotifyListeners(_value);
		}

		public virtual int Value()
		{
			return _value;
		}
	}
}
