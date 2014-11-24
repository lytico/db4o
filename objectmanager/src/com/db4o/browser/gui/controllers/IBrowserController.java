/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.browser.gui.controllers;

import com.db4o.objectmanager.model.GraphPosition;
import com.db4o.objectmanager.model.IGraphIterator;

/**
 * IBrowserController.  An interface for all controller objects in the 
 * Browser.
 *
 * @author djo
 */
public interface IBrowserController {
	/**
	 * Set the input to the control that is being managed by this
	 * controller.
	 * 
	 * @param input An IGraphIterator specifying the input
	 * @param selection The selection position
	 */
	public void setInput(IGraphIterator input, GraphPosition selection);
}
