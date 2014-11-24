/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Encoding;
using Db4objects.Db4o.Internal.Handlers;
using Db4objects.Db4o.Internal.Handlers.Array;
using Db4objects.Db4o.Internal.Handlers.Versions;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Internal.Replication;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Reflect.Generic;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Internal
{
	/// <exclude>
	/// TODO: This class was written to make ObjectContainerBase
	/// leaner, so TransportObjectContainer has less members.
	/// All functionality of this class should become part of
	/// ObjectContainerBase and the functionality in
	/// ObjectContainerBase should delegate to independent
	/// modules without circular references.
	/// </exclude>
	public sealed class HandlerRegistry
	{
		public const byte HandlerVersion = (byte)10;

		private readonly ObjectContainerBase _container;

		private static readonly IDb4oTypeImpl[] _db4oTypes = new IDb4oTypeImpl[] { new BlobImpl
			() };

		private ITypeHandler4 _openArrayHandler;

		private ITypeHandler4 _openMultiDimensionalArrayHandler;

		private ITypeHandler4 _openTypeHandler;

		public StringHandler _stringHandler;

		private Hashtable4 _mapIdToTypeInfo = NewHashtable();

		private Hashtable4 _mapReflectorToClassMetadata = NewHashtable();

		private int _highestBuiltinTypeID = Handlers4.AnyArrayNId + 1;

		private readonly VirtualFieldMetadata[] _virtualFields = new VirtualFieldMetadata
			[3];

		private readonly Hashtable4 _mapReflectorToTypeHandler = NewHashtable();

		private SharedIndexedFields _indexes;

		internal IDb4oReplicationReferenceProvider _replicationReferenceProvider;

		private readonly Db4objects.Db4o.Internal.Diagnostic.DiagnosticProcessor _diagnosticProcessor;

		public bool i_encrypt;

		internal byte[] i_encryptor;

		internal int i_lastEncryptorByte;

		internal readonly GenericReflector _reflector;

		private readonly HandlerVersionRegistry _handlerVersions;

		private LatinStringIO _stringIO;

		public IReflectClass IclassCompare;

		internal IReflectClass IclassDb4otype;

		internal IReflectClass IclassDb4otypeimpl;

		public IReflectClass IclassInternal;

		internal IReflectClass IclassUnversioned;

		public IReflectClass IclassObject;

		internal IReflectClass IclassObjectcontainer;

		public IReflectClass IclassStaticclass;

		public IReflectClass IclassString;

		internal IReflectClass IclassTransientclass;

		private PrimitiveTypeMetadata _untypedArrayMetadata;

		private PrimitiveTypeMetadata _untypedMultiDimensionalMetadata;

		internal HandlerRegistry(ObjectContainerBase container, byte stringEncoding, GenericReflector
			 reflector)
		{
			// this is the master container and not valid
			// for TransportObjectContainer
			_handlerVersions = new HandlerVersionRegistry(this);
			_stringIO = BuiltInStringEncoding.StringIoForEncoding(stringEncoding, container.ConfigImpl
				.StringEncoding());
			_container = container;
			container._handlers = this;
			_reflector = reflector;
			_diagnosticProcessor = container.ConfigImpl.DiagnosticProcessor();
			InitClassReflectors(reflector);
			_indexes = new SharedIndexedFields();
			_virtualFields[0] = _indexes._version;
			_virtualFields[1] = _indexes._uUID;
			_virtualFields[2] = _indexes._commitTimestamp;
			RegisterBuiltinHandlers();
			RegisterPlatformTypes();
			InitArrayHandlers();
			Platform4.RegisterPlatformHandlers(container);
		}

		private void InitArrayHandlers()
		{
			ITypeHandler4 elementHandler = OpenTypeHandler();
			_untypedArrayMetadata = new PrimitiveTypeMetadata(Container(), new ArrayHandler(elementHandler
				, false), Handlers4.AnyArrayId, IclassObject);
			_openArrayHandler = _untypedArrayMetadata.TypeHandler();
			MapTypeInfo(Handlers4.AnyArrayId, _untypedArrayMetadata, null);
			_untypedMultiDimensionalMetadata = new PrimitiveTypeMetadata(Container(), new MultidimensionalArrayHandler
				(elementHandler, false), Handlers4.AnyArrayNId, IclassObject);
			_openMultiDimensionalArrayHandler = _untypedMultiDimensionalMetadata.TypeHandler(
				);
			MapTypeInfo(Handlers4.AnyArrayNId, _untypedMultiDimensionalMetadata, null);
		}

		private void RegisterPlatformTypes()
		{
			NetTypeHandler[] handlers = Platform4.Types(_container.Reflector());
			for (int i = 0; i < handlers.Length; i++)
			{
				RegisterNetTypeHandler(handlers[i]);
			}
		}

		public void RegisterNetTypeHandler(NetTypeHandler handler)
		{
			handler.RegisterReflector(_reflector);
			IGenericConverter converter = (handler is IGenericConverter) ? (IGenericConverter
				)handler : null;
			RegisterBuiltinHandler(handler.GetID(), handler, true, handler.GetName(), converter
				);
		}

		private void RegisterBuiltinHandlers()
		{
			IntHandler intHandler = new IntHandler();
			RegisterBuiltinHandler(Handlers4.IntId, intHandler);
			RegisterHandlerVersion(intHandler, 0, new IntHandler0());
			LongHandler longHandler = new LongHandler();
			RegisterBuiltinHandler(Handlers4.LongId, longHandler);
			RegisterHandlerVersion(longHandler, 0, new LongHandler0());
			FloatHandler floatHandler = new FloatHandler();
			RegisterBuiltinHandler(Handlers4.FloatId, floatHandler);
			RegisterHandlerVersion(floatHandler, 0, new FloatHandler0());
			BooleanHandler booleanHandler = new BooleanHandler();
			RegisterBuiltinHandler(Handlers4.BooleanId, booleanHandler);
			// TODO: Are we missing a boolean handler version?
			DoubleHandler doubleHandler = new DoubleHandler();
			RegisterBuiltinHandler(Handlers4.DoubleId, doubleHandler);
			RegisterHandlerVersion(doubleHandler, 0, new DoubleHandler0());
			ByteHandler byteHandler = new ByteHandler();
			RegisterBuiltinHandler(Handlers4.ByteId, byteHandler);
			// TODO: Are we missing a byte handler version?
			CharHandler charHandler = new CharHandler();
			RegisterBuiltinHandler(Handlers4.CharId, charHandler);
			// TODO: Are we missing a char handler version?
			ShortHandler shortHandler = new ShortHandler();
			RegisterBuiltinHandler(Handlers4.ShortId, shortHandler);
			RegisterHandlerVersion(shortHandler, 0, new ShortHandler0());
			_stringHandler = new StringHandler();
			RegisterBuiltinHandler(Handlers4.StringId, _stringHandler);
			RegisterHandlerVersion(_stringHandler, 0, new StringHandler0());
			DateHandler dateHandler = new DateHandler();
			RegisterBuiltinHandler(Handlers4.DateId, dateHandler);
			RegisterHandlerVersion(dateHandler, 0, new DateHandler0());
			RegisterUntypedHandlers();
			RegisterCompositeHandlerVersions();
		}

		private void RegisterUntypedHandlers()
		{
			_openTypeHandler = new Db4objects.Db4o.Internal.OpenTypeHandler(Container());
			PrimitiveTypeMetadata classMetadata = new ObjectTypeMetadata(Container(), _openTypeHandler
				, Handlers4.UntypedId, IclassObject);
			Map(Handlers4.UntypedId, classMetadata, IclassObject);
			RegisterHandlerVersion(_openTypeHandler, 0, new OpenTypeHandler0(Container()));
			RegisterHandlerVersion(_openTypeHandler, 2, new OpenTypeHandler2(Container()));
			RegisterHandlerVersion(_openTypeHandler, 7, new OpenTypeHandler7(Container()));
		}

		private void RegisterCompositeHandlerVersions()
		{
			RegisterHandlerVersion(new StandardReferenceTypeHandler(), 0, new StandardReferenceTypeHandler0
				());
			ArrayHandler arrayHandler = new ArrayHandler();
			RegisterHandlerVersion(arrayHandler, 0, new ArrayHandler0());
			RegisterHandlerVersion(arrayHandler, 1, new ArrayHandler1());
			RegisterHandlerVersion(arrayHandler, 3, new ArrayHandler3());
			RegisterHandlerVersion(arrayHandler, 5, new ArrayHandler5());
			MultidimensionalArrayHandler multidimensionalArrayHandler = new MultidimensionalArrayHandler
				();
			RegisterHandlerVersion(multidimensionalArrayHandler, 0, new MultidimensionalArrayHandler0
				());
			RegisterHandlerVersion(multidimensionalArrayHandler, 3, new MultidimensionalArrayHandler3
				());
		}

		private void RegisterBuiltinHandler(int id, IBuiltinTypeHandler handler)
		{
			RegisterBuiltinHandler(id, handler, true, null, null);
		}

		private void RegisterBuiltinHandler(int id, IBuiltinTypeHandler typeHandler, bool
			 registerPrimitiveClass, string primitiveName, IGenericConverter converter)
		{
			typeHandler.RegisterReflector(_reflector);
			if (primitiveName == null)
			{
				primitiveName = typeHandler.ClassReflector().GetName();
			}
			if (registerPrimitiveClass)
			{
				_reflector.RegisterPrimitiveClass(id, primitiveName, converter);
			}
			IReflectClass classReflector = typeHandler.ClassReflector();
			PrimitiveTypeMetadata classMetadata = new PrimitiveTypeMetadata(Container(), typeHandler
				, id, classReflector);
			Map(id, classMetadata, classReflector);
			if (typeHandler is PrimitiveHandler)
			{
				IReflectClass primitiveClassReflector = ((PrimitiveHandler)typeHandler).PrimitiveClassReflector
					();
				if (primitiveClassReflector != null)
				{
					MapPrimitive(0, classMetadata, primitiveClassReflector);
				}
			}
		}

		private void Map(int id, PrimitiveTypeMetadata classMetadata, IReflectClass classReflector
			)
		{
			// TODO: remove when _mapIdToClassMetadata is gone 
			MapTypeInfo(id, classMetadata, classReflector);
			MapPrimitive(id, classMetadata, classReflector);
			if (id > _highestBuiltinTypeID)
			{
				_highestBuiltinTypeID = id;
			}
		}

		private void MapTypeInfo(int id, ClassMetadata classMetadata, IReflectClass classReflector
			)
		{
			_mapIdToTypeInfo.Put(id, new HandlerRegistry.TypeInfo(classMetadata, classReflector
				));
		}

		private void MapPrimitive(int id, ClassMetadata classMetadata, IReflectClass classReflector
			)
		{
			MapClassToTypeHandler(classReflector, classMetadata.TypeHandler());
			if (classReflector != null)
			{
				_mapReflectorToClassMetadata.Put(classReflector, classMetadata);
			}
		}

		private void MapClassToTypeHandler(IReflectClass classReflector, ITypeHandler4 typeHandler
			)
		{
			_mapReflectorToTypeHandler.Put(classReflector, typeHandler);
		}

		public void RegisterHandlerVersion(ITypeHandler4 handler, int version, ITypeHandler4
			 replacement)
		{
			if (replacement is IBuiltinTypeHandler)
			{
				((IBuiltinTypeHandler)replacement).RegisterReflector(_reflector);
			}
			_handlerVersions.Put(handler, version, replacement);
		}

		public ITypeHandler4 CorrectHandlerVersion(ITypeHandler4 handler, int version)
		{
			return _handlerVersions.CorrectHandlerVersion(handler, version);
		}

		public static ITypeHandler4 CorrectHandlerVersion(IHandlerVersionContext context, 
			ITypeHandler4 typeHandler, ClassMetadata classMetadata)
		{
			ITypeHandler4 correctHandlerVersion = CorrectHandlerVersion(context, typeHandler);
			if (typeHandler != correctHandlerVersion)
			{
				CorrectClassMetadataOn(correctHandlerVersion, classMetadata);
				if (correctHandlerVersion is ArrayHandler)
				{
					ArrayHandler arrayHandler = (ArrayHandler)correctHandlerVersion;
					CorrectClassMetadataOn(arrayHandler.DelegateTypeHandler(), classMetadata);
				}
			}
			return correctHandlerVersion;
		}

		private static void CorrectClassMetadataOn(ITypeHandler4 typeHandler, ClassMetadata
			 classMetadata)
		{
			if (typeHandler is StandardReferenceTypeHandler)
			{
				StandardReferenceTypeHandler handler = (StandardReferenceTypeHandler)typeHandler;
				handler.ClassMetadata(classMetadata);
			}
		}

		internal Db4objects.Db4o.Internal.ArrayType ArrayType(object obj)
		{
			IReflectClass claxx = Reflector().ForObject(obj);
			if (!claxx.IsArray())
			{
				return Db4objects.Db4o.Internal.ArrayType.None;
			}
			if (IsNDimensional(claxx))
			{
				return Db4objects.Db4o.Internal.ArrayType.MultidimensionalArray;
			}
			return Db4objects.Db4o.Internal.ArrayType.PlainArray;
		}

		public void Decrypt(ByteArrayBuffer reader)
		{
			if (i_encrypt)
			{
				int encryptorOffSet = i_lastEncryptorByte;
				byte[] bytes = reader._buffer;
				for (int i = reader.Length() - 1; i >= 0; i--)
				{
					bytes[i] += i_encryptor[encryptorOffSet];
					if (encryptorOffSet == 0)
					{
						encryptorOffSet = i_lastEncryptorByte;
					}
					else
					{
						encryptorOffSet--;
					}
				}
			}
		}

		public void Encrypt(ByteArrayBuffer reader)
		{
			if (i_encrypt)
			{
				byte[] bytes = reader._buffer;
				int encryptorOffSet = i_lastEncryptorByte;
				for (int i = reader.Length() - 1; i >= 0; i--)
				{
					bytes[i] -= i_encryptor[encryptorOffSet];
					if (encryptorOffSet == 0)
					{
						encryptorOffSet = i_lastEncryptorByte;
					}
					else
					{
						encryptorOffSet--;
					}
				}
			}
		}

		public void OldEncryptionOff()
		{
			i_encrypt = false;
			i_encryptor = null;
			i_lastEncryptorByte = 0;
			Container().ConfigImpl.OldEncryptionOff();
		}

		public IReflectClass ClassForID(int id)
		{
			HandlerRegistry.TypeInfo typeInfo = TypeInfoForID(id);
			if (typeInfo == null)
			{
				return null;
			}
			return typeInfo.classReflector;
		}

		private HandlerRegistry.TypeInfo TypeInfoForID(int id)
		{
			return (HandlerRegistry.TypeInfo)_mapIdToTypeInfo.Get(id);
		}

		private void InitClassReflectors(GenericReflector reflector)
		{
			IclassCompare = reflector.ForClass(Const4.ClassCompare);
			IclassDb4otype = reflector.ForClass(Const4.ClassDb4otype);
			IclassDb4otypeimpl = reflector.ForClass(Const4.ClassDb4otypeimpl);
			IclassInternal = reflector.ForClass(Const4.ClassInternal);
			IclassUnversioned = reflector.ForClass(Const4.ClassUnversioned);
			IclassObject = reflector.ForClass(Const4.ClassObject);
			IclassObjectcontainer = reflector.ForClass(Const4.ClassObjectcontainer);
			IclassStaticclass = reflector.ForClass(Const4.ClassStaticclass);
			IclassString = reflector.ForClass(typeof(string));
			IclassTransientclass = reflector.ForClass(Const4.ClassTransientclass);
			Platform4.RegisterCollections(reflector);
		}

		internal void InitEncryption(Config4Impl a_config)
		{
			if (a_config.Encrypt() && a_config.Password() != null && a_config.Password().Length
				 > 0)
			{
				i_encrypt = true;
				i_encryptor = new byte[a_config.Password().Length];
				for (int i = 0; i < i_encryptor.Length; i++)
				{
					i_encryptor[i] = (byte)(a_config.Password()[i] & unchecked((int)(0xff)));
				}
				i_lastEncryptorByte = a_config.Password().Length - 1;
				return;
			}
			OldEncryptionOff();
		}

		internal static IDb4oTypeImpl GetDb4oType(IReflectClass clazz)
		{
			for (int i = 0; i < _db4oTypes.Length; i++)
			{
				if (clazz.IsInstance(_db4oTypes[i]))
				{
					return _db4oTypes[i];
				}
			}
			return null;
		}

		public ClassMetadata ClassMetadataForId(int id)
		{
			HandlerRegistry.TypeInfo typeInfo = TypeInfoForID(id);
			if (typeInfo == null)
			{
				return null;
			}
			return typeInfo.classMetadata;
		}

		internal ClassMetadata ClassMetadataForClass(IReflectClass clazz)
		{
			if (clazz == null)
			{
				return null;
			}
			if (clazz.IsArray())
			{
				return IsNDimensional(clazz) ? _untypedMultiDimensionalMetadata : _untypedArrayMetadata;
			}
			return (ClassMetadata)_mapReflectorToClassMetadata.Get(clazz);
		}

		public ITypeHandler4 OpenTypeHandler()
		{
			return _openTypeHandler;
		}

		public ITypeHandler4 OpenArrayHandler(IReflectClass clazz)
		{
			if (clazz.IsArray())
			{
				if (IsNDimensional(clazz))
				{
					return _openMultiDimensionalArrayHandler;
				}
				return _openArrayHandler;
			}
			return null;
		}

		private bool IsNDimensional(IReflectClass clazz)
		{
			return Reflector().Array().IsNDimensional(clazz);
		}

		public ITypeHandler4 TypeHandlerForClass(IReflectClass clazz)
		{
			if (clazz == null)
			{
				return null;
			}
			if (clazz.IsArray())
			{
				if (IsNDimensional(clazz))
				{
					return _openMultiDimensionalArrayHandler;
				}
				return _openArrayHandler;
			}
			ITypeHandler4 cachedTypeHandler = (ITypeHandler4)_mapReflectorToTypeHandler.Get(clazz
				);
			if (cachedTypeHandler != null)
			{
				return cachedTypeHandler;
			}
			ITypeHandler4 configuredTypeHandler = ConfiguredTypeHandler(clazz);
			if (Handlers4.IsValueType(configuredTypeHandler))
			{
				return configuredTypeHandler;
			}
			return null;
		}

		public bool IsSystemHandler(int id)
		{
			return id > 0 && id <= _highestBuiltinTypeID;
		}

		public int LowestValidId()
		{
			return _highestBuiltinTypeID + 1;
		}

		public VirtualFieldMetadata VirtualFieldByName(string name)
		{
			for (int i = 0; i < _virtualFields.Length; i++)
			{
				if (name.Equals(_virtualFields[i].GetName()))
				{
					return _virtualFields[i];
				}
			}
			return null;
		}

		public SharedIndexedFields Indexes()
		{
			return _indexes;
		}

		public LatinStringIO StringIO()
		{
			return _stringIO;
		}

		public void StringIO(LatinStringIO io)
		{
			_stringIO = io;
		}

		private GenericReflector Reflector()
		{
			return Container().Reflector();
		}

		private ObjectContainerBase Container()
		{
			return _container;
		}

		private static Hashtable4 NewHashtable()
		{
			return new Hashtable4(32);
		}

		public ITypeHandler4 ConfiguredTypeHandler(IReflectClass claxx)
		{
			object cachedHandler = _mapReflectorToTypeHandler.Get(claxx);
			if (null != cachedHandler)
			{
				return (ITypeHandler4)cachedHandler;
			}
			ITypeHandler4 typeHandler = Container().ConfigImpl.TypeHandlerForClass(claxx, HandlerVersion
				);
			if (typeHandler is IBuiltinTypeHandler)
			{
				((IBuiltinTypeHandler)typeHandler).RegisterReflector(Reflector());
			}
			if (Handlers4.IsValueType(typeHandler))
			{
				MapClassToTypeHandler(claxx, typeHandler);
			}
			return typeHandler;
		}

		public static ITypeHandler4 CorrectHandlerVersion(IHandlerVersionContext context, 
			ITypeHandler4 handler)
		{
			int version = context.HandlerVersion();
			if (version >= HandlerVersion)
			{
				return handler;
			}
			return context.Transaction().Container().Handlers.CorrectHandlerVersion(handler, 
				version);
		}

		public bool IsTransient(IReflectClass claxx)
		{
			return IclassTransientclass.IsAssignableFrom(claxx) || Platform4.IsTransient(claxx
				);
		}

		public void TreatAsOpenType(Type clazz)
		{
			MapClassToTypeHandler(ReflectClassFor(clazz), OpenTypeHandler());
		}

		private IReflectClass ReflectClassFor(Type clazz)
		{
			return Container().Reflector().ForClass(clazz);
		}

		public Db4objects.Db4o.Internal.Diagnostic.DiagnosticProcessor DiagnosticProcessor
			()
		{
			return _diagnosticProcessor;
		}

		private class TypeInfo
		{
			public ClassMetadata classMetadata;

			public IReflectClass classReflector;

			public TypeInfo(ClassMetadata classMetadata_, IReflectClass classReflector_)
			{
				// TODO: remove when no longer needed in HandlerRegistry
				classMetadata = classMetadata_;
				classReflector = classReflector_;
			}
		}
	}
}
