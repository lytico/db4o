/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.legacy.soda.ordered;

import com.db4o.query.*;
import com.db4o.test.legacy.soda.*;

public class STOString implements STClass {

    public static transient SodaTest st;

    String foo;

    public STOString() {
    }

    public STOString(String str) {
        this.foo = str;
    }

    public Object[] store() {
        return new Object[] {
            new STOString(null),
            new STOString("bbb"),
            new STOString("bbb"),
            new STOString("dod"),
            new STOString("aaa"),
            new STOString("Xbb"),
            new STOString("bbq")};
    }

    public void testAscending() {
        Query q = st.query();
        q.constrain(STOString.class);
        q.descend("foo").orderAscending();
        Object[] r = store();
        st.expectOrdered(q, new Object[] { r[0], r[5], r[4], r[1], r[2], r[6], r[3], });
    }

    public void testDescending() {
        Query q = st.query();
        q.constrain(STOString.class);
        q.descend("foo").orderDescending();
        Object[] r = store();
        st.expectOrdered(q, new Object[] { r[3], r[6], r[2], r[1], r[4], r[5], r[0] });
    }

    public void testAscendingLike() {
        Query q = st.query();
        q.constrain(STOString.class);
        Query qStr = q.descend("foo");
        qStr.constrain("b").like();
        qStr.orderAscending();
        Object[] r = store();
        st.expectOrdered(q, new Object[] { r[5], r[1], r[2], r[6] });
    }

    public void testDescendingContains() {
        Query q = st.query();
        q.constrain(STOString.class);
        Query qStr = q.descend("foo");
        qStr.constrain("b").contains();
        qStr.orderDescending();
        Object[] r = store();
        st.expectOrdered(q, new Object[] { r[6], r[2], r[1], r[5] });
    }
}
