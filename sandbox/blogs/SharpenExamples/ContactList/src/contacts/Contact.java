package contacts;

public class Contact {

	private String _name;
	private String _email;

	public Contact(String name, String email) {
		_name = name;
		_email = email;
	}

	/**
	 * @sharpen.property
	 */
	public String name() {
		return _name;
	}

	/**
	 * @sharpen.property
	 */
	public String email() {
		return _email;
	}

}
