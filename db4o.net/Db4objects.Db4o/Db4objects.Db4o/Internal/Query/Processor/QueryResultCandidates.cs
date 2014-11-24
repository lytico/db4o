/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal.Classindex;
using Db4objects.Db4o.Internal.Query.Processor;

namespace Db4objects.Db4o.Internal.Query.Processor
{
	/// <exclude></exclude>
	public class QueryResultCandidates
	{
		private QCandidateBase _candidates;

		private QCandidates _qCandidates;

		internal IIntVisitable _candidateIds;

		public QueryResultCandidates(QCandidates qCandidates)
		{
			_qCandidates = qCandidates;
		}

		public virtual void Add(IInternalCandidate candidate)
		{
			ToQCandidates();
			_candidates = ((QCandidateBase)Tree.Add(_candidates, (QCandidateBase)candidate));
		}

		private void ToQCandidates()
		{
			if (_candidateIds == null)
			{
				return;
			}
			_candidateIds.Traverse(new _IIntVisitor_35(this));
			_candidateIds = null;
		}

		private sealed class _IIntVisitor_35 : IIntVisitor
		{
			public _IIntVisitor_35(QueryResultCandidates _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Visit(int id)
			{
				this._enclosing._candidates = ((QCandidateBase)Tree.Add(this._enclosing._candidates
					, new QCandidate(this._enclosing._qCandidates, null, id)));
			}

			private readonly QueryResultCandidates _enclosing;
		}

		public virtual void FieldIndexProcessorResult(Db4objects.Db4o.Internal.Fieldindex.FieldIndexProcessorResult
			 result)
		{
			_candidateIds = result;
		}

		public virtual void SingleCandidate(QCandidateBase candidate)
		{
			_candidates = candidate;
			_candidateIds = null;
		}

		internal virtual bool Filter(IVisitor4 visitor)
		{
			ToQCandidates();
			if (_candidates != null)
			{
				_candidates.Traverse(visitor);
				_candidates = (QCandidateBase)_candidates.Filter(new _IPredicate4_56());
			}
			return _candidates != null;
		}

		private sealed class _IPredicate4_56 : IPredicate4
		{
			public _IPredicate4_56()
			{
			}

			public bool Match(object a_candidate)
			{
				return ((QCandidateBase)a_candidate)._include;
			}
		}

		internal virtual bool Filter(QField field, IFieldFilterable filterable)
		{
			ToQCandidates();
			if (_candidates != null)
			{
				_candidates.Traverse(new _IVisitor4_68(filterable, field));
				_candidates = (QCandidateBase)_candidates.Filter(new _IPredicate4_73());
			}
			return _candidates != null;
		}

		private sealed class _IVisitor4_68 : IVisitor4
		{
			public _IVisitor4_68(IFieldFilterable filterable, QField field)
			{
				this.filterable = filterable;
				this.field = field;
			}

			public void Visit(object candidate)
			{
				filterable.Filter(field, (IParentCandidate)candidate);
			}

			private readonly IFieldFilterable filterable;

			private readonly QField field;
		}

		private sealed class _IPredicate4_73 : IPredicate4
		{
			public _IPredicate4_73()
			{
			}

			public bool Match(object a_candidate)
			{
				return ((QCandidateBase)a_candidate)._include;
			}
		}

		public virtual void LoadFromClassIndex(IClassIndexStrategy index)
		{
			_candidateIds = index.IdVisitable(_qCandidates.Transaction());
		}

		internal virtual void Traverse(IVisitor4 visitor)
		{
			ToQCandidates();
			if (_candidates != null)
			{
				_candidates.Traverse(visitor);
			}
		}

		public virtual void TraverseIds(IIntVisitor visitor)
		{
			if (_candidateIds != null)
			{
				_candidateIds.Traverse(visitor);
				return;
			}
			Traverse(new _IVisitor4_98(visitor));
		}

		private sealed class _IVisitor4_98 : IVisitor4
		{
			public _IVisitor4_98(IIntVisitor visitor)
			{
				this.visitor = visitor;
			}

			public void Visit(object obj)
			{
				QCandidateBase candidate = (QCandidateBase)obj;
				if (candidate.Include())
				{
					visitor.Visit(candidate._key);
				}
			}

			private readonly IIntVisitor visitor;
		}
	}
}
