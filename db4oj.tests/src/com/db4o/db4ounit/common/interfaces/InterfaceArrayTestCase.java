package com.db4o.db4ounit.common.interfaces;

import db4ounit.*;
import db4ounit.extensions.*;

public class InterfaceArrayTestCase extends AbstractDb4oTestCase {
        
        public interface Foo {
        }
        
        public static class FooImpl implements Foo {
        }
        
        public static class Bar {
                public Bar(Foo... foos) {
                        this.foos = foos; 
                }

                public Foo[] foos;
        }
        
        @Override
        protected void store() throws Exception {
                store(new Bar(new FooImpl()));
        }
        
        public void test() {
                Assert.areEqual(1, retrieveOnlyInstance(Bar.class).foos.length);
        }

}