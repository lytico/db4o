/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.IO;
using System.Reflection;
using Db4objects.Db4o.Instrumentation.Api;

namespace Db4objects.Db4o.Instrumentation.Api
{
	/// <summary>Cross platform interface for bytecode emission.</summary>
	/// <remarks>Cross platform interface for bytecode emission.</remarks>
	public interface IMethodBuilder
	{
		IReferenceProvider References
		{
			get;
		}

		void Ldc(object value);

		void LoadArgument(int index);

		void Pop();

		void LoadArrayElement(ITypeRef elementType);

		void Add(ITypeRef operandType);

		void Subtract(ITypeRef operandType);

		void Multiply(ITypeRef operandType);

		void Divide(ITypeRef operandType);

		void Modulo(ITypeRef operandType);

		void Invoke(IMethodRef method, CallingConvention convention);

		void Invoke(MethodInfo method);

		void LoadField(IFieldRef fieldRef);

		void LoadStaticField(IFieldRef fieldRef);

		void Box(ITypeRef boxedType);

		void EndMethod();

		void Print(TextWriter @out);
	}
}
