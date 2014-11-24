package com.db4odoc.db4otests;


import com.db4o.ObjectSet;
import db4ounit.Assert;
import db4ounit.extensions.AbstractDb4oTestCase;
// #example: Basic test case
public class ExampleTestCase extends AbstractDb4oTestCase{

    public static void main(String[] args) {
        new ExampleTestCase().runEmbedded();
    }

    public void testStoresElement(){
        db().store(new TestItem());
        ObjectSet<TestItem> result = db().query(TestItem.class);
        Assert.areEqual(1, result.size());
    }


    static class TestItem{

    }
}
// #end example
