package db4ounit.extensions.util;

import com.db4o.foundation.io.*;
import com.db4o.internal.*;

public class CrossPlatformServices {

	public static String simpleName(String typeName) {
		int index = typeName.indexOf(',');
		if (index < 0) return typeName;
		return typeName.substring(0, index);
	}

	public static String fullyQualifiedName(Class klass) {
		return ReflectPlatform.fullyQualifiedName(klass);
	}

	public static String databasePath(String fileName) {
		String path = System.getProperty("db4ounit.file.path");
		if(path == null || path.length() == 0) {
			path =".";
		} else {
		    File4.mkdirs(path);
		}
		return Path4.combine(path, fileName);
	}
}
