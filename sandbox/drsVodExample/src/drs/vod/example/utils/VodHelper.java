/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */
package drs.vod.example.utils;

import java.util.*;
import javax.jdo.*;

import com.db4o.drs.versant.*;

public class VodHelper {
	
	public static PersistenceManager getPersistenceManager() {
		Properties properties = properties();
		PersistenceManagerFactory pmf = 
			JDOHelper.getPersistenceManagerFactory(properties);
		return pmf.getPersistenceManager();
	}

	public static Properties properties() {
		Properties properties = new Properties();
		properties.setProperty("javax.jdo.option.ConnectionURL", "versant:dRSVodExample@localhost");
		properties.setProperty("javax.jdo.option.ConnectionUserName", "drs");
		properties.setProperty("javax.jdo.option.ConnectionPassword", "drs");
		properties.setProperty("javax.jdo.PersistenceManagerFactoryClass","com.versant.core.jdo.BootstrapPMF");
		properties.setProperty("javax.jdo.PersistenceManagerFactoryClass","com.versant.core.jdo.BootstrapPMF");
		properties.setProperty("versant.metadata.0", "drs/vod/example/model/package.jdo");
		properties.setProperty("versant.metadata.1", "com/db4o/drs/versant/metadata/package.jdo");
		return properties;
	}
	
	public static void ensureVodDatabaseExists(){
		VodDatabase vod = new VodDatabase("dRSVodExample", "drs", "drs");
		if(! vod.dbExists()){
			throw new RuntimeException("Database 'dRSVodExample' does not exist. You can create it with /scripts/createDatabase.");
		}
	}

}
