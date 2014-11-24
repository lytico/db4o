package db4ounit;

import java.io.*;
import java.lang.reflect.*;

/**
 * @sharpen.ignore
 */
public class TestPlatform {
	
	public static String NEW_LINE = System.getProperty("line.separator");
	
	public static Throwable getExceptionCause(Throwable e) {
		try {
			Method method = e.getClass().getMethod("getCause", new Class[0]);
			return (Throwable) method.invoke(e, new Object[0]);
		} catch (Exception exc) {
			return null;
		}
	}
	
	public static void printStackTrace(Writer writer, Throwable t) {
		java.io.PrintWriter printWriter = new java.io.PrintWriter(writer);
		t.printStackTrace(printWriter);
		printWriter.flush();
	}
	
    public static Writer getStdErr() {
        return new PrintWriter(System.err);
    }
    
	public static boolean isStatic(Method method) {
		return Modifier.isStatic(method.getModifiers());
	}

	public static boolean isPublic(Method method) {
		return Modifier.isPublic(method.getModifiers());
	}

	public static boolean hasParameters(Method method) {
		return method.getParameterTypes().length > 0;
	}

	public static void emitWarning(String warning) {
		System.err.println(warning);
	}

	public static Writer openTextFile(String fname) throws IOException {
		return new java.io.FileWriter(fname);
	}
	
}
