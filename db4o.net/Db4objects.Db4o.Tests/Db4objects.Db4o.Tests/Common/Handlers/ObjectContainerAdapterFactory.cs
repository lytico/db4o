/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Tests.Common.Handlers;

namespace Db4objects.Db4o.Tests.Common.Handlers
{
	/// <exclude></exclude>
	public class ObjectContainerAdapterFactory
	{
		public static readonly IObjectContainerAdapter Pre71Facade = new Pre7_1ObjectContainerAdapter
			();

		public static readonly IObjectContainerAdapter Post71Facade = new Post7_1ObjectContainerAdapter
			();

		public static IObjectContainerAdapter ForVersion(int major, int minor)
		{
			if ((major == 7 && minor >= 1) || major > 7)
			{
				return Post71Facade;
			}
			else
			{
				return Pre71Facade;
			}
		}
	}
}
