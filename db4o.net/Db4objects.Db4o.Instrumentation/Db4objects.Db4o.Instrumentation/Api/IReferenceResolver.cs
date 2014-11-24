/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Reflection;
using Db4objects.Db4o.Instrumentation.Api;

namespace Db4objects.Db4o.Instrumentation.Api
{
	public interface IReferenceResolver
	{
		/// <exception cref="Db4objects.Db4o.Instrumentation.Api.InstrumentationException"></exception>
		MethodInfo Resolve(IMethodRef methodRef);
	}
}
