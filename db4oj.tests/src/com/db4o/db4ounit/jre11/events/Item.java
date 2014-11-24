/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.jre11.events;

/**
 * User: treeder
 * Date: Aug 16, 2006
 * Time: 3:45:40 PM
 */
public class Item {
    public String id;
    public Item child;

    public Item() {
    }

    public Item(String id_, Item child_) {
        id = id_;
        child = child_;
    }
}
