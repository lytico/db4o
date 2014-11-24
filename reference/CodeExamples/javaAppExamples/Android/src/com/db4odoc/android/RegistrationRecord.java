package com.db4odoc.android;

import java.util.Date;

public class RegistrationRecord {
	private String number;
	private Date year;

	public RegistrationRecord(String number, Date year){
		this.number = number;
		this.year = year;
	}

	public String getNumber() {
		return number;
	}

	public void setNumber(String number) {
		this.number = number;
	}

	public Date getYear() {
		return year;
	}

	public void setYear(Date year) {
		this.year = year;
	}	
}

