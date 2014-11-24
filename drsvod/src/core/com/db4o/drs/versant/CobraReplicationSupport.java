/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.versant;

import com.db4o.*;
import com.db4o.drs.versant.metadata.*;
import com.db4o.foundation.*;

public class CobraReplicationSupport {
	
	private static final Class[] INTERNAL_CLASSES = new Class[] {
		ClassMetadata.class,
		DatabaseSignature.class,
		ObjectInfo.class,
		TimestampToken.class,
	};
	
	public static void initialize(VodCobraFacade cobra){
		initializeInternalClasses(cobra);
		intializeTimestamp(cobra);
	}

	public static void intializeTimestamp(final VodCobraFacade cobra) {
		if(isSingleTimestampTokenPresent(cobra)){
			return;
		}
		cobra.withLock(TimestampToken.class, new Closure4<Void>() {
			@Override
			public Void run() {
				if(isSingleTimestampTokenPresent(cobra)){
					return null;
				}
				TimestampToken timestampToken = new TimestampToken();
				cobra.store(timestampToken);
				cobra.commit();
				return null;
			}
		});
	}

	private static boolean isSingleTimestampTokenPresent(VodCobraFacade cobra) {
		ObjectSet<TimestampToken> timestamps = cobra.from(TimestampToken.class).select();
		if(timestamps.size() == 1){
			return true;
		}
		if(timestamps.size() > 1){
			throw new IllegalStateException("Only one " + TimestampToken.class.getCanonicalName() + " allowed in the database.");
		}
		return false;
	}

	private static void initializeInternalClasses(VodCobraFacade cobra) {
		for (Class clazz : INTERNAL_CLASSES) {
			cobra.produceSchema(clazz);
		}
	}

}
