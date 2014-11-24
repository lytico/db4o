/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Internal.Handlers
{
	/// <exclude></exclude>
	public class TypeHandlerPredicatePair
	{
		public readonly ITypeHandlerPredicate _predicate;

		public readonly ITypeHandler4 _typeHandler;

		public TypeHandlerPredicatePair(ITypeHandlerPredicate predicate, ITypeHandler4 typeHandler
			)
		{
			_predicate = predicate;
			_typeHandler = typeHandler;
		}
	}
}
