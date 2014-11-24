/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.j2me.bloat;

import java.io.*;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.j2me.bloat.testdata.*;
import com.db4o.reflect.self.*;

public class EnhanceTestMain {
	private static final String FILENAME = "enhanceddog.db4o";

	public static void main(String[] args) throws Exception {
        Class registryClazz=Class.forName("com.db4o.j2me.bloat.testdata.GeneratedDogSelfReflectionRegistry");
        SelfReflectionRegistry registry=(SelfReflectionRegistry)registryClazz.newInstance();
        new File(FILENAME).delete();
        ObjectContainer db=Db4o.openFile(selfReflectorConfig(registry), FILENAME);
        db.store(new Dog("Laika",111,new Dog[]{},new int[]{1,2,3}));
        db.close();
        db=Db4o.openFile(selfReflectorConfig(registry), FILENAME);
        ObjectSet result=db.queryByExample(Dog.class);
        while(result.hasNext()) {
        	System.out.println(result.next());
        }
        db.close();
	}

	private static Configuration selfReflectorConfig(SelfReflectionRegistry registry) {
	    final Configuration config = Db4o.newConfiguration();
		config.reflectWith(new SelfReflector(registry));
	    return config;
    }
}
