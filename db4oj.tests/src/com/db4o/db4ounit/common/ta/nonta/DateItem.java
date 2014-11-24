package com.db4o.db4ounit.common.ta.nonta;

import java.util.*;

public class DateItem {

    public Date _typed;

    public Object _untyped;

    public Date getTyped() {
        return _typed;
    }

    public Object getUntyped() {
        return _untyped;
    }


    public String toString() {
        return _typed.toString();
    }
}
