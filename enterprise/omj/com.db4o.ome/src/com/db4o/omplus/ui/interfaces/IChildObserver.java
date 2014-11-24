package com.db4o.omplus.ui.interfaces;

/**
 * An interface to be implemneted by all parent (Controls/windows)
 * who have to keep a track of actions on their child widgets
 * 
 * @author prameela_nair
 *
 */
public interface IChildObserver 
{
	/**
	 * Specifies that a child at index was closed 
	 * @param index
	 */
	public void close(int index);

	/**
	 * Child at index has been resized
	 * @param index
	 */
	public void resized(int index);
}
