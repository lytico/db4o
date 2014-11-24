/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Ext;

namespace Db4objects.Db4o.Ext
{
	/// <summary>the internal representation of a stored class.</summary>
	/// <remarks>the internal representation of a stored class.</remarks>
	public interface IStoredClass
	{
		/// <summary>returns the name of this stored class.</summary>
		/// <remarks>returns the name of this stored class.</remarks>
		string GetName();

		/// <summary>returns an array of IDs of all stored object instances of this stored class.
		/// 	</summary>
		/// <remarks>returns an array of IDs of all stored object instances of this stored class.
		/// 	</remarks>
		long[] GetIDs();

		/// <summary>returns the StoredClass for the parent of the class, this StoredClass represents.
		/// 	</summary>
		/// <remarks>returns the StoredClass for the parent of the class, this StoredClass represents.
		/// 	</remarks>
		IStoredClass GetParentStoredClass();

		/// <summary>returns all stored fields of this stored class.</summary>
		/// <remarks>returns all stored fields of this stored class.</remarks>
		IStoredField[] GetStoredFields();

		/// <summary>returns true if this StoredClass has a class index.</summary>
		/// <remarks>returns true if this StoredClass has a class index.</remarks>
		bool HasClassIndex();

		/// <summary>renames this stored class.</summary>
		/// <remarks>
		/// renames this stored class.
		/// <br /><br />After renaming one or multiple classes the ObjectContainer has
		/// to be closed and reopened to allow internal caches to be refreshed.
		/// <br /><br />.NET: As the name you should provide [Classname, Assemblyname]<br /><br />
		/// </remarks>
		/// <param name="name">the new name</param>
		void Rename(string name);

		// TODO: add field creation
		/// <summary>returns an existing stored field of this stored class.</summary>
		/// <remarks>returns an existing stored field of this stored class.</remarks>
		/// <param name="name">the name of the field</param>
		/// <param name="type">
		/// the type of the field.
		/// There are four possibilities how to supply the type:<br />
		/// - a Class object.  (.NET: a Type object)<br />
		/// - a fully qualified classname.<br />
		/// - any object to be used as a template.<br /><br />
		/// - null, if the first found field should be returned.
		/// </param>
		/// <returns>
		/// the
		/// <see cref="IStoredField">IStoredField</see>
		/// </returns>
		IStoredField StoredField(string name, object type);

		/// <summary>
		/// Returns the number of instances of this class that have been persisted to the
		/// database, as seen by the transaction (container) that produces this StoredClass
		/// instance.
		/// </summary>
		/// <remarks>
		/// Returns the number of instances of this class that have been persisted to the
		/// database, as seen by the transaction (container) that produces this StoredClass
		/// instance.
		/// </remarks>
		/// <returns>The number of instances</returns>
		int InstanceCount();
	}
}
