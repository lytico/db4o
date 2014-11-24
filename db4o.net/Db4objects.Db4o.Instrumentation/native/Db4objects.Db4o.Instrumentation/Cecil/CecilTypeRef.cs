using System;
using Db4objects.Db4o.Instrumentation.Api;
using Mono.Cecil;

namespace Db4objects.Db4o.Instrumentation.Cecil
{
	public class CecilTypeRef : CecilRef<TypeReference>, ITypeRef
	{
		public CecilTypeRef(CecilReferenceProvider provider, TypeReference type) : base(provider, type)
		{
		}

		public bool IsPrimitive
		{
			get
			{	
				switch (_reference.FullName)
				{
					case "System.Int32":
					case "System.Boolean":
						return true;
				}
				return false;
			}
		}

		public ITypeRef ElementType
		{
			get { throw new NotImplementedException(); }
		}

		public override string Name
		{
			get { return NormalizeNestedTypeNotation(_reference.FullName); }
		}

		private static string NormalizeNestedTypeNotation(string fullName)
		{
			return fullName.Replace('/', '+');
		}
	}
}