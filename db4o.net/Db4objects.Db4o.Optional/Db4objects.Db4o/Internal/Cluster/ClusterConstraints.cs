/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal.Cluster;
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Internal.Cluster
{
	/// <exclude></exclude>
	public class ClusterConstraints : ClusterConstraint, IConstraints
	{
		public ClusterConstraints(Db4objects.Db4o.Cluster.Cluster cluster, IConstraint[] 
			constraints) : base(cluster, constraints)
		{
		}

		public virtual IConstraint[] ToArray()
		{
			lock (_cluster)
			{
				Collection4 all = new Collection4();
				for (int i = 0; i < _constraints.Length; i++)
				{
					ClusterConstraint c = (ClusterConstraint)_constraints[i];
					for (int j = 0; j < c._constraints.Length; j++)
					{
						all.Add(c._constraints[j]);
					}
				}
				IConstraint[] res = new IConstraint[all.Size()];
				all.ToArray(res);
				return res;
			}
		}
	}
}
