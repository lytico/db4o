/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o;
using Db4objects.Db4o.Internal.Cluster;
using Db4objects.Db4o.Internal.Query;
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Internal.Cluster
{
	/// <exclude></exclude>
	public class ClusterQuery : IQuery
	{
		private readonly Db4objects.Db4o.Cluster.Cluster _cluster;

		private readonly IQuery[] _queries;

		public ClusterQuery(Db4objects.Db4o.Cluster.Cluster cluster, IQuery[] queries)
		{
			_cluster = cluster;
			_queries = queries;
		}

		public virtual IConstraint Constrain(object constraint)
		{
			lock (_cluster)
			{
				IConstraint[] constraints = new IConstraint[_queries.Length];
				for (int i = 0; i < constraints.Length; i++)
				{
					constraints[i] = _queries[i].Constrain(constraint);
				}
				return new ClusterConstraint(_cluster, constraints);
			}
		}

		public virtual IConstraints Constraints()
		{
			lock (_cluster)
			{
				IConstraint[] constraints = new IConstraint[_queries.Length];
				for (int i = 0; i < constraints.Length; i++)
				{
					constraints[i] = _queries[i].Constraints();
				}
				return new ClusterConstraints(_cluster, constraints);
			}
		}

		public virtual IQuery Descend(string fieldName)
		{
			lock (_cluster)
			{
				IQuery[] queries = new IQuery[_queries.Length];
				for (int i = 0; i < queries.Length; i++)
				{
					queries[i] = _queries[i].Descend(fieldName);
				}
				return new Db4objects.Db4o.Internal.Cluster.ClusterQuery(_cluster, queries);
			}
		}

		public virtual IObjectSet Execute()
		{
			lock (_cluster)
			{
				return new ObjectSetFacade(new ClusterQueryResult(_cluster, _queries));
			}
		}

		public virtual IQuery OrderAscending()
		{
			throw new NotSupportedException();
		}

		public virtual IQuery OrderDescending()
		{
			throw new NotSupportedException();
		}

		public virtual IQuery SortBy(IQueryComparator comparator)
		{
			// FIXME
			throw new NotSupportedException();
		}
		// FIXME
	}
}
