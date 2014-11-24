/* This file is part of the db4o object database http://www.db4o.com

Copyright (C) 2004 - 2009  Versant Corporation http://www.versant.com

db4o is free software; you can redistribute it and/or modify it under
the terms of version 3 of the GNU General Public License as published
by the Free Software Foundation.

db4o is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or
FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
for more details.

You should have received a copy of the GNU General Public License along
with this program.  If not, see http://www.gnu.org/licenses/. */
using System;
using System.Collections;

namespace Sharpen.Util
{
	public class IdentityHashMap : Hashtable
	{
		public IdentityHashMap() : base(EqualityComparer.Default)
		{	
		}

		public override void Add(object key, object value)
		{
			this[key] = value;
		}

		class EqualityComparer : IEqualityComparer
		{
			public static readonly IEqualityComparer Default = new EqualityComparer();

			public bool Equals(object x, object y)
			{
				return x == y;
			}

			public int GetHashCode(object obj)
			{
				return Sharpen.Runtime.IdentityHashCode(obj);
			}
		}
	}
}
