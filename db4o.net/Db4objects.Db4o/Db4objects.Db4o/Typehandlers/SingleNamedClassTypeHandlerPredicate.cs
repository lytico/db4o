/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Typehandlers
{
	/// <summary>allows installing a Typehandler for a single classname.</summary>
	/// <remarks>allows installing a Typehandler for a single classname.</remarks>
	public sealed class SingleNamedClassTypeHandlerPredicate : ITypeHandlerPredicate
	{
		private readonly string _className;

		public SingleNamedClassTypeHandlerPredicate(string className)
		{
			_className = className;
		}

		public bool Match(IReflectClass candidate)
		{
			return candidate.GetName().Equals(_className);
		}
	}
}
