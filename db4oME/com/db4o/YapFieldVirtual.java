/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

import com.db4o.foundation.*;
import com.db4o.inside.ix.*;
import com.db4o.inside.replication.*;

/**
 * 
 */
abstract class YapFieldVirtual extends YapField {

    YapFieldVirtual() {
        super(null);
    }
    
    void addFieldIndex(YapWriter a_writer, boolean a_new) {
        a_writer.incrementOffset(linkLength());
    }
    
    public void appendEmbedded2(YapWriter a_bytes) {
        a_bytes.incrementOffset(linkLength());
    }
    
    public boolean alive() {
        return true;
    }
    
    boolean canAddToQuery(String fieldName){
        return fieldName.equals(getName()); 
    }
    
    void collectConstraints(Transaction a_trans, QConObject a_parent,
        Object a_template, Visitor4 a_visitor) {
        
        // QBE constraint collection call
        // There isn't anything useful to do here, since virtual fields
        // are not on the actual object.
        
    }
    
    void deactivate(Transaction a_trans, Object a_onObject, int a_depth) {
        // do nothing
    }
    
    abstract void delete(YapWriter a_bytes, boolean isUpdate);
    
    Object getOrCreate(Transaction a_trans, Object a_OnObject) {
        // This is the first part of marshalling
        // Virtual fields do it all in #marshall() so it's fine to return null here
        return null;
    }

    int ownLength(YapStream a_stream) {
        return a_stream.stringIO().shortLength(i_name);
    }

    void initIndex(YapStream a_stream, MetaIndex a_metaIndex) {
        if (i_index == null) {
            i_index = new Index4(a_stream.getSystemTransaction(), getHandler(),a_metaIndex, false);
        }
    }

    void instantiate(YapObject a_yapObject, Object a_onObject, YapWriter a_bytes)
        throws CorruptionException {
        if (a_yapObject.i_virtualAttributes == null) {
            a_yapObject.i_virtualAttributes = new VirtualAttributes();
        }
        instantiate1(a_bytes.getTransaction(), a_yapObject, a_bytes);
    }

    abstract void instantiate1(Transaction a_trans, YapObject a_yapObject, YapReader a_bytes);
    
    void loadHandler(YapStream a_stream){
    	// do nothing
    }

    void marshall(YapObject a_yapObject, Object a_object, YapWriter a_bytes,
        Config4Class a_config, boolean a_new) {
        Transaction trans = a_bytes.i_trans;
        
        if(! trans.supportsVirtualFields()){
            marshallIgnore(a_bytes);
            return;
        }
        
        YapStream stream = trans.i_stream;
        YapHandlers handlers = stream.i_handlers;
        boolean migrating = false;
        
        // old replication code 
        
        if (stream._replicationCallState != YapConst.NONE) {
            if (stream._replicationCallState == YapConst.OLD) {
                migrating = true;
                if (a_yapObject.i_virtualAttributes == null) {
                    Object obj = a_yapObject.getObject();
                    YapObject migrateYapObject = null;
                    MigrationConnection mgc = handlers.i_migration;
                    if(mgc != null){
                        migrateYapObject = mgc.referenceFor(obj);
                        if(migrateYapObject == null){
                            migrateYapObject = mgc.peer(stream).getYapObject(obj);
                        }
                    }
                    if (migrateYapObject != null
                        && migrateYapObject.i_virtualAttributes != null
                        && migrateYapObject.i_virtualAttributes.i_database != null) {
                        migrating = true;
                        a_yapObject.i_virtualAttributes = (VirtualAttributes)migrateYapObject.i_virtualAttributes
                            .shallowClone();
                        if(migrateYapObject.i_virtualAttributes.i_database != null){
                            migrateYapObject.i_virtualAttributes.i_database.bind(trans);
                        }
                    }
                }
            }else {
                Db4oReplicationReferenceProvider provider = handlers._replicationReferenceProvider;
                Object parentObject = a_yapObject.getObject();
                Db4oReplicationReference ref = provider.referenceFor(parentObject); 
                if(ref != null){
                    migrating = true;
                    if(a_yapObject.i_virtualAttributes == null){
                        a_yapObject.i_virtualAttributes = new VirtualAttributes();
                    }
                    VirtualAttributes va = a_yapObject.i_virtualAttributes;
                    va.i_version = ref.version();
                    va.i_uuid = ref.longPart();
                    va.i_database = ref.signaturePart();
                }
            }
        }
        
        if (a_yapObject.i_virtualAttributes == null) {
            a_yapObject.i_virtualAttributes = new VirtualAttributes();
            migrating = false;
        }
	    marshall1(a_yapObject, a_bytes, migrating, a_new);
    }

    abstract void marshall1(YapObject a_yapObject, YapWriter a_bytes,
        boolean a_migrating, boolean a_new);
    
    abstract void marshallIgnore(YapWriter writer);
    
    public void readVirtualAttribute(Transaction a_trans, YapReader a_reader, YapObject a_yapObject) {
        if(! a_trans.supportsVirtualFields()){
            a_reader.incrementOffset(linkLength());
            return;
        }
        instantiate1(a_trans, a_yapObject, a_reader);
    }
    
    void writeThis(Transaction trans, YapReader a_writer, YapClass a_onClass) {
        a_writer.writeShortString(trans,i_name);
    }
}