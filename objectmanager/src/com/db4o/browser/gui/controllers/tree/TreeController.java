/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.browser.gui.controllers.tree;

import org.eclipse.jface.viewers.*;
import org.eclipse.swt.events.*;
import org.eclipse.swt.widgets.*;
import org.eclipse.ve.sweet.controllers.*;
import org.eclipse.ve.sweet.objectviewer.*;

import com.db4o.browser.gui.controllers.*;
import com.db4o.objectmanager.model.nodes.*;
import com.db4o.objectmanager.model.IGraphIterator;
import com.db4o.objectmanager.model.GraphPosition;

/**
 * TreeController.
 *
 * @author djo
 */
public class TreeController implements IBrowserController {
	private final BrowserTabController parent;
	private final TreeViewer viewer;
	private final SelectionChangedController selectionListener;
	private Button deleteButton;
	
	public TreeController(final BrowserTabController parent, Tree tree, Button deleteButton) {
		this.parent = parent;
		this.viewer = new TreeViewer(tree);
		this.deleteButton=deleteButton;
		
        viewer.setContentProvider(new TreeContentProvider());
        viewer.setLabelProvider(new TreeLabelProvider());
		final TreeSelectionChangedController treeSelectionChangedController = new TreeSelectionChangedController(deleteButton,parent);
		viewer.addSelectionChangedListener(treeSelectionChangedController);
        
        tree.addMouseListener(new MouseAdapter() {
            public void mouseDoubleClick(MouseEvent e) {
                IGraphIterator input = (IGraphIterator) viewer.getInput();
                input.reset();
                if (input.hasNext()) {
                	GraphPosition selectedNode = (GraphPosition)((StructuredSelection)viewer.getSelection()).getFirstElement();
                	input.setPath(selectedNode);
                    IModelNode selection = (IModelNode) input.next();
                    input.previous();
                    if (selection instanceof ClassNode) {
                        ClassNode node = (ClassNode) selection;
                        parent.getQueryController().open(node.getReflectClass(), parent.getCurrentConnection().path());
                    }
                }
            }
        });
        
        tree.addKeyListener(new KeyListener() {
            public void keyPressed(KeyEvent e) {
                if (e.character=='*')
                    e.doit = false;
            }

            public void keyReleased(KeyEvent e) {
                if (e.character=='*')
                    e.doit = false;
            }
        });

		selectionListener = parent.getSelectionChangedController();
		selectionListener.setTreeViewer(viewer);
		selectionListener.setTreeSelectionChangedController(treeSelectionChangedController);
		
		RefreshService.getDefault().addEditStateListener(editStateListener);
	}
	
	private IEditStateListener editStateListener = new IEditStateListener() {
		public void stateChanged(IObjectViewer sender) {
			if (viewer.getTree().isDisposed()) {
				RefreshService.getDefault().removeEditStateListener(editStateListener);
				return;
			}
			if (!sender.isDirty())
				viewer.refresh();
		}
	};
	
	/* (non-Javadoc)
	 * @see com.db4o.browser.gui.controllers.IBrowserController#setInpucom.db4o.objectmanager.model.IGraphIteratororcom.db4o.objectmanager.model.GraphPositionon)
	 */
	public void setInput(IGraphIterator input, GraphPosition selection) {
		IGraphIterator oldInput = (IGraphIterator) viewer.getInput();
		if (oldInput != null)
			oldInput.removeSelectionChangedListener(selectionListener);
		
		viewer.setInput(input);
		if(selection==null) {
	        unselect();
		}
		input.addSelectionChangedListener(selectionListener);
	}

	private void unselect() {
		viewer.setSelection(StructuredSelection.EMPTY);
		deleteButton.setEnabled(false);
	}

    public void deselectAll() {
        unselect();
        viewer.collapseAll();
    }
}
