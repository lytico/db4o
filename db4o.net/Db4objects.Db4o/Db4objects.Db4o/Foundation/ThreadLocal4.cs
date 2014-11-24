/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Sharpen.Lang;

namespace Db4objects.Db4o.Foundation
{
	/// <summary>
	/// ThreadLocal implementation for less capable platforms such as JRE 1.1 and
	/// Silverlight.
	/// </summary>
	/// <remarks>
	/// ThreadLocal implementation for less capable platforms such as JRE 1.1 and
	/// Silverlight.
	/// This class is not intended to be used directly, use
	/// <see cref="DynamicVariable">DynamicVariable</see>
	/// .
	/// WARNING: This implementation might leak Thread references unless
	/// <see cref="Set(object)">Set(object)</see>
	/// is called with null on the right thread to clean it up. This
	/// behavior is currently guaranteed by
	/// <see cref="DynamicVariable">DynamicVariable</see>
	/// .
	/// </remarks>
	public class ThreadLocal4
	{
		private readonly IDictionary _values = new Hashtable();

		public virtual void Set(object value)
		{
			lock (this)
			{
				if (value == null)
				{
					Sharpen.Collections.Remove(_values, Thread.CurrentThread());
				}
				else
				{
					_values[Thread.CurrentThread()] = value;
				}
			}
		}

		public virtual object Get()
		{
			lock (this)
			{
				return _values[Thread.CurrentThread()];
			}
		}

		protected object InitialValue()
		{
			return null;
		}
	}
}
