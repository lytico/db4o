/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.browser.gui.controllers;

import com.db4o.browser.gui.views.ITextProperty;
import com.db4o.objectmanager.model.GraphPosition;
import com.db4o.objectmanager.model.IGraphIteratorSelectionListener;
import com.db4o.objectmanager.model.IGraphIterator;

/**
 * PathLabelController.  A controller that keeps the path label current in
 * the user interface.
 *
 * @author djo
 */
public class PathLabelController implements IBrowserController {
	
	private ITextProperty pathlabel;
	private IGraphIterator model = null;

	/**
	 * Constructor PathLabelController.  Construct a PathLabelController,
	 * passing the ITextProperty object that will be used as the path
	 * label in the user interface.
	 * 
	 * @param pathlabel
	 */
	public PathLabelController(ITextProperty pathlabel) {
		this.pathlabel = pathlabel;
	}

	/* (non-Javadoc)
	 * @see com.db4o.browser.gui.controllers.IBrowserController#setInputcom.db4o.objectmanager.model.IGraphIteratorr,com.db4o.objectmanager.model.GraphPositionn)
	 */
	public void setInput(IGraphIterator input, GraphPosition selection) {
		if (model != null) {
			model.removeSelectionChangedListener(selectionListener);
		}
		
		model = input;
		model.addSelectionChangedListener(selectionListener);
//		pathlabel.setText(model.getPath().toString());
	}
	
	private IGraphIteratorSelectionListener selectionListener = new IGraphIteratorSelectionListener() {
		public void selectionChanged() {
			pathlabel.setText(model.getPath().toString());
		}

        public boolean canSelectionChange() {
            // The path label never vetoes a selection change...
            return true;
        }
	};

}
