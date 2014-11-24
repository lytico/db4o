package com.db4o.db4ounit.common.soda;

import com.db4o.ObjectSet;
import com.db4o.query.Query;
import db4ounit.Assert;
import db4ounit.extensions.AbstractDb4oTestCase;

public class SortingNotAvailableField extends AbstractDb4oTestCase{

    public static void main(String[] args) {
        new SortingNotAvailableField().runSolo();
    }
    @Override
    protected void store() throws Exception {
        super.store();
        db().store(new OrderedItem());
        db().store(new OrderedItem());
    }

    public void testOrderWithRightFieldName(){
        final Query query = db().query();
        query.constrain(OrderedItem.class);
        query.descend("myOrder").orderAscending();

        final ObjectSet<Object> result = query.execute();
        Assert.areEqual(2,result.size());
    }

    public void testOrderWithWrongFieldName(){
        final Query query = db().query();
        query.constrain(OrderedItem.class);
        query.descend("myorder").orderAscending();

        final ObjectSet<Object> result = query.execute();
        Assert.areEqual(2,result.size());
    }
    
    public static class OrderedItem {
        public int myOrder = 42;
    }
}


