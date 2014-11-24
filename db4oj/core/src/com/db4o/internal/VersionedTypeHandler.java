/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal;

import com.db4o.foundation.*;
import com.db4o.typehandlers.*;


/**
 * @exclude
 */
public interface VersionedTypeHandler extends TypeHandler4, DeepClone {

    TypeHandler4 unversionedTemplate();

}
