/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Ext
{
	/// <summary>the internal representation of a field on a stored class.</summary>
	/// <remarks>the internal representation of a field on a stored class.</remarks>
	public interface IStoredField
	{
		/// <summary>creates an index on this field at runtime.</summary>
		/// <remarks>creates an index on this field at runtime.</remarks>
		void CreateIndex();

		/// <summary>drops an existing index on this field at runtime.</summary>
		/// <remarks>drops an existing index on this field at runtime.</remarks>
		void DropIndex();

		/// <summary>returns the field value on the passed object.</summary>
		/// <remarks>
		/// returns the field value on the passed object.
		/// <br /><br />This method will also work, if the field is not present in the current
		/// version of the class.
		/// <br /><br />It is recommended to use this method for refactoring purposes, if fields
		/// are removed and the field values need to be copied to other fields.
		/// </remarks>
		object Get(object onObject);

		/// <summary>returns the name of the field.</summary>
		/// <remarks>returns the name of the field.</remarks>
		string GetName();

		/// <summary>returns the Class (Java) / Type (.NET) of the field.</summary>
		/// <remarks>
		/// returns the Class (Java) / Type (.NET) of the field.
		/// <br /><br />For array fields this method will return the type of the array.
		/// Use
		/// <see cref="IsArray()">IsArray()</see>
		/// to detect arrays.
		/// </remarks>
		IReflectClass GetStoredType();

		/// <summary>returns true if the field is an array.</summary>
		/// <remarks>returns true if the field is an array.</remarks>
		bool IsArray();

		/// <summary>modifies the name of this stored field.</summary>
		/// <remarks>
		/// modifies the name of this stored field.
		/// <br /><br />After renaming one or multiple fields the ObjectContainer has
		/// to be closed and reopened to allow internal caches to be refreshed.<br /><br />
		/// </remarks>
		/// <param name="name">the new name</param>
		void Rename(string name);

		/// <summary>
		/// specialized highspeed API to collect all values of a field for all instances
		/// of a class, if the field is indexed.
		/// </summary>
		/// <remarks>
		/// specialized highspeed API to collect all values of a field for all instances
		/// of a class, if the field is indexed.
		/// <br /><br />The field values will be taken directly from the index without the
		/// detour through class indexes or object instantiation.
		/// <br /><br />
		/// If this method is used to get the values of a first class object index,
		/// deactivated objects will be passed to the visitor.
		/// </remarks>
		/// <param name="visitor">the visitor to be called with each index value.</param>
		void TraverseValues(IVisitor4 visitor);

		/// <summary>Returns whether this field has an index or not.</summary>
		/// <remarks>Returns whether this field has an index or not.</remarks>
		/// <returns>true if this field has an index.</returns>
		bool HasIndex();
		//  will need for replication. Requested for 3.0 
		//	
		//	/**
		//	 * sets the field value on the passed object.
		//	 * @param onObject
		//	 * @param fieldValue
		//	 */
		//	public void set(Object onObject, Object fieldValue);
	}
}
