/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package drs.vod.example.utils;

import com.db4o.*;
import com.db4o.config.*;

public class Db4oHelper {
	
	public static ObjectContainer openObjectContainer(){
		EmbeddedConfiguration config = Db4oEmbedded.newConfiguration();
		configureAllClassesForReplication(config);
		return Db4oEmbedded.openFile(config, "dRSVodExample.db4o");
	}

	private static void configureAllClassesForReplication(EmbeddedConfiguration config) {
		config.file().generateUUIDs(ConfigScope.GLOBALLY);
		config.file().generateVersionNumbers(ConfigScope.GLOBALLY);
	}
	
}
