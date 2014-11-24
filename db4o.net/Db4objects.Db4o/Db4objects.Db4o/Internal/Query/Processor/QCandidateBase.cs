/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Query.Processor;

namespace Db4objects.Db4o.Internal.Query.Processor
{
	public abstract class QCandidateBase : TreeInt, IInternalCandidate
	{
		protected readonly QCandidates _candidates;

		protected List4 _dependants;

		internal bool _include = true;

		internal Tree _pendingJoins;

		internal IInternalCandidate _root;

		public QCandidateBase(QCandidates candidates, int id) : base(id)
		{
			if (DTrace.enabled)
			{
				DTrace.CreateCandidate.Log(id);
			}
			_candidates = candidates;
			if (id == 0)
			{
				_key = candidates.GenerateCandidateId();
			}
		}

		protected virtual void AddDependant(IInternalCandidate a_candidate)
		{
			_dependants = new List4(_dependants, a_candidate);
		}

		public virtual void DoNotInclude()
		{
			Include(false);
			if (_dependants != null)
			{
				IEnumerator i = new Iterator4Impl(_dependants);
				_dependants = null;
				while (i.MoveNext())
				{
					((IInternalCandidate)i.Current).DoNotInclude();
				}
			}
		}

		public virtual bool Evaluate(QPending pending)
		{
			QPending oldPending = (QPending)Tree.Find(_pendingJoins, pending);
			if (oldPending == null)
			{
				pending.ChangeConstraint();
				_pendingJoins = Tree.Add(_pendingJoins, pending.InternalClonePayload());
				return true;
			}
			_pendingJoins = _pendingJoins.RemoveNode(oldPending);
			oldPending._join.EvaluatePending(this, oldPending, pending._result);
			return false;
		}

		public virtual IObjectContainer ObjectContainer()
		{
			return Container();
		}

		public virtual IInternalCandidate GetRoot()
		{
			return _root == null ? this : _root;
		}

		internal LocalObjectContainer Container()
		{
			return Transaction().LocalContainer();
		}

		public LocalTransaction Transaction()
		{
			return _candidates.i_trans;
		}

		public virtual bool Include()
		{
			return _include;
		}

		/// <summary>For external interface use only.</summary>
		/// <remarks>
		/// For external interface use only. Call doNotInclude() internally so
		/// dependancies can be checked.
		/// </remarks>
		public virtual void Include(bool flag)
		{
			// TODO:
			// Internal and external flag may need to be handled seperately.
			_include = flag;
		}

		public override Tree OnAttemptToAddDuplicate(Tree oldNode)
		{
			_size = 0;
			_root = (IInternalCandidate)oldNode;
			return oldNode;
		}

		public override bool Duplicates()
		{
			return _root != null;
		}

		public virtual QCandidates Candidates()
		{
			return _candidates;
		}

		public virtual void Root(IInternalCandidate root)
		{
			_root = root;
		}

		public virtual Tree PendingJoins()
		{
			return _pendingJoins;
		}

		public virtual int Id()
		{
			return _key;
		}

		public abstract object GetObject();

		public abstract ClassMetadata ClassMetadata();

		public abstract bool Evaluate(QConObject arg1, QE arg2);

		public abstract bool FieldIsAvailable();

		public abstract IPreparedComparison PrepareComparison(ObjectContainerBase arg1, object
			 arg2);
	}
}
