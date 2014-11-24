/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.browser.gui.views;

import org.eclipse.swt.SWTException;
import org.eclipse.swt.events.SelectionEvent;
import org.eclipse.swt.events.SelectionListener;

/**
 * Interface ISelectionSource.  A DuckType interface for SWT objects that can 
 * fire events to SelectionListener objects.  (Button-like things)
 */
public interface ISelectionSource {

	/**
	 * Adds the listener to the collection of listeners who will
	 * be notified when the control is selected, by sending
	 * it one of the messages defined in the <code>SelectionListener</code>
	 * interface.
	 * <p>
	 * <code>widgetSelected</code> is called when the control is selected.
	 * <code>widgetDefaultSelected</code> is not called.
	 * </p>
	 *
	 * @param listener the listener which should be notified
	 *
	 * @exception IllegalArgumentException <ul>
	 *    <li>ERROR_NULL_ARGUMENT - if the listener is null</li>
	 * </ul>
	 * @exception SWTException <ul>
	 *    <li>ERROR_WIDGET_DISPOSED - if the receiver has been disposed</li>
	 *    <li>ERROR_THREAD_INVALID_ACCESS - if not called from the thread that created the receiver</li>
	 * </ul>
	 *
	 * @see SelectionListener
	 * @see #removeSelectionListener
	 * @see SelectionEvent
	 */
	public void addSelectionListener (SelectionListener listener);
	
	/**
	 * Removes the listener from the collection of listeners who will
	 * be notified when the control is selected.
	 *
	 * @param listener the listener which should be notified
	 *
	 * @exception IllegalArgumentException <ul>
	 *    <li>ERROR_NULL_ARGUMENT - if the listener is null</li>
	 * </ul>
	 * @exception SWTException <ul>
	 *    <li>ERROR_WIDGET_DISPOSED - if the receiver has been disposed</li>
	 *    <li>ERROR_THREAD_INVALID_ACCESS - if not called from the thread that created the receiver</li>
	 * </ul>
	 *
	 * @see SelectionListener
	 * @see #addSelectionListener
	 */
	public void removeSelectionListener (SelectionListener listener);
	
	/**
	 * Enables the receiver if the argument is <code>true</code>,
	 * and disables it otherwise. A disabled control is typically
	 * not selectable from the user interface and draws with an
	 * inactive or "grayed" look.
	 *
	 * @param enabled the new enabled state
	 *
	 * @exception SWTException <ul>
	 *    <li>ERROR_WIDGET_DISPOSED - if the receiver has been disposed</li>
	 *    <li>ERROR_THREAD_INVALID_ACCESS - if not called from the thread that created the receiver</li>
	 * </ul>
	 */
	public void setEnabled (boolean enabled);
}
