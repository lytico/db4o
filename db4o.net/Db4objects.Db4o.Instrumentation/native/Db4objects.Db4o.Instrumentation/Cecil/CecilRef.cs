using Db4objects.Db4o.Instrumentation.Api;
using Mono.Cecil;

namespace Db4objects.Db4o.Instrumentation.Cecil
{
	public class CecilRef<T> where T : MemberReference
	{
		public static T GetReference(object type)
		{
			return ((CecilRef<T>)type).Reference;
		}

		private readonly CecilReferenceProvider _provider;
		protected T _reference;

	    public CecilRef(CecilReferenceProvider provider, T reference)
		{
			_provider = provider;
			_reference = reference;
		}

		protected ITypeRef TypeRef(TypeReference type)
		{
			return _provider.ForCecilType(type);
		}

		public T Reference
		{
			get { return _reference; }
		    set { _reference = value; }
		}

		public virtual string Name
		{
			get { return _reference.Name;  }
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
