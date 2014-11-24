/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.browser.gui.controllers;

import org.eclipse.jface.viewers.StructuredSelection;
import org.eclipse.jface.viewers.TreeViewer;

import com.db4o.browser.gui.controllers.tree.TreeSelectionChangedController;
import com.db4o.objectmanager.model.GraphPosition;
import com.db4o.objectmanager.model.IGraphIterator;
import com.db4o.objectmanager.model.IGraphIteratorSelectionListener;

/**
 * SelectionChangedController.  This controller is responsible for notifying anyone who needs to know
 * about a window-global selection changed event.
 *
 * @author djo
 */
public class SelectionChangedController implements IGraphIteratorSelectionListener {
	private TreeViewer treeViewer;
	private TreeSelectionChangedController treeSelectionChangedController;
	private DetailController detailController;


    /* (non-Javadoc)
     * @seecom.db4o.objectmanager.model.IGraphIteratorSelectionListenerr#canSelectionChange()
     */
    public boolean canSelectionChange() {
        if (!treeSelectionChangedController.canSelectionChange()) {
            return false;
        }
        return detailController.canSelectionChange();
    }

	/* (non-Javadoc)
	 * @seecom.db4o.objectmanager.model.IGraphIteratorr.IListener#selectionChanged()
	 */
	public void selectionChanged() {
		IGraphIterator model = (IGraphIterator) treeViewer.getInput();
		GraphPosition selectedElement = model.getPath();

		if (!treeSelectionChangedController.isTreeSelectionChanging())
			treeViewer.setSelection(new StructuredSelection(selectedElement), true);
		
		detailController.setInput(model, selectedElement);
	}

	/**
	 * Sets the object that will know about if the tree initiated this selection
	 * change.
	 *  
	 * @param treeSelectionChangedController
	 */
	public void setTreeSelectionChangedController(TreeSelectionChangedController treeSelectionChangedController) {
		this.treeSelectionChangedController = treeSelectionChangedController;
	}

	/**
	 * Sets the TreeViewer controller
	 * 
	 * @param treeViewer The treeViewer to set.
	 */
	public void setTreeViewer(TreeViewer treeViewer) {
		this.treeViewer = treeViewer;
	}
	
	/**
	 * Tells this controller what controller is responsible for the 
	 * detail pane.
	 * 
	 * @param detailController The detailController to set.
	 */
	public void setDetailController(DetailController detailController) {
		this.detailController = detailController;
	}

}
