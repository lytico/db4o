/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Ext
{
	/// <summary>generic callback interface.</summary>
	/// <remarks>generic callback interface.</remarks>
	public interface IDb4oCallback
	{
		/// <summary>the callback method</summary>
		/// <param name="obj">the object passed to the callback method</param>
		void Callback(object obj);
	}
}
