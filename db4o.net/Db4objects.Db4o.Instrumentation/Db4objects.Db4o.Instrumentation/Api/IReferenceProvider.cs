/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Reflection;
using Db4objects.Db4o.Instrumentation.Api;

namespace Db4objects.Db4o.Instrumentation.Api
{
	public interface IReferenceProvider
	{
		ITypeRef ForType(Type type);

		IMethodRef ForMethod(MethodInfo method);

		IMethodRef ForMethod(ITypeRef declaringType, string methodName, ITypeRef[] parameterTypes
			, ITypeRef returnType);
	}
}
