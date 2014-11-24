/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

import com.db4o.foundation.*;
import com.db4o.inside.replication.*;
import com.db4o.reflect.*;
import com.db4o.reflect.generic.*;
import com.db4o.types.*;

/**
 * @exclude
 */
public final class YapHandlers {
	
	private final YapStream _masterStream;  // this is master YapStream and not valid
	                                   // for YapObjectCarrier

    private static final Db4oTypeImpl[]   i_db4oTypes     = { };

    public static final int         ANY_ARRAY_ID      = 12;
    public static final int         ANY_ARRAY_N_ID     = 13;

    // Array Indices in i_YapContainers
    private static final int        CLASSCOUNT      = 11;

    private YapClass                i_anyArray;
    private YapClass                i_anyArrayN;

    public final YapString          i_stringHandler;

    private TypeHandler4[]           i_handlers;

    private int                     i_maxTypeID     = ANY_ARRAY_N_ID + 1;

    private YapTypeAbstract[]       i_platformTypes;
    static private final int        PRIMITIVECOUNT  = 8;

    YapClass[]                      i_yapClasses;

    // need to keep getID Functions in Sync with ArrayIndex
    private static final int        ANY_INDEX          = 10;
    public static final int         ANY_ID        = 11;
    
    final YapFieldVirtual[]         i_virtualFields = new YapFieldVirtual[2]; 

    private final Hashtable4        i_classByClass  = new Hashtable4(32);
    
    Db4oCollections 				i_collections;
    
    YapIndexes              		i_indexes;
    
    ReplicationImpl				    i_replication;
    
    MigrationConnection             i_migration;
    
    Db4oReplicationReferenceProvider _replicationReferenceProvider;
    
    boolean                 i_encrypt;
    byte[]                  i_encryptor;
    int                     i_lastEncryptorByte;
    
    final GenericReflector                _reflector;
    
    ReflectClass ICLASS_COMPARE;
    ReflectClass ICLASS_DB4OTYPE;
    ReflectClass ICLASS_DB4OTYPEIMPL;
	public ReflectClass ICLASS_INTERNAL;
    ReflectClass ICLASS_UNVERSIONED;
    ReflectClass ICLASS_OBJECT;
    ReflectClass ICLASS_OBJECTCONTAINER;
    ReflectClass ICLASS_PBOOTRECORD;
	ReflectClass ICLASS_STATICCLASS;
	ReflectClass ICLASS_STRING;
    ReflectClass ICLASS_TRANSIENTCLASS;

    YapHandlers(final YapStream a_stream, byte stringEncoding, GenericReflector reflector) {
    	
    	_masterStream = a_stream;
    	a_stream.i_handlers = this;
        _reflector = reflector;
    	
    	initClassReflectors(reflector);
        
        i_indexes = new YapIndexes(a_stream);
        
        i_virtualFields[0] = i_indexes.i_fieldVersion;
        i_virtualFields[1] = i_indexes.i_fieldUUID;

        i_stringHandler = new YapString(a_stream, YapStringIO.forEncoding(stringEncoding));

        i_handlers = new TypeHandler4[] { new YInt(a_stream), new YLong(a_stream), new YFloat(a_stream),
            new YBoolean(a_stream), new YDouble(a_stream), new YByte(a_stream), new YChar(a_stream),
            new YShort(a_stream),

            // primitives first
            i_stringHandler, new YDate(a_stream), new YapClassAny(a_stream) // Index = 10, ID = 11
        };
        
        i_platformTypes = Platform4.types(a_stream);

        if (i_platformTypes.length > 0) {
            for (int i = 0; i < i_platformTypes.length; i++) {
                i_platformTypes[i].initialize();
                if (i_platformTypes[i].getID() > i_maxTypeID) {
                    i_maxTypeID = i_platformTypes[i].getID();
                }
            }

            TypeHandler4[] temp = i_handlers;
            i_handlers = new TypeHandler4[i_maxTypeID];
            System.arraycopy(temp, 0, i_handlers, 0, temp.length);
            for (int i = 0; i < i_platformTypes.length; i++) {
                int idx = i_platformTypes[i].getID() - 1;
                i_handlers[idx] = i_platformTypes[i];
            }
        }

        i_yapClasses = new YapClass[i_maxTypeID + 1];

        for (int i = 0; i < CLASSCOUNT; i++) {
            int id = i + 1; // note that we avoid 0 here
            i_yapClasses[i] = new YapClassPrimitive(a_stream, i_handlers[i]);
            i_yapClasses[i].i_id = id; 
            i_classByClass.put(i_handlers[i].classReflector(), i_yapClasses[i]);
            if(i < ANY_INDEX){
            	reflector.registerPrimitiveClass(id, i_handlers[i].classReflector().getName(), null);
            }
            if (!Deploy.csharp) {
                if (i_handlers[i].primitiveClassReflector() != null) {
                	i_classByClass.put(i_handlers[i].primitiveClassReflector(), i_yapClasses[i]);
                }
            }
        }
        for (int i = 0; i < i_platformTypes.length; i++) {
        	int id = i_platformTypes[i].getID();
            int idx = id - 1;
            GenericConverter converter = (i_platformTypes[i] instanceof GenericConverter) ? (GenericConverter)i_platformTypes[i] : null;  
            reflector.registerPrimitiveClass(id, i_platformTypes[i].getName(), converter);
            i_handlers[idx] = i_platformTypes[i];
            i_yapClasses[idx] = new YapClassPrimitive(a_stream, i_platformTypes[i]);
            i_yapClasses[idx].i_id = id;
            if (i_yapClasses[idx].i_id > i_maxTypeID) {
                i_maxTypeID = idx;
            }
            i_classByClass.put(i_platformTypes[i].classReflector(), i_yapClasses[idx]);
            if (!Deploy.csharp) {
                if (i_platformTypes[i].primitiveClassReflector() != null) {
                	i_classByClass.put(i_platformTypes[i].primitiveClassReflector(), i_yapClasses[idx]);
                }
            }
        }

        i_anyArray = new YapClassPrimitive(a_stream, new YapArray(_masterStream,
            i_handlers[ANY_INDEX], false));
        i_anyArray.i_id = ANY_ARRAY_ID;
        i_yapClasses[ANY_ARRAY_ID - 1] = i_anyArray;

        i_anyArrayN = new YapClassPrimitive(a_stream, new YapArrayN(_masterStream,
            i_handlers[ANY_INDEX], false));
        i_anyArrayN.i_id = ANY_ARRAY_N_ID;
        i_yapClasses[ANY_ARRAY_N_ID - 1] = i_anyArrayN;
    }

	int arrayType(Object a_object) {
    	ReflectClass claxx = _masterStream.reflector().forObject(a_object);
        if (claxx.isArray()) {
            if (_masterStream.reflector().array().isNDimensional(claxx)) {
                return YapConst.TYPE_NARRAY;
            } else {
                return YapConst.TYPE_ARRAY;
            }
        }
        return 0;
    }
	
    boolean createConstructor(final ReflectClass claxx, boolean skipConstructor){
        
        if (claxx == null) {
            return false;
        }
        
        if (claxx.isAbstract() || claxx.isInterface()) {
            return true;
        }
        
        if(! Platform4.callConstructor()){
            if(claxx.skipConstructor(skipConstructor)){
                return true;
            }
        }
        
        if (! _masterStream.i_config.testConstructors()) {
            return true;
        }
        
        if (claxx.newInstance() != null) {
            return true;
        }
        
        if (_masterStream.reflector().constructorCallsSupported()) {
            try {
                
                ReflectConstructor[] constructors = claxx.getDeclaredConstructors();
                
                Tree sortedConstructors = null;
                
                // sort constructors by parameter count  
                for (int i = 0; i < constructors.length; i++) {
                    try{
                        constructors[i].setAccessible();
                        int parameterCount =  constructors[i].getParameterTypes().length;
                        sortedConstructors = Tree.add(sortedConstructors, new TreeIntObject(i+constructors.length*parameterCount, constructors[i]));
                    } catch (Throwable t) {
                        if(Debug.atHome){
                            t.printStackTrace();
                        }
                    }
                }
                
                // call zero-arg constructors first
                final boolean[] foundConstructor={false};
                if(sortedConstructors != null){
                    sortedConstructors.traverse(new Visitor4() {
                        public void visit(Object a_object) {
                            if(! foundConstructor[0]) {
	                            ReflectConstructor constructor = (ReflectConstructor)((TreeIntObject)a_object)._object;
	                            try {
	                                ReflectClass[] pTypes = constructor.getParameterTypes();
	                                Object[] parms = new Object[pTypes.length];
	                                for (int j = 0; j < parms.length; j++) {
	                                    for (int k = 0; k < PRIMITIVECOUNT; k++) {
	                                        if (pTypes[j].equals(i_handlers[k].primitiveClassReflector())) {
	                                            parms[j] = ((YapJavaClass) i_handlers[k])
	                                                .primitiveNull();
	                                            break;
	                                        }
	                                    }
	                                }
	                                Object res = constructor.newInstance(parms);
	                                if (res != null) {
	                                    foundConstructor[0] = true;
                                        claxx.useConstructor(constructor, parms);
	                                }
	                            } catch (Throwable t) {
	                                if(Debug.atHome){
	                                    t.printStackTrace();
	                                }
	                            }
                            }
                        }
                    });
                    
                }
                if(foundConstructor[0]){
                    return true;
                }
                
            } catch (Throwable t1) {
                if(Debug.atHome){
                    t1.printStackTrace();
                }

            }
        }
        return false;
    }
    
	final void decrypt(YapReader reader) {
	    if(i_encrypt){
			int encryptorOffSet = i_lastEncryptorByte;
			byte[] bytes = reader._buffer;
			for (int i = reader.getLength() - 1; i >= 0; i--) {
				bytes[i] += i_encryptor[encryptorOffSet];
				if (encryptorOffSet == 0) {
					encryptorOffSet = i_lastEncryptorByte;
				} else {
					encryptorOffSet--;
				}
			}
	    }
	}
	
    final void encrypt(YapReader reader) {
        if(i_encrypt){
	        byte[] bytes = reader._buffer;
	        int encryptorOffSet = i_lastEncryptorByte;
	        for (int i = reader.getLength() - 1; i >= 0; i--) {
	            bytes[i] -= i_encryptor[encryptorOffSet];
	            if (encryptorOffSet == 0) {
	                encryptorOffSet = i_lastEncryptorByte;
	            } else {
	                encryptorOffSet--;
	            }
	        }
        }
    }
    
    final TypeHandler4 getHandler(int a_index) {
        return i_handlers[a_index - 1];
    }

    final TypeHandler4 handlerForClass(ReflectClass a_class, ReflectClass[] a_Supported) {
        for (int i = 0; i < a_Supported.length; i++) {
            if (a_Supported[i].equals(a_class)) {
                return i_handlers[i];
            }
        }
        return null;
    }

    /**
     * Can't return ANY class for interfaces, since that would kill the
     * translators built into the architecture.
     */
    final TypeHandler4 handlerForClass(YapStream a_stream, ReflectClass a_class) {
        if(a_class == null){
            return null;
        }
        if (a_class.isArray()) {
            return handlerForClass(a_stream, a_class.getComponentType());
        }
        YapClass yc = getYapClassStatic(a_class);
        if (yc != null) {
            return ((YapClassPrimitive) yc).i_handler;
        }
        return a_stream.getYapClass(a_class, true);
    }
    
    private void initClassReflectors(GenericReflector reflector){
		ICLASS_COMPARE = reflector.forClass(YapConst.CLASS_COMPARE);
		ICLASS_DB4OTYPE = reflector.forClass(YapConst.CLASS_DB4OTYPE);
		ICLASS_DB4OTYPEIMPL = reflector.forClass(YapConst.CLASS_DB4OTYPEIMPL);
        ICLASS_INTERNAL = reflector.forClass(YapConst.CLASS_INTERNAL);
        ICLASS_UNVERSIONED = reflector.forClass(YapConst.CLASS_UNVERSIONED);
		ICLASS_OBJECT = reflector.forClass(YapConst.CLASS_OBJECT);
		ICLASS_OBJECTCONTAINER = reflector
				.forClass(YapConst.CLASS_OBJECTCONTAINER);
		ICLASS_PBOOTRECORD = reflector.forClass(YapConst.CLASS_PBOOTRECORD);
		ICLASS_STATICCLASS = reflector.forClass(YapConst.CLASS_STATICCLASS);
		ICLASS_STRING = reflector.forClass(String.class);
		ICLASS_TRANSIENTCLASS = reflector
				.forClass(YapConst.CLASS_TRANSIENTCLASS);
		
		Platform4.registerCollections(reflector);
    }
    
    void initEncryption(Config4Impl a_config){
        if (a_config.encrypt() && a_config.password() != null
            && a_config.password().length() > 0) {
            i_encrypt = true;
            i_encryptor = new byte[a_config.password().length()];
            for (int i = 0; i < i_encryptor.length; i++) {
                i_encryptor[i] = (byte) (a_config.password().charAt(i) & 0xff);
            }
            i_lastEncryptorByte = a_config.password().length() - 1;
        } else {
            i_encrypt = false;
            i_encryptor = null;
            i_lastEncryptorByte = 0;
        }
    }
    
    static Db4oTypeImpl getDb4oType(ReflectClass clazz) {
        for (int i = 0; i < i_db4oTypes.length; i++) {
            if (clazz.isInstance(i_db4oTypes[i])) {
                return i_db4oTypes[i];
            }
        }
        return null;
    }

    public YapClass getYapClassStatic(int a_id) {
        if (a_id > 0 && a_id <= i_maxTypeID) {
            return i_yapClasses[a_id - 1];
        }
        return null;
    }

    YapClass getYapClassStatic(ReflectClass a_class) {
        if (a_class == null) {
            return null;
        }
        if (a_class.isArray()) {
            if (_masterStream.reflector().array().isNDimensional(a_class)) {
                return i_anyArrayN;
            }
            return i_anyArray;
        }
        return (YapClass) i_classByClass.get(a_class);
    }
    
    public boolean isSecondClass(Object a_object){
    	if(a_object != null){
    		ReflectClass claxx = _masterStream.reflector().forObject(a_object);
    		if(i_classByClass.get(claxx) != null){
    			return true;
    		}
            if(Deploy.csharp){
                return Platform4.isValueType(claxx);
            }
    	}
    	return false;
    }

    int maxTypeID() {
        return i_maxTypeID;
    }
}