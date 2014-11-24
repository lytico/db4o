package com.db4o.internal.logging;

import java.io.*;
import java.util.*;

import com.db4o.internal.*;

public class PrintWriterLoggerInterceptor implements LoggingInterceptor {
	
	private	PrintWriter out;

	public PrintWriterLoggerInterceptor(PrintWriter out) {
		this.out = out;
	}
	
	@Override
	public void log(Level loggingLevel, String method, Object[] args) {
		
		List<Throwable> throwables = translateArguments(args);
		
		out.println(formatLine(Platform4.now(), loggingLevel, method, args));
		
		if (throwables != null) {
			for(Throwable t : throwables) {
				Platform4.printStackTrace(t, out);
			}
		}
	}

	private List<Throwable> translateArguments(Object[] args) {
		List<Throwable> throwables = null;
		if (args == null) {
			return null;
		}

		for (int i = 0; i < args.length; i++) {
			Object obj = args[i];
			if (obj instanceof Throwable) {
				Throwable t = (Throwable) obj;
				args[i] = t.getClass().getSimpleName();
				if (throwables == null) {
					throwables = new ArrayList<Throwable>();
				}
				throwables.add(t);
			}
		}
		return throwables;
	}

	public static String formatLine(Date now, Level loggingLevel, String method, Object[] args) {
		return Platform4.format(now, true) + " " + formatMessage(loggingLevel, method, args);
	}

	public static String formatMessage(Level loggingLevel, String method, Object[] args) {
		
		String s = "";
		if (args != null) {
			for(Object obj : args) {
				if (s.length() > 0) {
					s += ", ";
				}
				s += obj;
			}
		}
		
		return "["+Logger.levelToString(loggingLevel)+"] "+ formatMethodName(method) + (args==null?"":"("+s+")");
	}

	private static String formatMethodName(String name) {
		return name;
	}
}