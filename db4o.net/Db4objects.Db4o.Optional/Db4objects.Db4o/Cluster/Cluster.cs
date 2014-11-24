/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o;
using Db4objects.Db4o.Internal.Cluster;
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Cluster
{
	/// <summary>allows running Queries against multiple ObjectContainers.</summary>
	/// <remarks>allows running Queries against multiple ObjectContainers.</remarks>
	/// <exclude></exclude>
	public class Cluster
	{
		public readonly IObjectContainer[] _objectContainers;

		/// <summary>
		/// use this constructor to create a Cluster and call
		/// add() to add ObjectContainers
		/// </summary>
		public Cluster(IObjectContainer[] objectContainers)
		{
			if (objectContainers == null)
			{
				throw new ArgumentNullException();
			}
			if (objectContainers.Length < 1)
			{
				throw new ArgumentException();
			}
			for (int i = 0; i < objectContainers.Length; i++)
			{
				if (objectContainers[i] == null)
				{
					throw new ArgumentException();
				}
			}
			_objectContainers = objectContainers;
		}

		/// <summary>
		/// starts a query against all ObjectContainers in
		/// this Cluster.
		/// </summary>
		/// <remarks>
		/// starts a query against all ObjectContainers in
		/// this Cluster.
		/// </remarks>
		/// <returns>the Query</returns>
		public virtual IQuery Query()
		{
			lock (this)
			{
				IQuery[] queries = new IQuery[_objectContainers.Length];
				for (int i = 0; i < _objectContainers.Length; i++)
				{
					queries[i] = _objectContainers[i].Query();
				}
				return new ClusterQuery(this, queries);
			}
		}

		/// <summary>
		/// returns the ObjectContainer in this cluster where the passed object
		/// is stored or null, if the object is not stored to any ObjectContainer
		/// in this cluster
		/// </summary>
		/// <param name="obj">the object</param>
		/// <returns>the ObjectContainer</returns>
		public virtual IObjectContainer ObjectContainerFor(object obj)
		{
			lock (this)
			{
				for (int i = 0; i < _objectContainers.Length; i++)
				{
					if (_objectContainers[i].Ext().IsStored(obj))
					{
						return _objectContainers[i];
					}
				}
			}
			return null;
		}
	}
}
