/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Config
{
	/// <summary>generic interface to allow returning an attribute of an object.</summary>
	/// <remarks>generic interface to allow returning an attribute of an object.</remarks>
	public interface IObjectAttribute
	{
		/// <summary>generic method to return an attribute of a parent object.</summary>
		/// <remarks>generic method to return an attribute of a parent object.</remarks>
		/// <param name="parent">the parent object</param>
		/// <returns>Object - the attribute</returns>
		object Attribute(object parent);
	}
}
