/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.versant;

import java.io.*;

import com.versant.util.*;

public class VodJvi {
	
	private static final int MAX_DB_NAME_LENGTH = 31;

	private static final String VEDSECHN_SCHEMA = "/lib/vedsechn.sch";

	private static final String CHANNEL_SCHEMA = "/lib/channel.sch";

	private final VodDatabase _vod;

	public VodJvi(VodDatabase vod) {
		_vod = vod;
	}

	public String versantRootPath() {
		return DBUtility.versantRootPath();
	}
	
	private void defineSchema(String schema) {
		DBUtility.defineSchema(_vod.name(), new File(new File(versantRootPath()), schema).getAbsolutePath());
	}
	
	public void createEventSchema() {
		System.out.println("-------> Creating event schema");
		defineSchema(CHANNEL_SCHEMA);
		defineSchema(VEDSECHN_SCHEMA);
	}
	
	public static String safeDatabaseName(String databaseName) {
		StringBuffer sb = new StringBuffer();
		for (int i = 0; i < databaseName.length(); i++) {
			char c = databaseName.charAt(i);
			if(Character.isLetterOrDigit(c)){
				sb.append(c);
			}
		}
		if(sb.length() > MAX_DB_NAME_LENGTH) {
			sb.delete(0, sb.length() - MAX_DB_NAME_LENGTH);
		}
		return sb.toString();
	}

}
