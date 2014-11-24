/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Instrumentation.Api;

namespace Db4objects.Db4o.Instrumentation.Api
{
	public interface IMethodRef
	{
		string Name
		{
			get;
		}

		ITypeRef ReturnType
		{
			get;
		}

		ITypeRef[] ParamTypes
		{
			get;
		}

		ITypeRef DeclaringType
		{
			get;
		}
	}
}
