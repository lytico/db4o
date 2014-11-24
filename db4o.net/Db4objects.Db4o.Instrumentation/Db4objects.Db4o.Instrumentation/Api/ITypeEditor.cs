/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Instrumentation.Api;

namespace Db4objects.Db4o.Instrumentation.Api
{
	/// <summary>Cross platform interface for type instrumentation.</summary>
	/// <remarks>Cross platform interface for type instrumentation.</remarks>
	public interface ITypeEditor
	{
		ITypeRef Type
		{
			get;
		}

		IReferenceProvider References
		{
			get;
		}

		void AddInterface(ITypeRef type);

		IMethodBuilder NewPublicMethod(string methodName, ITypeRef returnType, ITypeRef[]
			 parameterTypes);
	}
}
