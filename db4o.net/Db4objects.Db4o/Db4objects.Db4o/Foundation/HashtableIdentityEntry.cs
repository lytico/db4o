/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;
using Sharpen;

namespace Db4objects.Db4o.Foundation
{
	/// <exclude></exclude>
	public class HashtableIdentityEntry : HashtableIntEntry
	{
		public HashtableIdentityEntry(object obj) : base(Runtime.IdentityHashCode(obj), obj
			)
		{
		}

		public override bool SameKeyAs(HashtableIntEntry other)
		{
			if (!base.SameKeyAs(other))
			{
				return false;
			}
			return other._object == _object;
		}
	}
}
