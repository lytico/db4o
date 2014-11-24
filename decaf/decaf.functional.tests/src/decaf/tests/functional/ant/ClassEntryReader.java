package decaf.tests.functional.ant;

import java.io.*;
import java.util.zip.*;

import org.objectweb.asm.*;

public class ClassEntryReader {
	
	private final ZipInputStream zip;
	
	public ClassEntryReader(File file) throws IOException {
		zip = new ZipInputStream(new FileInputStream(file));
	}

	public void close() throws IOException {
		zip.close();
	}

	public ClassEntry readNext() throws IOException {
		while (true) {
			final ZipEntry entry = zip.getNextEntry();
			if (null == entry) {
				break;
			}
			if (isTopLevelClassEntry(entry)) {
				return nextClassEntry();
			}
		}
		return null;
	}
	
	private ClassEntry nextClassEntry()
			throws IOException {
		final ClassEntryBuilder builder = new ClassEntryBuilder();
		new ClassReader(zip).accept(builder, ClassReader.SKIP_CODE);
		return builder.classEntry();
	}

	private boolean isTopLevelClassEntry(ZipEntry entry) {
		return entry.getName().endsWith(".class")
			&& entry.getName().indexOf('$') == -1;
	}

	
}