/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Collections;

namespace Db4objects.Db4o.Collections
{
	/// <summary>
	/// Collection factory with methods to create collections with behaviour
	/// that is optimized for db4o.<br/><br/>
	/// Example usage:<br/>
	/// <code>CollectionFactory.forObjectContainer(objectContainer).newBigSet();</code>
	/// </summary>
	public class CollectionFactory
	{
		private readonly IObjectContainer _objectContainer;

		private CollectionFactory(IObjectContainer objectContainer)
		{
			_objectContainer = objectContainer;
		}

		/// <summary>returns a collection factory for an ObjectContainer</summary>
		/// <param name="objectContainer">- the ObjectContainer</param>
		/// <returns>the CollectionFactory</returns>
		public static Db4objects.Db4o.Collections.CollectionFactory ForObjectContainer(IObjectContainer
			 objectContainer)
		{
			if (IsClient(objectContainer))
			{
				throw new NotSupportedException("CollectionFactory is not yet available for Client/Server."
					);
			}
			return new Db4objects.Db4o.Collections.CollectionFactory(objectContainer);
		}

		/// <summary>
		/// creates a new BigSet.<br/><br/>
		/// Characteristics of BigSet:<br/>
		/// - It is optimized by using a BTree of IDs of persistent objects.<br/>
		/// - It can only hold persistent first class objects (no primitives, no strings, no objects that are not persistent)<br/>
		/// - Objects are activated upon getting them from the BigSet.
		/// </summary>
		/// <remarks>
		/// creates a new BigSet.<br/><br/>
		/// Characteristics of BigSet:<br/>
		/// - It is optimized by using a BTree of IDs of persistent objects.<br/>
		/// - It can only hold persistent first class objects (no primitives, no strings, no objects that are not persistent)<br/>
		/// - Objects are activated upon getting them from the BigSet.
		/// <br/><br/>
		/// BigSet is recommend whenever one object references a huge number of other objects and sorting is not required.
		/// </remarks>
		/// <returns></returns>
		public virtual Db4objects.Db4o.Collections.ISet<E> NewBigSet<E>()
		{
			return new BigSet<E>((LocalObjectContainer)_objectContainer);
		}

		private static bool IsClient(IObjectContainer oc)
		{
			return ((IInternalObjectContainer)oc).IsClient;
		}
	}
}
