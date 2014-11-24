package decaf.tests.functional.ant;

import java.util.*;

public class ClassEntry {

	private final String _name;
	private final String _superType;
	private final Set<String> _interfaces;
	private final ArrayList<MethodEntry> _methods = new ArrayList<MethodEntry>();

	public ClassEntry(String name, String superType, Set<String> interfaces) {
		_name = name;
		_superType = superType;
		_interfaces = interfaces;
	}
	
	public String name() {
		return _name;
	}
	
	public String superType() {
		return _superType;
	}
	
	public Set<String> interfaces() {
		return _interfaces;
	}
	
	@Override
	public String toString() {
		return _name + extendsClause() + implementsClause();
	}

	private String extendsClause() {
		if (_superType.equals("java.lang.Object")) {
			return "";
		}
		return " extends " + _superType;
	}

	private String implementsClause() {
		return " implements " + IterableExtensions.join(interfaces(), ", ");
	}

	public List<MethodEntry> methods() {
		return _methods;
	}

	public MethodEntry method(String name, String signature) {
		for (MethodEntry method : _methods) {
			if (method.name().equals(name) && method.descriptor().equals(signature)) {
				return method;
			}
		}
		return null;
	}
}
