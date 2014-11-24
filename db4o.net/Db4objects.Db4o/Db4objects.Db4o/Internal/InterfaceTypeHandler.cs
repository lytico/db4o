/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Internal
{
	public sealed class InterfaceTypeHandler : OpenTypeHandler
	{
		public InterfaceTypeHandler(ObjectContainerBase container) : base(container)
		{
		}

		public override bool Equals(object obj)
		{
			return obj is Db4objects.Db4o.Internal.InterfaceTypeHandler;
		}
	}
}
