package com.db4o.devtools.ant;

import java.util.*;

public class AssemblyInfo {
    
	public static final String PRODUCT = "db4o - database for objects";
    
    public static final String COPYRIGHT = "Versant Corp. 2000 - " + currentYear();

    public static final String COMPANY = "Versant Corp., Redwood City, CA, USA";

    private static int currentYear() {
    	return Calendar.getInstance(Locale.US).get(Calendar.YEAR);
	}
}