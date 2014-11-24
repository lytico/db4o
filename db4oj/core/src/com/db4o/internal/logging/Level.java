package com.db4o.internal.logging;

public class Level {
	private final String _name;
	private final int _level;

	Level(String name, int level) {
		_name = name;
		_level = level;
	}
	
	public int ordinal() {
		return _level;
	}
	
	@Override
	public String toString() {
		return _name;
	}
}