/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import com.db4o.*;
import com.db4o.query.*;


/**
 * 
 */
public class CreateIndexInherited {

    public int i_int;
    
    public CreateIndexInherited(){
    }
    
    public CreateIndexInherited(int a_int){
        i_int = a_int;
    }
    
    public void store() {
        Test.deleteAllInstances(this);

        Test.store(new CreateIndexFor("a"));
        Test.store(new CreateIndexFor("c"));
        Test.store(new CreateIndexFor("b"));
        Test.store(new CreateIndexFor("f"));
        Test.store(new CreateIndexFor("e"));

        Test.store(new CreateIndexFor(1));
        Test.store(new CreateIndexFor(5));
        Test.store(new CreateIndexFor(7));
        Test.store(new CreateIndexFor(3));
        Test.store(new CreateIndexFor(2));
        Test.store(new CreateIndexFor(3));
        
        
        Db4o.configure().objectClass(this).objectField("i_int").indexed(true);
        Db4o.configure().objectClass(CreateIndexFor.class).objectField("i_name").indexed(true);
        
        Test.reOpen();

        tQueryB();
        tQueryInts(5);
    }

    public void test() {
        Test.store(new CreateIndexFor("d"));
        tQueryB();
        tUpdateB();
        Test.store(new CreateIndexFor("z"));
        Test.store(new CreateIndexFor("y"));
        Test.reOpen();
        tQueryB();

        tQueryInts(8);
    }

    private void tQueryInts(int expectedZeroSize) {
        
        Query q = Test.query();
        q.constrain(CreateIndexFor.class);
        q.descend("i_int").constrain(new Integer(0));
        int zeroSize = q.execute().size();
        Test.ensure(zeroSize == expectedZeroSize);
        
        q = Test.query();
        q.constrain(CreateIndexFor.class);
        q.descend("i_int").constrain(new Integer(4)).greater().equal();
        tExpectInts(q, new int[] { 5, 7 });
         
        q = Test.query();
        q.constrain(CreateIndexFor.class);
        q.descend("i_int").constrain(new Integer(4)).greater();
        tExpectInts(q, new int[] { 5, 7 });

        q = Test.query();
        q.constrain(CreateIndexFor.class);
        q.descend("i_int").constrain(new Integer(3)).greater();
        tExpectInts(q, new int[] { 5, 7 });

        q = Test.query();
        q.constrain(CreateIndexFor.class);
        q.descend("i_int").constrain(new Integer(3)).greater().equal();
        tExpectInts(q, new int[] { 3, 3, 5, 7 });

        q = Test.query();
        q.constrain(CreateIndexFor.class);
        q.descend("i_int").constrain(new Integer(2)).greater().equal();
        tExpectInts(q, new int[] { 2, 3, 3, 5, 7 });
        q = Test.query();

        q = Test.query();
        q.constrain(CreateIndexFor.class);
        q.descend("i_int").constrain(new Integer(2)).greater();
        tExpectInts(q, new int[] { 3, 3, 5, 7 });

        q = Test.query();
        q.constrain(CreateIndexFor.class);
        q.descend("i_int").constrain(new Integer(1)).greater().equal();
        tExpectInts(q, new int[] { 1, 2, 3, 3, 5, 7 });

        q = Test.query();
        q.constrain(CreateIndexFor.class);
        q.descend("i_int").constrain(new Integer(1)).greater();
        tExpectInts(q, new int[] { 2, 3, 3, 5, 7 });

        q = Test.query();
        q.constrain(CreateIndexFor.class);
        q.descend("i_int").constrain(new Integer(4)).smaller();
        tExpectInts(q, new int[] { 1, 2, 3, 3 }, zeroSize);

        q = Test.query();
        q.constrain(CreateIndexFor.class);
        q.descend("i_int").constrain(new Integer(4)).smaller().equal();
        tExpectInts(q, new int[] { 1, 2, 3, 3 }, zeroSize);

        q = Test.query();
        q.constrain(CreateIndexFor.class);
        q.descend("i_int").constrain(new Integer(3)).smaller();
        tExpectInts(q, new int[] { 1, 2 }, zeroSize);

        q = Test.query();
        q.constrain(CreateIndexFor.class);
        q.descend("i_int").constrain(new Integer(3)).smaller().equal();
        tExpectInts(q, new int[] { 1, 2, 3, 3 }, zeroSize);

        q = Test.query();
        q.constrain(CreateIndexFor.class);
        q.descend("i_int").constrain(new Integer(2)).smaller().equal();
        tExpectInts(q, new int[] { 1, 2 }, zeroSize);
        q = Test.query();

        q = Test.query();
        q.constrain(CreateIndexFor.class);
        q.descend("i_int").constrain(new Integer(2)).smaller();
        tExpectInts(q, new int[] { 1 }, zeroSize);

        q = Test.query();
        q.constrain(CreateIndexFor.class);
        q.descend("i_int").constrain(new Integer(1)).smaller().equal();
        tExpectInts(q, new int[] { 1 }, zeroSize);

        q = Test.query();
        q.constrain(CreateIndexFor.class);
        q.descend("i_int").constrain(new Integer(1)).smaller();
        tExpectInts(q, new int[] {
        }, zeroSize);

    }

    private void tExpectInts(Query q, int[] ints, int zeroSize) {
        ObjectSet res = q.execute();
        Test.ensure(res.size() == (ints.length + zeroSize));
        while (res.hasNext()) {
            CreateIndexFor ci = (CreateIndexFor)res.next();
            for (int i = 0; i < ints.length; i++) {
                if (ints[i] == ci.i_int) {
                    ints[i] = 0;
                    break;
                }
            }
        }
        for (int i = 0; i < ints.length; i++) {
            Test.ensure(ints[i] == 0);
        }
    }

    private void tExpectInts(Query q, int[] ints) {
        tExpectInts(q, ints, 0);
    }

    private void tQueryB() {
        ObjectSet res = query("b");
        Test.ensure(res.size() == 1);
        CreateIndexFor ci = (CreateIndexFor)res.next();
        Test.ensure(ci.i_name.equals("b"));
    }

    private void tUpdateB() {
        ObjectSet res = query("b");
        CreateIndexFor ci = (CreateIndexFor)res.next();
        ci.i_name = "j";
        Test.objectContainer().store(ci);
        res = query("b");
        Test.ensure(res.size() == 0);
        res = query("j");
        Test.ensure(res.size() == 1);
        ci.i_name = "b";
        Test.objectContainer().store(ci);
        tQueryB();
    }

    private ObjectSet query(String n) {
        Query q = Test.query();
        q.constrain(CreateIndexFor.class);
        q.descend("i_name").constrain(n);
        return q.execute();
    }



    
    public static class CreateIndexFor extends CreateIndexInherited{
        
        public String i_name;

        public CreateIndexFor() {
        }

        public CreateIndexFor(String name) {
            this.i_name = name;
        }

        public CreateIndexFor(int a_int) {
            super(a_int);
        }

    }


}
