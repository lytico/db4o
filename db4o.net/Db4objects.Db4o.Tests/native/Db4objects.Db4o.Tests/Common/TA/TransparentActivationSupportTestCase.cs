/* Copyright (C) 2004-2007   Versant Inc.   http://www.db4o.com */

namespace Db4objects.Db4o.Tests.Common.TA
{
	public partial class TransparentActivationSupportTestCase
	{
		public sealed partial class Item
		{
#if CF || SILVERLIGHT
			public Item()
			{
			}
#endif
		}
	}
}
