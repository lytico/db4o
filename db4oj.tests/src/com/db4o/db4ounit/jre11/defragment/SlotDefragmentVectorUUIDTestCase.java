/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.jre11.defragment;

import java.io.*;
import java.util.*;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.db4ounit.common.defragment.*;
import com.db4o.db4ounit.jre11.defragment.SlotDefragmentVectorTestCase.*;
import com.db4o.defragment.*;
import com.db4o.internal.*;
import com.db4o.query.*;

import db4ounit.*;

public class SlotDefragmentVectorUUIDTestCase extends DefragmentTestCaseBase {

    public static class Holder {
        public Vector _vector;

        public Holder(Vector vector) {
            this._vector = vector;
        }
    }
    
    // FIXME runs fine in db4oj suite, but fails in db4ojdk1.2 suite?!?
    public void _testVectorDefragment() throws Exception {
        store();
        defrag();
        query();
    }

    private void query() {
        ObjectContainer db=openDatabase();
        Query query=db.query();
        query.constrain(Holder.class);
        ObjectSet result=query.execute();
        Assert.areEqual(1,result.size());
        db.close();
    }

    private void defrag() throws IOException {
        DefragmentConfig config=new DefragmentConfig(sourceFile());
        config.forceBackupDelete(true);
        config.db4oConfig(configuration());
        Defragment.defrag(config);
    }

    private void store() {
        new File(sourceFile()).delete();
        ObjectContainer db=openDatabase();
        db.store(new Holder(new Vector()));
        db.close();
    }

    private ObjectContainer openDatabase() {
    	EmbeddedConfiguration config = configuration();
        config.file().generateUUIDs(ConfigScope.GLOBALLY);
        return Db4oEmbedded.openFile(config, sourceFile());
    }
    
    private EmbeddedConfiguration configuration() {
    	EmbeddedConfiguration config = newConfiguration();
        config.common().reflectWith(Platform4.reflectorForType(Data.class));
        return config;
    }

}