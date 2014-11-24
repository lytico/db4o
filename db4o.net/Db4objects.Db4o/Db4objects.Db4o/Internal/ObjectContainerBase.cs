/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Activation;
using Db4objects.Db4o.Internal.Callbacks;
using Db4objects.Db4o.Internal.Encoding;
using Db4objects.Db4o.Internal.Events;
using Db4objects.Db4o.Internal.Handlers.Array;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Internal.Metadata;
using Db4objects.Db4o.Internal.Query;
using Db4objects.Db4o.Internal.Query.Processor;
using Db4objects.Db4o.Internal.Query.Result;
using Db4objects.Db4o.Internal.References;
using Db4objects.Db4o.Internal.Replication;
using Db4objects.Db4o.Internal.Slots;
using Db4objects.Db4o.Internal.Threading;
using Db4objects.Db4o.Internal.Weakref;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Reflect.Core;
using Db4objects.Db4o.Reflect.Generic;
using Db4objects.Db4o.Typehandlers;
using Db4objects.Db4o.Types;
using Sharpen;
using Sharpen.Lang;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public abstract partial class ObjectContainerBase : System.IDisposable, ITransientClass
		, IInternal4, IObjectContainerSpec, IInternalObjectContainer
	{
		protected ClassMetadataRepository _classCollection;

		protected Config4Impl _config;

		private int _stackDepth;

		private readonly int _maxStackDepth;

		private readonly Db4objects.Db4o.Internal.References.ReferenceSystemRegistry _referenceSystemRegistry
			 = new Db4objects.Db4o.Internal.References.ReferenceSystemRegistry();

		private Tree _justPeeked;

		protected object _lock;

		private List4 _pendingClassUpdates;

		internal int _showInternalClasses = 0;

		private List4 _stillToActivate;

		private List4 _stillToDeactivate;

		private List4 _stillToSet;

		private bool _handlingStackLimitPendings = false;

		private Db4objects.Db4o.Internal.Transaction _systemTransaction;

		protected Db4objects.Db4o.Internal.Transaction _transaction;

		public HandlerRegistry _handlers;

		internal int _replicationCallState;

		internal IWeakReferenceSupport _references;

		private NativeQueryHandler _nativeQueryHandler;

		private ICallbacks _callbacks = new NullCallbacks();

		protected readonly TimeStampIdGenerator _timeStampIdGenerator = new TimeStampIdGenerator
			();

		private int _topLevelCallId = 1;

		private IntIdGenerator _topLevelCallIdGenerator = new IntIdGenerator();

		private readonly IEnvironment _environment;

		private IReferenceSystemFactory _referenceSystemFactory;

		private string _name;

		protected IBlockConverter _blockConverter = new DisabledBlockConverter();

		protected ObjectContainerBase(IConfiguration config)
		{
			// Collection of all classes
			// if (_classCollection == null) the engine is down.
			// the Configuration context for this ObjectContainer
			// Counts the number of toplevel calls into YapStream
			// currently used to resolve self-linking concurrency problems
			// in cylic links, stores only ClassMetadata objects
			// a value greater than 0 indicates class implementing the
			// "Internal" interface are visible in queries and can
			// be used.
			// used for ClassMetadata and ClassMetadataRepository
			// may be parent or equal to i_trans
			// used for Objects
			// all the per-YapStream references that we don't
			// want created in YapobjectCarrier
			// One of three constants in ReplicationHandler: NONE, OLD, NEW
			// Detailed replication variables are stored in i_handlers.
			// Call state has to be maintained here, so YapObjectCarrier (who shares i_handlers) does
			// not accidentally think it operates in a replication call. 
			// weak reference management
			_lock = new object();
			_config = (Config4Impl)config;
			_environment = CreateEnvironment(_config);
			_maxStackDepth = _config.MaxStackDepth();
		}

		private IEnvironment CreateEnvironment(Config4Impl config)
		{
			ArrayList bindings = new ArrayList();
			Sharpen.Collections.AddAll(bindings, config.EnvironmentContributions());
			bindings.Add(this);
			// my(ObjectContainer.class)
			bindings.Add(config);
			// my(Configuration.class)
			return Environments.NewConventionBasedEnvironment(Sharpen.Collections.ToArray(bindings
				));
		}

		protected virtual IEnvironment Environment()
		{
			return _environment;
		}

		/// <exception cref="Db4objects.Db4o.Ext.OldFormatException"></exception>
		protected void Open()
		{
			WithEnvironment(new _IRunnable_129(this));
		}

		private sealed class _IRunnable_129 : IRunnable
		{
			public _IRunnable_129(ObjectContainerBase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Run()
			{
				bool ok = false;
				lock (this._enclosing._lock)
				{
					try
					{
						this._enclosing._name = this._enclosing.ConfigImpl.NameProvider().Name(this._enclosing
							);
						this._enclosing.InitializeReferenceSystemFactory(this._enclosing._config);
						this._enclosing.InitializeTransactions();
						this._enclosing.Initialize1(this._enclosing._config);
						this._enclosing.OpenImpl();
						this._enclosing.InitializePostOpen();
						this._enclosing.Callbacks().OpenOnFinished(this._enclosing);
						ok = true;
					}
					finally
					{
						if (!ok)
						{
							// TODO: This will swallow the causing exception if
							//       an exception occurs during shutdown.
							this._enclosing.ShutdownObjectContainer();
						}
					}
				}
			}

			private readonly ObjectContainerBase _enclosing;
		}

		private void InitializeReferenceSystemFactory(Config4Impl config)
		{
			_referenceSystemFactory = config.ReferenceSystemFactory();
		}

		public virtual void WithEnvironment(IRunnable runnable)
		{
			Environments.RunWith(_environment, runnable);
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		protected abstract void OpenImpl();

		public virtual IActivationDepth DefaultActivationDepth(ClassMetadata classMetadata
			)
		{
			return ActivationDepthProvider().ActivationDepthFor(classMetadata, ActivationMode
				.Activate);
		}

		public virtual IActivationDepthProvider ActivationDepthProvider()
		{
			return ConfigImpl.ActivationDepthProvider();
		}

		public void Activate(Db4objects.Db4o.Internal.Transaction trans, object obj)
		{
			lock (_lock)
			{
				Activate(trans, obj, DefaultActivationDepthForObject(obj));
			}
		}

		public void Deactivate(Db4objects.Db4o.Internal.Transaction trans, object obj)
		{
			Deactivate(trans, obj, 1);
		}

		private IActivationDepth DefaultActivationDepthForObject(object obj)
		{
			ClassMetadata classMetadata = ClassMetadataForObject(obj);
			return DefaultActivationDepth(classMetadata);
		}

		public void Activate(Db4objects.Db4o.Internal.Transaction trans, object obj, IActivationDepth
			 depth)
		{
			lock (_lock)
			{
				AsTopLevelCall(new _IFunction4_189(this, obj, depth), trans);
			}
		}

		private sealed class _IFunction4_189 : IFunction4
		{
			public _IFunction4_189(ObjectContainerBase _enclosing, object obj, IActivationDepth
				 depth)
			{
				this._enclosing = _enclosing;
				this.obj = obj;
				this.depth = depth;
			}

			public object Apply(object trans)
			{
				this._enclosing.StillToActivate(this._enclosing.ActivationContextFor(((Db4objects.Db4o.Internal.Transaction
					)trans), obj, depth));
				this._enclosing.ActivatePending(((Db4objects.Db4o.Internal.Transaction)trans));
				return null;
			}

			private readonly ObjectContainerBase _enclosing;

			private readonly object obj;

			private readonly IActivationDepth depth;
		}

		internal sealed class PendingActivation
		{
			public readonly ObjectReference @ref;

			public readonly IActivationDepth depth;

			public PendingActivation(ObjectReference ref_, IActivationDepth depth_)
			{
				this.@ref = ref_;
				this.depth = depth_;
			}
		}

		internal void ActivatePending(Transaction ta)
		{
			while (_stillToActivate != null)
			{
				// TODO: Optimize!  A lightweight int array would be faster.
				IEnumerator i = new Iterator4Impl(_stillToActivate);
				_stillToActivate = null;
				while (i.MoveNext())
				{
					ObjectContainerBase.PendingActivation item = (ObjectContainerBase.PendingActivation
						)i.Current;
					ObjectReference @ref = item.@ref;
					object obj = @ref.GetObject();
					if (obj == null)
					{
						ta.RemoveReference(@ref);
					}
					else
					{
						@ref.ActivateInternal(ActivationContextFor(ta, obj, item.depth));
					}
				}
			}
		}

		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual void Backup(string path)
		{
			Backup(ConfigImpl.Storage, path);
		}

		public virtual ActivationContext4 ActivationContextFor(Transaction ta, object obj
			, IActivationDepth depth)
		{
			return new ActivationContext4(ta, obj, depth);
		}

		/// <exception cref="System.ArgumentNullException"></exception>
		/// <exception cref="System.ArgumentException"></exception>
		public void Bind(Transaction trans, object obj, long id)
		{
			lock (_lock)
			{
				if (obj == null)
				{
					throw new ArgumentNullException();
				}
				if (DTrace.enabled)
				{
					DTrace.Bind.Log(id, " ihc " + Runtime.IdentityHashCode(obj));
				}
				trans = CheckTransaction(trans);
				int intID = (int)id;
				object oldObject = GetByID(trans, id);
				if (oldObject == null)
				{
					throw new ArgumentException("id");
				}
				ObjectReference @ref = trans.ReferenceForId(intID);
				if (@ref == null)
				{
					throw new ArgumentException("obj");
				}
				if (ReflectorForObject(obj) == @ref.ClassMetadata().ClassReflector())
				{
					ObjectReference newRef = Bind2(trans, @ref, obj);
					newRef.VirtualAttributes(trans, false);
				}
				else
				{
					throw new Db4oException(Db4objects.Db4o.Internal.Messages.Get(57));
				}
			}
		}

		public ObjectReference Bind2(Transaction trans, ObjectReference oldRef, object obj
			)
		{
			int id = oldRef.GetID();
			trans.RemoveReference(oldRef);
			ObjectReference newRef = new ObjectReference(ClassMetadataForObject(obj), id);
			newRef.SetObjectWeak(this, obj);
			newRef.SetStateDirty();
			trans.ReferenceSystem().AddExistingReference(newRef);
			return newRef;
		}

		public virtual ClassMetadata ClassMetadataForObject(object obj)
		{
			return ProduceClassMetadata(ReflectorForObject(obj));
		}

		public abstract byte BlockSize();

		private bool BreakDeleteForEnum(ObjectReference reference, bool userCall)
		{
			return false;
			if (userCall)
			{
				return false;
			}
			if (reference == null)
			{
				return false;
			}
			return Platform4.IsEnum(Reflector(), reference.ClassMetadata().ClassReflector());
		}

		internal virtual bool CanUpdate()
		{
			return true;
		}

		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		public void CheckClosed()
		{
			if (_classCollection == null)
			{
				throw new DatabaseClosedException();
			}
		}

		/// <exception cref="Db4objects.Db4o.Ext.DatabaseReadOnlyException"></exception>
		protected virtual void CheckReadOnly()
		{
			if (_config.IsReadOnly())
			{
				throw new DatabaseReadOnlyException();
			}
		}

		internal void ProcessPendingClassUpdates()
		{
			if (_pendingClassUpdates == null)
			{
				return;
			}
			IEnumerator i = new Iterator4Impl(_pendingClassUpdates);
			while (i.MoveNext())
			{
				ClassMetadata classMetadata = (ClassMetadata)i.Current;
				classMetadata.SetStateDirty();
				classMetadata.Write(_systemTransaction);
			}
			_pendingClassUpdates = null;
		}

		public Transaction CheckTransaction()
		{
			return CheckTransaction(null);
		}

		public Transaction CheckTransaction(Transaction ta)
		{
			CheckClosed();
			if (ta != null)
			{
				return ta;
			}
			return Transaction;
		}

		public bool Close()
		{
			lock (_lock)
			{
				Callbacks().CloseOnStarted(this);
				if (DTrace.enabled)
				{
					DTrace.CloseCalled.Log(this.ToString());
				}
				Close1();
				return true;
			}
		}

		protected virtual void HandleExceptionOnClose(Exception exc)
		{
			FatalException(exc);
		}

		private void Close1()
		{
			if (IsClosed())
			{
				return;
			}
			ProcessPendingClassUpdates();
			if (StateMessages())
			{
				LogMsg(2, ToString());
			}
			Close2();
		}

		protected abstract void Close2();

		public void ShutdownObjectContainer()
		{
			if (DTrace.enabled)
			{
				DTrace.Close.Log();
			}
			LogMsg(3, ToString());
			lock (_lock)
			{
				CloseUserTransaction();
				CloseSystemTransaction();
				CloseIdSystem();
				StopSession();
				ShutdownDataStorage();
			}
		}

		protected abstract void CloseIdSystem();

		protected void CloseUserTransaction()
		{
			CloseTransaction(_transaction, false, false);
		}

		protected void CloseSystemTransaction()
		{
			CloseTransaction(_systemTransaction, true, false);
		}

		public abstract void CloseTransaction(Transaction transaction, bool isSystemTransaction
			, bool rollbackOnClose);

		protected abstract void ShutdownDataStorage();

		/// <exception cref="Db4objects.Db4o.Ext.DatabaseReadOnlyException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		public void Commit(Transaction trans)
		{
			lock (_lock)
			{
				if (DTrace.enabled)
				{
					DTrace.Commit.Log();
				}
				CheckReadOnly();
				AsTopLevelCall(new _IFunction4_399(this), trans);
			}
		}

		private sealed class _IFunction4_399 : IFunction4
		{
			public _IFunction4_399(ObjectContainerBase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public object Apply(object trans)
			{
				this._enclosing.Commit1(((Transaction)trans));
				((Transaction)trans).PostCommit();
				return null;
			}

			private readonly ObjectContainerBase _enclosing;
		}

		private object AsTopLevelStore(IFunction4 block, Transaction trans)
		{
			trans = CheckTransaction(trans);
			object result = AsTopLevelCall(block, trans);
			if (_stackDepth == 0)
			{
				trans.ProcessDeletes();
			}
			return result;
		}

		// should never happen - just to make compiler happy
		public virtual void FatalShutdown(Exception origExc)
		{
			try
			{
				StopSession();
				FatalStorageShutdown();
			}
			catch (Exception exc)
			{
				throw new CompositeDb4oException(new Exception[] { origExc, exc });
			}
			Platform4.ThrowUncheckedException(origExc);
		}

		protected abstract void FatalStorageShutdown();

		public abstract void Commit1(Transaction trans);

		public virtual IConfiguration Configure()
		{
			return ConfigImpl;
		}

		public virtual Config4Impl Config()
		{
			return ConfigImpl;
		}

		public abstract int ConverterVersion();

		public abstract AbstractQueryResult NewQueryResult(Transaction trans, QueryEvaluationMode
			 mode);

		protected void CreateStringIO(byte encoding)
		{
			StringIO(BuiltInStringEncoding.StringIoForEncoding(encoding, ConfigImpl.StringEncoding
				()));
		}

		protected void InitializeTransactions()
		{
			_systemTransaction = NewSystemTransaction();
			_transaction = NewUserTransaction();
		}

		public abstract Transaction NewTransaction(Transaction parentTransaction, IReferenceSystem
			 referenceSystem, bool isSystemTransaction);

		public virtual Transaction NewUserTransaction()
		{
			return NewTransaction(SystemTransaction(), CreateReferenceSystem(), false);
		}

		public virtual Transaction NewSystemTransaction()
		{
			return NewTransaction(null, CreateReferenceSystem(), true);
		}

		public abstract long CurrentVersion();

		public virtual bool CreateClassMetadata(ClassMetadata classMeta, IReflectClass clazz
			, ClassMetadata superClassMeta)
		{
			return classMeta.Init(superClassMeta);
		}

		/// <summary>allows special handling for all Db4oType objects.</summary>
		/// <remarks>
		/// allows special handling for all Db4oType objects.
		/// Redirected here from #set() so only instanceof check is necessary
		/// in the #set() method.
		/// </remarks>
		/// <returns>object if handled here and #set() should not continue processing</returns>
		public virtual IDb4oType Db4oTypeStored(Transaction trans, object obj)
		{
			if (!(obj is Db4oDatabase))
			{
				return null;
			}
			Db4oDatabase database = (Db4oDatabase)obj;
			if (trans.ReferenceForObject(obj) != null)
			{
				return database;
			}
			ShowInternalClasses(true);
			try
			{
				return database.Query(trans);
			}
			finally
			{
				ShowInternalClasses(false);
			}
		}

		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		public void Deactivate(Transaction trans, object obj, int depth)
		{
			lock (_lock)
			{
				AsTopLevelCall(new _IFunction4_516(this, obj, depth), trans);
			}
		}

		private sealed class _IFunction4_516 : IFunction4
		{
			public _IFunction4_516(ObjectContainerBase _enclosing, object obj, int depth)
			{
				this._enclosing = _enclosing;
				this.obj = obj;
				this.depth = depth;
			}

			public object Apply(object trans)
			{
				this._enclosing.DeactivateInternal(((Transaction)trans), obj, this._enclosing.ActivationDepthProvider
					().ActivationDepth(depth, ActivationMode.Deactivate));
				return null;
			}

			private readonly ObjectContainerBase _enclosing;

			private readonly object obj;

			private readonly int depth;
		}

		private void DeactivateInternal(Transaction trans, object obj, IActivationDepth depth
			)
		{
			StillToDeactivate(trans, obj, depth, true);
			DeactivatePending(trans);
		}

		private void DeactivatePending(Transaction trans)
		{
			while (_stillToDeactivate != null)
			{
				IEnumerator i = new Iterator4Impl(_stillToDeactivate);
				_stillToDeactivate = null;
				while (i.MoveNext())
				{
					ObjectContainerBase.PendingActivation item = (ObjectContainerBase.PendingActivation
						)i.Current;
					item.@ref.Deactivate(trans, item.depth);
				}
			}
		}

		/// <exception cref="Db4objects.Db4o.Ext.DatabaseReadOnlyException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		public void Delete(Transaction trans, object obj)
		{
			if (null == obj)
			{
				throw new ArgumentNullException();
			}
			lock (Lock())
			{
				trans = CheckTransaction(trans);
				CheckReadOnly();
				Delete1(trans, obj, true);
				UnregisterFromTransparentPersistence(trans, obj);
				trans.ProcessDeletes();
			}
		}

		public void Delete1(Transaction trans, object obj, bool userCall)
		{
			if (obj == null)
			{
				return;
			}
			ObjectReference @ref = trans.ReferenceForObject(obj);
			if (@ref == null)
			{
				return;
			}
			if (userCall)
			{
				GenerateCallIDOnTopLevel();
			}
			AsTopLevelCall(new _IFunction4_565(this, @ref, obj, userCall), trans);
		}

		private sealed class _IFunction4_565 : IFunction4
		{
			public _IFunction4_565(ObjectContainerBase _enclosing, ObjectReference @ref, object
				 obj, bool userCall)
			{
				this._enclosing = _enclosing;
				this.@ref = @ref;
				this.obj = obj;
				this.userCall = userCall;
			}

			public object Apply(object trans)
			{
				this._enclosing.Delete2(((Transaction)trans), @ref, obj, 0, userCall);
				return null;
			}

			private readonly ObjectContainerBase _enclosing;

			private readonly ObjectReference @ref;

			private readonly object obj;

			private readonly bool userCall;
		}

		public void Delete2(Transaction trans, ObjectReference @ref, object obj, int cascade
			, bool userCall)
		{
			// This check is performed twice, here and in delete3, intentionally.
			if (BreakDeleteForEnum(@ref, userCall))
			{
				return;
			}
			if (obj is Entry)
			{
				if (!FlagForDelete(@ref))
				{
					return;
				}
				Delete3(trans, @ref, obj, cascade, userCall);
				return;
			}
			trans.Delete(@ref, @ref.GetID(), cascade);
		}

		internal void Delete3(Transaction trans, ObjectReference @ref, object obj, int cascade
			, bool userCall)
		{
			// The passed reference can be null, when calling from Transaction.
			if (@ref == null || !@ref.BeginProcessing())
			{
				return;
			}
			// This check is performed twice, here and in delete2, intentionally.
			if (BreakDeleteForEnum(@ref, userCall))
			{
				@ref.EndProcessing();
				return;
			}
			if (!@ref.IsFlaggedForDelete())
			{
				@ref.EndProcessing();
				return;
			}
			ClassMetadata yc = @ref.ClassMetadata();
			// We have to end processing temporarily here, otherwise the can delete callback
			// can't do anything at all with this object.
			@ref.EndProcessing();
			ActivateForDeletionCallback(trans, yc, @ref, obj);
			if (!ObjectCanDelete(trans, yc, @ref))
			{
				return;
			}
			@ref.BeginProcessing();
			if (DTrace.enabled)
			{
				DTrace.Delete.Log(@ref.GetID());
			}
			if (Delete4(trans, @ref, obj, cascade, userCall))
			{
				ObjectOnDelete(trans, yc, @ref);
				if (ConfigImpl.MessageLevel() > Const4.State)
				{
					Message(string.Empty + @ref.GetID() + " delete " + @ref.ClassMetadata().GetName()
						);
				}
			}
			@ref.EndProcessing();
		}

		private void UnregisterFromTransparentPersistence(Transaction trans, object obj)
		{
			if (!(ActivationDepthProvider() is ITransparentActivationDepthProvider))
			{
				return;
			}
			ITransparentActivationDepthProvider provider = (ITransparentActivationDepthProvider
				)ActivationDepthProvider();
			provider.RemoveModified(obj, trans);
		}

		private void ActivateForDeletionCallback(Transaction trans, ClassMetadata classMetadata
			, ObjectReference @ref, object obj)
		{
			if (!@ref.IsActive() && (CaresAboutDeleting(classMetadata) || CaresAboutDeleted(classMetadata
				)))
			{
				// Activate Objects for Callbacks, because in C/S mode Objects are not activated on the Server
				// FIXME: [TA] review activation depth
				IActivationDepth depth = classMetadata.AdjustCollectionDepthToBorders(new FixedActivationDepth
					(1));
				Activate(trans, obj, depth);
			}
		}

		private bool CaresAboutDeleting(ClassMetadata yc)
		{
			return this._callbacks.CaresAboutDeleting() || yc.HasEventRegistered(SystemTransaction
				(), EventDispatchers.CanDelete);
		}

		private bool CaresAboutDeleted(ClassMetadata yc)
		{
			return this._callbacks.CaresAboutDeleted() || yc.HasEventRegistered(SystemTransaction
				(), EventDispatchers.Delete);
		}

		private bool ObjectCanDelete(Transaction transaction, ClassMetadata yc, IObjectInfo
			 objectInfo)
		{
			return Callbacks().ObjectCanDelete(transaction, objectInfo) && yc.DispatchEvent(transaction
				, objectInfo.GetObject(), EventDispatchers.CanDelete);
		}

		private void ObjectOnDelete(Transaction transaction, ClassMetadata yc, IObjectInfo
			 reference)
		{
			Callbacks().ObjectOnDelete(transaction, reference);
			yc.DispatchEvent(transaction, reference.GetObject(), EventDispatchers.Delete);
		}

		public abstract bool Delete4(Transaction ta, ObjectReference @ref, object obj, int
			 a_cascade, bool userCall);

		internal virtual object Descend(Transaction trans, object obj, string[] path)
		{
			lock (_lock)
			{
				trans = CheckTransaction(trans);
				ObjectReference @ref = trans.ReferenceForObject(obj);
				if (@ref == null)
				{
					return null;
				}
				string fieldName = path[0];
				if (fieldName == null)
				{
					return null;
				}
				ClassMetadata classMetadata = @ref.ClassMetadata();
				ByRef foundField = new ByRef();
				classMetadata.TraverseAllAspects(new _TraverseFieldCommand_693(fieldName, foundField
					));
				FieldMetadata field = (FieldMetadata)foundField.value;
				if (field == null)
				{
					return null;
				}
				object child = @ref.IsActive() ? field.Get(trans, obj) : DescendMarshallingContext
					(trans, @ref).ReadFieldValue(field);
				if (path.Length == 1)
				{
					return child;
				}
				if (child == null)
				{
					return null;
				}
				string[] subPath = new string[path.Length - 1];
				System.Array.Copy(path, 1, subPath, 0, path.Length - 1);
				return Descend(trans, child, subPath);
			}
		}

		private sealed class _TraverseFieldCommand_693 : TraverseFieldCommand
		{
			public _TraverseFieldCommand_693(string fieldName, ByRef foundField)
			{
				this.fieldName = fieldName;
				this.foundField = foundField;
			}

			protected override void Process(FieldMetadata field)
			{
				if (field.CanAddToQuery(fieldName))
				{
					foundField.value = field;
				}
			}

			private readonly string fieldName;

			private readonly ByRef foundField;
		}

		private UnmarshallingContext DescendMarshallingContext(Transaction trans, ObjectReference
			 @ref)
		{
			UnmarshallingContext context = new UnmarshallingContext(trans, @ref, Const4.AddToIdTree
				, false);
			context.ActivationDepth(ActivationDepthProvider().ActivationDepth(1, ActivationMode
				.Activate));
			return context;
		}

		public virtual bool DetectSchemaChanges()
		{
			// overriden in YapClient
			return ConfigImpl.DetectSchemaChanges();
		}

		public virtual bool DispatchsEvents()
		{
			return true;
		}

		protected virtual bool DoFinalize()
		{
			return true;
		}

		internal void ShutdownHook()
		{
			if (IsClosed())
			{
				return;
			}
			if (AllOperationsCompleted())
			{
				Db4objects.Db4o.Internal.Messages.LogErr(ConfigImpl, 50, ToString(), null);
				Close();
			}
			else
			{
				ShutdownObjectContainer();
				if (OperationIsProcessing())
				{
					Db4objects.Db4o.Internal.Messages.LogErr(ConfigImpl, 24, null, null);
				}
			}
		}

		private bool OperationIsProcessing()
		{
			return _stackDepth > 0;
		}

		private bool AllOperationsCompleted()
		{
			return _stackDepth == 0;
		}

		internal virtual void FatalException(int msgID)
		{
			FatalException(null, msgID);
		}

		internal void FatalException(Exception t)
		{
			FatalException(t, Db4objects.Db4o.Internal.Messages.FatalMsgId);
		}

		internal void FatalException(Exception t, int msgID)
		{
			if (DTrace.enabled)
			{
				DTrace.FatalException.Log(t.ToString());
			}
			Db4objects.Db4o.Internal.Messages.LogErr(ConfigImpl, (msgID == Db4objects.Db4o.Internal.Messages
				.FatalMsgId ? 18 : msgID), null, t);
			if (!IsClosed())
			{
				ShutdownObjectContainer();
			}
			throw new Db4oException(Db4objects.Db4o.Internal.Messages.Get(msgID));
		}

		private bool ConfiguredForAutomaticShutDown()
		{
			return (ConfigImpl == null || ConfigImpl.AutomaticShutDown());
		}

		internal virtual void Gc()
		{
			_references.Purge();
		}

		public IObjectSet QueryByExample(Transaction trans, object template)
		{
			lock (_lock)
			{
				trans = CheckTransaction(trans);
				IQueryResult res = ((IQueryResult)AsTopLevelCall(new _IFunction4_810(this, template
					), trans));
				return new ObjectSetFacade(res);
			}
		}

		private sealed class _IFunction4_810 : IFunction4
		{
			public _IFunction4_810(ObjectContainerBase _enclosing, object template)
			{
				this._enclosing = _enclosing;
				this.template = template;
			}

			public object Apply(object trans)
			{
				return this._enclosing.QueryByExampleInternal(((Transaction)trans), template);
			}

			private readonly ObjectContainerBase _enclosing;

			private readonly object template;
		}

		private IQueryResult QueryByExampleInternal(Transaction trans, object template)
		{
			if (template == null || template.GetType() == Const4.ClassObject || template == Const4
				.ClassObject)
			{
				return QueryAllObjects(trans);
			}
			IQuery q = Query(trans);
			q.Constrain(template).ByExample();
			return ExecuteQuery((QQuery)q);
		}

		public abstract AbstractQueryResult QueryAllObjects(Transaction ta);

		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		public object TryGetByID(Transaction ta, long id)
		{
			try
			{
				return GetByID(ta, id);
			}
			catch (InvalidSlotException)
			{
			}
			catch (InvalidIDException)
			{
			}
			// can happen return null
			// can happen return null
			return null;
		}

		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.InvalidIDException"></exception>
		public object GetByID(Transaction ta, long id)
		{
			lock (_lock)
			{
				if (id <= 0 || id >= int.MaxValue)
				{
					throw new ArgumentException();
				}
				CheckClosed();
				ta = CheckTransaction(ta);
				BeginTopLevelCall();
				try
				{
					return GetByID2(ta, (int)id);
				}
				catch (Db4oRecoverableException exc)
				{
					throw;
				}
				catch (OutOfMemoryException e)
				{
					throw new Db4oRecoverableException(e);
				}
				catch (Exception e)
				{
					throw new Db4oRecoverableException(e);
				}
				finally
				{
					// Never shut down for getById()
					// There may be OutOfMemoryErrors or similar
					// The user may want to catch and continue working.
					EndTopLevelCall();
				}
			}
		}

		public virtual object GetByID2(Transaction ta, int id)
		{
			object obj = ta.ObjectForIdFromCache(id);
			if (obj != null)
			{
				// Take care about handling the returned candidate reference.
				// If you loose the reference, weak reference management might
				// also.
				return obj;
			}
			return new ObjectReference(id).Read(ta, new LegacyActivationDepth(0), Const4.AddToIdTree
				, true);
		}

		public object GetActivatedObjectFromCache(Transaction ta, int id)
		{
			object obj = ta.ObjectForIdFromCache(id);
			if (obj == null)
			{
				return null;
			}
			Activate(ta, obj);
			return obj;
		}

		public object ReadActivatedObjectNotInCache(Transaction trans, int id)
		{
			object obj = AsTopLevelCall(new _IFunction4_892(id), trans);
			ActivatePending(trans);
			return obj;
		}

		private sealed class _IFunction4_892 : IFunction4
		{
			public _IFunction4_892(int id)
			{
				this.id = id;
			}

			public object Apply(object trans)
			{
				return new ObjectReference(id).Read(((Transaction)trans), UnknownActivationDepth.
					Instance, Const4.AddToIdTree, true);
			}

			private readonly int id;
		}

		public object GetByUUID(Transaction trans, Db4oUUID uuid)
		{
			lock (_lock)
			{
				if (uuid == null)
				{
					return null;
				}
				HardObjectReference hardRef = GetHardReferenceBySignature(CheckTransaction(trans)
					, uuid.GetLongPart(), uuid.GetSignaturePart());
				return hardRef._object;
			}
		}

		public virtual HardObjectReference GetHardReferenceBySignature(Transaction trans, 
			long uuid, byte[] signature)
		{
			return UUIDIndex().GetHardObjectReferenceBySignature(trans, uuid, signature);
		}

		public int GetID(Transaction trans, object obj)
		{
			lock (_lock)
			{
				trans = CheckTransaction(trans);
				CheckClosed();
				if (obj == null)
				{
					return 0;
				}
				ObjectReference yo = trans.ReferenceForObject(obj);
				if (yo != null)
				{
					return yo.GetID();
				}
				return 0;
			}
		}

		public IObjectInfo GetObjectInfo(Transaction trans, object obj)
		{
			lock (_lock)
			{
				trans = CheckTransaction(trans);
				return trans.ReferenceForObject(obj);
			}
		}

		public HardObjectReference GetHardObjectReferenceById(Transaction trans, int id)
		{
			if (id <= 0)
			{
				return HardObjectReference.Invalid;
			}
			ObjectReference @ref = trans.ReferenceForId(id);
			if (@ref != null)
			{
				// Take care about handling the returned candidate reference.
				// If you loose the reference, weak reference management might also.
				object candidate = @ref.GetObject();
				if (candidate != null)
				{
					return new HardObjectReference(@ref, candidate);
				}
				trans.RemoveReference(@ref);
			}
			@ref = new ObjectReference(id);
			object readObject = @ref.Read(trans, new LegacyActivationDepth(0), Const4.AddToIdTree
				, true);
			if (readObject == null)
			{
				return HardObjectReference.Invalid;
			}
			// check class creation side effect and simply retry recursively
			// if it hits:
			if (readObject != @ref.GetObject())
			{
				return GetHardObjectReferenceById(trans, id);
			}
			return new HardObjectReference(@ref, readObject);
		}

		public StatefulBuffer CreateStatefulBuffer(Transaction trans, int address, int length
			)
		{
			if (Debug4.ExceedsMaximumBlockSize(length))
			{
				return null;
			}
			return new StatefulBuffer(trans, address, length);
		}

		public Transaction SystemTransaction()
		{
			return _systemTransaction;
		}

		public Transaction Transaction
		{
			get
			{
				return _transaction;
			}
		}

		public virtual ClassMetadata ClassMetadataForReflectClass(IReflectClass claxx)
		{
			if (null == claxx)
			{
				throw new ArgumentNullException();
			}
			if (HideClassForExternalUse(claxx))
			{
				return null;
			}
			ClassMetadata classMetadata = _handlers.ClassMetadataForClass(claxx);
			if (classMetadata != null)
			{
				return classMetadata;
			}
			return _classCollection.ClassMetadataForReflectClass(claxx);
		}

		// TODO: Some ReflectClass implementations could hold a 
		// reference to ClassMetadata to improve lookup performance here.
		public virtual ClassMetadata ProduceClassMetadata(IReflectClass claxx)
		{
			if (null == claxx)
			{
				throw new ArgumentNullException();
			}
			if (HideClassForExternalUse(claxx))
			{
				return null;
			}
			ClassMetadata classMetadata = _handlers.ClassMetadataForClass(claxx);
			if (classMetadata != null)
			{
				return classMetadata;
			}
			return _classCollection.ProduceClassMetadata(claxx);
		}

		/// <summary>
		/// Differentiating getActiveClassMetadata from getYapClass is a tuning
		/// optimization: If we initialize a YapClass, #set3() has to check for
		/// the possibility that class initialization associates the currently
		/// stored object with a previously stored static object, causing the
		/// object to be known afterwards.
		/// </summary>
		/// <remarks>
		/// Differentiating getActiveClassMetadata from getYapClass is a tuning
		/// optimization: If we initialize a YapClass, #set3() has to check for
		/// the possibility that class initialization associates the currently
		/// stored object with a previously stored static object, causing the
		/// object to be known afterwards.
		/// In this call we only return active YapClasses, initialization
		/// is not done on purpose
		/// </remarks>
		internal ClassMetadata GetActiveClassMetadata(IReflectClass claxx)
		{
			if (HideClassForExternalUse(claxx))
			{
				return null;
			}
			return _classCollection.GetActiveClassMetadata(claxx);
		}

		private bool HideClassForExternalUse(IReflectClass claxx)
		{
			if ((!ShowInternalClasses()) && _handlers.IclassInternal.IsAssignableFrom(claxx))
			{
				return true;
			}
			return false;
		}

		public virtual int ClassMetadataIdForName(string name)
		{
			return _classCollection.ClassMetadataIdForName(name);
		}

		public virtual ClassMetadata ClassMetadataForName(string name)
		{
			return ClassMetadataForID(ClassMetadataIdForName(name));
		}

		public virtual ClassMetadata ClassMetadataForID(int id)
		{
			if (DTrace.enabled)
			{
				DTrace.ClassmetadataById.Log(id);
			}
			if (id == 0)
			{
				return null;
			}
			ClassMetadata classMetadata = _handlers.ClassMetadataForId(id);
			if (classMetadata != null)
			{
				return classMetadata;
			}
			return _classCollection.ClassMetadataForId(id);
		}

		public virtual HandlerRegistry Handlers
		{
			get
			{
				return _handlers;
			}
		}

		public virtual bool NeedsLockFileThread()
		{
			if (!Platform4.NeedsLockFileThread())
			{
				return false;
			}
			if (ConfigImpl.IsReadOnly())
			{
				return false;
			}
			return ConfigImpl.LockFile();
		}

		protected virtual bool HasShutDownHook()
		{
			return ConfigImpl.AutomaticShutDown();
		}

		protected virtual void Initialize1(IConfiguration config)
		{
			_config = InitializeConfig(config);
			_handlers = new HandlerRegistry(this, ConfigImpl.Encoding(), ConfigImpl.Reflector
				());
			if (_references != null)
			{
				Gc();
				_references.Stop();
			}
			_references = WeakReferenceSupportFactory.ForObjectContainer(this);
			if (HasShutDownHook())
			{
				Platform4.AddShutDownHook(this);
			}
			_handlers.InitEncryption(ConfigImpl);
			_stillToSet = null;
		}

		private Config4Impl InitializeConfig(IConfiguration config)
		{
			Config4Impl impl = ((Config4Impl)config);
			impl.Container(this);
			impl.Reflector().SetTransaction(SystemTransaction());
			impl.Reflector().Configuration(new ReflectorConfigurationImpl(impl));
			impl.Taint();
			return impl;
		}

		public virtual IReferenceSystem CreateReferenceSystem()
		{
			IReferenceSystem referenceSystem = _referenceSystemFactory.NewReferenceSystem(this
				);
			_referenceSystemRegistry.AddReferenceSystem(referenceSystem);
			return referenceSystem;
		}

		protected virtual void InitalizeWeakReferenceSupport()
		{
			_references.Start();
		}

		protected virtual void InitializeClassMetadataRepository()
		{
			_classCollection = new ClassMetadataRepository(_systemTransaction);
		}

		private void InitializePostOpen()
		{
			_showInternalClasses = 100000;
			InitializePostOpenExcludingTransportObjectContainer();
			_showInternalClasses = 0;
		}

		protected virtual void InitializePostOpenExcludingTransportObjectContainer()
		{
			InitializeEssentialClasses();
			Rename(ConfigImpl);
			_classCollection.InitOnUp(_systemTransaction);
			_transaction.PostOpen();
			if (ConfigImpl.DetectSchemaChanges())
			{
				if (!ConfigImpl.IsReadOnly())
				{
					_systemTransaction.Commit();
				}
			}
			ConfigImpl.ApplyConfigurationItems(this);
		}

		internal virtual void InitializeEssentialClasses()
		{
			for (int i = 0; i < Const4.EssentialClasses.Length; i++)
			{
				ProduceClassMetadata(Reflector().ForClass(Const4.EssentialClasses[i]));
			}
		}

		internal bool IsActive(Transaction trans, object obj)
		{
			lock (_lock)
			{
				trans = CheckTransaction(trans);
				if (obj != null)
				{
					ObjectReference @ref = trans.ReferenceForObject(obj);
					if (@ref != null)
					{
						return @ref.IsActive();
					}
				}
				return false;
			}
		}

		public virtual bool IsCached(Transaction trans, long id)
		{
			lock (_lock)
			{
				trans = CheckTransaction(trans);
				return trans.ObjectForIdFromCache((int)id) != null;
			}
		}

		/// <summary>
		/// overridden in ClientObjectContainer
		/// The method allows checking whether will make it easier to refactor than
		/// an "instanceof YapClient" check.
		/// </summary>
		/// <remarks>
		/// overridden in ClientObjectContainer
		/// The method allows checking whether will make it easier to refactor than
		/// an "instanceof YapClient" check.
		/// </remarks>
		public virtual bool IsClient
		{
			get
			{
				return false;
			}
		}

		public bool IsClosed()
		{
			lock (_lock)
			{
				// this is set to null in close2 and is therefore our check for down.
				return _classCollection == null;
			}
		}

		internal virtual bool IsServer()
		{
			return false;
		}

		public bool IsStored(Transaction trans, object obj)
		{
			lock (_lock)
			{
				trans = CheckTransaction(trans);
				if (obj == null)
				{
					return false;
				}
				ObjectReference @ref = trans.ReferenceForObject(obj);
				if (@ref == null)
				{
					return false;
				}
				return !IsDeleted(trans, @ref.GetID());
			}
		}

		public virtual IReflectClass[] KnownClasses()
		{
			lock (_lock)
			{
				CheckClosed();
				return Reflector().KnownClasses();
			}
		}

		public virtual ITypeHandler4 TypeHandlerForClass(IReflectClass claxx)
		{
			if (HideClassForExternalUse(claxx))
			{
				return null;
			}
			ITypeHandler4 typeHandler = _handlers.TypeHandlerForClass(claxx);
			if (typeHandler != null)
			{
				return typeHandler;
			}
			return _classCollection.ProduceClassMetadata(claxx).TypeHandler();
		}

		public virtual ITypeHandler4 TypeHandlerForClassMetadataID(int id)
		{
			if (id < 1)
			{
				return null;
			}
			ClassMetadata classMetadata = ClassMetadataForID(id);
			if (classMetadata == null)
			{
				return null;
			}
			return classMetadata.TypeHandler();
		}

		public virtual object Lock()
		{
			return _lock;
		}

		public void LogMsg(int code, string msg)
		{
			Db4objects.Db4o.Internal.Messages.LogMsg(ConfigImpl, code, msg);
		}

		public virtual bool MaintainsIndices()
		{
			return true;
		}

		internal virtual void Message(string msg)
		{
			new MessageOutput(this, msg);
		}

		public void NeedsUpdate(ClassMetadata classMetadata)
		{
			_pendingClassUpdates = new List4(_pendingClassUpdates, classMetadata);
		}

		public virtual long GenerateTimeStampId()
		{
			return _timeStampIdGenerator.Generate();
		}

		public abstract int IdForNewUserObject(Transaction trans);

		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		public virtual object PeekPersisted(Transaction trans, object obj, IActivationDepth
			 depth, bool committed)
		{
			// TODO: peekPersisted is not stack overflow safe, if depth is too high. 
			lock (_lock)
			{
				CheckClosed();
				return AsTopLevelCall(new _IFunction4_1271(this, obj, committed, depth), trans);
			}
		}

		private sealed class _IFunction4_1271 : IFunction4
		{
			public _IFunction4_1271(ObjectContainerBase _enclosing, object obj, bool committed
				, IActivationDepth depth)
			{
				this._enclosing = _enclosing;
				this.obj = obj;
				this.committed = committed;
				this.depth = depth;
			}

			public object Apply(object trans)
			{
				trans = this._enclosing.CheckTransaction(((Transaction)trans));
				ObjectReference @ref = ((Transaction)trans).ReferenceForObject(obj);
				trans = committed ? this._enclosing._systemTransaction : ((Transaction)trans);
				object cloned = null;
				if (@ref != null)
				{
					cloned = this._enclosing.PeekPersisted(((Transaction)trans), @ref.GetID(), depth, 
						true);
				}
				return cloned;
			}

			private readonly ObjectContainerBase _enclosing;

			private readonly object obj;

			private readonly bool committed;

			private readonly IActivationDepth depth;
		}

		public object PeekPersisted(Transaction trans, int id, IActivationDepth depth, bool
			 resetJustPeeked)
		{
			if (resetJustPeeked)
			{
				_justPeeked = null;
			}
			else
			{
				TreeInt ti = new TreeInt(id);
				TreeIntObject tio = (TreeIntObject)Tree.Find(_justPeeked, ti);
				if (tio != null)
				{
					return tio._object;
				}
			}
			ObjectReference @ref = PeekReference(trans, id, depth, resetJustPeeked);
			return @ref.GetObject();
		}

		public virtual ObjectReference PeekReference(Transaction trans, int id, IActivationDepth
			 depth, bool resetJustPeeked)
		{
			ObjectReference @ref = new ObjectReference(id);
			@ref.PeekPersisted(trans, depth);
			if (resetJustPeeked)
			{
				_justPeeked = null;
			}
			return @ref;
		}

		internal virtual void Peeked(int id, object obj)
		{
			_justPeeked = Tree.Add(_justPeeked, new TreeIntObject(id, obj));
		}

		public virtual void Purge()
		{
			lock (_lock)
			{
				CheckClosed();
				Runtime.Gc();
				Runtime.RunFinalization();
				Runtime.Gc();
				Gc();
				_classCollection.Purge();
			}
		}

		public void Purge(Transaction trans, object obj)
		{
			lock (_lock)
			{
				trans = CheckTransaction(trans);
				trans.RemoveObjectFromReferenceSystem(obj);
			}
		}

		internal void RemoveFromAllReferenceSystems(object obj)
		{
			if (obj == null)
			{
				return;
			}
			if (obj is ObjectReference)
			{
				_referenceSystemRegistry.RemoveReference((ObjectReference)obj);
				return;
			}
			_referenceSystemRegistry.RemoveObject(obj);
		}

		public NativeQueryHandler GetNativeQueryHandler()
		{
			lock (_lock)
			{
				if (null == _nativeQueryHandler)
				{
					_nativeQueryHandler = new NativeQueryHandler(this);
				}
				return _nativeQueryHandler;
			}
		}

		public IObjectSet Query(Transaction trans, Predicate predicate)
		{
			return Query(trans, predicate, (IQueryComparator)null);
		}

		public IObjectSet Query(Transaction trans, Predicate predicate, IQueryComparator 
			comparator)
		{
			lock (_lock)
			{
				return GetNativeQueryHandler().Execute(Query(trans), predicate, comparator);
			}
		}

		public IObjectSet Query(Transaction trans, Type clazz)
		{
			return QueryByExample(trans, clazz);
		}

		public IQuery Query(Transaction ta)
		{
			return new QQuery(CheckTransaction(ta), null, null);
		}

		public abstract void RaiseCommitTimestamp(long minimumTimestamp);

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public abstract void ReadBytes(byte[] a_bytes, int a_address, int a_length);

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public abstract void ReadBytes(byte[] bytes, int address, int addressOffset, int 
			length);

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public ByteArrayBuffer DecryptedBufferByAddress(int address, int length)
		{
			ByteArrayBuffer reader = RawBufferByAddress(address, length);
			_handlers.Decrypt(reader);
			return reader;
		}

		public virtual ByteArrayBuffer RawBufferByAddress(int address, int length)
		{
			CheckAddress(address);
			ByteArrayBuffer reader = new ByteArrayBuffer(length);
			ReadBytes(reader._buffer, address, length);
			return reader;
		}

		/// <exception cref="System.ArgumentException"></exception>
		private void CheckAddress(int address)
		{
			if (address <= 0)
			{
				throw new ArgumentException("Invalid address offset: " + address);
			}
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public StatefulBuffer ReadWriterByAddress(Transaction a_trans, int address, int length
			)
		{
			CheckAddress(address);
			StatefulBuffer reader = CreateStatefulBuffer(a_trans, address, length);
			reader.ReadEncrypt(this, address);
			return reader;
		}

		public abstract StatefulBuffer ReadStatefulBufferById(Transaction trans, int id);

		public abstract StatefulBuffer ReadStatefulBufferById(Transaction trans, int id, 
			bool lastCommitted);

		public abstract ByteArrayBuffer ReadBufferById(Transaction trans, int id);

		public abstract ByteArrayBuffer ReadBufferById(Transaction trans, int id, bool lastCommitted
			);

		public abstract ByteArrayBuffer[] ReadSlotBuffers(Transaction trans, int[] ids);

		private void Reboot()
		{
			Commit(null);
			Close();
			Open();
		}

		public virtual GenericReflector Reflector()
		{
			return _handlers._reflector;
		}

		public void Refresh(Transaction trans, object obj, int depth)
		{
			lock (_lock)
			{
				RefreshInternal(trans, obj, depth);
			}
		}

		protected virtual void RefreshInternal(Transaction trans, object obj, int depth)
		{
			Activate(trans, obj, RefreshActivationDepth(depth));
		}

		private IActivationDepth RefreshActivationDepth(int depth)
		{
			return ActivationDepthProvider().ActivationDepth(depth, ActivationMode.Refresh);
		}

		public abstract void ReleaseSemaphore(string name);

		public virtual void FlagAsHandled(ObjectReference @ref)
		{
			@ref.FlagAsHandled(_topLevelCallId);
		}

		internal virtual bool FlagForDelete(ObjectReference @ref)
		{
			if (@ref == null)
			{
				return false;
			}
			if (HandledInCurrentTopLevelCall(@ref))
			{
				return false;
			}
			@ref.FlagForDelete(_topLevelCallId);
			return true;
		}

		public abstract void ReleaseSemaphores(Transaction ta);

		internal virtual void Rename(Config4Impl config)
		{
			bool renamedOne = false;
			if (config.Rename() != null)
			{
				renamedOne = ApplyRenames(config);
			}
			_classCollection.CheckChanges();
			if (renamedOne)
			{
				Reboot();
			}
		}

		protected virtual bool ApplyRenames(Config4Impl config)
		{
			bool renamed = false;
			IEnumerator i = config.Rename().GetEnumerator();
			while (i.MoveNext())
			{
				Rename ren = (Rename)i.Current;
				if (AlreadyApplied(ren))
				{
					continue;
				}
				if (ApplyRename(ren))
				{
					renamed = true;
				}
			}
			return renamed;
		}

		private bool ApplyRename(Rename ren)
		{
			if (ren.IsField())
			{
				return ApplyFieldRename(ren);
			}
			return ApplyClassRename(ren);
		}

		private bool ApplyClassRename(Rename ren)
		{
			ClassMetadata classToRename = _classCollection.GetClassMetadata(ren.rFrom);
			if (classToRename == null)
			{
				return false;
			}
			ClassMetadata existing = _classCollection.GetClassMetadata(ren.rTo);
			if (existing != null)
			{
				LogMsg(9, "class " + ren.rTo);
				return false;
			}
			classToRename.SetName(ren.rTo);
			CommitRenameFor(ren, classToRename);
			return true;
		}

		private bool ApplyFieldRename(Rename ren)
		{
			ClassMetadata parentClass = _classCollection.GetClassMetadata(ren.rClass);
			if (parentClass == null)
			{
				return false;
			}
			if (!parentClass.RenameField(ren.rFrom, ren.rTo))
			{
				return false;
			}
			CommitRenameFor(ren, parentClass);
			return true;
		}

		private void CommitRenameFor(Rename rename, ClassMetadata classMetadata)
		{
			SetDirtyInSystemTransaction(classMetadata);
			LogMsg(8, rename.rFrom + " to " + rename.rTo);
			DeleteInverseRenames(rename);
			// store the rename, so we only do it once
			Store(SystemTransaction(), rename);
		}

		private void DeleteInverseRenames(Rename rename)
		{
			// delete all that rename from the new name
			// to allow future backswitching
			IObjectSet inverseRenames = QueryInverseRenames(rename);
			while (inverseRenames.HasNext())
			{
				Delete(SystemTransaction(), inverseRenames.Next());
			}
		}

		private IObjectSet QueryInverseRenames(Rename ren)
		{
			return QueryByExample(SystemTransaction(), Renames.ForInverseQBE(ren));
		}

		private bool AlreadyApplied(Rename ren)
		{
			return QueryByExample(SystemTransaction(), ren).Count != 0;
		}

		public bool HandledInCurrentTopLevelCall(ObjectReference @ref)
		{
			return @ref.IsFlaggedAsHandled(_topLevelCallId);
		}

		public abstract void Reserve(int byteCount);

		public void Rollback(Transaction trans)
		{
			lock (_lock)
			{
				trans = CheckTransaction(trans);
				CheckReadOnly();
				Rollback1(trans);
				trans.RollbackReferenceSystem();
			}
		}

		public abstract void Rollback1(Transaction trans);

		/// <param name="obj"></param>
		public virtual void Send(object obj)
		{
			// TODO: implement
			throw new NotSupportedException();
		}

		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseReadOnlyException"></exception>
		public void Store(Transaction trans, object obj)
		{
			Store(trans, obj, UpdateDepthProvider().Unspecified(NullModifiedObjectQuery.Instance
				));
		}

		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseReadOnlyException"></exception>
		public int Store(Transaction trans, object obj, IUpdateDepth depth)
		{
			lock (_lock)
			{
				try
				{
					ShowInternalClasses(true);
					return StoreInternal(trans, obj, depth, true);
				}
				finally
				{
					ShowInternalClasses(false);
				}
			}
		}

		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseReadOnlyException"></exception>
		public int StoreInternal(Transaction trans, object obj, bool checkJustSet)
		{
			return StoreInternal(trans, obj, UpdateDepthProvider().Unspecified(NullModifiedObjectQuery
				.Instance), checkJustSet);
		}

		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseReadOnlyException"></exception>
		public virtual int StoreInternal(Transaction trans, object obj, IUpdateDepth depth
			, bool checkJustSet)
		{
			CheckReadOnly();
			return (((int)AsTopLevelStore(new _IFunction4_1599(this, obj, depth, checkJustSet
				), trans)));
		}

		private sealed class _IFunction4_1599 : IFunction4
		{
			public _IFunction4_1599(ObjectContainerBase _enclosing, object obj, IUpdateDepth 
				depth, bool checkJustSet)
			{
				this._enclosing = _enclosing;
				this.obj = obj;
				this.depth = depth;
				this.checkJustSet = checkJustSet;
			}

			public object Apply(object trans)
			{
				return this._enclosing.StoreAfterReplication(((Transaction)trans), obj, depth, checkJustSet
					);
			}

			private readonly ObjectContainerBase _enclosing;

			private readonly object obj;

			private readonly IUpdateDepth depth;

			private readonly bool checkJustSet;
		}

		public int StoreAfterReplication(Transaction trans, object obj, IUpdateDepth depth
			, bool checkJust)
		{
			if (obj is IDb4oType)
			{
				IDb4oType db4oType = Db4oTypeStored(trans, obj);
				if (db4oType != null)
				{
					return GetID(trans, db4oType);
				}
			}
			return Store2(trans, obj, depth, checkJust);
		}

		public void StoreByNewReplication(IDb4oReplicationReferenceProvider referenceProvider
			, object obj)
		{
			lock (_lock)
			{
				_replicationCallState = Const4.New;
				_handlers._replicationReferenceProvider = referenceProvider;
				try
				{
					Store2(CheckTransaction(), obj, UpdateDepthProvider().ForDepth(1), false);
				}
				finally
				{
					_replicationCallState = Const4.None;
					_handlers._replicationReferenceProvider = null;
				}
			}
		}

		public virtual void CheckStillToSet()
		{
			List4 postponedStillToSet = null;
			while (_stillToSet != null)
			{
				IEnumerator i = new Iterator4Impl(_stillToSet);
				_stillToSet = null;
				while (i.MoveNext())
				{
					ObjectContainerBase.PendingSet item = (ObjectContainerBase.PendingSet)i.Current;
					ObjectReference @ref = item.@ref;
					Transaction trans = item.transaction;
					if (!@ref.ContinueSet(trans, item.depth))
					{
						postponedStillToSet = new List4(postponedStillToSet, item);
					}
				}
			}
			_stillToSet = postponedStillToSet;
		}

		internal virtual void NotStorable(IReflectClass claxx, object obj, string message
			)
		{
			if (!ConfigImpl.ExceptionsOnNotStorable())
			{
				return;
			}
			if (claxx == null)
			{
				throw new ObjectNotStorableException(obj.ToString());
			}
			if (_handlers.IsTransient(claxx))
			{
				return;
			}
			if (message != null)
			{
				throw new ObjectNotStorableException(claxx, message);
			}
			throw new ObjectNotStorableException(claxx);
		}

		public int Store2(Transaction trans, object obj, IUpdateDepth updateDepth, bool checkJustSet
			)
		{
			if (obj == null || (obj is ITransientClass))
			{
				return 0;
			}
			ObjectAnalyzer analyzer = new ObjectAnalyzer(this, obj);
			analyzer.Analyze(trans);
			if (analyzer.NotStorable())
			{
				return 0;
			}
			ObjectReference @ref = analyzer.ObjectReference();
			if (@ref == null)
			{
				ClassMetadata classMetadata = analyzer.ClassMetadata();
				if (!ObjectCanNew(trans, classMetadata, obj))
				{
					return 0;
				}
				@ref = new ObjectReference();
				@ref.Store(trans, classMetadata, obj);
				trans.AddNewReference(@ref);
				if (obj is IDb4oTypeImpl)
				{
					((IDb4oTypeImpl)obj).SetTrans(trans);
				}
				if (ConfigImpl.MessageLevel() > Const4.State)
				{
					Message(string.Empty + @ref.GetID() + " new " + @ref.ClassMetadata().GetName());
				}
				FlagAsHandled(@ref);
				StillToSet(trans, @ref, updateDepth);
			}
			else
			{
				if (@ref.IsFlaggedAsHandled(_topLevelCallId))
				{
					AssertNotInCallback();
				}
				if (CanUpdate())
				{
					if (checkJustSet)
					{
						if ((!@ref.IsNew()) && HandledInCurrentTopLevelCall(@ref))
						{
							return @ref.GetID();
						}
					}
					if (updateDepth.SufficientDepth())
					{
						FlagAsHandled(@ref);
						@ref.WriteUpdate(trans, updateDepth);
					}
				}
			}
			ProcessPendingClassUpdates();
			return @ref.GetID();
		}

		private void AssertNotInCallback()
		{
			if (InCallback())
			{
				throw new Db4oIllegalStateException("Objects must not be updated in callback");
			}
		}

		private bool ObjectCanNew(Transaction transaction, ClassMetadata yc, object obj)
		{
			return Callbacks().ObjectCanNew(transaction, obj) && yc.DispatchEvent(transaction
				, obj, EventDispatchers.CanNew);
		}

		public abstract void SetDirtyInSystemTransaction(PersistentBase a_object);

		public abstract bool SetSemaphore(string name, int timeout);

		public abstract bool SetSemaphore(Transaction trans, string name, int timeout);

		public abstract void ReleaseSemaphore(Transaction trans, string name);

		internal virtual void StringIO(LatinStringIO io)
		{
			_handlers.StringIO(io);
		}

		internal bool ShowInternalClasses()
		{
			return IsServer() || _showInternalClasses > 0;
		}

		/// <summary>
		/// Objects implementing the "Internal4" marker interface are
		/// not visible to queries, unless this flag is set to true.
		/// </summary>
		/// <remarks>
		/// Objects implementing the "Internal4" marker interface are
		/// not visible to queries, unless this flag is set to true.
		/// The caller should reset the flag after the call.
		/// </remarks>
		public virtual void ShowInternalClasses(bool show)
		{
			lock (this)
			{
				if (show)
				{
					_showInternalClasses++;
				}
				else
				{
					_showInternalClasses--;
				}
				if (_showInternalClasses < 0)
				{
					_showInternalClasses = 0;
				}
			}
		}

		private bool StackIsSmall()
		{
			return _stackDepth < _maxStackDepth;
		}

		internal virtual bool StateMessages()
		{
			return true;
		}

		// overridden to do nothing in YapObjectCarrier
		internal List4 StillTo1(Transaction trans, List4 still, object obj, IActivationDepth
			 depth)
		{
			if (obj == null || !depth.RequiresActivation())
			{
				return still;
			}
			ObjectReference @ref = trans.ReferenceForObject(obj);
			if (@ref != null)
			{
				if (HandledInCurrentTopLevelCall(@ref))
				{
					return still;
				}
				FlagAsHandled(@ref);
				return new List4(still, new ObjectContainerBase.PendingActivation(@ref, depth));
			}
			IReflectClass clazz = ReflectorForObject(obj);
			if (clazz.IsArray())
			{
				if (!clazz.GetComponentType().IsPrimitive())
				{
					IEnumerator arr = ArrayHandler.Iterator(clazz, obj);
					while (arr.MoveNext())
					{
						object current = arr.Current;
						if (current == null)
						{
							continue;
						}
						ClassMetadata classMetadata = ClassMetadataForObject(current);
						still = StillTo1(trans, still, current, depth.Descend(classMetadata));
					}
				}
				return still;
			}
			else
			{
				if (obj is Entry)
				{
					still = StillTo1(trans, still, ((Entry)obj).key, depth);
					still = StillTo1(trans, still, ((Entry)obj).value, depth);
				}
				else
				{
					if (depth.Mode().IsDeactivate())
					{
						// Special handling to deactivate .net structs
						ClassMetadata metadata = ClassMetadataForObject(obj);
						if (metadata != null && metadata.IsStruct())
						{
							metadata.ForceDeactivation(trans, depth, obj);
						}
					}
				}
			}
			return still;
		}

		public void StillToActivate(IActivationContext context)
		{
			// TODO: We don't want the simple classes to search the hc_tree
			// Kick them out here.
			//		if (a_object != null) {
			//			Class clazz = a_object.getClass();
			//			if(! clazz.isPrimitive()){
			if (ProcessedByImmediateActivation(context))
			{
				return;
			}
			_stillToActivate = StillTo1(context.Transaction(), _stillToActivate, context.TargetObject
				(), context.Depth());
		}

		private bool ProcessedByImmediateActivation(IActivationContext context)
		{
			if (!StackIsSmall())
			{
				return false;
			}
			if (!context.Depth().RequiresActivation())
			{
				return true;
			}
			ObjectReference @ref = context.Transaction().ReferenceForObject(context.TargetObject
				());
			if (@ref == null)
			{
				return false;
			}
			if (HandledInCurrentTopLevelCall(@ref))
			{
				return true;
			}
			FlagAsHandled(@ref);
			IncStackDepth();
			try
			{
				@ref.ActivateInternal(context);
			}
			finally
			{
				DecStackDepth();
			}
			return true;
		}

		private int DecStackDepth()
		{
			int i = _stackDepth--;
			if (StackIsSmall() && !_handlingStackLimitPendings)
			{
				_handlingStackLimitPendings = true;
				try
				{
					HandleStackLimitPendings();
				}
				finally
				{
					_handlingStackLimitPendings = false;
				}
			}
			return i;
		}

		private void HandleStackLimitPendings()
		{
			CheckStillToSet();
		}

		//		activatePending();
		//		deactivatePending();
		private int IncStackDepth()
		{
			return _stackDepth++;
		}

		public void StillToDeactivate(Transaction trans, object a_object, IActivationDepth
			 a_depth, bool a_forceUnknownDeactivate)
		{
			_stillToDeactivate = StillTo1(trans, _stillToDeactivate, a_object, a_depth);
		}

		internal class PendingSet
		{
			public readonly Transaction transaction;

			public readonly ObjectReference @ref;

			public readonly IUpdateDepth depth;

			public PendingSet(Transaction transaction_, ObjectReference ref_, IUpdateDepth depth_
				)
			{
				this.transaction = transaction_;
				this.@ref = ref_;
				this.depth = depth_;
			}
		}

		internal virtual void StillToSet(Transaction transaction, ObjectReference @ref, IUpdateDepth
			 updateDepth)
		{
			if (StackIsSmall())
			{
				if (@ref.ContinueSet(transaction, updateDepth))
				{
					return;
				}
			}
			_stillToSet = new List4(_stillToSet, new ObjectContainerBase.PendingSet(transaction
				, @ref, updateDepth));
		}

		protected void StopSession()
		{
			if (HasShutDownHook())
			{
				Platform4.RemoveShutDownHook(this);
			}
			_classCollection = null;
			if (_references != null)
			{
				_references.Stop();
			}
			_systemTransaction = null;
			_transaction = null;
		}

		public IStoredClass StoredClass(Transaction trans, object clazz)
		{
			lock (_lock)
			{
				trans = CheckTransaction(trans);
				IReflectClass claxx = ReflectorUtils.ReflectClassFor(Reflector(), clazz);
				if (claxx == null)
				{
					return null;
				}
				ClassMetadata classMetadata = ClassMetadataForReflectClass(claxx);
				if (classMetadata == null)
				{
					return null;
				}
				return new StoredClassImpl(trans, classMetadata);
			}
		}

		public virtual IStoredClass[] StoredClasses(Transaction trans)
		{
			lock (_lock)
			{
				trans = CheckTransaction(trans);
				IStoredClass[] classMetadata = _classCollection.StoredClasses();
				IStoredClass[] storedClasses = new IStoredClass[classMetadata.Length];
				for (int i = 0; i < classMetadata.Length; i++)
				{
					storedClasses[i] = new StoredClassImpl(trans, (ClassMetadata)classMetadata[i]);
				}
				return storedClasses;
			}
		}

		public virtual LatinStringIO StringIO()
		{
			return _handlers.StringIO();
		}

		public abstract ISystemInfo SystemInfo();

		private void BeginTopLevelCall()
		{
			if (DTrace.enabled)
			{
				DTrace.BeginTopLevelCall.Log();
			}
			GenerateCallIDOnTopLevel();
			IncStackDepth();
		}

		private void EndTopLevelCall()
		{
			if (DTrace.enabled)
			{
				DTrace.EndTopLevelCall.Log();
			}
			DecStackDepth();
			GenerateCallIDOnTopLevel();
		}

		private void GenerateCallIDOnTopLevel()
		{
			if (_stackDepth == 0)
			{
				_topLevelCallId = _topLevelCallIdGenerator.Next();
			}
		}

		public virtual int StackDepth()
		{
			return _stackDepth;
		}

		public virtual void StackDepth(int depth)
		{
			_stackDepth = depth;
		}

		public virtual int TopLevelCallId()
		{
			return _topLevelCallId;
		}

		public virtual void TopLevelCallId(int id)
		{
			_topLevelCallId = id;
		}

		public virtual long Version()
		{
			lock (_lock)
			{
				return CurrentVersion();
			}
		}

		public abstract void Shutdown();

		public abstract void WriteDirtyClassMetadata();

		public abstract void WriteNew(Transaction trans, Pointer4 pointer, ClassMetadata 
			classMetadata, ByteArrayBuffer buffer);

		public abstract void WriteUpdate(Transaction trans, Pointer4 pointer, ClassMetadata
			 classMetadata, ArrayType arrayType, ByteArrayBuffer buffer);

		public virtual ICallbacks Callbacks()
		{
			return _callbacks;
		}

		public virtual void Callbacks(ICallbacks cb)
		{
			if (cb == null)
			{
				throw new ArgumentException();
			}
			_callbacks = cb;
		}

		public virtual Config4Impl ConfigImpl
		{
			get
			{
				return _config;
			}
		}

		public virtual UUIDFieldMetadata UUIDIndex()
		{
			return _handlers.Indexes()._uUID;
		}

		public virtual VersionFieldMetadata VersionIndex()
		{
			return _handlers.Indexes()._version;
		}

		public virtual CommitTimestampFieldMetadata CommitTimestampIndex()
		{
			return _handlers.Indexes()._commitTimestamp;
		}

		public virtual ClassMetadataRepository ClassCollection()
		{
			return _classCollection;
		}

		public abstract long[] GetIDsForClass(Transaction trans, ClassMetadata clazz);

		public abstract IQueryResult ClassOnlyQuery(QQueryBase queryBase, ClassMetadata clazz
			);

		public abstract IQueryResult ExecuteQuery(QQuery query);

		public virtual void ReplicationCallState(int state)
		{
			_replicationCallState = state;
		}

		public virtual ReferenceSystemRegistry ReferenceSystemRegistry()
		{
			return _referenceSystemRegistry;
		}

		public virtual ObjectContainerBase Container
		{
			get
			{
				return this;
			}
		}

		public virtual void DeleteByID(Transaction transaction, int id, int cascadeDeleteDepth
			)
		{
			if (id <= 0)
			{
				throw new ArgumentException("ID: " + id);
			}
			//			return;
			if (cascadeDeleteDepth <= 0)
			{
				return;
			}
			object obj = GetByID2(transaction, id);
			if (obj == null)
			{
				return;
			}
			cascadeDeleteDepth--;
			IReflectClass claxx = ReflectorForObject(obj);
			if (claxx.IsCollection())
			{
				cascadeDeleteDepth += 1;
			}
			ObjectReference @ref = transaction.ReferenceForId(id);
			if (@ref == null)
			{
				return;
			}
			Delete2(transaction, @ref, obj, cascadeDeleteDepth, false);
		}

		internal virtual IReflectClass ReflectorForObject(object obj)
		{
			return Reflector().ForObject(obj);
		}

		public virtual object SyncExec(IClosure4 block)
		{
			lock (_lock)
			{
				CheckClosed();
				return block.Run();
			}
		}

		public virtual void StoreAll(Transaction transaction, IEnumerator objects)
		{
			while (objects.MoveNext())
			{
				Store(transaction, objects.Current);
			}
		}

		public virtual void StoreAll(Transaction transaction, IEnumerator objects, IUpdateDepth
			 depth)
		{
			while (objects.MoveNext())
			{
				Store(transaction, objects.Current, depth);
			}
		}

		public virtual void WithTransaction(Transaction transaction, IRunnable runnable)
		{
			lock (_lock)
			{
				Transaction old = _transaction;
				_transaction = transaction;
				try
				{
					runnable.Run();
				}
				finally
				{
					_transaction = old;
				}
			}
		}

		public virtual IThreadPool4 ThreadPool()
		{
			return ((IThreadPool4)Environment().Provide(typeof(IThreadPool4)));
		}

		public virtual object NewWeakReference(ObjectReference referent, object obj)
		{
			return _references.NewWeakReference(referent, obj);
		}

		public sealed override string ToString()
		{
			if (_name != null)
			{
				return _name;
			}
			return DefaultToString();
		}

		protected abstract string DefaultToString();

		public abstract bool IsDeleted(Transaction trans, int id);

		public abstract void BlockSize(int size);

		public virtual IBlockConverter BlockConverter()
		{
			return _blockConverter;
		}

		protected virtual void CreateBlockConverter(int blockSize)
		{
			if (blockSize == 1)
			{
				_blockConverter = new DisabledBlockConverter();
			}
			else
			{
				_blockConverter = new BlockSizeBlockConverter(blockSize);
			}
		}

		public virtual IUpdateDepthProvider UpdateDepthProvider()
		{
			return ConfigImpl.UpdateDepthProvider();
		}

		public virtual void ReplaceClassMetadataRepository(ClassMetadataRepository repository
			)
		{
			_classCollection = repository;
		}

		public long GenerateTransactionTimestamp(long forcedTimestamp)
		{
			lock (Lock())
			{
				return CheckTransaction().GenerateTransactionTimestamp(forcedTimestamp);
			}
		}

		public void UseDefaultTransactionTimestamp()
		{
			lock (Lock())
			{
				CheckTransaction().UseDefaultTransactionTimestamp();
			}
		}

		public abstract void Activate(object arg1, int arg2);

		public abstract void Commit();

		public abstract void Deactivate(object arg1, int arg2);

		public abstract void Delete(object arg1);

		public abstract IExtObjectContainer Ext();

		public abstract IQuery Query();

		public abstract IObjectSet Query(Type arg1);

		public abstract IObjectSet Query(Predicate arg1);

		public abstract IObjectSet Query(Predicate arg1, IQueryComparator arg2);

		public abstract IObjectSet QueryByExample(object arg1);

		public abstract void Rollback();

		public abstract void Store(object arg1);

		public abstract void Activate(object arg1);

		public abstract void Backup(IStorage arg1, string arg2);

		public abstract void Bind(object arg1, long arg2);

		public abstract void Deactivate(object arg1);

		public abstract object Descend(object arg1, string[] arg2);

		public abstract object GetByID(long arg1);

		public abstract object GetByUUID(Db4oUUID arg1);

		public abstract long GetID(object arg1);

		public abstract IObjectInfo GetObjectInfo(object arg1);

		public abstract Db4oDatabase Identity();

		public abstract bool IsActive(object arg1);

		public abstract bool IsCached(long arg1);

		public abstract bool IsStored(object arg1);

		public abstract IObjectContainer OpenSession();

		public abstract object PeekPersisted(object arg1, int arg2, bool arg3);

		public abstract void Purge(object arg1);

		public abstract void Refresh(object arg1, int arg2);

		public abstract void Store(object arg1, int arg2);

		public abstract IStoredClass StoredClass(object arg1);

		public abstract IStoredClass[] StoredClasses();

		public abstract bool InCallback();

		public abstract int InstanceCount(ClassMetadata arg1, Transaction arg2);

		public abstract EventRegistryImpl NewEventRegistry();
	}
}
