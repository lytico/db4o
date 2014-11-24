package com.db4o.omplus.ui.interfaces;

/**
 * Interface that would deal with how to enable/disable child 
 * components depending on some conditions/event from its child 
 * components
 * 
 * @author prameela_nair
 *
 */
public interface IChildModifier 
{
	public void objectTableModified(Object resultObject);
	
	public void objectTreeModified(boolean b);
	
	/**
	 * Set the selection in the table when a tab is selcted in the ObjectViewer
	 * @param index
	 */
	public void objectTabSelectedInTree(int index);
	
}
