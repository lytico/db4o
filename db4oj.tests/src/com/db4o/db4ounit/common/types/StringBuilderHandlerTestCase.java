package com.db4o.db4ounit.common.types;

import com.db4o.config.*;
import com.db4o.typehandlers.*;

import db4ounit.*;
import db4ounit.extensions.*;

/**
 * @sharpen.remove
 */
@decaf.Remove(unlessCompatible=decaf.Platform.JDK15)
public class StringBuilderHandlerTestCase  extends AbstractDb4oTestCase {
	
    public void testCanRead(){
        canRead();
    }
    public void testCanReadAfterDefragment() throws Exception {
        canReadAfterDefragment();
    }

    public void canRead(){
        assertCanReadData();
    }
    
    public void canReadAfterDefragment() throws Exception {
        defragment();
        assertCanReadData();

    }
    
    @Override
    protected void configure(Configuration config) throws Exception {
    	config.registerTypeHandler(new SingleClassTypeHandlerPredicate(StringBuilder.class),
                new StringBuilderHandler());
    }

    private void assertCanReadData() {
        final Holder holder = db().query(Holder.class).get(0);
        Assert.areEqual(holder.getBuilder().toString(), "TestData");
    }

    @Override
    protected void store() throws Exception {
    	db().store(new Holder());
    }

    public static class Holder{
        StringBuilder builder ;

        public Holder(String data) {
            this.builder = new StringBuilder(data);
        }

        public Holder() {
            this("TestData");
        }

        public StringBuilder getBuilder() {
            return builder;
        }

        public void setBuilder(StringBuilder builder) {
            this.builder = builder;
        }
    }

}
