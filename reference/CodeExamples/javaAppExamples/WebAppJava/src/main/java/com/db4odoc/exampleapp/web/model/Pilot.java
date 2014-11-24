package com.db4odoc.exampleapp.web.model;

public class Pilot extends IDHolder{
	
	private String name;
	private int points;
	
	public Pilot() {
		super();
		this.name = "";
		this.points = 0;
	}
	
	public Pilot(String name, int points) {
		super();
		this.name = name;
		this.points = points;
	}
	
	
	public String getName() {
		return name;
	}
	public void setName(String name) {
		this.name = name;
	}
	public int getPoints() {
		return points;
	}
	public void setPoints(int points) {
		this.points = points;
	}
	
	
}
