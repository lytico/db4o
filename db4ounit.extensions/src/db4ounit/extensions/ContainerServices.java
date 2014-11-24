/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package db4ounit.extensions;

import com.db4o.*;

public class ContainerServices {

	public static void withContainer(final ObjectContainer container, final ContainerBlock block) throws Throwable {
		try {
			block.run(container);			
		} finally {
			container.close();
		}
	}
}
