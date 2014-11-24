/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */

using System;
using System.Reflection;
using Db4oTool.Core;
using Mono.Cecil;
using MethodAttributes=Mono.Cecil.MethodAttributes;

namespace Db4oTool.TA
{
	internal class MethodEmitter
	{
		protected FieldReference _activatorField;
		protected InstrumentationContext _context;

		public MethodEmitter(InstrumentationContext context, FieldReference field)
		{	
			_context = context;
			_activatorField = FieldReferenceFor(field);
		}

		private static FieldReference FieldReferenceFor(FieldReference field)
		{
			if (!IsGeneric(field.DeclaringType)) return field;
			FieldReference reference = new FieldReference(field.Name, field.FieldType);
			reference.DeclaringType = GenericReferenceFor(field.DeclaringType);
			return reference;
		}

		private static bool IsGeneric(TypeReference type)
		{
			return type.GenericParameters.Count > 0;
		}

		private static TypeReference GenericReferenceFor(TypeReference type)
		{
			GenericInstanceType instance = new GenericInstanceType(type);
			foreach (GenericParameter param in type.GenericParameters)
			{
				instance.GenericArguments.Add(param);
			}
			return instance;
		}

		protected MethodDefinition NewExplicitMethod(MethodInfo method)
		{
			MethodAttributes attributes = MethodAttributes.SpecialName|MethodAttributes.Private|MethodAttributes.Virtual;
			MethodDefinition definition = new MethodDefinition(method.DeclaringType.FullName + "." + method.Name, attributes, Import(method.ReturnType));
			foreach (ParameterInfo pi in method.GetParameters())
			{
				definition.Parameters.Add(new ParameterDefinition(pi.Name, Mono.Cecil.ParameterAttributes.None, Import(pi.ParameterType)));
			}
			definition.Overrides.Add(_context.Import(method));
			return definition;
		}

		protected TypeReference Import(Type type)
		{
			return _context.Import(type);
		}
	}
}