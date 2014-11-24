/* Copyright (C) 2009   Versant Inc.   http://www.db4o.com */

package com.db4o.foundation;

public interface Environment {

	<T> T provide(Class<T> service);

}
