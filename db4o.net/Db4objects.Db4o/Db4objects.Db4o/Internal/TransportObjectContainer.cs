/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Activation;
using Db4objects.Db4o.Internal.Convert;
using Db4objects.Db4o.Internal.Ids;
using Db4objects.Db4o.Internal.References;
using Db4objects.Db4o.Internal.Slots;
using Db4objects.Db4o.Internal.Weakref;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Types;
using Sharpen.Lang;

namespace Db4objects.Db4o.Internal
{
	/// <summary>
	/// no reading
	/// no writing
	/// no updates
	/// no weak references
	/// navigation by ID only both sides need synchronised ClassCollections and
	/// MetaInformationCaches
	/// </summary>
	/// <exclude></exclude>
	public class TransportObjectContainer : LocalObjectContainer
	{
		private readonly ObjectContainerBase _parent;

		private readonly MemoryBin _memoryBin;

		public TransportObjectContainer(ObjectContainerBase parent, MemoryBin memoryFile)
			 : base(parent.Config())
		{
			_memoryBin = memoryFile;
			_parent = parent;
			_lock = parent.Lock();
			_showInternalClasses = parent._showInternalClasses;
			Open();
		}

		protected override void Initialize1(IConfiguration config)
		{
			_handlers = _parent._handlers;
			_classCollection = _parent.ClassCollection();
			_config = _parent.ConfigImpl;
			_references = WeakReferenceSupportFactory.DisabledWeakReferenceSupport();
		}

		protected override void InitializeClassMetadataRepository()
		{
		}

		// do nothing, it's passed from the parent ObjectContainer
		protected override void InitalizeWeakReferenceSupport()
		{
		}

		// do nothing, no Weak references
		internal override void InitializeEssentialClasses()
		{
		}

		// do nothing
		protected override void InitializePostOpenExcludingTransportObjectContainer()
		{
		}

		// do nothing
		internal override void InitNewClassCollection()
		{
		}

		// do nothing
		internal override bool CanUpdate()
		{
			return false;
		}

		public override ClassMetadata ClassMetadataForID(int id)
		{
			return _parent.ClassMetadataForID(id);
		}

		internal override void ConfigureNewFile()
		{
		}

		// do nothing
		public override int ConverterVersion()
		{
			return Converter.Version;
		}

		protected virtual void DropReferences()
		{
			_config = null;
		}

		protected override void HandleExceptionOnClose(Exception exc)
		{
		}

		// do nothing here
		public sealed override Transaction NewTransaction(Transaction parentTransaction, 
			IReferenceSystem referenceSystem, bool isSystemTransaction)
		{
			if (null != parentTransaction)
			{
				return parentTransaction;
			}
			return new TransactionObjectCarrier(this, null, new TransportIdSystem(this), referenceSystem
				);
		}

		public override long CurrentVersion()
		{
			return 0;
		}

		public override IDb4oType Db4oTypeStored(Transaction a_trans, object a_object)
		{
			return null;
		}

		public override bool DispatchsEvents()
		{
			return false;
		}

		~TransportObjectContainer()
		{
		}

		// do nothing
		public sealed override void Free(int a_address, int a_length)
		{
		}

		// do nothing
		public sealed override void Free(Slot slot)
		{
		}

		// do nothing
		public override Slot AllocateSlot(int length)
		{
			return AppendBytes(length);
		}

		protected override bool IsValidPointer(int id)
		{
			return id != 0 && base.IsValidPointer(id);
		}

		public override Db4oDatabase Identity()
		{
			return ((ExternalObjectContainer)_parent).Identity();
		}

		public override bool MaintainsIndices()
		{
			return false;
		}

		public override long GenerateTimeStampId()
		{
			return _parent.GenerateTimeStampId();
		}

		internal override void Message(string msg)
		{
		}

		// do nothing
		public override ClassMetadata ProduceClassMetadata(IReflectClass claxx)
		{
			return _parent.ProduceClassMetadata(claxx);
		}

		public override void RaiseCommitTimestamp(long a_minimumVersion)
		{
		}

		// do nothing
		internal override void ReadThis()
		{
		}

		// do nothing
		internal override bool StateMessages()
		{
			return false;
		}

		// overridden to do nothing in YapObjectCarrier
		public override void Shutdown()
		{
			ProcessPendingClassUpdates();
			WriteDirtyClassMetadata();
			Transaction.Commit();
		}

		public sealed override void WriteHeader(bool startFileLockingThread, bool shuttingDown
			)
		{
		}

		public class KnownObjectIdentity
		{
			public int _id;

			public KnownObjectIdentity(int id)
			{
				// do nothing
				_id = id;
			}
		}

		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseReadOnlyException"></exception>
		public override int StoreInternal(Transaction trans, object obj, IUpdateDepth depth
			, bool checkJustSet)
		{
			int id = _parent.GetID(null, obj);
			if (id > 0)
			{
				return base.StoreInternal(trans, new TransportObjectContainer.KnownObjectIdentity
					(id), depth, checkJustSet);
			}
			return base.StoreInternal(trans, obj, depth, checkJustSet);
		}

		/// <exception cref="Db4objects.Db4o.Ext.DatabaseReadOnlyException"></exception>
		protected sealed override void CheckReadOnly()
		{
		}

		public override object GetByID2(Transaction ta, int id)
		{
			object obj = base.GetByID2(ta, id);
			if (obj is TransportObjectContainer.KnownObjectIdentity)
			{
				TransportObjectContainer.KnownObjectIdentity oi = (TransportObjectContainer.KnownObjectIdentity
					)obj;
				Activate(oi);
				obj = _parent.GetByID(null, oi._id);
			}
			return obj;
		}

		public virtual void DeferredOpen()
		{
			Open();
		}

		/// <exception cref="Db4objects.Db4o.Ext.OldFormatException"></exception>
		protected sealed override void OpenImpl()
		{
			CreateIdSystem();
			if (_memoryBin.Length() == 0)
			{
				ConfigureNewFile();
				CommitTransaction();
			}
			else
			{
				ReadThis();
			}
		}

		/// <exception cref="System.NotSupportedException"></exception>
		public override void Backup(IStorage targetStorage, string path)
		{
			throw new NotSupportedException();
		}

		public override void BlockSize(int size)
		{
		}

		// do nothing, blocksize is always 1
		public override void CloseTransaction(Transaction transaction, bool isSystemTransaction
			, bool rollbackOnClose)
		{
		}

		// do nothing	
		protected override void ShutdownDataStorage()
		{
			DropReferences();
		}

		public override long FileLength()
		{
			return _memoryBin.Length();
		}

		public override string FileName()
		{
			return "Memory File";
		}

		protected override bool HasShutDownHook()
		{
			return false;
		}

		public sealed override bool NeedsLockFileThread()
		{
			return false;
		}

		public override void ReadBytes(byte[] bytes, int address, int length)
		{
			try
			{
				_memoryBin.Read(address, bytes, length);
			}
			catch (Exception e)
			{
				Exceptions4.ThrowRuntimeException(13, e);
			}
		}

		public override void ReadBytes(byte[] bytes, int address, int addressOffset, int 
			length)
		{
			ReadBytes(bytes, address + addressOffset, length);
		}

		public override void SyncFiles()
		{
		}

		public override void WriteBytes(ByteArrayBuffer buffer, int address, int addressOffset
			)
		{
			_memoryBin.Write(address + addressOffset, buffer._buffer, buffer.Length());
		}

		public override void OverwriteDeletedBytes(int a_address, int a_length)
		{
		}

		public override void Reserve(int byteCount)
		{
			throw new NotSupportedException();
		}

		public override byte BlockSize()
		{
			return 1;
		}

		protected override void FatalStorageShutdown()
		{
			ShutdownDataStorage();
		}

		public override IReferenceSystem CreateReferenceSystem()
		{
			return new HashcodeReferenceSystem();
		}

		protected override void CreateIdSystem()
		{
		}

		// do nothing
		public override IRunnable CommitHook()
		{
			return Runnable4.DoNothing;
		}

		public override void SyncFiles(IRunnable runnable)
		{
			runnable.Run();
		}

		protected override long MaximumDatabaseFileSize(Config4Impl configImpl)
		{
			return int.MaxValue;
		}
	}
}
