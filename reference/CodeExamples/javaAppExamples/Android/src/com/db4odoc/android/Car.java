/* Copyright (C) 2004 - 2007 db4objects Inc. http://www.db4o.com */
package com.db4odoc.android;

import java.text.DateFormat;
import java.text.SimpleDateFormat;


public class Car {   
    private String model;
    private Pilot pilot;
    
    // #example: Add a new field to the car
    private RegistrationRecord registration;
    
	public RegistrationRecord getRegistration() {
		return registration;
	}

	public void setRegistration(RegistrationRecord registration) {
		this.registration = registration;
	}
    // #end example

    public Car(){
    }
    
    public Car(String model) {    			
        this.model=model;
        this.pilot=null;
    }
      
    public Pilot getPilot() {
        return pilot;
    }
    
    public void setPilot(Pilot pilot) {
        this.pilot = pilot;
    }
    
    public String getModel() {
        return model;
    }
    
    public String toString() {
    	if (registration == null){
    		return model + "["+pilot+"]";
    	} else {
    		DateFormat df = new SimpleDateFormat("d/M/yyyy");
    		return model + ": " + df.format(registration.getYear());
    	}
    }

	public void setModel(String model) {
		this.model = model;
	}

}
