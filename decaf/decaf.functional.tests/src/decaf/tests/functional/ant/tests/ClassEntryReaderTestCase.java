package decaf.tests.functional.ant.tests;

import java.io.*;
import java.util.*;

import junit.framework.*;
import sharpen.ui.tests.*;
import decaf.tests.functional.ant.*;
import decaf.tests.functional.ant.tests.jar.*;

public class ClassEntryReaderTestCase extends TestCase {
	
	public void test() throws Exception {

		final ArrayList<ClassEntry> entries = readAll(createJar(Foo.class, IFoo.class));
		assertEquals(2, entries.size());
		assertEntry(Foo.class, entries.get(0));
		assertEquals(2, entries.get(0).methods().size());
		assertEntry(IFoo.class, entries.get(1));
	}

	private void assertEntry(final Class<?> expected, final ClassEntry actual) {
		assertEquals(expected.getName(), actual.name());
		assertEquals(safeSuperType(expected).getName(), actual.superType());
	}

	private Class<?> safeSuperType(final Class<?> expected) {
		final Class<?> superclass = expected.getSuperclass();
		if (null == superclass) {
			return Object.class;
		}
		return superclass;
	}

	private ArrayList<ClassEntry> readAll(final File file) throws IOException {
		final ArrayList<ClassEntry> entries = new ArrayList<ClassEntry>();
		final ClassEntryReader reader = new ClassEntryReader(file);
		try {
			while (true) {
				final ClassEntry entry = reader.readNext();
				if (null == entry) {
					break;
				}
				entries.add(entry);
			}
		} finally {
			reader.close();
		}
		return entries;
	}

	private File createJar(Class<?>...cookies) throws Exception {
		return new File(JarUtilities.createJar(cookies));
	}

}
