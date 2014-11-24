package contacts;

import com.db4o.*;

public class ContactList {
	
	private ObjectContainer _container = Db4o.openFile("contacts.db4o");
	
	public void add(Contact contact) {
		_container.store(contact);
	}

	/**
	 * @sharpen.property
	 */
	public Iterable<Contact> entries() {
		return _container.query(Contact.class);
	}
	
	public void close() {
		_container.close();
	}

}
