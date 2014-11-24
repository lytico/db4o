/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Foundation
{
	/// <summary>Deep clone</summary>
	/// <exclude></exclude>
	public interface IDeepClone
	{
		/// <summary>
		/// The parameter allows passing one new object so parent
		/// references can be corrected on children.
		/// </summary>
		/// <remarks>
		/// The parameter allows passing one new object so parent
		/// references can be corrected on children.
		/// </remarks>
		object DeepClone(object context);
	}
}
