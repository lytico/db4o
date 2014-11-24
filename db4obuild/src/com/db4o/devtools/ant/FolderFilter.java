package com.db4o.devtools.ant;

/**
 * FolderFilter interface. <br> 
 * 
 * Models the strategy for filtering folders.
 */
public interface FolderFilter {
	boolean filter(String path);
}
