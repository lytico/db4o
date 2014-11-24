/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.browser.gui.controllers.tree;

import org.eclipse.jface.viewers.*;
import org.eclipse.swt.events.*;
import org.eclipse.swt.widgets.*;

import com.db4o.browser.gui.controllers.*;
import com.db4o.objectmanager.model.nodes.field.*;
import com.db4o.objectmanager.model.GraphPosition;
import com.db4o.objectmanager.model.IGraphIterator;
import com.swtworkbench.community.xswt.metalogger.*;

/**
 * TreeSelectionChangedController.  When the tree's selection changes, updates
 * the model's selection.
 *
 * @author djo
 */
public class TreeSelectionChangedController implements
		ISelectionChangedListener {

	private int treeSelectionChanging=0;
    
    private GraphPosition lastSelection = null;
    
    private Button deleteButton;

    public TreeSelectionChangedController(Button deleteButton,final BrowserTabController tabCtrl) {
        if(deleteButton == null) {
            throw new IllegalArgumentException("Button cannot be null");
        }
        if(tabCtrl == null){
            throw new IllegalArgumentException("TabController cannot be null");
        }
        this.deleteButton=deleteButton;

        deleteButton.addSelectionListener(new SelectionAdapter() {
			public void widgetSelected(SelectionEvent e) {
				//System.out.println(lastSelection.getCurrent().getEditValue().getClass());
				//System.out.println(lastSelection.getCurrent().getDatabase());
				lastSelection.getCurrent().getDatabase().delete(lastSelection.getCurrent().getEditValue());
				tabCtrl.dirty();
			}
        });

    }

	/* (non-Javadoc)
	 * @see org.eclipse.jface.viewers.ISelectionChangedListener#selectionChanged(org.eclipse.jface.viewers.SelectionChangedEvent)
	 */
	public void selectionChanged(SelectionChangedEvent event) {
		try {
			++treeSelectionChanging;
			
			IStructuredSelection selection = (IStructuredSelection) event.getSelection();
			
			if (selection.getFirstElement() == null || selection.getFirstElement().equals(lastSelection)) {
				return;
			}
			
			if (!selection.isEmpty()) {
				GraphPosition node = (GraphPosition) selection.getFirstElement();
	
				final TreeViewer source = (TreeViewer) event.getSource();
				IGraphIterator model = (IGraphIterator) source.getInput();
                if (model.isPathSelectionChangable()) {
                    model.setSelectedPath(node);
                    lastSelection = node;
                    deleteButton.setEnabled((node.getCurrent() instanceof FieldNode));
                	source.refresh();
                } else if (lastSelection != null) {
                    Display.getCurrent().asyncExec(new Runnable() {
                        public void run() {
                            source.setSelection(new StructuredSelection(lastSelection));
                        }
                    });
                } else {
                    lastSelection = node;
                    throw new RuntimeException(getClass().getName() + ": Cannot reset the selection back to null!");
                }
			}
			else {
				deleteButton.setEnabled(false);
			}
		} catch (Throwable t) {
        	String message = "Exception handling tree selection change.\n" +
    		"This usually happens when a class needs its constructor called because it has transient fields.\n" +
    		"See File | Preferences | Constructor Calling";
        Logger.log().error(t, message);
            Logger.log().error(t, message);
		} finally {
            --treeSelectionChanging;
        }
	}
	
	/**
	 * Indicates to clients if the tree's selection is in the process of
	 * changing.
	 * 
	 * @return true if the tree's selection is changing; false otherwise.
	 */
	public boolean isTreeSelectionChanging() {
		return treeSelectionChanging > 0;
	}

    /**
     * Delegated from SelectionChangedController (IGraphIteratorSelectionListener method)
     * 
     * @return true always.
     */
    public boolean canSelectionChange() {
        return true;
    }

}
