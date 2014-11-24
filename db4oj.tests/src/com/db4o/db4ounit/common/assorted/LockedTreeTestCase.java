/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.assorted;

import com.db4o.foundation.*;
import com.db4o.internal.*;

import db4ounit.*;
import db4ounit.extensions.*;


public class LockedTreeTestCase extends AbstractDb4oTestCase{

    public static void main(String[] args) {
        new LockedTreeTestCase().runSolo();
    }
    
    public void testAdd(){
        final LockedTree lockedTree = new LockedTree();
        lockedTree.add(new TreeInt(1));
        Assert.expect(IllegalStateException.class, new CodeBlock() {
            public void run() throws Throwable {
                lockedTree.traverseLocked(new Visitor4() {
                    public void visit(Object obj) {
                        TreeInt treeInt = (TreeInt) obj;
                        if(treeInt._key == 1){
                            lockedTree.add(new TreeInt(2));
                        }
                    }
                });
            }
        });
    }
    
    public void testClear(){
        final LockedTree lockedTree = new LockedTree();
        lockedTree.add(new TreeInt(1));
        Assert.expect(IllegalStateException.class, new CodeBlock() {
            public void run() throws Throwable {
                lockedTree.traverseLocked(new Visitor4() {
                    public void visit(Object obj) {
                        lockedTree.clear();
                    }
                });
            }
        });
    }


}
