/* Copyright (C) 2004 - 2006  db4objects Inc.  http://www.db4o.com */

package com.db4o.config;

/**
 * Wildcard Alias functionality to create aliases for packages, 
 * namespaces or multiple similar named classes. One single '*' 
 * wildcard character is supported in the names. 
 * <br><br>See {@link Alias} for concrete examples.
 */
public class WildcardAlias implements Alias {
	
	private final WildcardPattern _storedPattern;
    
	private final WildcardPattern _runtimePattern;

	/**
     * Create a WildcardAlias with two patterns, the 
     * stored pattern and the pattern that is to be used 
     * at runtime. One single '*' is allowed as a wildcard
     * character.
	 */
    public WildcardAlias(String storedPattern, String runtimePattern) {
        
		if (null == storedPattern || null == runtimePattern) throw new IllegalArgumentException();
		
		_storedPattern = new WildcardPattern(storedPattern);
		_runtimePattern = new WildcardPattern(runtimePattern);
    }
    
    /**
     * resolving is done through simple pattern matching  
     */
	public String resolve(String runtimeType) {
		String match = _runtimePattern.matches(runtimeType);
		return match != null
			? _storedPattern.inject(match)
			: null;
	}
	
	static class WildcardPattern {
		private String _head;
		private String _tail;

		public WildcardPattern(String pattern) {
			String[] parts = split(pattern);
			
			_head = parts[0];
			_tail = parts[1];
		}

		public String inject(String s) {
			return _head + s + _tail; 
		}

		public String matches(String s) {
			if (!s.startsWith(_head) || !s.endsWith(_tail)) return null;
			return s.substring(_head.length(), s.length()-_tail.length());
		}

		private void invalidPattern() {
			throw new  IllegalArgumentException("only one '*' character");
		}
		
		String[] split(String pattern) {
			int index = pattern.indexOf('*');
			if (-1 == index || index != pattern.lastIndexOf('*')) invalidPattern();
			return new String[] {
					pattern.substring(0, index),
					pattern.substring(index+1)
			};
		}
	}

}
