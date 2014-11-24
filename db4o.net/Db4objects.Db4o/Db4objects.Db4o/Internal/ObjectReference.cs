/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o;
using Db4objects.Db4o.Activation;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Activation;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Internal.Slots;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Typehandlers;
using Sharpen;

namespace Db4objects.Db4o.Internal
{
	/// <summary>A weak reference to an known object.</summary>
	/// <remarks>
	/// A weak reference to an known object.
	/// "Known" ~ has been stored and/or retrieved within a transaction.
	/// References the corresponding ClassMetaData along with further metadata:
	/// internal id, UUID/version information, ...
	/// </remarks>
	/// <exclude></exclude>
	public class ObjectReference : Identifiable, IObjectInfo, IActivator
	{
		private Db4objects.Db4o.Internal.ClassMetadata _class;

		private object _object;

		private Db4objects.Db4o.Internal.VirtualAttributes _virtualAttributes;

		private Db4objects.Db4o.Internal.ObjectReference _idPreceding;

		private Db4objects.Db4o.Internal.ObjectReference _idSubsequent;

		private int _idSize;

		private Db4objects.Db4o.Internal.ObjectReference _hcPreceding;

		private Db4objects.Db4o.Internal.ObjectReference _hcSubsequent;

		private int _hcSize;

		public int _hcHashcode;

		private int _lastTopLevelCallId;

		public ObjectReference()
		{
		}

		public ObjectReference(int id)
		{
			// redundant hashCode
			_id = id;
			if (DTrace.enabled)
			{
				DTrace.ObjectReferenceCreated.Log(id);
			}
		}

		public ObjectReference(Db4objects.Db4o.Internal.ClassMetadata classMetadata, int 
			id) : this(id)
		{
			_class = classMetadata;
		}

		public virtual void Activate(ActivationPurpose purpose)
		{
			ActivateOn(Container().Transaction, purpose);
		}

		public virtual void ActivateOn(Db4objects.Db4o.Internal.Transaction transaction, 
			ActivationPurpose purpose)
		{
			if (Activating())
			{
				return;
			}
			try
			{
				Activating(true);
				ObjectContainerBase container = transaction.Container();
				if (!(container.ActivationDepthProvider() is ITransparentActivationDepthProvider))
				{
					return;
				}
				ITransparentActivationDepthProvider provider = (ITransparentActivationDepthProvider
					)container.ActivationDepthProvider();
				if (ActivationPurpose.Write == purpose)
				{
					lock (container.Lock())
					{
						provider.AddModified(GetObject(), transaction);
					}
				}
				if (IsActive())
				{
					return;
				}
				lock (container.Lock())
				{
					Activate(transaction, GetObject(), new DescendingActivationDepth(provider, ActivationMode
						.Activate));
				}
			}
			finally
			{
				Activating(false);
			}
		}

		private bool Activating()
		{
			return BitIsTrue(Const4.Activating);
		}

		private void Activating(bool isActivating)
		{
			if (isActivating)
			{
				BitTrue(Const4.Activating);
			}
			else
			{
				BitFalse(Const4.Activating);
			}
		}

		public virtual void Activate(Db4objects.Db4o.Internal.Transaction ta, object obj, 
			IActivationDepth depth)
		{
			ObjectContainerBase container = ta.Container();
			ActivateInternal(container.ActivationContextFor(ta, obj, depth));
			container.ActivatePending(ta);
		}

		internal virtual void ActivateInternal(IActivationContext context)
		{
			if (null == context)
			{
				throw new ArgumentNullException();
			}
			if (!context.Depth().RequiresActivation())
			{
				return;
			}
			ObjectContainerBase container = context.Container();
			if (context.Depth().Mode().IsRefresh())
			{
				LogActivation(container, "refresh");
			}
			else
			{
				if (IsActive())
				{
					_class.CascadeActivation(context);
					return;
				}
				LogActivation(container, "activate");
			}
			ReadForActivation(context);
		}

		private void ReadForActivation(IActivationContext context)
		{
			Read(context.Transaction(), null, context.TargetObject(), context.Depth(), Const4
				.AddMembersToIdTreeOnly, false);
		}

		private void LogActivation(ObjectContainerBase container, string @event)
		{
			LogEvent(container, @event, Const4.Activation);
		}

		private void LogEvent(ObjectContainerBase container, string @event, int level)
		{
			if (container.ConfigImpl.MessageLevel() > level)
			{
				container.Message(string.Empty + GetID() + " " + @event + " " + _class.GetName());
			}
		}

		/// <summary>return false if class not completely initialized, otherwise true</summary>
		internal virtual bool ContinueSet(Db4objects.Db4o.Internal.Transaction trans, IUpdateDepth
			 updateDepth)
		{
			if (!BitIsTrue(Const4.Continue))
			{
				return true;
			}
			if (!_class.StateOK())
			{
				return false;
			}
			if (!_class.AspectsAreInitialized())
			{
				return false;
			}
			if (DTrace.enabled)
			{
				DTrace.Continueset.Log(GetID());
			}
			BitFalse(Const4.Continue);
			MarshallingContext context = new MarshallingContext(trans, this, updateDepth, true
				);
			Handlers4.Write(ClassMetadata().TypeHandler(), context, GetObject());
			Pointer4 pointer = context.AllocateSlot();
			ByteArrayBuffer buffer = context.ToWriteBuffer(pointer);
			ObjectContainerBase container = trans.Container();
			container.WriteNew(trans, pointer, _class, buffer);
			object obj = _object;
			ObjectOnNew(trans, obj);
			if (_class.HasIdentity())
			{
				_object = container.NewWeakReference(this, obj);
			}
			SetStateClean();
			EndProcessing();
			return true;
		}

		private void ObjectOnNew(Db4objects.Db4o.Internal.Transaction transaction, object
			 obj)
		{
			ObjectContainerBase container = transaction.Container();
			container.Callbacks().ObjectOnNew(transaction, this);
			_class.DispatchEvent(transaction, obj, EventDispatchers.New);
		}

		public virtual void Deactivate(Db4objects.Db4o.Internal.Transaction trans, IActivationDepth
			 depth)
		{
			if (!depth.RequiresActivation())
			{
				return;
			}
			object obj = GetObject();
			if (obj == null)
			{
				return;
			}
			ObjectContainerBase container = trans.Container();
			LogActivation(container, "deactivate");
			SetStateDeactivated();
			_class.Deactivate(trans, this, depth);
		}

		public virtual byte GetIdentifier()
		{
			return Const4.Yapobject;
		}

		public virtual long GetInternalID()
		{
			return GetID();
		}

		public virtual object GetObject()
		{
			if (Platform4.HasWeakReferences())
			{
				return Platform4.GetYapRefObject(_object);
			}
			return _object;
		}

		public virtual object GetObjectReference()
		{
			return _object;
		}

		public virtual ObjectContainerBase Container()
		{
			if (_class == null)
			{
				throw new InvalidOperationException();
			}
			return _class.Container();
		}

		public virtual Db4objects.Db4o.Internal.Transaction Transaction()
		{
			return Container().Transaction;
		}

		public virtual Db4oUUID GetUUID()
		{
			Db4objects.Db4o.Internal.VirtualAttributes va = VirtualAttributes(Transaction());
			if (va != null && va.i_database != null)
			{
				return new Db4oUUID(va.i_uuid, va.i_database.i_signature);
			}
			return null;
		}

		public virtual long GetVersion()
		{
			return GetCommitTimestamp();
		}

		public virtual long GetCommitTimestamp()
		{
			lock (Container().Lock())
			{
				return Container().SystemTransaction().VersionForId(GetID());
			}
		}

		public Db4objects.Db4o.Internal.ClassMetadata ClassMetadata()
		{
			return _class;
		}

		public virtual void ClassMetadata(Db4objects.Db4o.Internal.ClassMetadata classMetadata
			)
		{
			if (_class == classMetadata)
			{
				return;
			}
			if (_class != null)
			{
				throw new InvalidOperationException("Object types aren't supposed to change!");
			}
			_class = classMetadata;
		}

		public virtual int OwnLength()
		{
			throw Exceptions4.ShouldNeverBeCalled();
		}

		public virtual Db4objects.Db4o.Internal.VirtualAttributes ProduceVirtualAttributes
			()
		{
			if (_virtualAttributes == null)
			{
				_virtualAttributes = new Db4objects.Db4o.Internal.VirtualAttributes();
			}
			return _virtualAttributes;
		}

		internal void PeekPersisted(Db4objects.Db4o.Internal.Transaction trans, IActivationDepth
			 depth)
		{
			SetObject(Read(trans, depth, Const4.Transient, false));
		}

		internal object Read(Db4objects.Db4o.Internal.Transaction trans, IActivationDepth
			 instantiationDepth, int addToIDTree, bool checkIDTree)
		{
			return Read(trans, null, null, instantiationDepth, addToIDTree, checkIDTree);
		}

		public object Read(Db4objects.Db4o.Internal.Transaction trans, ByteArrayBuffer buffer
			, object obj, IActivationDepth instantiationDepth, int addToIDTree, bool checkIDTree
			)
		{
			UnmarshallingContext context = new UnmarshallingContext(trans, buffer, this, addToIDTree
				, checkIDTree);
			context.PersistentObject(obj);
			context.ActivationDepth(instantiationDepth);
			return context.Read();
		}

		public virtual object ReadPrefetch(Db4objects.Db4o.Internal.Transaction trans, ByteArrayBuffer
			 buffer, int addToIDTree)
		{
			UnmarshallingContext context = new UnmarshallingContext(trans, buffer, this, addToIDTree
				, false);
			context.ActivationDepth(new FixedActivationDepth(1, ActivationMode.Prefetch));
			return context.Read();
		}

		public void ReadThis(Db4objects.Db4o.Internal.Transaction trans, ByteArrayBuffer 
			buffer)
		{
		}

		public virtual void SetObjectWeak(ObjectContainerBase container, object obj)
		{
			if (_object != null)
			{
				Platform4.KillYapRef(_object);
			}
			_object = container.NewWeakReference(this, obj);
		}

		public virtual void SetObject(object obj)
		{
			_object = obj;
		}

		internal void Store(Db4objects.Db4o.Internal.Transaction trans, Db4objects.Db4o.Internal.ClassMetadata
			 classMetadata, object obj)
		{
			_object = obj;
			_class = classMetadata;
			int id = trans.Container().IdForNewUserObject(trans);
			SetID(id);
			// will be ended in continueset()
			BeginProcessing();
			BitTrue(Const4.Continue);
		}

		public virtual void FlagForDelete(int callId)
		{
			_lastTopLevelCallId = -callId;
		}

		public virtual bool IsFlaggedForDelete()
		{
			return _lastTopLevelCallId < 0;
		}

		public virtual void FlagAsHandled(int callId)
		{
			_lastTopLevelCallId = callId;
		}

		public bool IsFlaggedAsHandled(int callID)
		{
			return _lastTopLevelCallId == callID;
		}

		public bool IsValid()
		{
			return IsValidId(GetID()) && GetObject() != null;
		}

		public static bool IsValidId(int id)
		{
			return id > 0;
		}

		public virtual Db4objects.Db4o.Internal.VirtualAttributes VirtualAttributes()
		{
			return _virtualAttributes;
		}

		public virtual Db4objects.Db4o.Internal.VirtualAttributes VirtualAttributes(Db4objects.Db4o.Internal.Transaction
			 trans, bool lastCommitted)
		{
			if (trans == null)
			{
				return _virtualAttributes;
			}
			lock (trans.Container().Lock())
			{
				if (_virtualAttributes == null)
				{
					if (_class.HasVirtualAttributes())
					{
						_virtualAttributes = new Db4objects.Db4o.Internal.VirtualAttributes();
						_class.ReadVirtualAttributes(trans, this, lastCommitted);
					}
				}
				else
				{
					if (!_virtualAttributes.SuppliesUUID())
					{
						if (_class.HasVirtualAttributes())
						{
							_class.ReadVirtualAttributes(trans, this, lastCommitted);
						}
					}
				}
				return _virtualAttributes;
			}
		}

		public virtual Db4objects.Db4o.Internal.VirtualAttributes VirtualAttributes(Db4objects.Db4o.Internal.Transaction
			 trans)
		{
			return VirtualAttributes(trans, false);
		}

		public virtual void SetVirtualAttributes(Db4objects.Db4o.Internal.VirtualAttributes
			 at)
		{
			_virtualAttributes = at;
		}

		public virtual void WriteThis(Db4objects.Db4o.Internal.Transaction trans, ByteArrayBuffer
			 buffer)
		{
		}

		public virtual void WriteUpdate(Db4objects.Db4o.Internal.Transaction transaction, 
			IUpdateDepth updatedepth)
		{
			ContinueSet(transaction, updatedepth);
			// make sure, a concurrent new, possibly triggered by objectOnNew
			// is written to the file
			// preventing recursive
			if (!BeginProcessing())
			{
				return;
			}
			object obj = GetObject();
			if (!ObjectCanUpdate(transaction, obj) || !IsActive() || obj == null || !ClassMetadata
				().IsModified(obj))
			{
				EndProcessing();
				return;
			}
			MarshallingContext context = new MarshallingContext(transaction, this, updatedepth
				, false);
			if (context.UpdateDepth().Negative())
			{
				EndProcessing();
				return;
			}
			ObjectContainerBase container = transaction.Container();
			LogEvent(container, "update", Const4.State);
			SetStateClean();
			context.PurgeFieldIndexEntriesOnUpdate(transaction, container._handlers.ArrayType
				(obj));
			Handlers4.Write(_class.TypeHandler(), context, obj);
			if (context.UpdateDepth().CanSkip(this))
			{
				EndProcessing();
				return;
			}
			Pointer4 pointer = context.AllocateSlot();
			ByteArrayBuffer buffer = context.ToWriteBuffer(pointer);
			container.WriteUpdate(transaction, pointer, _class, container._handlers.ArrayType
				(obj), buffer);
			if (IsActive())
			{
				SetStateClean();
			}
			EndProcessing();
			container.Callbacks().ObjectOnUpdate(transaction, this);
			ClassMetadata().DispatchEvent(transaction, obj, EventDispatchers.Update);
		}

		protected virtual bool ObjectCanUpdate(Db4objects.Db4o.Internal.Transaction transaction
			, object obj)
		{
			ObjectContainerBase container = transaction.Container();
			return container.Callbacks().ObjectCanUpdate(transaction, this) && _class.DispatchEvent
				(transaction, obj, EventDispatchers.CanUpdate);
		}

		public virtual void Ref_init()
		{
			Hc_init();
			Id_init();
		}

		/// <summary>HCTREE</summary>
		public virtual Db4objects.Db4o.Internal.ObjectReference Hc_add(Db4objects.Db4o.Internal.ObjectReference
			 newRef)
		{
			if (newRef.GetObject() == null)
			{
				return this;
			}
			newRef.Hc_init();
			return Hc_add1(newRef);
		}

		private void Hc_init()
		{
			_hcPreceding = null;
			_hcSubsequent = null;
			_hcSize = 1;
			_hcHashcode = Hc_getCode(GetObject());
		}

		private Db4objects.Db4o.Internal.ObjectReference Hc_add1(Db4objects.Db4o.Internal.ObjectReference
			 newRef)
		{
			int cmp = Hc_compare(newRef);
			if (cmp < 0)
			{
				if (_hcPreceding == null)
				{
					_hcPreceding = newRef;
					_hcSize++;
				}
				else
				{
					_hcPreceding = _hcPreceding.Hc_add1(newRef);
					if (_hcSubsequent == null)
					{
						return Hc_rotateRight();
					}
					return Hc_balance();
				}
			}
			else
			{
				if (_hcSubsequent == null)
				{
					_hcSubsequent = newRef;
					_hcSize++;
				}
				else
				{
					_hcSubsequent = _hcSubsequent.Hc_add1(newRef);
					if (_hcPreceding == null)
					{
						return Hc_rotateLeft();
					}
					return Hc_balance();
				}
			}
			return this;
		}

		private Db4objects.Db4o.Internal.ObjectReference Hc_balance()
		{
			int cmp = _hcSubsequent._hcSize - _hcPreceding._hcSize;
			if (cmp < -2)
			{
				return Hc_rotateRight();
			}
			else
			{
				if (cmp > 2)
				{
					return Hc_rotateLeft();
				}
				else
				{
					_hcSize = _hcPreceding._hcSize + _hcSubsequent._hcSize + 1;
					return this;
				}
			}
		}

		private void Hc_calculateSize()
		{
			if (_hcPreceding == null)
			{
				if (_hcSubsequent == null)
				{
					_hcSize = 1;
				}
				else
				{
					_hcSize = _hcSubsequent._hcSize + 1;
				}
			}
			else
			{
				if (_hcSubsequent == null)
				{
					_hcSize = _hcPreceding._hcSize + 1;
				}
				else
				{
					_hcSize = _hcPreceding._hcSize + _hcSubsequent._hcSize + 1;
				}
			}
		}

		private int Hc_compare(Db4objects.Db4o.Internal.ObjectReference toRef)
		{
			int cmp = toRef._hcHashcode - _hcHashcode;
			if (cmp == 0)
			{
				cmp = toRef._id - _id;
			}
			return cmp;
		}

		public virtual Db4objects.Db4o.Internal.ObjectReference Hc_find(object obj)
		{
			return Hc_find(Hc_getCode(obj), obj);
		}

		private Db4objects.Db4o.Internal.ObjectReference Hc_find(int id, object obj)
		{
			int cmp = id - _hcHashcode;
			if (cmp < 0)
			{
				if (_hcPreceding != null)
				{
					return _hcPreceding.Hc_find(id, obj);
				}
			}
			else
			{
				if (cmp > 0)
				{
					if (_hcSubsequent != null)
					{
						return _hcSubsequent.Hc_find(id, obj);
					}
				}
				else
				{
					if (obj == GetObject())
					{
						return this;
					}
					if (_hcPreceding != null)
					{
						Db4objects.Db4o.Internal.ObjectReference inPreceding = _hcPreceding.Hc_find(id, obj
							);
						if (inPreceding != null)
						{
							return inPreceding;
						}
					}
					if (_hcSubsequent != null)
					{
						return _hcSubsequent.Hc_find(id, obj);
					}
				}
			}
			return null;
		}

		public static int Hc_getCode(object obj)
		{
			int hcode = Runtime.IdentityHashCode(obj);
			if (hcode < 0)
			{
				hcode = ~hcode;
			}
			return hcode;
		}

		private Db4objects.Db4o.Internal.ObjectReference Hc_rotateLeft()
		{
			Db4objects.Db4o.Internal.ObjectReference tree = _hcSubsequent;
			_hcSubsequent = tree._hcPreceding;
			Hc_calculateSize();
			tree._hcPreceding = this;
			if (tree._hcSubsequent == null)
			{
				tree._hcSize = 1 + _hcSize;
			}
			else
			{
				tree._hcSize = 1 + _hcSize + tree._hcSubsequent._hcSize;
			}
			return tree;
		}

		private Db4objects.Db4o.Internal.ObjectReference Hc_rotateRight()
		{
			Db4objects.Db4o.Internal.ObjectReference tree = _hcPreceding;
			_hcPreceding = tree._hcSubsequent;
			Hc_calculateSize();
			tree._hcSubsequent = this;
			if (tree._hcPreceding == null)
			{
				tree._hcSize = 1 + _hcSize;
			}
			else
			{
				tree._hcSize = 1 + _hcSize + tree._hcPreceding._hcSize;
			}
			return tree;
		}

		private Db4objects.Db4o.Internal.ObjectReference Hc_rotateSmallestUp()
		{
			if (_hcPreceding != null)
			{
				_hcPreceding = _hcPreceding.Hc_rotateSmallestUp();
				return Hc_rotateRight();
			}
			return this;
		}

		public virtual Db4objects.Db4o.Internal.ObjectReference Hc_remove(Db4objects.Db4o.Internal.ObjectReference
			 findRef)
		{
			if (this == findRef)
			{
				return Hc_remove();
			}
			int cmp = Hc_compare(findRef);
			if (cmp <= 0)
			{
				if (_hcPreceding != null)
				{
					_hcPreceding = _hcPreceding.Hc_remove(findRef);
				}
			}
			if (cmp >= 0)
			{
				if (_hcSubsequent != null)
				{
					_hcSubsequent = _hcSubsequent.Hc_remove(findRef);
				}
			}
			Hc_calculateSize();
			return this;
		}

		public virtual void Hc_traverse(IVisitor4 visitor)
		{
			if (_hcPreceding != null)
			{
				_hcPreceding.Hc_traverse(visitor);
			}
			if (_hcSubsequent != null)
			{
				_hcSubsequent.Hc_traverse(visitor);
			}
			// Traversing the leaves first allows to add ObjectReference 
			// nodes to different ReferenceSystem trees during commit
			visitor.Visit(this);
		}

		private Db4objects.Db4o.Internal.ObjectReference Hc_remove()
		{
			if (_hcSubsequent != null && _hcPreceding != null)
			{
				_hcSubsequent = _hcSubsequent.Hc_rotateSmallestUp();
				_hcSubsequent._hcPreceding = _hcPreceding;
				_hcSubsequent.Hc_calculateSize();
				return _hcSubsequent;
			}
			if (_hcSubsequent != null)
			{
				return _hcSubsequent;
			}
			return _hcPreceding;
		}

		/// <summary>IDTREE</summary>
		public virtual Db4objects.Db4o.Internal.ObjectReference Id_add(Db4objects.Db4o.Internal.ObjectReference
			 newRef)
		{
			newRef.Id_init();
			return Id_add1(newRef);
		}

		private void Id_init()
		{
			_idPreceding = null;
			_idSubsequent = null;
			_idSize = 1;
		}

		private Db4objects.Db4o.Internal.ObjectReference Id_add1(Db4objects.Db4o.Internal.ObjectReference
			 newRef)
		{
			int cmp = newRef._id - _id;
			if (cmp < 0)
			{
				if (_idPreceding == null)
				{
					_idPreceding = newRef;
					_idSize++;
				}
				else
				{
					_idPreceding = _idPreceding.Id_add1(newRef);
					if (_idSubsequent == null)
					{
						return Id_rotateRight();
					}
					return Id_balance();
				}
			}
			else
			{
				if (cmp > 0)
				{
					if (_idSubsequent == null)
					{
						_idSubsequent = newRef;
						_idSize++;
					}
					else
					{
						_idSubsequent = _idSubsequent.Id_add1(newRef);
						if (_idPreceding == null)
						{
							return Id_rotateLeft();
						}
						return Id_balance();
					}
				}
			}
			return this;
		}

		private Db4objects.Db4o.Internal.ObjectReference Id_balance()
		{
			int cmp = _idSubsequent._idSize - _idPreceding._idSize;
			if (cmp < -2)
			{
				return Id_rotateRight();
			}
			else
			{
				if (cmp > 2)
				{
					return Id_rotateLeft();
				}
				else
				{
					_idSize = _idPreceding._idSize + _idSubsequent._idSize + 1;
					return this;
				}
			}
		}

		private void Id_calculateSize()
		{
			if (_idPreceding == null)
			{
				if (_idSubsequent == null)
				{
					_idSize = 1;
				}
				else
				{
					_idSize = _idSubsequent._idSize + 1;
				}
			}
			else
			{
				if (_idSubsequent == null)
				{
					_idSize = _idPreceding._idSize + 1;
				}
				else
				{
					_idSize = _idPreceding._idSize + _idSubsequent._idSize + 1;
				}
			}
		}

		public virtual Db4objects.Db4o.Internal.ObjectReference Id_find(int id)
		{
			int cmp = id - _id;
			if (cmp > 0)
			{
				if (_idSubsequent != null)
				{
					return _idSubsequent.Id_find(id);
				}
			}
			else
			{
				if (cmp < 0)
				{
					if (_idPreceding != null)
					{
						return _idPreceding.Id_find(id);
					}
				}
				else
				{
					return this;
				}
			}
			return null;
		}

		private Db4objects.Db4o.Internal.ObjectReference Id_rotateLeft()
		{
			Db4objects.Db4o.Internal.ObjectReference tree = _idSubsequent;
			_idSubsequent = tree._idPreceding;
			Id_calculateSize();
			tree._idPreceding = this;
			if (tree._idSubsequent == null)
			{
				tree._idSize = _idSize + 1;
			}
			else
			{
				tree._idSize = _idSize + 1 + tree._idSubsequent._idSize;
			}
			return tree;
		}

		private Db4objects.Db4o.Internal.ObjectReference Id_rotateRight()
		{
			Db4objects.Db4o.Internal.ObjectReference tree = _idPreceding;
			_idPreceding = tree._idSubsequent;
			Id_calculateSize();
			tree._idSubsequent = this;
			if (tree._idPreceding == null)
			{
				tree._idSize = _idSize + 1;
			}
			else
			{
				tree._idSize = _idSize + 1 + tree._idPreceding._idSize;
			}
			return tree;
		}

		private Db4objects.Db4o.Internal.ObjectReference Id_rotateSmallestUp()
		{
			if (_idPreceding != null)
			{
				_idPreceding = _idPreceding.Id_rotateSmallestUp();
				return Id_rotateRight();
			}
			return this;
		}

		public virtual Db4objects.Db4o.Internal.ObjectReference Id_remove(Db4objects.Db4o.Internal.ObjectReference
			 @ref)
		{
			int cmp = @ref._id - _id;
			if (cmp < 0)
			{
				if (_idPreceding != null)
				{
					_idPreceding = _idPreceding.Id_remove(@ref);
				}
			}
			else
			{
				if (cmp > 0)
				{
					if (_idSubsequent != null)
					{
						_idSubsequent = _idSubsequent.Id_remove(@ref);
					}
				}
				else
				{
					if (this == @ref)
					{
						return Id_remove();
					}
					return this;
				}
			}
			Id_calculateSize();
			return this;
		}

		private Db4objects.Db4o.Internal.ObjectReference Id_remove()
		{
			if (_idSubsequent != null && _idPreceding != null)
			{
				_idSubsequent = _idSubsequent.Id_rotateSmallestUp();
				_idSubsequent._idPreceding = _idPreceding;
				_idSubsequent.Id_calculateSize();
				return _idSubsequent;
			}
			if (_idSubsequent != null)
			{
				return _idSubsequent;
			}
			return _idPreceding;
		}

		public override string ToString()
		{
			try
			{
				int id = GetID();
				string str = "ObjectReference\nID=" + id;
				object obj = GetObject();
				if (obj == null && _class != null)
				{
					ObjectContainerBase container = _class.Container();
					if (container != null && id > 0)
					{
						obj = container.PeekPersisted(container.Transaction, id, container.DefaultActivationDepth
							(ClassMetadata()), true).ToString();
					}
				}
				if (obj == null)
				{
					str += "\nfor [null]";
				}
				else
				{
					string objToString = string.Empty;
					try
					{
						objToString = obj.ToString();
					}
					catch (Exception)
					{
					}
					if (ClassMetadata() != null)
					{
						IReflectClass claxx = ClassMetadata().Reflector().ForObject(obj);
						str += "\n" + claxx.GetName();
					}
					str += "\n" + objToString;
				}
				return str;
			}
			catch (Exception)
			{
			}
			return "ObjectReference " + GetID();
		}
	}
}
