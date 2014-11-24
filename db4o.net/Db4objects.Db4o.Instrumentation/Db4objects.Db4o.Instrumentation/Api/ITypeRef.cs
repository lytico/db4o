/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Instrumentation.Api;

namespace Db4objects.Db4o.Instrumentation.Api
{
	public interface ITypeRef
	{
		bool IsPrimitive
		{
			get;
		}

		ITypeRef ElementType
		{
			get;
		}

		string Name
		{
			get;
		}
	}
}
