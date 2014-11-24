package com.db4o.drs.test.data;

public final class UntypedFieldData {
	
	private int id;
	
	public UntypedFieldData() {
	}
	
	public UntypedFieldData(int value) {
		setId(value);
	}
	
	public boolean equals(Object obj) {
		UntypedFieldData other = (UntypedFieldData)obj;
		return getId() == other.getId();
	}

	public void setId(int id) {
		this.id = id;
	}

	public int getId() {
		return id;
	}
}