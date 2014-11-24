using System;
using System.IO;
using System.Reflection;
using Db4objects.Db4o.Instrumentation.Api;
using Mono.Cecil;
using MethodAttributes=Mono.Cecil.MethodAttributes;

namespace Db4objects.Db4o.Instrumentation.Cecil
{
	public class CecilTypeEditor : ITypeEditor
	{
		private readonly TypeDefinition _type;
		private readonly CecilReferenceProvider _references;

		public CecilTypeEditor(TypeDefinition type)
		{
			_type = type;
			_references = CecilReferenceProvider.ForModule(type.Module.Assembly.MainModule);
		}

		public ITypeRef Type
		{
			get { return _references.ForCecilType(_type); }
		}

		public IReferenceProvider References
		{
			get { return _references; }
		}

		public void AddInterface(ITypeRef type)
		{
			_type.Interfaces.Add(GetTypeReference(type));
		}

		public IMethodBuilder NewPublicMethod(string methodName, ITypeRef returnType, ITypeRef[] parameterTypes)
		{
			MethodDefinition method = NewMethod(methodName, parameterTypes, returnType);
			_type.Methods.Add(method);
			return new CecilMethodBuilder(method);
		}

		private static MethodDefinition NewMethod(string methodName, ITypeRef[] parameterTypes, ITypeRef returnType)
		{
			MethodDefinition method = new MethodDefinition(methodName,
			                                               MethodAttributes.Virtual | MethodAttributes.Public,
			                                               GetTypeReference(returnType));
			foreach (ITypeRef paramType in parameterTypes)
			{
				method.Parameters.Add(new ParameterDefinition(GetTypeReference(paramType)));
			}
			return method;
		}

		private static TypeReference GetTypeReference(ITypeRef type)
		{
			return CecilTypeRef.GetReference(type);
		}
	}
}
