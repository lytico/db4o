/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

using System;

namespace Db4objects.Db4o
{
    /// <summary>
    /// Marks a field or event as transient.
    /// </summary>
    /// <remarks>
    /// Transient fields are not stored by db4o.
    /// <br />
    /// If you don't want a field to be stored by db4o,
    /// simply mark it with this attribute.
    /// </remarks>
    /// <exclude />
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Event)]
    public class TransientAttribute : Attribute
	{
        public TransientAttribute()
		{
        }
    }
}
