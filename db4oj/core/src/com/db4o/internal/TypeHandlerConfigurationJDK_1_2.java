/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal;

import java.util.*;

import com.db4o.collections.*;
import com.db4o.internal.collections.*;
import com.db4o.reflect.*;
import com.db4o.typehandlers.*;
import com.db4o.typehandlers.internal.*;

/**
 * @exclude
 * @sharpen.ignore
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class TypeHandlerConfigurationJDK_1_2 extends TypeHandlerConfiguration{
	
	public TypeHandlerConfigurationJDK_1_2(Config4Impl config) {
		super(config);
        listTypeHandler(new CollectionTypeHandler());
        mapTypeHandler(new MapTypeHandler());
	}

	public void apply(){
        registerCollection(AbstractCollection.class);
		ignoreFieldsOn(AbstractList.class);
		ignoreFieldsOn(AbstractSequentialList.class);
		ignoreFieldsOn(LinkedList.class);
		ignoreFieldsOn(ArrayList.class);
		ignoreFieldsOn(Vector.class);
		ignoreFieldsOn(Stack.class);
		ignoreFieldsOn(AbstractSet.class);
		ignoreFieldsOn(HashSet.class);
		
		registerMap(AbstractMap.class);
		registerMap(Hashtable.class);
		
		ignoreFieldsOn(HashMap.class);
		ignoreFieldsOn(WeakHashMap.class);
		
		registerTypeHandlerFor(BigSet.class, new BigSetTypeHandler());
		registerTypeHandlerFor(TreeSet.class, new TreeSetTypeHandler() {
			@Override
			protected TreeSet create(Comparator comparator) {
				return new TreeSet(comparator);
			}
		});
		registerTypeHandlerFor(ActivatableTreeSet.class, new TreeSetTypeHandler() {
			@Override
			protected TreeSet create(Comparator comparator) {
				return new ActivatableTreeSet(comparator);
			}
		});
		registerTypeHandlerFor("java.util.Collections$UnmodifiableRandomAccessList", new UnmodifiableListTypeHandler());
		ignoreFieldsOn("java.util.Collections$UnmodifiableList");
		ignoreFieldsOn("java.util.Collections$UnmodifiableCollection");
	}

}
