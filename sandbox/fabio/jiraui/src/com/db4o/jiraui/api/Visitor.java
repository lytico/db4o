package com.db4o.jiraui.api;

public interface Visitor<T> {

	T visit(T t);
	
}
