/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */
package com.db4o.omplus.connection;

import java.util.*;

public class SunSPIUtil {

	@SuppressWarnings({ "restriction", "unchecked" })
	public static <T> Iterator<T> retrieveSPIImplementors(Class<T> spi, ClassLoader loader) {
		return sun.misc.Service.providers(spi, loader);
	}
	
	private SunSPIUtil() {
	}	
}
