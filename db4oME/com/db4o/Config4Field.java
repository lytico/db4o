/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

import com.db4o.config.*;
import com.db4o.foundation.*;
import com.db4o.reflect.*;

class Config4Field extends Config4Abstract implements ObjectField, DeepClone {
    
	private final static KeySpec CLASS=new KeySpec(null);
    
	private final static KeySpec FIELD_REFLECTOR=new KeySpec(null);
    
	private final static KeySpec QUERY_EVALUATION=new KeySpec(true);
    
	private final static KeySpec INDEXED=new KeySpec(YapConst.DEFAULT);
    
	private final static KeySpec METAFIELD=new KeySpec(null);
    
	private final static KeySpec INITIALIZED=new KeySpec(false);

	protected Config4Field(KeySpecHashtable4 config) {
		super(config);
	}
	
    Config4Field(Config4Class a_class, String a_name) {
        _config.put(CLASS, a_class);
        setName(a_name);
    }

    private Config4Class classConfig() {
    	return (Config4Class)_config.get(CLASS);
    }
    
    String className() {
        return classConfig().getName();
    }

    public Object deepClone(Object param) {
        return new Config4Field(_config);
    }

    private ReflectField fieldReflector() {
    	ReflectField fieldReflector=(ReflectField)_config.get(FIELD_REFLECTOR);
        if (fieldReflector == null) {
            try {
                fieldReflector = classConfig().classReflector().getDeclaredField(getName());
                fieldReflector.setAccessible();
                _config.put(FIELD_REFLECTOR, fieldReflector);
            } catch (Exception e) {
            }
        }
        return fieldReflector;
    }

    public void queryEvaluation(boolean flag) {
    	_config.put(QUERY_EVALUATION, flag);
    }

    public void rename(String newName) {
        classConfig().config().rename(new Rename(className(), getName(), newName));
        setName(newName);
    }

    public void indexed(boolean flag) {
    	putThreeValued(INDEXED, flag);
    }

    public void initOnUp(Transaction systemTrans, YapField yapField) {
        if (!_config.getAsBoolean(INITIALIZED)) {
            YapStream anyStream = systemTrans.i_stream;
            if(Tuning.fieldIndices){
	            if (anyStream.maintainsIndices()) {
	                if(Debug.indexAllFields){
	                    indexed(true);
	                }
	                if (! yapField.supportsIndex()) {
	                    indexed(false);
	                }
	                
	                boolean indexInitCalled = false;
	                
	            	YapFile stream = (YapFile)anyStream;
	                MetaField metaField = classConfig().metaClass().ensureField(systemTrans, getName());
	                _config.put(METAFIELD, metaField);
	                int indexedFlag=_config.getAsInt(INDEXED);
	                if (indexedFlag == YapConst.YES) {
	                    if (metaField.index == null) {
	                        metaField.index = new MetaIndex();
	                        stream.setInternal(systemTrans, metaField.index, YapConst.UNSPECIFIED, false);
	                        stream.setInternal(systemTrans, metaField, YapConst.UNSPECIFIED, false);
	                        yapField.initIndex(systemTrans, metaField.index);
	                        indexInitCalled = true;
	        				if (stream.i_config.messageLevel() > YapConst.NONE) {
	        					stream.message("creating index " + yapField.toString());
	        				}
	        				YapClass yapClassField = yapField.getParentYapClass();
	        				long[] ids = yapClassField.getIDs();
	        				for (int i = 0; i < ids.length; i++) {
                                YapWriter writer = stream.readWriterByID(systemTrans, (int)ids[i]);
                                if(writer != null){
                                    Object obj = null;
                                    YapClass yapClassObject = YapClassAny.readYapClass(writer);
                                    if(yapClassObject != null){
	                                    if(yapClassObject.findOffset(writer, yapField)){
                                            try {
                                                obj = yapField.i_handler.readIndexValueOrID(writer);
                                            } catch (CorruptionException e) {
                                                if(Deploy.debug || Debug.atHome){
                                                    e.printStackTrace();
                                                }
                                            }
	                                    }
                                    }
                                    yapField.addIndexEntry(systemTrans, (int)ids[i], obj);
                                }else{
                                    if(Deploy.debug){
                                        throw new RuntimeException("Unexpected null object for ID");
                                    }
                                }
                            }
	        				if(ids.length > 0){
	        				    systemTrans.commit();
	        				}
	                    }
	                }
	                if (indexedFlag == YapConst.NO) {
	                    if (metaField.index != null) {
	        				if (stream.i_config.messageLevel() > YapConst.NONE) {
	        					stream.message("dropping index " + yapField.toString());
	        				}
	                        MetaIndex mi = metaField.index;
	                        if (mi.indexLength > 0) {
	                            stream.free(mi.indexAddress, mi.indexLength);
	                        }
	                        if (mi.patchLength > 0) {
	                            stream.free(mi.patchAddress, mi.patchLength);
	                        }
	                        stream.delete1(systemTrans, mi, false);
	                        metaField.index = null;
	                        stream.setInternal(systemTrans, metaField, YapConst.UNSPECIFIED, false);
	                    }
	                }
	                if (metaField.index != null) {
	                    if(! indexInitCalled){
	                        yapField.initIndex(systemTrans, metaField.index);
	                    }
	                }
	            }
            }
            _config.put(INITIALIZED, true);
        }
    }

	boolean queryEvaluation() {
		return _config.getAsBoolean(QUERY_EVALUATION);
	}


}
