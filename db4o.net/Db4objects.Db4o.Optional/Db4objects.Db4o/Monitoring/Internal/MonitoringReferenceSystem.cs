/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.References;
using Db4objects.Db4o.Monitoring;

namespace Db4objects.Db4o.Monitoring.Internal
{
	/// <exclude></exclude>
	public class MonitoringReferenceSystem : TransactionalReferenceSystemBase, IReferenceSystem
	{
		private readonly IReferenceSystemListener _referenceSystemListener;

		private int _referenceCount;

		public MonitoringReferenceSystem(IReferenceSystemListener referenceSystem)
		{
			_referenceSystemListener = referenceSystem;
		}

		public override void Commit()
		{
			IntByRef removedReferenceCount = new IntByRef();
			TraverseNewReferences(new _IVisitor4_26(this, removedReferenceCount));
			CreateNewReferences();
			ReferenceCountChanged(-removedReferenceCount.value);
		}

		private sealed class _IVisitor4_26 : IVisitor4
		{
			public _IVisitor4_26(MonitoringReferenceSystem _enclosing, IntByRef removedReferenceCount
				)
			{
				this._enclosing = _enclosing;
				this.removedReferenceCount = removedReferenceCount;
			}

			public void Visit(object obj)
			{
				ObjectReference oref = (ObjectReference)obj;
				if (oref.GetObject() != null)
				{
					this._enclosing._committedReferences.AddExistingReference(oref);
				}
				else
				{
					removedReferenceCount.value++;
				}
			}

			private readonly MonitoringReferenceSystem _enclosing;

			private readonly IntByRef removedReferenceCount;
		}

		public override void AddExistingReference(ObjectReference @ref)
		{
			_committedReferences.AddExistingReference(@ref);
			ReferenceCountChanged(1);
		}

		public override void AddNewReference(ObjectReference @ref)
		{
			_newReferences.AddNewReference(@ref);
			ReferenceCountChanged(1);
		}

		public override void RemoveReference(ObjectReference @ref)
		{
			if (_newReferences.ReferenceForId(@ref.GetID()) != null)
			{
				_newReferences.RemoveReference(@ref);
				ReferenceCountChanged(-1);
			}
			if (_committedReferences.ReferenceForId(@ref.GetID()) != null)
			{
				_committedReferences.RemoveReference(@ref);
				ReferenceCountChanged(-1);
			}
		}

		public override void Rollback()
		{
			IntByRef newReferencesCount = new IntByRef();
			TraverseNewReferences(new _IVisitor4_63(newReferencesCount));
			CreateNewReferences();
			ReferenceCountChanged(-newReferencesCount.value);
		}

		private sealed class _IVisitor4_63 : IVisitor4
		{
			public _IVisitor4_63(IntByRef newReferencesCount)
			{
				this.newReferencesCount = newReferencesCount;
			}

			public void Visit(object obj)
			{
				newReferencesCount.value++;
			}

			private readonly IntByRef newReferencesCount;
		}

		private void ReferenceCountChanged(int changedBy)
		{
			if (changedBy == 0)
			{
				return;
			}
			_referenceCount += changedBy;
			_referenceSystemListener.NotifyReferenceCountChanged(changedBy);
		}

		public virtual void Discarded()
		{
			ReferenceCountChanged(-_referenceCount);
		}
	}
}
