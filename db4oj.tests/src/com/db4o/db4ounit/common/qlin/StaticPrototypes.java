/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.qlin;

import com.db4o.db4ounit.common.qlin.BasicQLinTestCase.*;

import static com.db4o.qlin.QLinSupport.*;

/**
 * @sharpen.if !SILVERLIGHT
 */
@decaf.Remove(decaf.Platform.JDK11)
public class StaticPrototypes {
	
	static Cat cat = prototype(Cat.class); 
	static Dog dog = prototype(Dog.class);

}
