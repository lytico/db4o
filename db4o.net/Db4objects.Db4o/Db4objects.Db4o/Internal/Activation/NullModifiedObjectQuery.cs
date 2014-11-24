/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Activation;

namespace Db4objects.Db4o.Internal.Activation
{
	public class NullModifiedObjectQuery : IModifiedObjectQuery
	{
		public static readonly IModifiedObjectQuery Instance = new Db4objects.Db4o.Internal.Activation.NullModifiedObjectQuery
			();

		private NullModifiedObjectQuery()
		{
		}

		public virtual bool IsModified(object @ref)
		{
			return false;
		}
	}
}
