package decaf;

import java.io.*;

public class Resources {

	public static final String DECAF_ANNOTATIONS_JAR = "decaf-annotations.jar";

	public static String decafAnnotationsJar() throws IOException {
		return Activator.getResource("lib/" + DECAF_ANNOTATIONS_JAR);
	}

}
