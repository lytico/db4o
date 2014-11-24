/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Tests.Jre5.Collections.Typehandler;

namespace Db4objects.Db4o.Tests.Jre5.Collections.Typehandler
{
	public abstract class AbstractListItemFactory : AbstractItemFactory
	{
		public override string FieldName()
		{
			return AbstractItemFactory.ListFieldName;
		}
	}
}
