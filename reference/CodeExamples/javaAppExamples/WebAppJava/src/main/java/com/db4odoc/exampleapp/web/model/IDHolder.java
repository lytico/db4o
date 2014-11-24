package com.db4odoc.exampleapp.web.model;

import java.util.UUID;

public abstract class IDHolder {
	private final String id = UUID.randomUUID().toString();

	public String getId() {
		return id;
	}
	
	

}
