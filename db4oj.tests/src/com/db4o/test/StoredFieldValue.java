/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import com.db4o.ext.*;


public class StoredFieldValue {
    
    public String foo;
    public int bar;
    public Atom[] atoms;
    
    public void storeOne(){
        foo = "foo";
        bar = 10;
        atoms = new Atom[2];
        atoms[0] = new Atom("one");
        atoms[1] = new Atom("two");
    }
    
    public void testOne(){
        ExtObjectContainer oc = Test.objectContainer();
        StoredClass sc = oc.storedClass(this);
        StoredField[] sf = sc.getStoredFields();
        boolean[] cases = new boolean[3];
        for (int i = 0; i < sf.length; i++) {
            StoredField f = sf[i];
            if(f.getName().equals("foo")){
                Test.ensure(f.get(this).equals("foo"));
                Test.ensure(f.getStoredType().getName().equals(String.class.getName()));
                cases[0] = true;
            }
            if(f.getName().equals("bar")){
                Test.ensure(f.get(this).equals(new Integer(10)));
                Test.ensure(f.getStoredType().getName().equals(int.class.getName()));
                cases[1] = true;
            }
            if(f.getName().equals("atoms")){
                Test.ensure(f.getStoredType().getName().equals(Atom.class.getName()));
                Test.ensure(f.isArray());
                Atom[] at = (Atom[])f.get(this);
                Test.ensure(at[0].name.equals("one"));
                Test.ensure(at[1].name.equals("two"));
                cases[2] = true;
            }
        }
        for (int i = 0; i < cases.length; i++) {
            Test.ensure(cases[i]);
        }
    }
}
