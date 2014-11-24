using System;
using System.Reflection;
using Db4objects.Db4o.Instrumentation.Api;

namespace Db4objects.Db4o.Instrumentation.Cecil
{
	public class CecilReferenceResolver : IReferenceResolver
	{
		public MethodInfo Resolve(IMethodRef methodRef)
		{
			throw new NotImplementedException();
		}
	}
}
