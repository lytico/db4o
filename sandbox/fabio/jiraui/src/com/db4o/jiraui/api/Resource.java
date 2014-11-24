package com.db4o.jiraui.api;

public class Resource {

	private String id;
	private String name;
	private boolean favorite;

	public Resource(String id) {
		this.id = id;
	}

	public String getId() {
		return id;
	}

	public void setId(String id) {
		this.id = id;
	}

	public String getName() {
		return name;
	}

	public void setName(String name) {
		this.name = name;
	}

	public void setFavorite(boolean favorite) {
		this.favorite = favorite;
	}
	
	public boolean isFavorite() {
		return favorite;
	}

}
