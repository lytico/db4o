/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.browser.gui.views;

import org.eclipse.swt.SWTException;

/**
 * ICaption. An interface for objects with a caption (getText/setText method).
 *
 * @author djo
 */
public interface ITextProperty {
	/**
	 * Returns the receiver's text, which will be an empty
	 * string if it has never been set or if the receiver is
	 * a <code>SEPARATOR</code> label.
	 *
	 * @return the receiver's text
	 *
	 * @exception SWTException <ul>
	 *    <li>ERROR_WIDGET_DISPOSED - if the receiver has been disposed</li>
	 *    <li>ERROR_THREAD_INVALID_ACCESS - if not called from the thread that created the receiver</li>
	 * </ul>
	 */
	public String getText ();
	
	/**
	 * Sets the receiver's text.
	 * <p>
	 * This method sets the widget label.  The label may include
	 * the mnemonic character and line delimiters for some widgets.
	 * </p>
	 * <p>
	 * Mnemonics are indicated by an '&amp' that causes the next
	 * character to be the mnemonic.  When the user presses a
	 * key sequence that matches the mnemonic, focus is assigned
	 * to the control that follows the label. On most platforms,
	 * the mnemonic appears underlined but may be emphasised in a
	 * platform specific manner.  The mnemonic indicator character
	 *'&amp' can be escaped by doubling it in the string, causing
	 * a single '&amp' to be displayed.
	 * </p>
	 * 
	 * @param string the new text
	 *
	 * @exception IllegalArgumentException <ul>
	 *    <li>ERROR_NULL_ARGUMENT - if the text is null</li>
	 * </ul>
	 * @exception SWTException <ul>
	 *    <li>ERROR_WIDGET_DISPOSED - if the receiver has been disposed</li>
	 *    <li>ERROR_THREAD_INVALID_ACCESS - if not called from the thread that created the receiver</li>
	 * </ul>
	 */
	public void setText (String string);
}
