/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.omplus.datalayer;

import java.util.*;

public interface OMEDataStore {

	<T> List<T> getGlobalEntry(String key);

	<T> void setGlobalEntry(String key, List<T> list);

	<T> List<T> getContextLocalEntry(String key);

	<T> void setContextLocalEntry(String key, List<T> list);

	void close();

}