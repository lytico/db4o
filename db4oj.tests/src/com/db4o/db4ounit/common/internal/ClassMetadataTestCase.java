/* Copyright (C) 2009  Versant Corp.  http://www.db4o.com */

package com.db4o.db4ounit.common.internal;

import com.db4o.internal.*;

import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;

public class ClassMetadataTestCase extends AbstractDb4oTestCase implements OptOutMultiSession{
	
	public static class Item{
		
	}
	
	public void testDropClassIndex(){
		Item item = new Item();
		store(item);
		assertOccurrences(Item.class, 1);
		ClassMetadata classMetadata = container().classMetadataForObject(item);
		classMetadata.dropClassIndex();
		assertOccurrences(Item.class, 0);
	}


}
