package com.db4o.instrumentation.ant;

import java.io.*;

import org.apache.tools.ant.types.*;

import com.db4o.instrumentation.file.*;
import com.db4o.instrumentation.util.*;

public class AntJarEntryInstrumentationClassSource implements InstrumentationClassSource {

	private final Resource _resource;
	
	public AntJarEntryInstrumentationClassSource(Resource resource) {
		_resource = resource;
	}

	public String className() throws IOException {
		return BloatUtil.classNameForPath(_resource.getName());
	}

	public InputStream inputStream() throws IOException {
		return _resource.getInputStream();
	}

	public File targetPath(File targetBase) throws IOException {
		return new File(targetBase, _resource.getName());
	}

    public File sourceFile() {
        return null;
    }

}
