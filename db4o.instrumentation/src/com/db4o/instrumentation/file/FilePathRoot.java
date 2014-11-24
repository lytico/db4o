package com.db4o.instrumentation.file;

import java.io.*;
import java.util.*;

/**
 * @exclude
 */
public interface FilePathRoot extends Iterable<InstrumentationClassSource> {
	String[] rootDirs() throws IOException;
	Iterator<InstrumentationClassSource> iterator();
}
