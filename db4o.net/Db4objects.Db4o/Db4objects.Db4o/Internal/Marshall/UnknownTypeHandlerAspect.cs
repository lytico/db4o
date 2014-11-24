/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Internal.Marshall
{
	public class UnknownTypeHandlerAspect : FieldMetadata
	{
		public UnknownTypeHandlerAspect(ClassMetadata containingClass, string name) : base
			(containingClass, name)
		{
		}

		public override void DefragAspect(IDefragmentContext context)
		{
			throw new InvalidOperationException("Type handler for '" + ContainingClass() + "' could not be found. Defragment cannot proceed. "
				 + " Please ensure all required types are available and try again.");
		}

		public override bool Alive()
		{
			return false;
		}
	}
}
