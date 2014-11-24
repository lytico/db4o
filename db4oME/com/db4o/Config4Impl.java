/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

import java.io.*;

import com.db4o.config.*;
import com.db4o.foundation.*;
import com.db4o.inside.*;
import com.db4o.inside.freespace.*;
import com.db4o.io.*;
import com.db4o.messaging.*;
import com.db4o.reflect.*;
import com.db4o.reflect.generic.*;

/**
 * Configuration template for creating new db4o files
 * 
 * @exclude
 */
public final class Config4Impl

implements Configuration, DeepClone, MessageSender, FreespaceConfiguration {
    
	private KeySpecHashtable4 _config=new KeySpecHashtable4(50);
	
	private final static KeySpec ACTIVATION_DEPTH=new KeySpec(5);
    
	private final static KeySpec ALLOW_VERSION_UPDATES=new KeySpec(false);

    private final static KeySpec AUTOMATIC_SHUTDOWN=new KeySpec(true);

    //  TODO: consider setting default to 8, it's more efficient with freespace.
    private final static KeySpec BLOCKSIZE=new KeySpec((byte)1);
    
	private final static KeySpec CALLBACKS=new KeySpec(true);
    
	private final static KeySpec CALL_CONSTRUCTORS=new KeySpec(YapConst.DEFAULT);
    
	private final static KeySpec CLASS_ACTIVATION_DEPTH_CONFIGURABLE=new KeySpec(true);
    
	private final static KeySpec CLASSLOADER=new KeySpec(null);
    
	private final static KeySpec DETECT_SCHEMA_CHANGES=new KeySpec(true);
    
	private final static KeySpec DISABLE_COMMIT_RECOVERY=new KeySpec(false);
    
	private final static KeySpec DISCARD_FREESPACE=new KeySpec(0);
    
	private final static KeySpec ENCODING=new KeySpec(YapConst.UNICODE);
    
	private final static KeySpec ENCRYPT=new KeySpec(false);
    
	private final static KeySpec EXCEPTIONAL_CLASSES=new KeySpec(new Hashtable4(16));
    
	private final static KeySpec EXCEPTIONS_ON_NOT_STORABLE=new KeySpec(false);
    
	private final static KeySpec FLUSH_FILE_BUFFERS=new KeySpec(true);
    
	private final static KeySpec FREESPACE_SYSTEM=new KeySpec(FreespaceManager.FM_DEFAULT);
    
	private final static KeySpec GENERATE_UUIDS=new KeySpec(0);
    
	private final static KeySpec GENERATE_VERSION_NUMBERS=new KeySpec(0);
    
	private final static KeySpec INTERN_STRINGS=new KeySpec(false);
    
	private final static KeySpec IS_SERVER=new KeySpec(false);
    
	private final static KeySpec LOCK_FILE=new KeySpec(true);
    
	private final static KeySpec MESSAGE_LEVEL=new KeySpec(YapConst.NONE);
    
	private final static KeySpec MESSAGE_RECIPIENT=new KeySpec(null);
    
	// TODO: who uses this at all?
//	private final static KeySpec MESSAGE_SENDER=new KeySpec(null);
//    private MessageSender    i_messageSender;
    
	private final static KeySpec OPTIMIZE_NQ=new KeySpec(true);
    
	private final static KeySpec OUTSTREAM=new KeySpec(null);
    
	private final static KeySpec PASSWORD=new KeySpec((String)null);
    
	private final static KeySpec READ_AS=new KeySpec(new Hashtable4(16));
    
	private final static KeySpec READ_ONLY=new KeySpec(false);
    
	private final static KeySpec CONFIGURED_REFLECTOR=new KeySpec(null);
    
	private final static KeySpec REFLECTOR=new KeySpec(null);
    
	private final static KeySpec RENAME=new KeySpec(null);
    
	private final static KeySpec RESERVED_STORAGE_SPACE=new KeySpec(0);
    
	private final static KeySpec SINGLE_THREADED_CLIENT=new KeySpec(false);
    
	private final static KeySpec TEST_CONSTRUCTORS=new KeySpec(true);
    
	private final static KeySpec TIMEOUT_CLIENT_SOCKET=new KeySpec(YapConst.CLIENT_SOCKET_TIMEOUT);
    
	private final static KeySpec TIMEOUT_PING_CLIENTS=new KeySpec(YapConst.CONNECTION_TIMEOUT);
    
	private final static KeySpec TIMEOUT_SERVER_SOCKET=new KeySpec(YapConst.SERVER_SOCKET_TIMEOUT);
    
	private final static KeySpec UPDATE_DEPTH=new KeySpec(0);
    
	private final static KeySpec WEAK_REFERENCE_COLLECTION_INTERVAL=new KeySpec(1000);
    
	private final static KeySpec WEAK_REFERENCES=new KeySpec(true);
    
	private final static KeySpec IOADAPTER=new KeySpec(null);
    
    	// NOTE: activate this config to trigger the defragment failure
    	//= new NIOFileAdapter(512,3);
    
	private final static KeySpec ALIASES=new KeySpec(null);
    
	//  is null in the global configuration until deepClone is called
	private YapStream        i_stream;                                                   


    int activationDepth() {
    	return _config.getAsInt(ACTIVATION_DEPTH);
    }

    public void activationDepth(int depth) {
    	_config.put(ACTIVATION_DEPTH,depth);
    }
    
    public void allowVersionUpdates(boolean flag){
    	_config.put(ALLOW_VERSION_UPDATES,flag);
    }

    public void automaticShutDown(boolean flag) {
    	_config.put(AUTOMATIC_SHUTDOWN,flag);
    }
    
    public void blockSize(int bytes){
       if (bytes < 1 || bytes > 127) {
           Exceptions4.throwRuntimeException(1);
       }
       
       if (i_stream != null) {
           Exceptions4.throwRuntimeException(46);   // see readable message for code in Messages.java
       }
       
       _config.put(BLOCKSIZE,(byte)bytes);
    }

    public void callbacks(boolean turnOn) {
        _config.put(CALLBACKS,turnOn);
    }
    
    public void callConstructors(boolean flag){
        _config.put(CALL_CONSTRUCTORS,(flag ? YapConst.YES : YapConst.NO));
    }

    public void classActivationDepthConfigurable(boolean turnOn) {
        _config.put(CLASS_ACTIVATION_DEPTH_CONFIGURABLE,turnOn);
    }

    Config4Class configClass(String className) {
		Config4Class config = (Config4Class)exceptionalClasses().get(className);

        if (Debug.configureAllClasses) {
            if (config == null) {

                boolean skip = false;

                Class[] ignore = new Class[] { MetaClass.class,
                    MetaField.class, MetaIndex.class, P1HashElement.class,
                    P1ListElement.class, P1Object.class, P1Collection.class,

                    // XXX You may need the following for indexing tests. 

                    //                        P2HashMap.class,
                    //                        P2LinkedList.class,

                    StaticClass.class, StaticField.class

                };
                for (int i = 0; i < ignore.length; i++) {
                    if (ignore[i].getName().equals(className)) {
                        skip = true;
                        break;
                    }

                }
                if (!skip) {
                    config = (Config4Class) objectClass(className);
                }

            }
        }
        return config;
    }

    public Object deepClone(Object param) {
        Config4Impl ret = new Config4Impl();
        ret._config=(KeySpecHashtable4)_config.deepClone(this);
        ret.i_stream = (YapStream) param;
        return ret;
    }

    public void detectSchemaChanges(boolean flag) {
        _config.put(DETECT_SCHEMA_CHANGES,flag);
    }

    public void disableCommitRecovery() {
        _config.put(DISABLE_COMMIT_RECOVERY,true);
    }

    public void discardFreeSpace(int bytes) {
        _config.put(DISCARD_FREESPACE,bytes);
    }
    
    public void discardSmallerThan(int byteCount) {
        discardFreeSpace(byteCount);
    }

    public void encrypt(boolean flag) {
        globalSettingOnly();
        _config.put(ENCRYPT,flag);
    }

    PrintStream errStream() {
    	PrintStream outStream=outStreamOrNull();
        return outStream == null ? System.err : outStream;
    }

    public void exceptionsOnNotStorable(boolean flag) {
        _config.put(EXCEPTIONS_ON_NOT_STORABLE,flag);
    }
    
    public void flushFileBuffers(boolean flag){
        _config.put(FLUSH_FILE_BUFFERS,flag);
    }

    public FreespaceConfiguration freespace() {
        return this;
    }
    
    public void generateUUIDs(int setting) {
        _config.put(GENERATE_UUIDS,setting);
        storeStreamBootRecord();
    }
    
    private void storeStreamBootRecord() {
        if(i_stream == null){
            return;
        }
        PBootRecord bootRecord = i_stream.bootRecord();
        if(bootRecord != null) {
            bootRecord.initConfig(this);
            Transaction trans = i_stream.getSystemTransaction();
            i_stream.setInternal(trans, bootRecord, false);
            trans.commit();
        }
    }

    public void generateVersionNumbers(int setting) {
        _config.put(GENERATE_VERSION_NUMBERS,setting);
        storeStreamBootRecord();
    }

    public MessageSender getMessageSender() {
        return this;
    }

    private void globalSettingOnly() {
        if (i_stream != null) {
            new Exception().printStackTrace();
            Exceptions4.throwRuntimeException(46);
        }
    }
    
    public void internStrings(boolean doIntern) {
    	_config.put(INTERN_STRINGS,doIntern);
    }
    
    public void io(IoAdapter adapter){
        globalSettingOnly();
        _config.put(IOADAPTER,adapter);
    }

    public void lockDatabaseFile(boolean flag) {
    	_config.put(LOCK_FILE,flag);
    }
    
    public void markTransient(String marker) {
        Platform4.markTransient(marker);
    }

    public void messageLevel(int level) {
    	_config.put(MESSAGE_LEVEL,level);
        if (outStream() == null) {
            setOut(System.out);
        }
    }

    public void optimizeNativeQueries(boolean optimizeNQ) {
    	_config.put(OPTIMIZE_NQ,optimizeNQ);
    }
    
    public boolean optimizeNativeQueries() {
    	return _config.getAsBoolean(OPTIMIZE_NQ);
    }
    
    public ObjectClass objectClass(Object clazz) {
        
        String className = null;
        
        if(clazz instanceof String){
            className = (String)clazz;
        }else{
            ReflectClass claxx = reflectorFor(clazz);
            if(claxx == null){
                return null;
            }
            className = claxx.getName();
        }
        
        Hashtable4 xClasses=exceptionalClasses();
        Config4Class c4c = (Config4Class) xClasses.get(className);
        if (c4c == null) {
            c4c = new Config4Class(this, className);
            xClasses.put(className, c4c);
        }
        return c4c;
    }

    private PrintStream outStreamOrNull() {
    	return (PrintStream)_config.get(OUTSTREAM);
    }
    
    PrintStream outStream() {
    	PrintStream outStream=outStreamOrNull();
        return outStream == null ? System.out : outStream;
    }

    public void password(String pw) {
        globalSettingOnly();
        _config.put(PASSWORD,pw);
    }

    public void readOnly(boolean flag) {
        globalSettingOnly();
        _config.put(READ_ONLY,flag);
    }

	GenericReflector reflector() {
		GenericReflector reflector=(GenericReflector)_config.get(REFLECTOR);
		if(reflector == null){
			Reflector configuredReflector=(Reflector)_config.get(CONFIGURED_REFLECTOR);
			if(configuredReflector == null){
				configuredReflector=Platform4.createReflector(classLoader());
				if(configuredReflector==null) {
					return null;
				}
				_config.put(CONFIGURED_REFLECTOR,configuredReflector);	
			}
			reflector=new GenericReflector(null, configuredReflector);
            _config.put(REFLECTOR,reflector);
            configuredReflector.setParent(reflector);
		}
		if(! reflector.hasTransaction() && i_stream != null){
			reflector.setTransaction(i_stream.i_systemTrans);
		}
		return reflector;
	}

	public void reflectWith(Reflector reflect) {
		
        if(i_stream != null){
        	Exceptions4.throwRuntimeException(46);   // see readable message for code in Messages.java
        }
		
        if (reflect == null) {
            throw new NullPointerException();
        }
        _config.put(CONFIGURED_REFLECTOR,reflect);
		_config.put(REFLECTOR,null);
    }

    public void refreshClasses() {
        if (i_stream == null) {
            Db4o.forEachSession(new Visitor4() {

                public void visit(Object obj) {
                    YapStream ys = ((Session) obj).i_stream;
                    if (!ys.isClosed()) {
                        ys.refreshClasses();
                    }
                }
            });
        } else {
            i_stream.refreshClasses();
        }
    }

    void rename(Rename a_rename) {
    	Collection4 renameCollection=rename();
        if (renameCollection == null) {
            renameCollection = new Collection4();
            _config.put(RENAME,renameCollection);
        }
        renameCollection.add(a_rename);
    }

    public void reserveStorageSpace(long byteCount) {
        int reservedStorageSpace = (int) byteCount;
        if (reservedStorageSpace < 0) {
            reservedStorageSpace = 0;
        }
        _config.put(RESERVED_STORAGE_SPACE,reservedStorageSpace);
        if (i_stream != null) {
            i_stream.reserve(reservedStorageSpace);
        }
    }

    /**
     * The ConfigImpl also is our messageSender
     */
    public void send(Object obj) {
        if (i_stream == null) {
            Db4o.forEachSession(new Visitor4() {

                public void visit(Object session) {
                    YapStream ys = ((Session) session).i_stream;
                    if (!ys.isClosed()) {
                        ys.send(session);
                    }

                }
            });
        } else {
            i_stream.send(obj);
        }
    }

    public void setClassLoader(Object classLoader) {
        reflectWith(Platform4.createReflector(classLoader));
    }

    public void setMessageRecipient(MessageRecipient messageRecipient) {
    	_config.put(MESSAGE_RECIPIENT,messageRecipient);
    }

    public void setOut(PrintStream outStream) {
        _config.put(OUTSTREAM,outStream);
        if (i_stream != null) {
            i_stream.logMsg(19, Db4o.version());
        } else {
            Messages.logMsg(Db4o.i_config, 19, Db4o.version());
        }
    }

    public void singleThreadedClient(boolean flag) {
    	_config.put(SINGLE_THREADED_CLIENT,flag);
    }

    public void testConstructors(boolean flag) {
    	_config.put(TEST_CONSTRUCTORS,flag);
    }

    public void timeoutClientSocket(int milliseconds) {
    	_config.put(TIMEOUT_CLIENT_SOCKET,milliseconds);
    }

    public void timeoutPingClients(int milliseconds) {
    	_config.put(TIMEOUT_PING_CLIENTS,milliseconds);
    }

    public void timeoutServerSocket(int milliseconds) {
    	_config.put(TIMEOUT_SERVER_SOCKET,milliseconds);

    }

    public void unicode(boolean unicodeOn) {
    	_config.put(ENCODING,(unicodeOn ? YapConst.UNICODE : YapConst.ISO8859));
    }

    public void updateDepth(int depth) {
    	_config.put(UPDATE_DEPTH,depth);
    }

    public void useRamSystem() {
        _config.put(FREESPACE_SYSTEM,FreespaceManager.FM_RAM);
    }

    public void useIndexSystem() {
        _config.put(FREESPACE_SYSTEM,FreespaceManager.FM_IX);
    }
    
    public void weakReferenceCollectionInterval(int milliseconds) {
    	_config.put(WEAK_REFERENCE_COLLECTION_INTERVAL,milliseconds);
    }

    public void weakReferences(boolean flag) {
    	_config.put(WEAK_REFERENCES,flag);
    }
    
    private Collection4 aliases() {
    	Collection4 aliasesCollection=(Collection4)_config.get(ALIASES);
    	if (null == aliasesCollection) {
    		aliasesCollection = new Collection4();
    		_config.put(ALIASES,aliasesCollection);
    	}
    	return aliasesCollection;
    }
    
    public void addAlias(Alias alias) {
    	if (null == alias) throw new IllegalArgumentException("alias");
    	aliases().add(alias);
    }
    
    public String resolveAlias(String runtimeType) {

    	Collection4 configuredAliases=aliases();
    	if (null == configuredAliases) return runtimeType;
    	
    	Iterator4 i = configuredAliases.iterator();
    	while (i.hasNext()) {
    		String resolved = ((Alias)i.next()).resolve(runtimeType);
    		if (null != resolved) return resolved; 
    	}
    	
    	return runtimeType;
    }
    
    ReflectClass reflectorFor(Object clazz) {
        
        clazz = Platform4.getClassForType(clazz);
        
        if(clazz instanceof ReflectClass){
            return (ReflectClass)clazz;
        }
        
        if(clazz instanceof Class){
            return reflector().forClass((Class)clazz);
        }
        
        if(clazz instanceof String){
            return reflector().forName((String)clazz);
        }
        
        return reflector().forObject(clazz);
    }

	public boolean allowVersionUpdates() {
		return _config.getAsBoolean(ALLOW_VERSION_UPDATES);
	}

	boolean automaticShutDown() {
		return _config.getAsBoolean(AUTOMATIC_SHUTDOWN);
	}

	byte blockSize() {
		return _config.getAsByte(BLOCKSIZE);
	}

	boolean callbacks() {
		return _config.getAsBoolean(CALLBACKS);
	}

	int callConstructors() {
		return _config.getAsInt(CALL_CONSTRUCTORS);
	}

	boolean classActivationDepthConfigurable() {
		return _config.getAsBoolean(CLASS_ACTIVATION_DEPTH_CONFIGURABLE);
	}

	Object classLoader() {
		return _config.get(CLASSLOADER);
	}

	boolean detectSchemaChanges() {
		return _config.getAsBoolean(DETECT_SCHEMA_CHANGES);
	}

	boolean commitRecoveryDisabled() {
		return _config.getAsBoolean(DISABLE_COMMIT_RECOVERY);
	}

	public int discardFreeSpace() {
		return _config.getAsInt(DISCARD_FREESPACE);
	}

	byte encoding() {
		return _config.getAsByte(ENCODING);
	}

	boolean encrypt() {
		return _config.getAsBoolean(ENCRYPT);
	}

	Hashtable4 exceptionalClasses() {
		return (Hashtable4)_config.get(EXCEPTIONAL_CLASSES);
	}

	boolean exceptionsOnNotStorable() {
		return _config.getAsBoolean(EXCEPTIONS_ON_NOT_STORABLE);
	}

	public boolean flushFileBuffers() {
		return _config.getAsBoolean(FLUSH_FILE_BUFFERS);
	}

	byte freespaceSystem() {
		return _config.getAsByte(FREESPACE_SYSTEM);
	}

	int generateUUIDs() {
		return _config.getAsInt(GENERATE_UUIDS);
	}

	int generateVersionNumbers() {
		return _config.getAsInt(GENERATE_VERSION_NUMBERS);
	}

	boolean internStrings() {
		return _config.getAsBoolean(INTERN_STRINGS);
	}
	
	void isServer(boolean flag){
		_config.put(IS_SERVER,flag);
	}

	boolean isServer() {
		return _config.getAsBoolean(IS_SERVER);
	}

	boolean lockFile() {
		return _config.getAsBoolean(LOCK_FILE);
	}

	int messageLevel() {
		return _config.getAsInt(MESSAGE_LEVEL);
	}

	MessageRecipient messageRecipient() {
		return (MessageRecipient)_config.get(MESSAGE_RECIPIENT);
	}

	boolean optimizeNQ() {
		return _config.getAsBoolean(OPTIMIZE_NQ);
	}

	String password() {
		return _config.getAsString(PASSWORD);
	}

	Hashtable4 readAs() {
		return (Hashtable4)_config.get(READ_AS);
	}

	boolean isReadOnly() {
		return _config.getAsBoolean(READ_ONLY);
	}

	Collection4 rename() {
		return (Collection4)_config.get(RENAME);
	}

	int reservedStorageSpace() {
		return _config.getAsInt(RESERVED_STORAGE_SPACE);
	}

	boolean singleThreadedClient() {
		return _config.getAsBoolean(SINGLE_THREADED_CLIENT);
	}

	boolean testConstructors() {
		return _config.getAsBoolean(TEST_CONSTRUCTORS);
	}

	int timeoutClientSocket() {
		return _config.getAsInt(TIMEOUT_CLIENT_SOCKET);
	}

	int timeoutPingClients() {
		return _config.getAsInt(TIMEOUT_PING_CLIENTS);
	}

	int timeoutServerSocket() {
		return _config.getAsInt(TIMEOUT_SERVER_SOCKET);
	}

	int updateDepth() {
		return _config.getAsInt(UPDATE_DEPTH);
	}

	int weakReferenceCollectionInterval() {
		return _config.getAsInt(WEAK_REFERENCE_COLLECTION_INTERVAL);
	}

	boolean weakReferences() {
		return _config.getAsBoolean(WEAK_REFERENCES);
	}

	IoAdapter ioAdapter() {
		return (IoAdapter)_config.get(IOADAPTER);
	}
}