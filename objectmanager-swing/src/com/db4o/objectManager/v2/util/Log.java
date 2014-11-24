package com.db4o.objectManager.v2.util;

import java.io.StringWriter;
import java.io.PrintWriter;
import java.util.List;
import java.util.ArrayList;


/**
 * User: treeder
 * Date: Sep 11, 2006
 * Time: 3:41:31 PM
 */
public class Log {
    static List<Exception> exceptions = new ArrayList<Exception>();
    private static final int MAX_EXCEPTIONS = 10;

    public static void addException(Exception e) {
        exceptions.add(e);
        if (exceptions.size() > MAX_EXCEPTIONS) {
            exceptions.remove(0);
        }
    }

    public static String dump() {
        StringWriter stringWriter = new StringWriter();
        PrintWriter out = new PrintWriter(stringWriter);
        for (int i = 0; i < exceptions.size(); i++) {
            Exception exception = exceptions.get(i);
            exception.printStackTrace(out);
        }
        return stringWriter.toString();
    }
}
