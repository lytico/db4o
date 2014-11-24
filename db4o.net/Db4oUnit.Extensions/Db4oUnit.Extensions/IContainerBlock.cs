/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o;

namespace Db4oUnit.Extensions
{
	public interface IContainerBlock
	{
		/// <exception cref="System.Exception"></exception>
		void Run(IObjectContainer client);
	}
}
