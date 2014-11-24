/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import java.io.*;

import com.db4o.*;
import com.db4o.query.*;


public class Backup {
    
    static int allAtomCount;
    static int specialAtomCount;
    
    static final String FILE = "backuptest.db4o";
    static final String NAME = "backuptest";
    
    public void store(){
        if(! Test.isClientServer()){
	        new File(FILE).delete();
	        Test.store(new Atom(NAME));
	        Test.commit();
	        
	        Query q = Test.query();
	        q.constrain(Atom.class);
	        allAtomCount = q.execute().size();
	        q = Test.query();
	        q.constrain(Atom.class);
	        q.descend("name").constrain(NAME);
	        specialAtomCount = q.execute().size();
	        Test.objectContainer().ext().backup(FILE);
        }
    }
    
    public void test(){
        if(! Test.isClientServer()){
	        ObjectContainer objectContainer = Db4o.openFile(FILE);
	        Query q = objectContainer.query();
	        q.constrain(Atom.class);
	        Test.ensure(allAtomCount == q.execute().size());
	        q = Test.query();
	        q.constrain(Atom.class);
	        q.descend("name").constrain(NAME);
	        ObjectSet objectSet = q.execute();
	        Test.ensure(objectSet.size() == specialAtomCount);
	        Atom atom = (Atom)objectSet.next();
	        Test.ensure(atom.name.equals(NAME));
	        objectContainer.close();
        }
    }
    
    
}
