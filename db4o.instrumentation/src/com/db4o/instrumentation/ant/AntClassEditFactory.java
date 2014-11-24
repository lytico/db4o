package com.db4o.instrumentation.ant;

import com.db4o.instrumentation.core.*;


/**
 * @exclude
 */
public interface AntClassEditFactory {
	BloatClassEdit createEdit(ClassFilter clazzFilter);
}
