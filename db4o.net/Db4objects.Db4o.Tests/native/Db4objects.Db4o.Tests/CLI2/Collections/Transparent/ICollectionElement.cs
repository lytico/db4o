/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */
using System;

namespace Db4objects.Db4o.Tests.CLI2.Collections.Transparent
{
	public interface ICollectionElement : IComparable<ICollectionElement>
	{
		string Name
		{ 
			get;
		}
	}
}
