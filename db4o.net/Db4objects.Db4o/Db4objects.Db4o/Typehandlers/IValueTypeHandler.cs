/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Typehandlers
{
	public interface IValueTypeHandler : ITypeHandler4
	{
		/// <summary>gets called when an value type is to be read from the database.</summary>
		/// <remarks>gets called when an value type is to be read from the database.</remarks>
		/// <param name="context"></param>
		/// <returns>the read value type</returns>
		object Read(IReadContext context);
	}
}
