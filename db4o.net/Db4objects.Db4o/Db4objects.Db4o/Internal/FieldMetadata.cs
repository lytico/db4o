/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using System.Text;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Activation;
using Db4objects.Db4o.Internal.Btree;
using Db4objects.Db4o.Internal.Delete;
using Db4objects.Db4o.Internal.Handlers;
using Db4objects.Db4o.Internal.Handlers.Array;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Internal.Query.Processor;
using Db4objects.Db4o.Internal.Reflect;
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Reflect.Generic;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public class FieldMetadata : ClassAspect, IStoredField
	{
		private ClassMetadata _containingClass;

		private string _name;

		protected bool _isArray;

		private bool _isNArray;

		private bool _isPrimitive;

		private IReflectField _reflectField;

		private FieldMetadataState _state = FieldMetadataState.NotLoaded;

		private Config4Field _config;

		private IDb4oTypeImpl _db4oType;

		private BTree _index;

		protected ClassMetadata _fieldType;

		protected int _fieldTypeID;

		internal static readonly Db4objects.Db4o.Internal.FieldMetadata[] EmptyArray = new 
			Db4objects.Db4o.Internal.FieldMetadata[0];

		public FieldMetadata(ClassMetadata classMetadata)
		{
			_containingClass = classMetadata;
		}

		protected Type TranslatorStoredClass(IObjectTranslator translator)
		{
			try
			{
				return translator.StoredClass();
			}
			catch (Exception e)
			{
				throw new ReflectException(e);
			}
		}

		internal FieldMetadata(ClassMetadata containingClass, IReflectField field, ClassMetadata
			 fieldType) : this(containingClass)
		{
			Init(field.GetName());
			_reflectField = field;
			_fieldType = fieldType;
			_fieldTypeID = fieldType.GetID();
			// TODO: beautify !!!  possibly pull up isPrimitive to ReflectField
			bool isPrimitive = field is GenericField ? ((GenericField)field).IsPrimitive() : 
				false;
			Configure(field.GetFieldType(), isPrimitive);
			CheckDb4oType();
			SetAvailable();
		}

		protected virtual void SetAvailable()
		{
			_state = FieldMetadataState.Available;
		}

		protected FieldMetadata(int fieldTypeID)
		{
			_fieldTypeID = fieldTypeID;
		}

		public FieldMetadata(ClassMetadata containingClass, string name, int fieldTypeID, 
			bool primitive, bool isArray, bool isNArray) : this(containingClass)
		{
			Init(name, fieldTypeID, primitive, isArray, isNArray);
		}

		protected FieldMetadata(ClassMetadata containingClass, string name) : this(containingClass
			)
		{
			Init(name);
		}

		/// <exception cref="Db4objects.Db4o.Internal.FieldIndexException"></exception>
		public virtual void AddFieldIndex(ObjectIdContextImpl context)
		{
			if (!HasIndex())
			{
				IncrementOffset(context, context);
				return;
			}
			try
			{
				AddIndexEntry(context.Transaction(), context.ObjectId(), ReadIndexEntry(context));
			}
			catch (CorruptionException exc)
			{
				throw new FieldIndexException(exc, this);
			}
		}

		protected void AddIndexEntry(StatefulBuffer a_bytes, object indexEntry)
		{
			AddIndexEntry(a_bytes.Transaction(), a_bytes.GetID(), indexEntry);
		}

		public virtual void AddIndexEntry(Transaction trans, int parentID, object indexEntry
			)
		{
			if (!HasIndex())
			{
				return;
			}
			BTree index = GetIndex(trans);
			index.Add(trans, CreateFieldIndexKey(parentID, indexEntry));
		}

		protected virtual IFieldIndexKey CreateFieldIndexKey(int parentID, object indexEntry
			)
		{
			object convertedIndexEntry = IndexEntryFor(indexEntry);
			return new FieldIndexKeyImpl(parentID, convertedIndexEntry);
		}

		protected virtual object IndexEntryFor(object indexEntry)
		{
			return _reflectField.IndexEntry(indexEntry);
		}

		public virtual bool CanUseNullBitmap()
		{
			return true;
		}

		/// <exception cref="Db4objects.Db4o.CorruptionException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public object ReadIndexEntry(IObjectIdContext context)
		{
			IIndexableTypeHandler indexableTypeHandler = (IIndexableTypeHandler)HandlerRegistry
				.CorrectHandlerVersion(context, GetHandler());
			return indexableTypeHandler.ReadIndexEntry(context);
		}

		public virtual void RemoveIndexEntry(Transaction trans, int parentID, object indexEntry
			)
		{
			if (!HasIndex())
			{
				return;
			}
			BTree index = GetIndex(trans);
			if (index == null)
			{
				return;
			}
			index.Remove(trans, CreateFieldIndexKey(parentID, indexEntry));
		}

		//TODO: Split into command query separation.
		public virtual bool Alive()
		{
			if (_state == FieldMetadataState.Available)
			{
				return true;
			}
			if (_state == FieldMetadataState.NotLoaded)
			{
				return Load();
			}
			return _state == FieldMetadataState.Available;
		}

		private bool Load()
		{
			if (_fieldType == null)
			{
				// this may happen if the local ClassMetadataRepository
				// has not been updated from the server and presumably 
				// in some refactoring cases. 
				// We try to heal the problem by re-reading the class.
				// This could be dangerous, if the class type of a field
				// has been modified.
				// TODO: add class refactoring features
				_fieldType = DetectFieldType();
				CheckFieldTypeID();
			}
			CheckCorrectTypeForField();
			if (_fieldType == null || _reflectField == null)
			{
				_state = FieldMetadataState.Unavailable;
				_reflectField = null;
				return false;
			}
			if (Updating())
			{
				return false;
			}
			SetAvailable();
			CheckDb4oType();
			return true;
		}

		private bool ShouldStoreField()
		{
			return !_reflectField.IsTransient() || (_containingClass != null && _containingClass
				.ShouldStoreTransientFields());
		}

		public virtual bool Updating()
		{
			return _state == FieldMetadataState.Updating;
		}

		private void CheckFieldTypeID()
		{
			int id = _fieldType != null ? _fieldType.GetID() : 0;
			if (_fieldTypeID == 0)
			{
				_fieldTypeID = id;
				return;
			}
			if (id > 0 && id != _fieldTypeID)
			{
				// wrong type, refactoring, field should be turned off
				// TODO: it would be cool to log something here
				_fieldType = null;
			}
		}

		internal virtual bool CanAddToQuery(string fieldName)
		{
			if (!Alive())
			{
				return false;
			}
			return fieldName.Equals(GetName()) && ContainingClass() != null && !ContainingClass
				().IsInternal();
		}

		private bool CanHold(IReflectClass type)
		{
			if (type == null)
			{
				throw new ArgumentNullException();
			}
			ITypeHandler4 typeHandler = GetHandler();
			if (typeHandler is IQueryableTypeHandler)
			{
				if (((IQueryableTypeHandler)typeHandler).DescendsIntoMembers())
				{
					return true;
				}
			}
			IReflectClass classReflector = FieldType().ClassReflector();
			if (classReflector.IsCollection())
			{
				return true;
			}
			return classReflector.IsAssignableFrom(type);
		}

		public virtual GenericReflector Reflector()
		{
			ObjectContainerBase container = Container();
			if (container == null)
			{
				return null;
			}
			return container.Reflector();
		}

		public virtual object Coerce(IReflectClass valueClass, object value)
		{
			if (value == null)
			{
				return _isPrimitive ? No4.Instance : value;
			}
			if (valueClass == null)
			{
				throw new ArgumentNullException();
			}
			if (GetHandler() is PrimitiveHandler)
			{
				return ((PrimitiveHandler)GetHandler()).Coerce(valueClass, value);
			}
			if (!CanHold(valueClass))
			{
				return No4.Instance;
			}
			return value;
		}

		public bool CanLoadByIndex()
		{
			return Handlers4.CanLoadFieldByIndex(GetHandler());
		}

		public sealed override void CascadeActivation(IActivationContext context)
		{
			if (!Alive())
			{
				return;
			}
			object cascadeTo = CascadingTarget(context);
			if (cascadeTo == null)
			{
				return;
			}
			IActivationContext cascadeContext = context.ForObject(cascadeTo);
			ClassMetadata classMetadata = cascadeContext.ClassMetadata();
			if (classMetadata == null)
			{
				return;
			}
			EnsureObjectIsActive(cascadeContext);
			Handlers4.CascadeActivation(cascadeContext, classMetadata.TypeHandler());
		}

		private void EnsureObjectIsActive(IActivationContext context)
		{
			if (!context.Depth().Mode().IsActivate())
			{
				return;
			}
			if (Handlers4.IsValueType(GetHandler()))
			{
				return;
			}
			ObjectContainerBase container = context.Container();
			ClassMetadata classMetadata = container.ClassMetadataForObject(context.TargetObject
				());
			if (classMetadata == null || !classMetadata.HasIdentity())
			{
				return;
			}
			if (container.IsActive(context.TargetObject()))
			{
				return;
			}
			container.StillToActivate(context.Descend());
		}

		protected object CascadingTarget(IActivationContext context)
		{
			if (context.Depth().Mode().IsDeactivate())
			{
				if (null == _reflectField)
				{
					return null;
				}
				return FieldAccessor().Get(_reflectField, context.TargetObject());
			}
			return GetOrCreate(context.Transaction(), context.TargetObject());
		}

		private void CheckDb4oType()
		{
			if (_reflectField != null)
			{
				if (Container()._handlers.IclassDb4otype.IsAssignableFrom(_reflectField.GetFieldType
					()))
				{
					_db4oType = HandlerRegistry.GetDb4oType(_reflectField.GetFieldType());
				}
			}
		}

		internal virtual void CollectConstraints(Transaction trans, QConObject a_parent, 
			object a_template, IVisitor4 a_visitor)
		{
			object obj = GetOn(trans, a_template);
			if (obj != null)
			{
				Collection4 objs = Platform4.FlattenCollection(trans.Container(), obj);
				IEnumerator j = objs.GetEnumerator();
				while (j.MoveNext())
				{
					obj = j.Current;
					if (obj != null)
					{
						if (_isPrimitive && !_isArray)
						{
							object nullValue = _reflectField.GetFieldType().NullValue();
							if (obj.Equals(nullValue))
							{
								return;
							}
						}
						if (Platform4.IgnoreAsConstraint(obj))
						{
							return;
						}
						if (!a_parent.HasObjectInParentPath(obj))
						{
							QConObject constraint = new QConObject(trans, a_parent, QField(trans), obj);
							constraint.ByExample();
							a_visitor.Visit(constraint);
						}
					}
				}
			}
		}

		/// <exception cref="Db4objects.Db4o.Internal.FieldIndexException"></exception>
		public sealed override void CollectIDs(CollectIdContext context)
		{
			if (!Alive())
			{
				IncrementOffset(context.Buffer(), context);
				return;
			}
			ITypeHandler4 handler = HandlerRegistry.CorrectHandlerVersion(context, GetHandler
				());
			Handlers4.CollectIdsInternal(context, handler, LinkLength(context), true);
		}

		internal virtual void Configure(IReflectClass clazz, bool isPrimitive)
		{
			_isArray = clazz.IsArray();
			if (_isArray)
			{
				IReflectArray reflectArray = Reflector().Array();
				_isNArray = reflectArray.IsNDimensional(clazz);
				_isPrimitive = reflectArray.GetComponentType(clazz).IsPrimitive();
			}
			else
			{
				_isPrimitive = isPrimitive | clazz.IsPrimitive();
			}
		}

		protected ITypeHandler4 WrapHandlerToArrays(ITypeHandler4 handler)
		{
			if (handler == null)
			{
				return null;
			}
			if (_isNArray)
			{
				return new MultidimensionalArrayHandler(handler, ArraysUsePrimitiveClassReflector
					());
			}
			if (_isArray)
			{
				return new ArrayHandler(handler, ArraysUsePrimitiveClassReflector());
			}
			return handler;
		}

		private bool ArraysUsePrimitiveClassReflector()
		{
			return _isPrimitive;
		}

		public override void Deactivate(IActivationContext context)
		{
			if (!Alive() || !ShouldStoreField())
			{
				return;
			}
			bool isEnumClass = _containingClass.IsEnum();
			if (_isPrimitive && !_isArray)
			{
				if (!isEnumClass)
				{
					object nullValue = _reflectField.GetFieldType().NullValue();
					FieldAccessor().Set(_reflectField, context.TargetObject(), nullValue);
				}
				return;
			}
			if (context.Depth().RequiresActivation())
			{
				CascadeActivation(context);
			}
			if (!isEnumClass)
			{
				FieldAccessor().Set(_reflectField, context.TargetObject(), null);
			}
		}

		private IFieldAccessor FieldAccessor()
		{
			return _containingClass.FieldAccessor();
		}

		/// <exception cref="Db4objects.Db4o.Internal.FieldIndexException"></exception>
		public override void Delete(DeleteContextImpl context, bool isUpdate)
		{
			if (!CheckAlive(context, context))
			{
				return;
			}
			try
			{
				RemoveIndexEntry(context);
				if (isUpdate && !IsStruct())
				{
					IncrementOffset(context, context);
					return;
				}
				StatefulBuffer buffer = (StatefulBuffer)context.Buffer();
				DeleteContextImpl childContext = new DeleteContextImpl(context, GetStoredType(), 
					_config);
				context.SlotFormat().DoWithSlotIndirection(buffer, GetHandler(), new _IClosure4_443
					(this, childContext));
			}
			catch (CorruptionException exc)
			{
				throw new FieldIndexException(exc, this);
			}
		}

		private sealed class _IClosure4_443 : IClosure4
		{
			public _IClosure4_443(FieldMetadata _enclosing, DeleteContextImpl childContext)
			{
				this._enclosing = _enclosing;
				this.childContext = childContext;
			}

			public object Run()
			{
				childContext.Delete(this._enclosing.GetHandler());
				return null;
			}

			private readonly FieldMetadata _enclosing;

			private readonly DeleteContextImpl childContext;
		}

		/// <exception cref="Db4objects.Db4o.CorruptionException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		private void RemoveIndexEntry(DeleteContextImpl context)
		{
			if (!HasIndex())
			{
				return;
			}
			int offset = context.Offset();
			object obj = ReadIndexEntry(context);
			RemoveIndexEntry(context.Transaction(), context.ObjectId(), obj);
			context.Seek(offset);
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Db4objects.Db4o.Internal.FieldMetadata))
			{
				return false;
			}
			Db4objects.Db4o.Internal.FieldMetadata other = (Db4objects.Db4o.Internal.FieldMetadata
				)obj;
			other.Alive();
			Alive();
			return other._isPrimitive == _isPrimitive && other._fieldType == _fieldType && other
				._name.Equals(_name);
		}

		public override int GetHashCode()
		{
			return _name.GetHashCode();
		}

		public object Get(object onObject)
		{
			return Get(null, onObject);
		}

		public object Get(Transaction trans, object onObject)
		{
			if (_containingClass == null)
			{
				return null;
			}
			ObjectContainerBase container = Container();
			if (container == null)
			{
				return null;
			}
			lock (container.Lock())
			{
				// FIXME: The following is not really transactional.
				//        This will work OK for normal C/S and for
				//        single local mode but the transaction will
				//        be wrong for MTOC.
				if (trans == null)
				{
					trans = container.Transaction;
				}
				container.CheckClosed();
				ObjectReference @ref = trans.ReferenceForObject(onObject);
				if (@ref == null)
				{
					return null;
				}
				int id = @ref.GetID();
				if (id <= 0)
				{
					return null;
				}
				UnmarshallingContext context = new UnmarshallingContext(trans, @ref, Const4.AddToIdTree
					, false);
				context.ActivationDepth(new LegacyActivationDepth(1));
				return context.ReadFieldValue(this);
			}
		}

		public override string GetName()
		{
			return _name;
		}

		public ClassMetadata FieldType()
		{
			// alive needs to be checked by all callers: Done
			return _fieldType;
		}

		public virtual ITypeHandler4 GetHandler()
		{
			if (_fieldType == null)
			{
				return null;
			}
			// alive needs to be checked by all callers: Done
			return WrapHandlerToArrays(_fieldType.TypeHandler());
		}

		public virtual int FieldTypeID()
		{
			// alive needs to be checked by all callers: Done
			return _fieldTypeID;
		}

		public virtual object GetOn(Transaction trans, object onObject)
		{
			if (Alive())
			{
				return FieldAccessor().Get(_reflectField, onObject);
			}
			return null;
		}

		/// <summary>
		/// dirty hack for com.db4o.types some of them (BlobImpl) need to be set automatically
		/// TODO: Derive from FieldMetadata for Db4oTypes
		/// </summary>
		public virtual object GetOrCreate(Transaction trans, object onObject)
		{
			if (!Alive())
			{
				return null;
			}
			object obj = FieldAccessor().Get(_reflectField, onObject);
			if (_db4oType != null && obj == null)
			{
				obj = _db4oType.CreateDefault(trans);
				FieldAccessor().Set(_reflectField, onObject, obj);
			}
			return obj;
		}

		public ClassMetadata ContainingClass()
		{
			// alive needs to be checked by all callers: Done
			return _containingClass;
		}

		public virtual IReflectClass GetStoredType()
		{
			if (_reflectField == null)
			{
				return null;
			}
			return Handlers4.BaseType(_reflectField.GetFieldType());
		}

		public virtual ObjectContainerBase Container()
		{
			if (_containingClass == null)
			{
				return null;
			}
			return _containingClass.Container();
		}

		public virtual bool HasConfig()
		{
			return _config != null;
		}

		public virtual bool HasIndex()
		{
			return _index != null;
		}

		public void Init(string name)
		{
			_name = name;
			InitConfiguration(name);
		}

		internal void InitConfiguration(string name)
		{
			Config4Class containingClassConfig = _containingClass.Config();
			if (containingClassConfig == null)
			{
				return;
			}
			_config = containingClassConfig.ConfigField(name);
		}

		public virtual void Init(string name, int fieldTypeID, bool isPrimitive, bool isArray
			, bool isNArray)
		{
			_fieldTypeID = fieldTypeID;
			_isPrimitive = isPrimitive;
			_isArray = isArray;
			_isNArray = isNArray;
			Init(name);
			LoadFieldTypeById();
			Alive();
		}

		private bool _initialized = false;

		internal void InitConfigOnUp(Transaction trans)
		{
			if (_initialized)
			{
				return;
			}
			_initialized = true;
			if (_config != null)
			{
				_config.InitOnUp(trans, this);
			}
		}

		public override void Activate(UnmarshallingContext context)
		{
			if (!CheckAlive(context, context))
			{
				return;
			}
			if (!ShouldStoreField())
			{
				IncrementOffset(context, context);
				return;
			}
			object toSet = Read(context);
			InformAboutTransaction(toSet, context.Transaction());
			Set(context.PersistentObject(), toSet);
		}

		public virtual void AttemptUpdate(UnmarshallingContext context)
		{
			if (!Updating())
			{
				IncrementOffset(context, context);
				return;
			}
			int savedOffset = context.Offset();
			try
			{
				object toSet = context.Read(GetHandler());
				if (toSet != null)
				{
					Set(context.PersistentObject(), toSet);
				}
			}
			catch (Exception)
			{
				// FIXME: COR-547 Diagnostics here please.
				context.Buffer().Seek(savedOffset);
				IncrementOffset(context, context);
			}
		}

		private bool CheckAlive(IAspectVersionContext context, IHandlerVersionContext versionContext
			)
		{
			if (!CheckEnabled(context, versionContext))
			{
				return false;
			}
			bool alive = Alive();
			if (!alive)
			{
				IncrementOffset((IReadBuffer)context, versionContext);
			}
			return alive;
		}

		private void InformAboutTransaction(object obj, Transaction trans)
		{
			if (_db4oType != null && obj != null)
			{
				((IDb4oTypeImpl)obj).SetTrans(trans);
			}
		}

		public virtual bool IsArray()
		{
			return _isArray;
		}

		public override int LinkLength(IHandlerVersionContext context)
		{
			Alive();
			return CalculateLinkLength(context);
		}

		private int CalculateLinkLength(IHandlerVersionContext context)
		{
			return Handlers4.CalculateLinkLength(HandlerRegistry.CorrectHandlerVersion(context
				, GetHandler()));
		}

		public virtual void LoadFieldTypeById()
		{
			_fieldType = Container().ClassMetadataForID(_fieldTypeID);
		}

		private ClassMetadata DetectFieldType()
		{
			IReflectClass claxx = _containingClass.ClassReflector();
			if (claxx == null)
			{
				return null;
			}
			_reflectField = claxx.GetDeclaredField(_name);
			if (_reflectField == null)
			{
				return null;
			}
			IReflectClass fieldType = _reflectField.GetFieldType();
			if (fieldType == null)
			{
				return null;
			}
			return Handlers4.ErasedFieldType(Container(), fieldType);
		}

		protected virtual ITypeHandler4 TypeHandlerForClass(ObjectContainerBase container
			, IReflectClass fieldType)
		{
			container.ShowInternalClasses(true);
			try
			{
				return container.TypeHandlerForClass(Handlers4.BaseType(fieldType));
			}
			finally
			{
				container.ShowInternalClasses(false);
			}
		}

		private void CheckCorrectTypeForField()
		{
			ClassMetadata currentFieldType = DetectFieldType();
			if (currentFieldType == null)
			{
				_reflectField = null;
				_state = FieldMetadataState.Unavailable;
				return;
			}
			if (currentFieldType == _fieldType && Handlers4.BaseType(_reflectField.GetFieldType
				()).IsPrimitive() == _isPrimitive)
			{
				return;
			}
			// special case when migrating from type handler ids
			// to class metadata ids which caused
			// any interface metadata id to be mapped to UNTYPED_ID
			if (Handlers4.IsUntyped(currentFieldType.TypeHandler()) && Handlers4.IsUntyped(_fieldType
				.TypeHandler()))
			{
				return;
			}
			// FIXME: COR-547 Diagnostics here please.
			_state = FieldMetadataState.Updating;
		}

		private IUpdateDepth AdjustUpdateDepthForCascade(object obj, IUpdateDepth updateDepth
			)
		{
			return updateDepth.AdjustUpdateDepthForCascade(_containingClass.IsCollection(obj)
				);
		}

		private bool CascadeOnUpdate(Config4Class parentClassConfiguration)
		{
			return ((parentClassConfiguration != null && (parentClassConfiguration.CascadeOnUpdate
				().DefiniteYes())) || (_config != null && (_config.CascadeOnUpdate().DefiniteYes
				())));
		}

		public override void Marshall(MarshallingContext context, object obj)
		{
			// alive needs to be checked by all callers: Done
			IUpdateDepth updateDepth = context.UpdateDepth();
			if (obj != null && CascadeOnUpdate(context.ClassConfiguration()))
			{
				context.UpdateDepth(AdjustUpdateDepthForCascade(obj, updateDepth));
			}
			context.WriteObjectWithCurrentState(GetHandler(), obj);
			context.UpdateDepth(updateDepth);
			if (HasIndex())
			{
				context.AddIndexEntry(this, obj);
			}
		}

		public virtual bool NeedsArrayAndPrimitiveInfo()
		{
			return true;
		}

		public virtual IPreparedComparison PrepareComparison(IContext context, object obj
			)
		{
			if (!Alive())
			{
				return null;
			}
			return Handlers4.PrepareComparisonFor(GetHandler(), context, obj);
		}

		public virtual Db4objects.Db4o.Internal.Query.Processor.QField QField(Transaction
			 a_trans)
		{
			int classMetadataID = 0;
			if (_containingClass != null)
			{
				classMetadataID = _containingClass.GetID();
			}
			return new Db4objects.Db4o.Internal.Query.Processor.QField(a_trans, _name, this, 
				classMetadataID, _handle);
		}

		public virtual object Read(IObjectIdContext context)
		{
			if (!CanReadFromSlot((IAspectVersionContext)context))
			{
				IncrementOffset(context, context);
				return null;
			}
			return context.Read(GetHandler());
		}

		private bool CanReadFromSlot(IAspectVersionContext context)
		{
			if (!IsEnabledOn(context))
			{
				return false;
			}
			if (Alive())
			{
				return true;
			}
			return _state != FieldMetadataState.NotLoaded;
		}

		internal virtual void Refresh()
		{
			ClassMetadata newFieldType = DetectFieldType();
			if (newFieldType != null && newFieldType.Equals(_fieldType))
			{
				return;
			}
			_reflectField = null;
			_state = FieldMetadataState.Unavailable;
		}

		// FIXME: needs test case
		public virtual void Rename(string newName)
		{
			ObjectContainerBase container = Container();
			if (!container.IsClient)
			{
				_name = newName;
				_containingClass.SetStateDirty();
				_containingClass.Write(container.SystemTransaction());
			}
			else
			{
				Exceptions4.ThrowRuntimeException(58);
			}
		}

		public virtual void Set(object onObject, object obj)
		{
			// TODO: remove the following if and check callers
			if (null == _reflectField)
			{
				return;
			}
			FieldAccessor().Set(_reflectField, onObject, obj);
		}

		internal virtual void SetName(string a_name)
		{
			_name = a_name;
		}

		internal virtual bool SupportsIndex()
		{
			return Alive() && (GetHandler() is IIndexable4) && (!Handlers4.IsUntyped(GetHandler
				()));
		}

		public void TraverseValues(IVisitor4 userVisitor)
		{
			if (!Alive())
			{
				return;
			}
			TraverseValues(Container().Transaction, userVisitor);
		}

		public void TraverseValues(Transaction transaction, IVisitor4 userVisitor)
		{
			if (!Alive())
			{
				return;
			}
			AssertHasIndex();
			ObjectContainerBase stream = transaction.Container();
			if (stream.IsClient)
			{
				Exceptions4.ThrowRuntimeException(Db4objects.Db4o.Internal.Messages.ClientServerUnsupported
					);
			}
			lock (stream.Lock())
			{
				IContext context = transaction.Context();
				_index.TraverseKeys(transaction, new _IVisitor4_861(this, userVisitor, context));
			}
		}

		private sealed class _IVisitor4_861 : IVisitor4
		{
			public _IVisitor4_861(FieldMetadata _enclosing, IVisitor4 userVisitor, IContext context
				)
			{
				this._enclosing = _enclosing;
				this.userVisitor = userVisitor;
				this.context = context;
			}

			public void Visit(object obj)
			{
				IFieldIndexKey key = (IFieldIndexKey)obj;
				userVisitor.Visit(((IIndexableTypeHandler)this._enclosing.GetHandler()).IndexEntryToObject
					(context, key.Value()));
			}

			private readonly FieldMetadata _enclosing;

			private readonly IVisitor4 userVisitor;

			private readonly IContext context;
		}

		private void AssertHasIndex()
		{
			if (!HasIndex())
			{
				Exceptions4.ThrowRuntimeException(Db4objects.Db4o.Internal.Messages.OnlyForIndexedFields
					);
			}
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			if (_containingClass != null)
			{
				sb.Append(_containingClass.GetName());
				sb.Append(".");
			}
			sb.Append(GetName());
			return sb.ToString();
		}

		private void InitIndex(Transaction systemTrans)
		{
			InitIndex(systemTrans, 0);
		}

		public virtual void InitIndex(Transaction systemTrans, int id)
		{
			if (_index != null)
			{
				throw new InvalidOperationException();
			}
			if (systemTrans.Container().IsClient)
			{
				return;
			}
			_index = NewBTree(systemTrans, id);
		}

		protected BTree NewBTree(Transaction systemTrans, int id)
		{
			ObjectContainerBase stream = systemTrans.Container();
			IIndexable4 indexHandler = IndexHandler(stream);
			if (indexHandler == null)
			{
				return null;
			}
			return new BTree(systemTrans, id, new FieldIndexKeyHandler(indexHandler));
		}

		protected virtual IIndexable4 IndexHandler(ObjectContainerBase stream)
		{
			if (_reflectField == null)
			{
				return null;
			}
			IReflectClass indexType = _reflectField.IndexType();
			ITypeHandler4 classHandler = TypeHandlerForClass(stream, indexType);
			if (!(classHandler is IIndexable4))
			{
				return null;
			}
			return (IIndexable4)classHandler;
		}

		/// <param name="trans"></param>
		public virtual BTree GetIndex(Transaction trans)
		{
			return _index;
		}

		public virtual bool IsPrimitive()
		{
			return _isPrimitive;
		}

		public virtual IBTreeRange Search(Transaction transaction, object value)
		{
			AssertHasIndex();
			object transActionalValue = Handlers4.WrapWithTransactionContext(transaction, value
				, GetHandler());
			BTreeNodeSearchResult lowerBound = SearchLowerBound(transaction, transActionalValue
				);
			BTreeNodeSearchResult upperBound = SearchUpperBound(transaction, transActionalValue
				);
			return lowerBound.CreateIncludingRange(upperBound);
		}

		private BTreeNodeSearchResult SearchUpperBound(Transaction transaction, object value
			)
		{
			return SearchBound(transaction, int.MaxValue, value);
		}

		private BTreeNodeSearchResult SearchLowerBound(Transaction transaction, object value
			)
		{
			return SearchBound(transaction, 0, value);
		}

		private BTreeNodeSearchResult SearchBound(Transaction transaction, int parentID, 
			object keyPart)
		{
			return GetIndex(transaction).SearchLeafByObject(transaction, CreateFieldIndexKey(
				parentID, keyPart), SearchTarget.Lowest);
		}

		public virtual bool RebuildIndexForClass(LocalObjectContainer stream, ClassMetadata
			 classMetadata)
		{
			// FIXME: BTree traversal over index here.
			long[] ids = classMetadata.GetIDs();
			for (int i = 0; i < ids.Length; i++)
			{
				RebuildIndexForObject(stream, classMetadata, (int)ids[i]);
			}
			return ids.Length > 0;
		}

		/// <exception cref="Db4objects.Db4o.Internal.FieldIndexException"></exception>
		protected virtual void RebuildIndexForObject(LocalObjectContainer stream, ClassMetadata
			 classMetadata, int objectId)
		{
			StatefulBuffer writer = stream.ReadStatefulBufferById(stream.SystemTransaction(), 
				objectId);
			if (writer != null)
			{
				RebuildIndexForWriter(stream, writer, objectId);
			}
		}

		protected virtual void RebuildIndexForWriter(LocalObjectContainer stream, StatefulBuffer
			 writer, int objectId)
		{
			ObjectHeader oh = new ObjectHeader(stream, writer);
			object obj = ReadIndexEntryForRebuild(writer, oh);
			AddIndexEntry(stream.SystemTransaction(), objectId, obj);
		}

		private object ReadIndexEntryForRebuild(StatefulBuffer writer, ObjectHeader oh)
		{
			ClassMetadata classMetadata = oh.ClassMetadata();
			if (classMetadata == null)
			{
				return DefaultValueForFieldType();
			}
			ObjectIdContextImpl context = new ObjectIdContextImpl(writer.Transaction(), writer
				, oh, writer.GetID());
			if (!classMetadata.SeekToField(context, this))
			{
				return DefaultValueForFieldType();
			}
			try
			{
				return ReadIndexEntry(context);
			}
			catch (CorruptionException exc)
			{
				throw new FieldIndexException(exc, this);
			}
		}

		private object DefaultValueForFieldType()
		{
			ITypeHandler4 handler = _fieldType.TypeHandler();
			return (handler is PrimitiveHandler) ? ((PrimitiveHandler)handler).PrimitiveNull(
				) : null;
		}

		public void DropIndex(LocalTransaction systemTrans)
		{
			if (_index == null)
			{
				return;
			}
			ObjectContainerBase stream = systemTrans.Container();
			if (stream.ConfigImpl.MessageLevel() > Const4.None)
			{
				stream.Message("dropping index " + ToString());
			}
			_index.Free(systemTrans);
			stream.SetDirtyInSystemTransaction(ContainingClass());
			_index = null;
		}

		public override void DefragAspect(IDefragmentContext context)
		{
			if (!CanDefragment())
			{
				throw new InvalidOperationException("Field '" + ToString() + "' cannot be defragmented at this time."
					);
			}
			ITypeHandler4 correctTypeHandlerVersion = HandlerRegistry.CorrectHandlerVersion(context
				, GetHandler(), _fieldType);
			context.SlotFormat().DoWithSlotIndirection(context, correctTypeHandlerVersion, new 
				_IClosure4_1020(context, correctTypeHandlerVersion));
		}

		private sealed class _IClosure4_1020 : IClosure4
		{
			public _IClosure4_1020(IDefragmentContext context, ITypeHandler4 correctTypeHandlerVersion
				)
			{
				this.context = context;
				this.correctTypeHandlerVersion = correctTypeHandlerVersion;
			}

			public object Run()
			{
				context.Defragment(correctTypeHandlerVersion);
				return null;
			}

			private readonly IDefragmentContext context;

			private readonly ITypeHandler4 correctTypeHandlerVersion;
		}

		private bool CanDefragment()
		{
			if (Alive() || Updating())
			{
				return true;
			}
			if (_fieldType == null || GetHandler() == null)
			{
				return false;
			}
			return !_fieldType.StateDead();
		}

		public virtual void CreateIndex()
		{
			if (HasIndex())
			{
				return;
			}
			LocalObjectContainer container = (LocalObjectContainer)Container();
			if (container.ConfigImpl.MessageLevel() > Const4.None)
			{
				container.Message("creating index " + ToString());
			}
			InitIndex(container.SystemTransaction());
			container.SetDirtyInSystemTransaction(ContainingClass());
			Reindex(container);
		}

		private void Reindex(LocalObjectContainer container)
		{
			ClassMetadata clazz = ContainingClass();
			if (RebuildIndexForClass(container, clazz))
			{
				container.SystemTransaction().Commit();
			}
		}

		public override Db4objects.Db4o.Internal.Marshall.AspectType AspectType()
		{
			return Db4objects.Db4o.Internal.Marshall.AspectType.Field;
		}

		public override bool CanBeDisabled()
		{
			return true;
		}

		public virtual void DropIndex()
		{
			DropIndex((LocalTransaction)Container().SystemTransaction());
		}

		public virtual bool CanUpdateFast()
		{
			if (HasIndex())
			{
				return false;
			}
			if (IsStruct())
			{
				return false;
			}
			return true;
		}

		private bool IsStruct()
		{
			return _fieldType != null && _fieldType.IsStruct();
		}
	}
}
