/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.internal;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.ext.*;
import com.db4o.internal.config.*;


public class ObjectContainerFactory {
	
	public static EmbeddedObjectContainer openObjectContainer(EmbeddedConfiguration config, String databaseFileName) throws OldFormatException {		
		Configuration legacyConfig = Db4oLegacyConfigurationBridge.asLegacy(config);		
		Config4Impl.assertIsNotTainted(legacyConfig);
		
		emitDebugInfo();		
		EmbeddedObjectContainer oc = new IoAdaptedObjectContainer(legacyConfig, databaseFileName);	
		((EmbeddedConfigurationImpl)config).applyConfigurationItems(oc);
		Messages.logMsg(legacyConfig, 5, databaseFileName);
		return oc;
	}

	private static void emitDebugInfo() {
	    if (Deploy.debug) {
			System.out.println("db4o Debug is ON");
			if (!Deploy.flush) {
				System.out.println("Debug option set NOT to flush file.");
			}
		}
    }
}
