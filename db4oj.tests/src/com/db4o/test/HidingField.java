/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import com.db4o.*;
import com.db4o.query.*;

public class HidingField {

    public String name;

    public void store() {
        ExtendHidingField ehf = new ExtendHidingField();
        ehf.name = "child";
        ehf.setParentName("parent");
        Test.store(ehf);
    }

    public void test() {
        Query q = Test.query();
        q.constrain(ExtendHidingField.class);
        q.descend("name").constrain("child");
        ObjectSet objectSet = q.execute();
        System.out.println(objectSet.size());
        while (objectSet.hasNext()) {
            System.out.println(objectSet.next());
        }

        q = Test.query();
        q.constrain(ExtendHidingField.class);
        q.constrain(new Evaluation() {

            public void evaluate(Candidate candidate) {
                ExtendHidingField ehf = (ExtendHidingField) candidate
                        .getObject();
                candidate.include("child".equals(ehf.name));
            }
        });
        objectSet = q.execute();
        System.out.println(objectSet.size());
        while (objectSet.hasNext()) {
            System.out.println(objectSet.next());
        }
    }

    public void setParentName(String name) {
        this.name = name;
    }

    public String toString() {
        return "HidingField " + name;
    }

    public static class ExtendHidingField extends HidingField {

        public String name;

        public String toString() {
            return super.toString() + " ExtendHidingField " + name;
        }
    }
}
