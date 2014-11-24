/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit.Extensions;
using Db4objects.Db4o;

namespace Db4oUnit.Extensions
{
	public class ContainerServices
	{
		/// <exception cref="System.Exception"></exception>
		public static void WithContainer(IObjectContainer container, IContainerBlock block
			)
		{
			try
			{
				block.Run(container);
			}
			finally
			{
				container.Close();
			}
		}
	}
}
