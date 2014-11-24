/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.legacy;

import com.db4o.*;
import com.db4o.test.*;

public class PersistStaticFieldValues {
    
    public static final PsfvHelper ONE = new PsfvHelper();
    public static final PsfvHelper TWO = new PsfvHelper();
    public static final PsfvHelper THREE = new PsfvHelper();
    
    public PsfvHelper one;
    public PsfvHelper two;
    public PsfvHelper three;
    

    public void configure() {
        Db4o
            .configure()
            .objectClass(PersistStaticFieldValues.class)
            .persistStaticFieldValues();
    }
    
    public void store(){
        Test.deleteAllInstances(this);
        PersistStaticFieldValues psfv = new PersistStaticFieldValues();
        psfv.one = ONE;
        psfv.two = TWO;
        psfv.three = THREE; 
        Test.store(psfv);
    }
    
    public void test(){
        PersistStaticFieldValues psfv = (PersistStaticFieldValues)Test.getOne(this);
        Test.ensure(psfv.one == ONE);
        Test.ensure(psfv.two == TWO);
        Test.ensure(psfv.three == THREE);
    }
    
    public static class PsfvHelper{
        
    }
    

}
