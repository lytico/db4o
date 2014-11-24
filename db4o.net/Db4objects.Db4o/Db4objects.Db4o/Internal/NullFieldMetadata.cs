/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Marshall;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public class NullFieldMetadata : FieldMetadata
	{
		public NullFieldMetadata() : base(null)
		{
		}

		/// <param name="obj"></param>
		public virtual IPreparedComparison PrepareComparison(object obj)
		{
			return Null.Instance;
		}

		public sealed override object Read(IObjectIdContext context)
		{
			return null;
		}
	}
}
