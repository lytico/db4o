/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Internal.Fieldindex;
using Db4objects.Db4o.Internal.Query.Processor;

namespace Db4objects.Db4o.Internal.Fieldindex
{
	public class FieldIndexProcessor
	{
		private readonly QCandidates _candidates;

		public FieldIndexProcessor(QCandidates candidates)
		{
			_candidates = candidates;
		}

		public virtual FieldIndexProcessorResult Run()
		{
			IIndexedNode bestIndex = SelectBestIndex();
			if (null == bestIndex)
			{
				return FieldIndexProcessorResult.NoIndexFound;
			}
			IIndexedNode resolved = ResolveFully(bestIndex);
			if (!bestIndex.IsEmpty())
			{
				bestIndex.MarkAsBestIndex(_candidates);
				return new FieldIndexProcessorResult(resolved);
			}
			return FieldIndexProcessorResult.FoundIndexButNoMatch;
		}

		private IIndexedNode ResolveFully(IIndexedNode indexedNode)
		{
			if (null == indexedNode)
			{
				return null;
			}
			if (indexedNode.IsResolved())
			{
				return indexedNode;
			}
			return ResolveFully(indexedNode.Resolve());
		}

		public virtual IIndexedNode SelectBestIndex()
		{
			IEnumerator i = CollectIndexedNodes();
			IIndexedNode best = null;
			while (i.MoveNext())
			{
				IIndexedNode indexedNode = (IIndexedNode)i.Current;
				IIndexedNode resolved = ResolveFully(indexedNode);
				if (resolved == null)
				{
					continue;
				}
				if (best == null)
				{
					best = indexedNode;
					continue;
				}
				if (indexedNode.ResultSize() < best.ResultSize())
				{
					best = indexedNode;
				}
			}
			return best;
		}

		public virtual IEnumerator CollectIndexedNodes()
		{
			return new IndexedNodeCollector(_candidates).GetNodes();
		}
	}
}
