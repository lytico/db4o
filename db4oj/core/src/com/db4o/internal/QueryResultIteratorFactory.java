/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.internal;

import com.db4o.foundation.*;
import com.db4o.internal.query.result.*;

public interface QueryResultIteratorFactory {
	
	Iterator4 newInstance(AbstractQueryResult result);

}
