/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4oUnit.Tests.Fixtures
{
	public interface ISet4
	{
		void Add(object value);

		bool Contains(object value);

		int Size();
	}
}
