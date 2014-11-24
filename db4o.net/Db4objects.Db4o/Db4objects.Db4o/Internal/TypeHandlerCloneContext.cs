/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public class TypeHandlerCloneContext
	{
		private readonly HandlerRegistry handlerRegistry;

		public readonly ITypeHandler4 original;

		private readonly int version;

		public TypeHandlerCloneContext(HandlerRegistry handlerRegistry_, ITypeHandler4 original_
			, int version_)
		{
			handlerRegistry = handlerRegistry_;
			original = original_;
			version = version_;
		}

		public virtual ITypeHandler4 CorrectHandlerVersion(ITypeHandler4 typeHandler)
		{
			return handlerRegistry.CorrectHandlerVersion(typeHandler, version);
		}
	}
}
