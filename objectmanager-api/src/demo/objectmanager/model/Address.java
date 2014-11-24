package demo.objectmanager.model;


/**
 * User: treeder
 * Date: Aug 29, 2006
 * Time: 9:52:35 AM
 */
public class Address {
	private Integer id;
	Contact contact; // bi-directional
    String street;
    String city;
    String state;
    String zip;
	boolean primary;
	Boolean workAddress;


	public Address() {
    }

    public Address(Integer id, Contact c, String street, String city, String state, String zip, boolean primary, Boolean workAddress) {
		this.id = id;
		contact = c;
		this.street = street;
		this.city = city;
		this.state = state;
		this.zip = zip;
		this.primary = primary;
		this.workAddress = workAddress;
	}


	public Integer getId() {
		return id;
	}

	public void setId(Integer id) {
		this.id = id;
	}

	public Contact getContact() {
        return contact;
    }

    public void setContact(Contact contact) {
        this.contact = contact;
    }

    public String getStreet() {
        return street;
    }

    public void setStreet(String street) {
        this.street = street;
    }

    public String getCity() {
        return city;
    }

    public void setCity(String city) {
        this.city = city;
    }

    public String getState() {
        return state;
    }

    public void setState(String state) {
        this.state = state;
    }

    public String getZip() {
        return zip;
    }

    public void setZip(String zip) {
        this.zip = zip;
    }

	public boolean isPrimary() {
		return primary;
	}

	public void setPrimary(boolean primary) {
		this.primary = primary;
	}

	public Boolean getWorkAddress() {
		return workAddress;
	}

	public void setWorkAddress(Boolean workAddress) {
		this.workAddress = workAddress;
	}
}

