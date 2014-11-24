package com.db4o.internal.logging;

public interface Logging<T> {
	
	T trace();
	T debug();
	T info();
	T warn();
	T error();
	T fatal();
	
	void loggingLevel(Level loggingLevel);
	Level loggingLevel();
	
	void forward(T forward);
	
	T forward();
	

}
