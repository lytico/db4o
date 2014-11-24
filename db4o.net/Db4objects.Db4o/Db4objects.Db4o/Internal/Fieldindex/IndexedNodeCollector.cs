/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal.Fieldindex;
using Db4objects.Db4o.Internal.Query.Processor;

namespace Db4objects.Db4o.Internal.Fieldindex
{
	public class IndexedNodeCollector
	{
		private readonly Collection4 _nodes;

		private readonly Hashtable4 _nodeCache;

		public IndexedNodeCollector(QCandidates candidates)
		{
			_nodes = new Collection4();
			_nodeCache = new Hashtable4();
			CollectIndexedNodes(candidates);
		}

		public virtual IEnumerator GetNodes()
		{
			return _nodes.GetEnumerator();
		}

		private void CollectIndexedNodes(QCandidates candidates)
		{
			CollectIndexedNodes(candidates.IterateConstraints());
			ImplicitlyAndJoinsOnSameField();
		}

		private void ImplicitlyAndJoinsOnSameField()
		{
			object[] nodes = _nodes.ToArray();
			for (int i = 0; i < nodes.Length; i++)
			{
				object node = nodes[i];
				if (node is OrIndexedLeaf)
				{
					OrIndexedLeaf current = (OrIndexedLeaf)node;
					OrIndexedLeaf other = FindJoinOnSameFieldAtSameLevel(current);
					if (null != other)
					{
						nodes[Arrays4.IndexOfIdentity(nodes, other)] = null;
						CollectImplicitAnd(current.GetConstraint(), current, other);
					}
				}
			}
		}

		private OrIndexedLeaf FindJoinOnSameFieldAtSameLevel(OrIndexedLeaf join)
		{
			IEnumerator i = _nodes.GetEnumerator();
			while (i.MoveNext())
			{
				if (i.Current == join)
				{
					continue;
				}
				if (i.Current is OrIndexedLeaf)
				{
					OrIndexedLeaf current = (OrIndexedLeaf)i.Current;
					if (current.GetIndex() == join.GetIndex() && ParentConstraint(current) == ParentConstraint
						(join))
					{
						return current;
					}
				}
			}
			return null;
		}

		private object ParentConstraint(OrIndexedLeaf node)
		{
			return node.GetConstraint().Parent();
		}

		private void CollectIndexedNodes(IEnumerator qcons)
		{
			while (qcons.MoveNext())
			{
				QCon qcon = (QCon)qcons.Current;
				if (IsCached(qcon))
				{
					continue;
				}
				if (IsLeaf(qcon))
				{
					if (qcon.CanLoadByIndex() && qcon.CanBeIndexLeaf())
					{
						QConObject conObject = (QConObject)qcon;
						if (conObject.HasJoins())
						{
							CollectJoinedNode(conObject);
						}
						else
						{
							CollectStandaloneNode(conObject);
						}
					}
				}
				else
				{
					if (!qcon.HasJoins())
					{
						CollectIndexedNodes(qcon.IterateChildren());
					}
				}
			}
		}

		private bool IsCached(QCon qcon)
		{
			return null != _nodeCache.Get(qcon);
		}

		private void CollectStandaloneNode(QConObject conObject)
		{
			IndexedLeaf existing = FindLeafOnSameField(conObject);
			if (existing != null)
			{
				CollectImplicitAnd(conObject, existing, new IndexedLeaf(conObject));
			}
			else
			{
				_nodes.Add(new IndexedLeaf(conObject));
			}
		}

		private void CollectJoinedNode(QConObject constraintWithJoins)
		{
			Collection4 joins = CollectTopLevelJoins(constraintWithJoins);
			if (!CanJoinsBeSearchedByIndex(joins))
			{
				return;
			}
			if (1 == joins.Size())
			{
				_nodes.Add(NodeForConstraint((QCon)joins.SingleElement()));
				return;
			}
			CollectImplicitlyAndingJoins(joins, constraintWithJoins);
		}

		private bool AllHaveSamePath(Collection4 leaves)
		{
			IEnumerator i = leaves.GetEnumerator();
			i.MoveNext();
			QCon first = (QCon)i.Current;
			while (i.MoveNext())
			{
				if (!HaveSamePath(first, (QCon)i.Current))
				{
					return false;
				}
			}
			return true;
		}

		private bool HaveSamePath(QCon x, QCon y)
		{
			if (x == y)
			{
				return true;
			}
			if (!x.OnSameFieldAs(y))
			{
				return false;
			}
			if (!x.HasParent())
			{
				return !y.HasParent();
			}
			return HaveSamePath(x.Parent(), y.Parent());
		}

		private Collection4 CollectLeaves(Collection4 joins)
		{
			Collection4 leaves = new Collection4();
			CollectLeaves(leaves, joins);
			return leaves;
		}

		private void CollectLeaves(Collection4 leaves, Collection4 joins)
		{
			IEnumerator i = joins.GetEnumerator();
			while (i.MoveNext())
			{
				QConJoin join = ((QConJoin)i.Current);
				CollectLeavesFromJoin(leaves, join);
			}
		}

		private void CollectLeavesFromJoin(Collection4 leaves, QConJoin join)
		{
			CollectLeavesFromJoinConstraint(leaves, join.Constraint1());
			CollectLeavesFromJoinConstraint(leaves, join.Constraint2());
		}

		private void CollectLeavesFromJoinConstraint(Collection4 leaves, QCon constraint)
		{
			if (constraint is QConJoin)
			{
				CollectLeavesFromJoin(leaves, (QConJoin)constraint);
			}
			else
			{
				if (!leaves.ContainsByIdentity(constraint))
				{
					leaves.Add(constraint);
				}
			}
		}

		private bool CanJoinsBeSearchedByIndex(Collection4 joins)
		{
			Collection4 leaves = CollectLeaves(joins);
			return AllHaveSamePath(leaves) && AllCanBeSearchedByIndex(leaves);
		}

		private bool AllCanBeSearchedByIndex(Collection4 leaves)
		{
			IEnumerator i = leaves.GetEnumerator();
			while (i.MoveNext())
			{
				QCon leaf = ((QCon)i.Current);
				if (!leaf.CanLoadByIndex())
				{
					return false;
				}
			}
			return true;
		}

		private void CollectImplicitlyAndingJoins(Collection4 joins, QConObject constraintWithJoins
			)
		{
			IEnumerator i = joins.GetEnumerator();
			i.MoveNext();
			IIndexedNodeWithRange last = NodeForConstraint((QCon)i.Current);
			while (i.MoveNext())
			{
				IIndexedNodeWithRange node = NodeForConstraint((QCon)i.Current);
				last = new AndIndexedLeaf(constraintWithJoins, node, last);
				_nodes.Add(last);
			}
		}

		private Collection4 CollectTopLevelJoins(QConObject constraintWithJoins)
		{
			Collection4 joins = new Collection4();
			CollectTopLevelJoins(joins, constraintWithJoins);
			return joins;
		}

		private void CollectTopLevelJoins(Collection4 joins, QCon constraintWithJoins)
		{
			IEnumerator i = constraintWithJoins.IterateJoins();
			while (i.MoveNext())
			{
				QConJoin join = (QConJoin)i.Current;
				if (!join.HasJoins())
				{
					if (!joins.ContainsByIdentity(join))
					{
						joins.Add(join);
					}
				}
				else
				{
					CollectTopLevelJoins(joins, join);
				}
			}
		}

		private IIndexedNodeWithRange NewNodeForConstraint(QConJoin join)
		{
			IIndexedNodeWithRange c1 = NodeForConstraint(join.Constraint1());
			IIndexedNodeWithRange c2 = NodeForConstraint(join.Constraint2());
			if (join.IsOr())
			{
				return new OrIndexedLeaf(FindLeafForJoin(join), c1, c2);
			}
			return new AndIndexedLeaf(join.Constraint1(), c1, c2);
		}

		private QCon FindLeafForJoin(QConJoin join)
		{
			if (join.Constraint1() is QConObject)
			{
				return join.Constraint1();
			}
			QCon con = join.Constraint2();
			if (con is QConObject)
			{
				return con;
			}
			return FindLeafForJoin((QConJoin)con);
		}

		private IIndexedNodeWithRange NodeForConstraint(QCon con)
		{
			IIndexedNodeWithRange node = (IIndexedNodeWithRange)_nodeCache.Get(con);
			if (null != node || _nodeCache.ContainsKey(con))
			{
				return node;
			}
			node = NewNodeForConstraint(con);
			_nodeCache.Put(con, node);
			return node;
		}

		private IIndexedNodeWithRange NewNodeForConstraint(QCon con)
		{
			if (con is QConJoin)
			{
				return NewNodeForConstraint((QConJoin)con);
			}
			return new IndexedLeaf((QConObject)con);
		}

		private void CollectImplicitAnd(QCon constraint, IIndexedNodeWithRange x, IIndexedNodeWithRange
			 y)
		{
			_nodes.Remove(x);
			_nodes.Remove(y);
			_nodes.Add(new AndIndexedLeaf(constraint, x, y));
		}

		private IndexedLeaf FindLeafOnSameField(QConObject conObject)
		{
			IEnumerator i = _nodes.GetEnumerator();
			while (i.MoveNext())
			{
				if (i.Current is IndexedLeaf)
				{
					IndexedLeaf leaf = (IndexedLeaf)i.Current;
					if (conObject.OnSameFieldAs(leaf.Constraint()))
					{
						return leaf;
					}
				}
			}
			return null;
		}

		private bool IsLeaf(QCon qcon)
		{
			return !qcon.HasChildren();
		}
	}
}
