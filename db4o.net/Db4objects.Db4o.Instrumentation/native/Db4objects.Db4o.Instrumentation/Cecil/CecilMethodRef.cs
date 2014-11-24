using System;
using Db4objects.Db4o.Instrumentation.Api;
using Mono.Cecil;

namespace Db4objects.Db4o.Instrumentation.Cecil
{
	internal class CecilMethodRef : CecilRef<MethodReference>, IMethodRef
	{
		public CecilMethodRef(CecilReferenceProvider provider, MethodReference method) : base(provider, method)
		{
		}

		public ITypeRef ReturnType
		{
			get { return TypeRef(_reference.ReturnType); }
		}

		public ITypeRef[] ParamTypes
		{
			get { throw new NotImplementedException(); }
		}

		public ITypeRef DeclaringType
		{
			get { throw new NotImplementedException(); }
		}
	}
}