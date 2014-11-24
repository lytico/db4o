package com.db4o.jiraui.api;

import java.util.*;

public class Comment {
	
	private Resource author;
	private Date timestamp;
	private String text;
	
	public Resource getAuthor() {
		return author;
	}
	public void setAuthor(Resource author) {
		this.author = author;
	}
	public Date getTimestamp() {
		return timestamp;
	}
	public void setTimestamp(Date timestamp) {
		this.timestamp = timestamp;
	}
	public String getText() {
		return text;
	}
	public void setText(String text) {
		this.text = text;
	}

}
