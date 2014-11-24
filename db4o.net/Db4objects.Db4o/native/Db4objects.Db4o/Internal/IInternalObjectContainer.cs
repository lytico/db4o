/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Internal
{
	public partial interface IInternalObjectContainer
	{
		void WithEnvironment(Action4 action);
	}
}
