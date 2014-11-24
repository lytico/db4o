package decaf.tests.functional.ant;

import java.util.*;

public class IgnoreSettings {

	public static class Entry {
		private String _className;
		private final Set<String> _methods = new HashSet<String>();
		private final Set<String> _interfaces = new HashSet<String>();

		public void setClass(String className) {
			_className = className;
		}
		
		public boolean isIgnoredClass() {
			return _methods.isEmpty() && _interfaces.isEmpty();
		}

		public String className() {
			return _className;
		}
		
		public class MethodEntry {
			public void setName(String name) {
				_methods.add(name);
			}
		}
		
		public class InterfaceEntry {
			public void setName(String name) {
				_interfaces.add(name);
			}
		}
		
		public InterfaceEntry createInterface() {
			return new InterfaceEntry();
		}
		
		public MethodEntry createMethod() {
			return new MethodEntry();
		}

		public Set<String> methods() {
			return _methods;
		}
		
		public Set<String> interfaces() {
			return _interfaces;
		}
	}
	
	private final List<Entry> _entries = new ArrayList<Entry>();

	public Entry createEntry() {
		final Entry entry = new Entry();
		_entries.add(entry);
		return entry;
	}

	public Iterable<String> ignoredClasses() {
		final HashSet<String> set = new HashSet<String>();
		for (Entry entry : _entries) {
			if (entry.isIgnoredClass()) {
				set.add(entry.className());
			}
		}
		return set;
	}

	public Set<String> ignoredMethodsFor(String name) {
		final Entry found = entry(name);
		if (null == found) {
			return Collections.emptySet();
		}
		return found.methods();
	}

	private Entry entry(String name) {
		for (Entry entry : _entries) {
			if (entry.className().equals(name)) {
				return entry;
			}
		}
		return null;
	}

	public Set<String> ignoredInterfacesFor(String name) {
		final Entry found = entry(name);
		if (null == found) {
			return Collections.emptySet();
		}
		return found.interfaces();
	}

}
