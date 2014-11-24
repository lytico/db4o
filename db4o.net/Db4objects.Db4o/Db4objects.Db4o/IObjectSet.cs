/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Ext;

namespace Db4objects.Db4o
{
	/// <summary>An ObjectSet is a representation for a set of objects returned  by a query.
	/// 	</summary>
	/// <remarks>
	/// An ObjectSet is a representation for a set of objects returned  by a query.
	/// <br /><br />
	/// ObjectSet extends the list interface. It is
	/// recommended to never reference ObjectSet directly in code but to use the list interface instead.
	/// <br /><br />
	/// Note that the underlying
	/// <see cref="IObjectContainer">IObjectContainer</see>
	/// of an ObjectSet
	/// needs to remain open as long as an ObjectSet is used. This is necessary
	/// for lazy instantiation. The objects in an ObjectSet are only instantiated
	/// when they are actually being used by the application. In case you want to use a query
	/// result after the object container has been closed, you need to copy the result set.
	/// </remarks>
	/// <seealso cref="Db4objects.Db4o.Ext.IExtObjectSet">for extended functionality.</seealso>
	public interface IObjectSet : IList, IEnumerable
	{
		/// <summary>Returns an ObjectSet with extended functionality.</summary>
		/// <remarks>
		/// Returns an ObjectSet with extended functionality.
		/// <br /><br />
		/// Every ObjectSet that db4o provides can be casted to
		/// an ExtObjectSet. This method is supplied for your convenience
		/// to work without a cast.
		/// <br /><br />
		/// The ObjectSet functionality is split to two interfaces
		/// to allow newcomers to focus on the essential methods.
		/// </remarks>
		IExtObjectSet Ext();

		/// <summary>Returns true if the ObjectSet has more elements.</summary>
		/// <remarks>Returns true if the ObjectSet has more elements.</remarks>
		/// <returns>
		/// boolean - true if the ObjectSet has more
		/// elements.
		/// </returns>
		bool HasNext();

		/// <summary>Returns the next object in the ObjectSet.</summary>
		/// <remarks>
		/// Returns the next object in the ObjectSet.
		/// <br /><br />
		/// Before returning the object, next() triggers automatic activation of the
		/// object with the respective
		/// <see cref="Db4objects.Db4o.Config.ICommonConfiguration.ActivationDepth()">global</see>
		/// or
		/// <see cref="Db4objects.Db4o.Config.IObjectClass.MaximumActivationDepth(int)">class specific
		/// 	</see>
		/// setting.
		/// <br /><br />
		/// </remarks>
		/// <returns>the next object in the ObjectSet.</returns>
		object Next();

		/// <summary>Resets the ObjectSet cursor before the first element.</summary>
		/// <remarks>
		/// Resets the ObjectSet cursor before the first element.
		/// A subsequent call to
		/// <see cref="Next()">Next()</see>
		/// will return the first element.
		/// </remarks>
		void Reset();
	}
}
