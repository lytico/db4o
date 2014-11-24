/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Text;
using Db4objects.Db4o;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.References;

namespace Db4objects.Db4o.Internal.References
{
	/// <exclude></exclude>
	public class HashcodeReferenceSystem : IReferenceSystem
	{
		private ObjectReference _hashCodeTree;

		private ObjectReference _idTree;

		public virtual void AddNewReference(ObjectReference @ref)
		{
			AddReference(@ref);
		}

		public virtual void AddExistingReference(ObjectReference @ref)
		{
			AddReference(@ref);
		}

		private void AddReference(ObjectReference @ref)
		{
			@ref.Ref_init();
			IdAdd(@ref);
			HashCodeAdd(@ref);
		}

		public virtual void Commit()
		{
		}

		// do nothing
		private void HashCodeAdd(ObjectReference @ref)
		{
			if (_hashCodeTree == null)
			{
				_hashCodeTree = @ref;
				return;
			}
			_hashCodeTree = _hashCodeTree.Hc_add(@ref);
		}

		private void IdAdd(ObjectReference @ref)
		{
			if (DTrace.enabled)
			{
				DTrace.IdTreeAdd.Log(@ref.GetID());
			}
			if (_idTree == null)
			{
				_idTree = @ref;
				return;
			}
			_idTree = _idTree.Id_add(@ref);
		}

		public virtual ObjectReference ReferenceForId(int id)
		{
			if (DTrace.enabled)
			{
				DTrace.GetYapobject.Log(id);
			}
			if (_idTree == null)
			{
				return null;
			}
			if (!ObjectReference.IsValidId(id))
			{
				return null;
			}
			return _idTree.Id_find(id);
		}

		public virtual ObjectReference ReferenceForObject(object obj)
		{
			if (_hashCodeTree == null)
			{
				return null;
			}
			return _hashCodeTree.Hc_find(obj);
		}

		public virtual void RemoveReference(ObjectReference @ref)
		{
			if (DTrace.enabled)
			{
				DTrace.ReferenceRemoved.Log(@ref.GetID());
			}
			if (_hashCodeTree != null)
			{
				_hashCodeTree = _hashCodeTree.Hc_remove(@ref);
			}
			if (_idTree != null)
			{
				_idTree = _idTree.Id_remove(@ref);
			}
		}

		public virtual void Rollback()
		{
		}

		// do nothing
		public virtual void TraverseReferences(IVisitor4 visitor)
		{
			if (_hashCodeTree == null)
			{
				return;
			}
			_hashCodeTree.Hc_traverse(visitor);
		}

		public override string ToString()
		{
			BooleanByRef found = new BooleanByRef();
			StringBuilder str = new StringBuilder("HashcodeReferenceSystem {");
			TraverseReferences(new _IVisitor4_117(found, str));
			str.Append("}");
			return str.ToString();
		}

		private sealed class _IVisitor4_117 : IVisitor4
		{
			public _IVisitor4_117(BooleanByRef found, StringBuilder str)
			{
				this.found = found;
				this.str = str;
			}

			public void Visit(object obj)
			{
				if (found.value)
				{
					str.Append(", ");
				}
				ObjectReference @ref = (ObjectReference)obj;
				str.Append(@ref.GetID());
				found.value = true;
			}

			private readonly BooleanByRef found;

			private readonly StringBuilder str;
		}

		public virtual void Discarded()
		{
		}
		// do nothing
	}
}
