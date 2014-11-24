package com.db4o.drs.test.data;


/**
 * This class has just the regular short name, like stated in most JDO tutorials
 */
public class UnqualifiedNamed {
	
    private String data;

    public UnqualifiedNamed() {
    }
    
    public UnqualifiedNamed(String data) {
        this.data = data;
    }

    public String getData() {
        return data;
    }
    
    public void setData(String data){
    	this.data = data;
    }
}
