package contacts;

public class Program {
	
	private ContactList _contacts = new ContactList();
	
	public void readEvalLoop() {
		boolean running = true;
		while (running) {
			String option = prompt("(a)dd new entry, (l)ist entries, (q)uit: ");
			if (option.length() == 0) continue;
			switch (option.charAt(0)) {
			case 'a':
				addEntry();
				break;
				
			case 'l':
				listEntries();
				break;
				
			case 'q':
				running = false;
				break;
			default:
				System.out.println("'" + option + "' DOES NOT COMPUTE!");
			}
		}
	}

	private String prompt(String message) {
		return Console.prompt(message);
	}

	private void listEntries() {
		for (Contact c : _contacts.entries()) {
			System.out.println(c.name() + " <" + c.email() + ">");
		}
	}

	private void addEntry() {
		String name = prompt("Name: ");
		String email = prompt("Email: ");
		_contacts.add(new Contact(name, email));
	}
	
	public void close() {
		_contacts.close();
	}

	public static void main(String[] args) {
		Program program = new Program();
		try {
			program.readEvalLoop();
		} finally {
			program.close();
		}
	}
}
