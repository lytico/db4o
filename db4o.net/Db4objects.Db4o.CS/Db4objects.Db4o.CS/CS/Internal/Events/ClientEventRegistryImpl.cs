/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.Internal.Events;

namespace Db4objects.Db4o.CS.Internal.Events
{
	public partial class ClientEventRegistryImpl : EventRegistryImpl
	{
		private readonly ClientObjectContainer _container;

		public ClientEventRegistryImpl(ClientObjectContainer container)
		{
			_container = container;
		}

		protected override void OnCommittedListenerAdded()
		{
			_container.OnCommittedListenerAdded();
		}
	}
}
