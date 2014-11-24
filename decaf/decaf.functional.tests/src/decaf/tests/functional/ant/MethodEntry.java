package decaf.tests.functional.ant;

public class MethodEntry {

	private final String _name;
	private final String _descriptor;

	public MethodEntry(String name, String descriptor) {
		_name = name;
		_descriptor = descriptor;
	}

	public String name() {
		return _name;
	}

	public String descriptor() {
		return _descriptor;
	}
	
	@Override
	public String toString() {
		return _name + _descriptor;
	}

}
