/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */

package com.db4o.collections;

import java.util.*;

import com.db4o.ta.*;

/**
 * extends Map with Transparent Activation and
 * Transparent Persistence support
 * @sharpen.ignore
 * @since 7.9
 */
@decaf.Remove(decaf.Platform.JDK11)
public interface ActivatableMap <K,V> extends Map<K,V>, Activatable{

}
