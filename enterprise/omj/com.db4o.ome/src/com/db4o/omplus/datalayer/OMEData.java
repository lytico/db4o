package com.db4o.omplus.datalayer;

import java.util.*;

@SuppressWarnings("unchecked")
public class OMEData {
	
	HashMap<String, List> data;
	
	OMEData(){
		data = new HashMap<String, List>();
	}
	
	public HashMap<String, List> getData() {
		return data;
	}

	public void setData(HashMap<String, List> data) {
		this.data = data;
	}
	
}
