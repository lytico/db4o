/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

import com.db4o.query.*;

/**
 * QQuery is the users hook on our graph.
 * 
 * A QQuery is defined by it's constraints.
 * 
 * @exclude
 */
public class QQuery extends QQueryBase implements Query {
    QQuery(Transaction a_trans, QQuery a_parent, String a_field) {
    	super(a_trans,a_parent,a_field);
    }

    public QQuery() {
        // C/S only
    }
}
