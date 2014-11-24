package com.db4o.db4ounit.common.ta.ta;

import java.util.*;

import com.db4o.activation.*;
import com.db4o.db4ounit.common.ta.*;

public class TADateItem extends ActivatableImpl {

    public static final long DAY = 1000 * 60 * 60 * 24;

    public Date _typed;

    public Object _untyped;

    public Date getTyped() {
        activate(ActivationPurpose.READ);
        return _typed;
    }

    public Object getUntyped() {
        activate(ActivationPurpose.READ);
        return _untyped;
    }

    public String toString() {
        activate(ActivationPurpose.READ);
        return _typed.toString();
    }
}
