package com.db4odoc.tutorial.runner;

import javax.tools.*;
import java.io.ByteArrayOutputStream;
import java.io.FilterOutputStream;
import java.io.IOException;
import java.io.OutputStream;
import java.net.URI;
import java.net.URISyntaxException;
import java.util.HashMap;
import java.util.Map;

import static com.db4odoc.tutorial.utils.ExceptionUtils.reThrow;


class MemoryFileManager extends ForwardingJavaFileManager {
    private static final String URL_PROTOCOLL = "file:///";
    private final Map<String, byte[]> classes;


    public JavaFileObject makeSource(String name, String code) {
        return new InMemoryJavaSource(name, code);
    }


    public MemoryFileManager(JavaFileManager fileManager) {
        super(fileManager);
        classes = new HashMap<String, byte[]>();
    }


    @Override
    public ClassLoader getClassLoader(Location location) {
        return new ByteArrayClassLoader(classes);
    }

    @Override
    public JavaFileObject getJavaFileForOutput(Location location,  String name,
                                               JavaFileObject.Kind kind,
                                               FileObject originatingSource)
            throws UnsupportedOperationException {
        if (originatingSource instanceof InMemoryJavaSource) {
            return new InMemoryJavaFile(name);
        } else {
            throw new UnsupportedOperationException();
        }
    }

    protected static URI uri(String uri) {
        try {
            return new URI(uri);
        } catch (URISyntaxException e) {
            throw reThrow(e);
        }
    }

    static class ByteArrayClassLoader extends ClassLoader {
        private final Map<String, byte[]> classes;

        public ByteArrayClassLoader(Map<String, byte[]> classes) {
            this.classes = classes;
        }

        @Override
        public Class<?> loadClass(String name) throws ClassNotFoundException {
            try {
                return super.loadClass(name);
            } catch (ClassNotFoundException e) {
                byte[] classData = classes.get(name);
                return defineClass(name, classData, 0, classData.length);
            }
        }


    }

    private class InMemoryJavaFile extends SimpleJavaFileObject {

        private final String name;


        InMemoryJavaFile(String name) {
            super(uri(URL_PROTOCOLL + name.replace('.', '/') + Kind.CLASS.extension),
                    Kind.CLASS);
            this.name = name;
        }

        public OutputStream openOutputStream() {
            return new FilterOutputStream(new ByteArrayOutputStream()) {
                public void close() throws IOException {
                    out.close();
                    ByteArrayOutputStream bos = (ByteArrayOutputStream) out;
                    classes.put(name, bos.toByteArray());
                }
            };
        }
    }

    private static class InMemoryJavaSource extends SimpleJavaFileObject {
        private final String code;

        InMemoryJavaSource(String name, String code) {
            super(uri(URL_PROTOCOLL + name.replace('.', '/') + Kind.SOURCE.extension),
                    Kind.SOURCE);
            this.code = code;
        }

        @Override
        public CharSequence getCharContent(boolean ignoreEncodingErrors) {
            return code;
        }
    }
}
