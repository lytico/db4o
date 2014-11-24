/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import java.util.*;

import com.db4o.*;
import com.db4o.query.*;

public class HashtableModifiedUpdateDepth {

    Hashtable ht;

    public void configure() {
        Db4o.configure().updateDepth(Integer.MAX_VALUE);
    }

    public void storeOne() {
        ht = new Hashtable();
        ht.put("hi", "five");
    }

    public void testOne() {
        Test.ensure(ht.get("hi").equals("five"));
        ht.put("hi", "six");
        Test.store(this);
        Test.reOpen();
        Query q = Test.query();
        q.constrain(this.getClass());
        HashtableModifiedUpdateDepth hmud = 
            (HashtableModifiedUpdateDepth) q.execute().next();
        Test.ensure(hmud.ht.get("hi").equals("six"));
    }
}