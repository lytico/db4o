/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.config;

import com.db4o.internal.*;
import com.db4o.internal.ids.*;

/**
 * Factory interface to create a custom IdSystem.
 * @see IdSystemConfiguration#useCustomSystem(IdSystemFactory) 
 */
public interface IdSystemFactory {
	
	/**
	 * creates 
	 * @param container
	 * @return
	 */
	public IdSystem newInstance(LocalObjectContainer container);

}
