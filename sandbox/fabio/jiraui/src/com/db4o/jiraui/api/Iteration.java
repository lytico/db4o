package com.db4o.jiraui.api;

import java.util.*;

public class Iteration {

	private int id;
	private String description;
	private Date begin;
	private Date end;

	public Iteration(int id) {
		this.id = id;
	}

	public int getId() {
		return id;
	}

	public void setId(int id) {
		this.id = id;
	}

	public String getDescription() {
		return description;
	}

	public void setDescription(String description) {
		this.description = description;
	}

	public Date getBegin() {
		return begin;
	}

	public void setBegin(Date begin) {
		this.begin = begin;
	}

	public Date getEnd() {
		return end;
	}

	public void setEnd(Date end) {
		this.end = end;
	}
	
	@Override
	public String toString() {
		return ""+id;
	}

}
