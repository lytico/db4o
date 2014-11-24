/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o;

namespace Db4objects.Db4o.Ext
{
	/// <summary>
	/// extended functionality for the
	/// <see cref="Db4objects.Db4o.IObjectSet">IObjectSet</see>
	/// interface.
	/// <br /><br />Every db4o
	/// <see cref="Db4objects.Db4o.IObjectSet">IObjectSet</see>
	/// always is an ExtObjectSet so a cast is possible.<br /><br />
	/// <see cref="Db4objects.Db4o.IObjectSet.Ext()">Db4objects.Db4o.IObjectSet.Ext()</see>
	/// is a convenient method to perform the cast.<br /><br />
	/// The ObjectSet functionality is split to two interfaces to allow newcomers to
	/// focus on the essential methods.
	/// </summary>
	public interface IExtObjectSet : IObjectSet
	{
		/// <summary>returns an array of internal IDs that correspond to the contained objects.
		/// 	</summary>
		/// <remarks>
		/// returns an array of internal IDs that correspond to the contained objects.
		/// <br /><br />
		/// </remarks>
		/// <seealso cref="IExtObjectContainer.GetID(object)">IExtObjectContainer.GetID(object)
		/// 	</seealso>
		/// <seealso cref="IExtObjectContainer.GetByID(long)">IExtObjectContainer.GetByID(long)
		/// 	</seealso>
		long[] GetIDs();

		/// <summary>returns the item at position [index] in this ObjectSet.</summary>
		/// <remarks>
		/// returns the item at position [index] in this ObjectSet.
		/// <br /><br />
		/// The object will be activated.
		/// </remarks>
		/// <param name="index">the index position in this ObjectSet.</param>
		/// <returns>the activated object.</returns>
		object Get(int index);

		/// <summary>skips the specified number of objects without activating them.</summary>
		/// <remarks>
		/// skips the specified number of objects without activating them.
		/// Call this method before starting iterating over the iterator returned by
		/// <see cref="System.Collections.IList{E}.GetEnumerator()">System.Collections.IList&lt;E&gt;.GetEnumerator()
		/// 	</see>
		/// .
		/// This method has no effect on calls to
		/// <see cref="Get(int)">Get(int)</see>
		/// 
		/// </remarks>
		/// <since>8.1</since>
		void Skip(int count);
	}
}
