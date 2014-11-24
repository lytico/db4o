/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

import com.db4o.ext.*;
import com.db4o.foundation.*;

/**
 * database boot record. Responsible for ID generation, version generation and
 * holding a reference to the Db4oDatabase object of the ObjectContainer
 *
 * @exclude
 * @persistent
 */
public class PBootRecord extends P1Object implements Db4oTypeImpl, Internal4{

    transient YapFile         i_stream;

    public Db4oDatabase       i_db;

    public long               i_versionGenerator;

    public int                i_generateVersionNumbers;

    public int                i_generateUUIDs;

    private transient boolean i_dirty;

    public MetaIndex          i_uuidMetaIndex;

    private transient TimeStampIdGenerator _versionTimeGenerator;


    public PBootRecord(){
    }

    public int activationDepth() {
        return Integer.MAX_VALUE;
    }

    private void createVersionTimeGenerator(){
        if(_versionTimeGenerator == null){
            _versionTimeGenerator = new TimeStampIdGenerator(i_versionGenerator);
        }
    }

    void init(Config4Impl a_config) {
        i_db = Db4oDatabase.generate();
        i_uuidMetaIndex = new MetaIndex();
        initConfig(a_config);
        i_dirty = true;
    }

    boolean initConfig(Config4Impl a_config) {

        boolean modified = false;

        if(i_generateVersionNumbers != a_config.generateVersionNumbers()){
            i_generateVersionNumbers = a_config.generateVersionNumbers();
            modified = true;
        }

        if(i_generateUUIDs != a_config.generateUUIDs()){
            i_generateUUIDs = a_config.generateUUIDs();
            modified = true;
        }

        return modified;
    }

    MetaIndex getUUIDMetaIndex(){

        // TODO: This is legacy code for old database files.
        // Newer versions create i_uuidMetaIndex when PBootRecord
        // is created. Remove this code after June 2006.
        if (i_uuidMetaIndex == null) {
            i_uuidMetaIndex = new MetaIndex();
            Transaction systemTrans = i_stream.getSystemTransaction();
            i_stream.showInternalClasses(true);
            i_stream.setInternal(systemTrans, this, false);
            i_stream.showInternalClasses(false);
            systemTrans.commit();
        }

        return i_uuidMetaIndex;
    }

    long newUUID() {
        return nextVersion();
    }

    public void raiseVersion(long a_minimumVersion) {
        if (i_versionGenerator < a_minimumVersion) {
            createVersionTimeGenerator();
            _versionTimeGenerator.setMinimumNext(a_minimumVersion);
            i_versionGenerator = a_minimumVersion;
            setDirty();
            store(1);
        }
    }

    public void setDirty(){
        i_dirty = true;
    }

    public void store(int a_depth) {
        if (i_dirty) {
            createVersionTimeGenerator();
            i_versionGenerator = _versionTimeGenerator.generate();
            i_stream.showInternalClasses(true);
            super.store(a_depth);
            i_stream.showInternalClasses(false);
        }
        i_dirty = false;
    }

    long nextVersion() {
        i_dirty = true;
        createVersionTimeGenerator();
        i_versionGenerator = _versionTimeGenerator.generate();
        return i_versionGenerator;
    }

    long currentVersion(){
        return i_versionGenerator;
    }

}