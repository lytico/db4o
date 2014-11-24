using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Db4objects.Db4o.Instrumentation.Api;
using Mono.Cecil;

namespace Db4objects.Db4o.Instrumentation.Cecil
{
	public class CecilReferenceProvider : IReferenceProvider
	{
		public static CecilReferenceProvider ForModule(ModuleDefinition module)
		{
			return new CecilReferenceProvider(module);
		}

		private readonly ModuleDefinition _module;
		private readonly Dictionary<TypeReference, ITypeRef> _typeCache = new Dictionary<TypeReference, ITypeRef>();

		private CecilReferenceProvider(ModuleDefinition module)
		{
			if (null == module) throw new ArgumentNullException();
			_module = module;
		}

		public ITypeRef ForType(Type type)
		{
#if CF
			return ForCecilType(ImportType(type));
#else
			return ForCecilType(_module.Import(type));
#endif
		}

		public ITypeRef ForCecilType(TypeReference type)
		{
			ITypeRef typeRef;
			if (!_typeCache.TryGetValue(type, out typeRef))
			{
				typeRef = new CecilTypeRef(this, type);
				_typeCache.Add(type, typeRef);
			}
			return typeRef;
		}

		public IMethodRef ForMethod(MethodInfo method)
		{
#if CF
			return new CecilMethodRef(this, _module.Import(new MethodReference(method.Name, ImportType(method.ReturnType))));
#else
			return new CecilMethodRef(this, _module.Import(method));
#endif
		}

#if CF
		private TypeReference ImportType(Type type)
		{
			return _module.Import(new TypeReference(type.Namespace, type.Name, _module, _module));
		}
#endif

		public IMethodRef ForMethod(ITypeRef declaringType, string methodName, ITypeRef[] parameterTypes, ITypeRef returnType)
		{
			throw new NotImplementedException();
		}

		public IFieldRef ForCecilField(FieldReference field)
		{
			return new CecilFieldRef(this, field);
		}
	}
}
