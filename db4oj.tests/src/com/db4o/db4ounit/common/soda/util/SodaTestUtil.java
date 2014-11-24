/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.soda.util;

import com.db4o.*;
import com.db4o.query.*;

import db4ounit.*;

public class SodaTestUtil {

    public static void expectOne(Query query, Object object) {
        expect(query, new Object[] { object });
    }

    public static void expectNone(Query query) {
        expect(query, null);
    }

    public static void expect(Query query, Object[] results) {
        expect(query, results, false);
    }

    public static void expectOrdered(Query query, Object[] results) {
        expect(query, results, true);
    }

    public static void expect(Query query, Object[] results, boolean ordered) {
        ObjectSet set = query.execute();
        if (results == null || results.length == 0) {
            if (set.size() > 0) {
                Assert.fail("No content expected.");
            }
            return;
        }
        int j = 0;
        Assert.areEqual(results.length, set.size());
        while (set.hasNext()) {
            Object obj = set.next();
            boolean found = false;
            if (ordered) {
                if (TCompare.isEqual(results[j], obj)) {
                    results[j] = null;
                    found = true;
                }
                j++;
            } else {
                for (int i = 0; i < results.length; i++) {
                    if (results[i] != null) {
                        if (TCompare.isEqual(results[i], obj)) {
                            results[i] = null;
                            found = true;
                            break;
                        }
                    }
                }
            }
            if (ordered){
            	Assert.isTrue(found, "Expected '" + safeToString(results[j-1]) + "' but got '" +  safeToString(obj) + "' at index " + (j-1));
            } else {
            	Assert.isTrue(found, "Object not expected: " + safeToString(obj));
            }
        }
        for (int i = 0; i < results.length; i++) {
            if (results[i] != null) {
                Assert.fail("Expected object not returned: " + results[i]);
            }
        }
    }
	
	private static String safeToString(Object obj) {
		return obj != null ? obj.toString() : "";
	}

	private SodaTestUtil() {}
}
