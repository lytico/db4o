/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import java.util.*;

import com.db4o.*;
import com.db4o.query.*;

/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class SelectDistinct {

    public String name;

    public SelectDistinct() {
    }

    public SelectDistinct(String name) {
        this.name = name;
    }

    public void store() {
        Test.store(new SelectDistinct("a"));
        Test.store(new SelectDistinct("a"));
        Test.store(new SelectDistinct("a"));
        Test.store(new SelectDistinct("b"));
        Test.store(new SelectDistinct("b"));
        Test.store(new SelectDistinct("c"));
        Test.store(new SelectDistinct("c"));
        Test.store(new SelectDistinct("d"));
        Test.store(new SelectDistinct("e"));
    }

    public void test() {

        String[] expected = new String[] { "a", "b", "c", "d", "e"};

        Query q = Test.query();
        q.constrain(SelectDistinct.class);
        q.constrain(new Evaluation() {

            private Hashtable ht = new Hashtable();

            public void evaluate(Candidate candidate) {
                SelectDistinct sd = (SelectDistinct) candidate.getObject();
                boolean isDistinct = ht.get(sd.name) == null;
                candidate.include(isDistinct);
                if (isDistinct) {
                    ht.put(sd.name, new Object());
                }

            }
        });

        ObjectSet objectSet = q.execute();
        while (objectSet.hasNext()) {
            SelectDistinct sd = (SelectDistinct) objectSet.next();
            boolean found = false;
            for (int i = 0; i < expected.length; i++) {
                if (sd.name.equals(expected[i])) {
                    expected[i] = null;
                    found = true;
                    break;
                }
            }
            Test.ensure(found);
        }

        for (int i = 0; i < expected.length; i++) {
            Test.ensure(expected[i] == null);
        }
    }
}