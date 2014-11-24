using System;
using Db4objects.Db4o.Instrumentation.Api;
using Mono.Cecil;

namespace Db4objects.Db4o.Instrumentation.Cecil
{
	public class CecilFieldRef : CecilRef<FieldReference>, IFieldRef
	{
		public CecilFieldRef(CecilReferenceProvider provider, FieldReference field) : base(provider, field)
		{	
		}

		public FieldReference Field
		{
			get { return _reference; }
		}

		public TypeReference FieldType
		{
			get { return _reference.FieldType; }
		}

		public ITypeRef Type
		{
			get { return TypeRef(FieldType); }
		}
	}
}
