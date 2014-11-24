package com.db4o.instrumentation.file;

import java.io.*;
import java.util.*;

/**
 * @exclude
 */
public class DefaultFilePathRoot implements FilePathRoot {

	private final String[] _rootDirs;
	private final String _extension;
	
	public DefaultFilePathRoot(String[] rootDirs) {
		this(rootDirs, "");
	}

	public DefaultFilePathRoot(String[] rootDirs, String extension) {
		_rootDirs = rootDirs;
		_extension = extension;
	}

	public Iterator iterator() {
		return new FileSystemIterator(_rootDirs, _extension);
	}

	public String[] rootDirs() {
		return _rootDirs;
	}

	private static class FileSystemIterator implements Iterator {
		private final String _extension;
		private LinkedList _stack = new LinkedList();

		public FileSystemIterator(String[] roots, String extension) {
			_extension = extension;
			for (int rootIdx = 0; rootIdx < roots.length; rootIdx++) {
				File root = new File(roots[rootIdx]);
				push(new FileInstrumentationClassSource(root, root));
			}
			advanceQueue();
		}

		public boolean hasNext() {
			return !_stack.isEmpty();
		}

		public Object next() {
			InstrumentationClassSource top = pop();
			advanceQueue();
			return top;
		}


		public void remove() {
			throw new UnsupportedOperationException();
		}
		
		private void advanceQueue() {
			while(!_stack.isEmpty() && !accept(peek())) {
				FileInstrumentationClassSource dir = pop();
				if(!dir.file().isDirectory()) {
					continue;
				}
				File[] children = dir.file().listFiles();
				for (int childIdx = 0; childIdx < children.length; childIdx++) {
					_stack.addFirst(new FileInstrumentationClassSource(dir.root(), children[childIdx]));
				}
			}
		}

		private boolean accept(FileInstrumentationClassSource file) {
			return file.file().isFile() && file.file().getName().endsWith(_extension);
		}

		private void push(InstrumentationClassSource root) {
			_stack.addFirst(root);
		}		

		private FileInstrumentationClassSource pop() {
			return (FileInstrumentationClassSource) _stack.removeFirst();
		}

		private FileInstrumentationClassSource peek() {
			return (FileInstrumentationClassSource)_stack.getFirst();
		}
	}
}
