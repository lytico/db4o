/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Tests.Jre5.Collections.Typehandler;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Tests.Jre5.Collections.Typehandler
{
	public class ListTypeHandlerTestUnitBase : TypeHandlerTestUnitBase
	{
		protected override AbstractItemFactory ItemFactory()
		{
			return (AbstractItemFactory)ListTypeHandlerTestVariables.ListImplementation.Value;
		}

		protected override ITypeHandler4 TypeHandler()
		{
			return (ITypeHandler4)ListTypeHandlerTestVariables.ListTypehander.Value;
		}

		protected override void FillItem(object item)
		{
			FillListItem(item);
		}

		protected override void AssertContent(object item)
		{
			AssertListContent(item);
		}

		protected override void AssertPlainContent(object item)
		{
			AssertPlainListContent((IList)item);
		}

		protected override ListTypeHandlerTestElementsSpec ElementsSpec()
		{
			return (ListTypeHandlerTestElementsSpec)ListTypeHandlerTestVariables.ElementsSpec
				.Value;
		}
	}
}
