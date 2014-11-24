package com.db4o.ta.instrumentation.samples.pathological;


import com.db4o.activation.*;
import com.db4o.ta.*;


public class MyDate extends SuperDate implements Activatable {
	
    public boolean after(SuperDate date) {
    	activate(ActivationPurpose.READ);
    	if(date instanceof Activatable) {
    		((Activatable)date).activate(ActivationPurpose.READ);
    	}
    	return super.after(date);
    }

	public void activate(ActivationPurpose purpose) {
		// TODO Auto-generated method stub
		
	}

	public void bind(Activator activator) {
		// TODO Auto-generated method stub
		
	}

	public int compareTo(Object arg0) {
		// TODO Auto-generated method stub
		return 0;
	}

}

