/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.test.pending;


import java.util.*;


/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class SimpleNode {
    private String name;
    private List children;
    
    public SimpleNode(String name,SimpleNode[] children) {
        this.name=name;
        this.children=new ArrayList();
        if(children!=null) {
	        for (int chidx=0;chidx<children.length;chidx++) {
	            this.children.add(children[chidx]);
	        }
        }
    }
}

