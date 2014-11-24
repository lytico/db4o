/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Metadata;
using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public sealed class ClassMetadataRepository : PersistentBase
	{
		private Collection4 _classes;

		private Hashtable4 _creating;

		private readonly Transaction _systemTransaction;

		private Hashtable4 _classMetadataByBytes;

		private Hashtable4 _classMetadataByClass;

		private Hashtable4 _classMetadataByName;

		private Hashtable4 _classMetadataByID;

		private int _classMetadataCreationDepth;

		private IQueue4 _initClassMetadataOnUp;

		private readonly PendingClassInits _classInits;

		public ClassMetadataRepository(Transaction systemTransaction)
		{
			_systemTransaction = systemTransaction;
			_initClassMetadataOnUp = new NonblockingQueue();
			_classInits = new PendingClassInits(_systemTransaction);
		}

		public void AddClassMetadata(ClassMetadata clazz)
		{
			Container().SetDirtyInSystemTransaction(this);
			_classes.Add(clazz);
			if (clazz.StateUnread())
			{
				_classMetadataByBytes.Put(clazz.i_nameBytes, clazz);
			}
			else
			{
				_classMetadataByClass.Put(clazz.ClassReflector(), clazz);
			}
			RegisterClassMetadataById(clazz);
		}

		private void RegisterClassMetadataById(ClassMetadata clazz)
		{
			if (clazz.GetID() == 0)
			{
				clazz.Write(_systemTransaction);
			}
			_classMetadataByID.Put(clazz.GetID(), clazz);
		}

		private byte[] AsBytes(string str)
		{
			return Container().StringIO().Write(str);
		}

		public void AttachQueryNode(string fieldName, IVisitor4 visitor)
		{
			ClassMetadataIterator i = Iterator();
			while (i.MoveNext())
			{
				ClassMetadata classMetadata = i.CurrentClass();
				if (!classMetadata.IsInternal())
				{
					classMetadata.TraverseAllAspects(new _TraverseFieldCommand_65(fieldName, visitor, 
						classMetadata));
				}
			}
		}

		private sealed class _TraverseFieldCommand_65 : TraverseFieldCommand
		{
			public _TraverseFieldCommand_65(string fieldName, IVisitor4 visitor, ClassMetadata
				 classMetadata)
			{
				this.fieldName = fieldName;
				this.visitor = visitor;
				this.classMetadata = classMetadata;
			}

			protected override void Process(FieldMetadata field)
			{
				if (field.CanAddToQuery(fieldName))
				{
					visitor.Visit(new object[] { classMetadata, field });
				}
			}

			private readonly string fieldName;

			private readonly IVisitor4 visitor;

			private readonly ClassMetadata classMetadata;
		}

		public void IterateTopLevelClasses(IVisitor4 visitor)
		{
			ClassMetadataIterator i = Iterator();
			while (i.MoveNext())
			{
				ClassMetadata classMetadata = i.CurrentClass();
				if (!classMetadata.IsInternal())
				{
					if (classMetadata.GetAncestor() == null)
					{
						visitor.Visit(classMetadata);
					}
				}
			}
		}

		internal void CheckChanges()
		{
			IEnumerator i = _classes.GetEnumerator();
			while (i.MoveNext())
			{
				((ClassMetadata)i.Current).CheckChanges();
			}
		}

		internal bool CreateClassMetadata(ClassMetadata clazz, IReflectClass reflectClazz
			)
		{
			bool result = false;
			_classMetadataCreationDepth++;
			try
			{
				IReflectClass parentReflectClazz = reflectClazz.GetSuperclass();
				ClassMetadata parentClazz = null;
				if (parentReflectClazz != null && !parentReflectClazz.Equals(Container()._handlers
					.IclassObject))
				{
					parentClazz = ProduceClassMetadata(parentReflectClazz);
				}
				result = Container().CreateClassMetadata(clazz, reflectClazz, parentClazz);
			}
			finally
			{
				_classMetadataCreationDepth--;
			}
			InitClassMetadataOnUp();
			return result;
		}

		private void EnsureAllClassesRead()
		{
			bool allClassesRead = false;
			while (!allClassesRead)
			{
				Collection4 unreadClasses = new Collection4();
				int numClasses = _classes.Size();
				IEnumerator classIter = _classes.GetEnumerator();
				while (classIter.MoveNext())
				{
					ClassMetadata clazz = (ClassMetadata)classIter.Current;
					if (clazz.StateUnread())
					{
						unreadClasses.Add(clazz);
					}
				}
				IEnumerator unreadIter = unreadClasses.GetEnumerator();
				while (unreadIter.MoveNext())
				{
					ClassMetadata clazz = (ClassMetadata)unreadIter.Current;
					clazz = ReadClassMetadata(clazz, null);
					if (clazz.ClassReflector() == null)
					{
						clazz.ForceRead();
					}
				}
				allClassesRead = (_classes.Size() == numClasses);
			}
			ApplyReadAs();
		}

		internal bool FieldExists(string field)
		{
			ClassMetadataIterator i = Iterator();
			while (i.MoveNext())
			{
				if (i.CurrentClass().FieldMetadataForName(field) != null)
				{
					return true;
				}
			}
			return false;
		}

		public Collection4 ForInterface(IReflectClass claxx)
		{
			Collection4 col = new Collection4();
			ClassMetadataIterator i = Iterator();
			while (i.MoveNext())
			{
				ClassMetadata clazz = i.CurrentClass();
				IReflectClass candidate = clazz.ClassReflector();
				if (!candidate.IsInterface())
				{
					if (claxx.IsAssignableFrom(candidate))
					{
						col.Add(clazz);
						IEnumerator j = new Collection4(col).GetEnumerator();
						while (j.MoveNext())
						{
							ClassMetadata existing = (ClassMetadata)j.Current;
							if (existing != clazz)
							{
								ClassMetadata higher = clazz.GetHigherHierarchy(existing);
								if (higher != null)
								{
									if (higher == clazz)
									{
										col.Remove(existing);
									}
									else
									{
										col.Remove(clazz);
									}
								}
							}
						}
					}
				}
			}
			return col;
		}

		public override byte GetIdentifier()
		{
			return Const4.Yapclasscollection;
		}

		internal ClassMetadata GetActiveClassMetadata(IReflectClass reflectClazz)
		{
			return (ClassMetadata)_classMetadataByClass.Get(reflectClazz);
		}

		internal ClassMetadata ClassMetadataForReflectClass(IReflectClass reflectClazz)
		{
			ClassMetadata cached = (ClassMetadata)_classMetadataByClass.Get(reflectClazz);
			if (cached != null)
			{
				return cached;
			}
			ClassMetadata byName = (ClassMetadata)_classMetadataByName.Get(reflectClazz.GetName
				());
			if (byName != null)
			{
				return byName;
			}
			return ReadClassMetadata(reflectClazz);
		}

		private ClassMetadata ReadClassMetadata(IReflectClass reflectClazz)
		{
			ClassMetadata clazz = (ClassMetadata)_classMetadataByBytes.Remove(GetNameBytes(reflectClazz
				.GetName()));
			if (clazz == null)
			{
				return null;
			}
			return ReadClassMetadata(clazz, reflectClazz);
		}

		internal ClassMetadata ProduceClassMetadata(IReflectClass reflectClazz)
		{
			ClassMetadata classMetadata = ClassMetadataForReflectClass(reflectClazz);
			if (classMetadata != null)
			{
				return classMetadata;
			}
			ClassMetadata classBeingCreated = (ClassMetadata)_creating.Get(reflectClazz);
			if (classBeingCreated != null)
			{
				return classBeingCreated;
			}
			ClassMetadata newClassMetadata = new ClassMetadata(Container(), reflectClazz);
			_creating.Put(reflectClazz, newClassMetadata);
			try
			{
				if (!CreateClassMetadata(newClassMetadata, reflectClazz))
				{
					return null;
				}
				// ObjectContainerBase#createClassMetadata may add the ClassMetadata already,
				// so we have to check again
				if (!IsRegistered(reflectClazz))
				{
					AddClassMetadata(newClassMetadata);
					_classInits.Process(newClassMetadata);
				}
				else
				{
					RegisterClassMetadataById(newClassMetadata);
					if (newClassMetadata.AspectsAreNull())
					{
						_classInits.Process(newClassMetadata);
					}
				}
				Container().SetDirtyInSystemTransaction(this);
			}
			finally
			{
				_creating.Remove(reflectClazz);
			}
			return newClassMetadata;
		}

		private bool IsRegistered(IReflectClass reflectClazz)
		{
			return _classMetadataByClass.Get(reflectClazz) != null;
		}

		internal ClassMetadata ClassMetadataForId(int id)
		{
			ClassMetadata classMetadata = (ClassMetadata)_classMetadataByID.Get(id);
			if (null == classMetadata)
			{
				return null;
			}
			return ReadClassMetadata(classMetadata, null);
		}

		public int ClassMetadataIdForName(string name)
		{
			ClassMetadata classMetadata = (ClassMetadata)_classMetadataByBytes.Get(GetNameBytes
				(name));
			if (classMetadata == null)
			{
				classMetadata = FindInitializedClassByName(name);
			}
			if (classMetadata != null)
			{
				return classMetadata.GetID();
			}
			return 0;
		}

		public ClassMetadata GetClassMetadata(string name)
		{
			byte[] nameBytes = GetNameBytes(name);
			ClassMetadata classMetadata = (ClassMetadata)_classMetadataByBytes.Get(nameBytes);
			if (classMetadata == null)
			{
				classMetadata = FindInitializedClassByName(name);
			}
			if (classMetadata != null)
			{
				classMetadata = ReadClassMetadata(classMetadata, null);
				_classMetadataByBytes.Remove(nameBytes);
			}
			return classMetadata;
		}

		private ClassMetadata FindInitializedClassByName(string name)
		{
			ClassMetadata classMetadata = (ClassMetadata)_classMetadataByName.Get(name);
			if (classMetadata != null)
			{
				return classMetadata;
			}
			ClassMetadataIterator i = Iterator();
			while (i.MoveNext())
			{
				classMetadata = (ClassMetadata)i.Current;
				if (name.Equals(classMetadata.GetName()))
				{
					_classMetadataByName.Put(name, classMetadata);
					return classMetadata;
				}
			}
			return null;
		}

		public int GetClassMetadataID(string name)
		{
			ClassMetadata clazz = (ClassMetadata)_classMetadataByBytes.Get(GetNameBytes(name)
				);
			if (clazz != null)
			{
				return clazz.GetID();
			}
			return 0;
		}

		internal byte[] GetNameBytes(string name)
		{
			return AsBytes(ResolveAliasRuntimeName(name));
		}

		private string ResolveAliasRuntimeName(string name)
		{
			return Container().ConfigImpl.ResolveAliasRuntimeName(name);
		}

		public void InitOnUp(Transaction systemTrans)
		{
			_classMetadataCreationDepth++;
			systemTrans.Container().ShowInternalClasses(true);
			try
			{
				IEnumerator i = _classes.GetEnumerator();
				while (i.MoveNext())
				{
					((ClassMetadata)i.Current).InitOnUp(systemTrans);
				}
			}
			finally
			{
				systemTrans.Container().ShowInternalClasses(false);
				_classMetadataCreationDepth--;
			}
			InitClassMetadataOnUp();
		}

		internal void InitTables(int size)
		{
			_classes = new Collection4();
			_classMetadataByBytes = new Hashtable4(size);
			if (size < 16)
			{
				size = 16;
			}
			_classMetadataByClass = new Hashtable4(size);
			_classMetadataByName = new Hashtable4(size);
			_classMetadataByID = new Hashtable4(size);
			_creating = new Hashtable4(1);
		}

		private void InitClassMetadataOnUp()
		{
			if (_classMetadataCreationDepth != 0)
			{
				return;
			}
			ClassMetadata clazz = (ClassMetadata)_initClassMetadataOnUp.Next();
			while (clazz != null)
			{
				clazz.InitOnUp(_systemTransaction);
				clazz = (ClassMetadata)_initClassMetadataOnUp.Next();
			}
		}

		public ClassMetadataIterator Iterator()
		{
			return new ClassMetadataIterator(this, new ArrayIterator4(_classes.ToArray()));
		}

		private class ClassIDIterator : MappingIterator
		{
			public ClassIDIterator(Collection4 classes) : base(classes.GetEnumerator())
			{
			}

			protected override object Map(object current)
			{
				return ((ClassMetadata)current).GetID();
			}
		}

		public IEnumerator Ids()
		{
			return new ClassMetadataRepository.ClassIDIterator(_classes);
		}

		public override int OwnLength()
		{
			return Const4.ObjectLength + Const4.IntLength + (_classes.Size() * Const4.IdLength
				);
		}

		internal void Purge()
		{
			IEnumerator i = _classes.GetEnumerator();
			while (i.MoveNext())
			{
				((ClassMetadata)i.Current).Purge();
			}
		}

		public sealed override void ReadThis(Transaction trans, ByteArrayBuffer buffer)
		{
			int classCount = buffer.ReadInt();
			InitTables(classCount);
			ObjectContainerBase container = Container();
			int[] ids = ReadMetadataIds(buffer, classCount);
			ByteArrayBuffer[] metadataSlots = container.ReadSlotBuffers(trans, ids);
			for (int i = 0; i < classCount; ++i)
			{
				ClassMetadata classMetadata = new ClassMetadata(container, null);
				classMetadata.SetID(ids[i]);
				_classes.Add(classMetadata);
				_classMetadataByID.Put(ids[i], classMetadata);
				byte[] name = classMetadata.ReadName1(trans, metadataSlots[i]);
				if (name != null)
				{
					_classMetadataByBytes.Put(name, classMetadata);
				}
			}
			ApplyReadAs();
		}

		private int[] ReadMetadataIds(ByteArrayBuffer buffer, int classCount)
		{
			int[] ids = new int[classCount];
			for (int i = 0; i < classCount; ++i)
			{
				ids[i] = buffer.ReadInt();
			}
			return ids;
		}

		internal Hashtable4 ClassByBytes()
		{
			return _classMetadataByBytes;
		}

		private void ApplyReadAs()
		{
			Hashtable4 readAs = Container().ConfigImpl.ReadAs();
			IEnumerator i = readAs.Iterator();
			while (i.MoveNext())
			{
				IEntry4 entry = (IEntry4)i.Current;
				string dbName = (string)entry.Key();
				string useName = (string)entry.Value();
				byte[] dbbytes = GetNameBytes(dbName);
				byte[] useBytes = GetNameBytes(useName);
				if (ClassByBytes().Get(useBytes) == null)
				{
					ClassMetadata clazz = (ClassMetadata)ClassByBytes().Get(dbbytes);
					if (clazz != null)
					{
						clazz.i_nameBytes = useBytes;
						clazz.SetConfig(ConfigClass(dbName));
						ClassByBytes().Remove(dbbytes);
						ClassByBytes().Put(useBytes, clazz);
					}
				}
			}
		}

		private Config4Class ConfigClass(string name)
		{
			return Container().ConfigImpl.ConfigClass(name);
		}

		public ClassMetadata ReadClassMetadata(ClassMetadata classMetadata, IReflectClass
			 clazz)
		{
			if (classMetadata == null)
			{
				throw new ArgumentNullException();
			}
			if (!classMetadata.StateUnread())
			{
				return classMetadata;
			}
			_classMetadataCreationDepth++;
			try
			{
				classMetadata.ResolveNameConfigAndReflector(this, clazz);
				IReflectClass claxx = classMetadata.ClassReflector();
				if (claxx != null)
				{
					_classMetadataByClass.Put(claxx, classMetadata);
					classMetadata.ReadThis();
					classMetadata.CheckChanges();
					_initClassMetadataOnUp.Add(classMetadata);
				}
			}
			finally
			{
				_classMetadataCreationDepth--;
			}
			InitClassMetadataOnUp();
			return classMetadata;
		}

		public void CheckAllClassChanges()
		{
			IEnumerator i = _classMetadataByID.Keys();
			while (i.MoveNext())
			{
				int classMetadataID = ((int)i.Current);
				ClassMetadataForId(classMetadataID);
			}
		}

		public void RefreshClasses()
		{
			ClassMetadataRepository rereader = new ClassMetadataRepository(_systemTransaction
				);
			rereader._id = _id;
			rereader.Read(Container().SystemTransaction());
			IEnumerator i = rereader._classes.GetEnumerator();
			while (i.MoveNext())
			{
				ClassMetadata clazz = (ClassMetadata)i.Current;
				RefreshClass(clazz);
			}
			i = _classes.GetEnumerator();
			while (i.MoveNext())
			{
				ClassMetadata clazz = (ClassMetadata)i.Current;
				clazz.Refresh();
			}
		}

		private void RefreshClass(ClassMetadata clazz)
		{
			if (_classMetadataByID.Get(clazz.GetID()) == null)
			{
				_classes.Add(clazz);
				_classMetadataByID.Put(clazz.GetID(), clazz);
				RefreshClassCache(clazz, null);
			}
		}

		public void RefreshClassCache(ClassMetadata clazz, IReflectClass oldReflector)
		{
			if (clazz.StateUnread())
			{
				_classMetadataByBytes.Put(clazz.ReadName(_systemTransaction), clazz);
			}
			else
			{
				if (oldReflector != null)
				{
					_classMetadataByClass.Remove(oldReflector);
				}
				_classMetadataByClass.Put(clazz.ClassReflector(), clazz);
			}
		}

		internal void ReReadClassMetadata(ClassMetadata clazz)
		{
			if (clazz != null)
			{
				ReReadClassMetadata(clazz._ancestor);
				clazz.ReadName(_systemTransaction);
				clazz.ForceRead();
				clazz.SetStateClean();
				clazz.BitFalse(Const4.CheckedChanges);
				clazz.BitFalse(Const4.Reading);
				clazz.BitFalse(Const4.Continue);
				clazz.BitFalse(Const4.Dead);
				clazz.CheckChanges();
			}
		}

		public IStoredClass[] StoredClasses()
		{
			EnsureAllClassesRead();
			IStoredClass[] sclasses = new IStoredClass[_classes.Size()];
			_classes.ToArray(sclasses);
			return sclasses;
		}

		public void WriteAllClasses()
		{
			Collection4 deadClasses = new Collection4();
			IStoredClass[] storedClasses = StoredClasses();
			for (int i = 0; i < storedClasses.Length; i++)
			{
				ClassMetadata clazz = (ClassMetadata)storedClasses[i];
				clazz.SetStateDirty();
				if (clazz.StateDead())
				{
					deadClasses.Add(clazz);
					clazz.SetStateOK();
				}
			}
			for (int i = 0; i < storedClasses.Length; i++)
			{
				ClassMetadata clazz = (ClassMetadata)storedClasses[i];
				clazz.Write(_systemTransaction);
			}
			IEnumerator it = deadClasses.GetEnumerator();
			while (it.MoveNext())
			{
				((ClassMetadata)it.Current).SetStateDead();
			}
		}

		public override void WriteThis(Transaction trans, ByteArrayBuffer buffer)
		{
			buffer.WriteInt(_classes.Size());
			IEnumerator i = _classes.GetEnumerator();
			while (i.MoveNext())
			{
				buffer.WriteIDOf(trans, i.Current);
			}
		}

		public override string ToString()
		{
			string str = "Active:\n";
			IEnumerator i = _classes.GetEnumerator();
			while (i.MoveNext())
			{
				ClassMetadata clazz = (ClassMetadata)i.Current;
				str += clazz.GetID() + " " + clazz + "\n";
			}
			return str;
		}

		internal ObjectContainerBase Container()
		{
			return _systemTransaction.Container();
		}

		public override void SetID(int id)
		{
			if (Container().IsClient)
			{
				base.SetID(id);
				return;
			}
			if (_id == 0)
			{
				SystemData().ClassCollectionID(id);
			}
			base.SetID(id);
		}

		private SystemData SystemData()
		{
			return LocalSystemTransaction().LocalContainer().SystemData();
		}

		private LocalTransaction LocalSystemTransaction()
		{
			return ((LocalTransaction)_systemTransaction);
		}

		public void ClassMetadataNameResolved(ClassMetadata classMetadata, byte[] nameBytes
			)
		{
			_classMetadataByBytes.Remove(nameBytes);
			_classMetadataByName.Put(classMetadata.GetName(), classMetadata);
		}
	}
}
