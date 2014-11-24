/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Qlin;

namespace Db4objects.Db4o.Qlin
{
	/// <summary>support for the new experimental QLin ("Coolin") query interface.</summary>
	/// <remarks>
	/// support for the new experimental QLin ("Coolin") query interface.
	/// We would really like to have LINQ for Java instead.
	/// </remarks>
	/// <since>8.0</since>
	public interface IQLinable
	{
		/// <summary>
		/// starts a
		/// <see cref="IQLin">IQLin</see>
		/// query against a class.
		/// </summary>
		IQLin From(Type clazz);
	}
}
