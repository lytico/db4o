/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.collections;

import java.util.*;

/**
 * extends java.util.Set with Transparent Activation and
 * Transparent Persistence support
 * @since 7.9
 * @sharpen.ignore
 */
@decaf.Remove(decaf.Platform.JDK11)
public interface ActivatableSet<E> extends ActivatableCollection<E>, Set<E> {
}
