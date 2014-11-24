/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.browser.gui.controllers;

import org.eclipse.swt.*;
import org.eclipse.swt.events.*;
import org.eclipse.swt.graphics.*;
import org.eclipse.swt.layout.*;
import org.eclipse.swt.widgets.*;
import org.eclipse.ve.sweet.*;
import org.eclipse.ve.sweet.controllers.*;
import org.eclipse.ve.sweet.objectviewer.*;

import com.db4o.browser.gui.views.*;
import com.db4o.objectmanager.model.nodes.*;
import com.db4o.objectmanager.model.*;
import com.db4o.binding.dataeditors.db4o.Db4oObjectEditorFactory;
import com.swtworkbench.community.xswt.metalogger.*;

/**
 * DetailController.  A Controller for the detail pane of the browser.
 *
 * @author djo
 */
public class DetailController implements IBrowserController {
	private DbBrowserPane ui;
    private Composite parentComposite;
	private BrowserTabController parent;
    private IGraphIterator input = null;
    private GraphPosition selection;
	private static final Color WHITE = Display.getCurrent().getSystemColor(SWT.COLOR_WHITE);
	private IObjectViewer objectViewer = null;

	/**
	 * Constructor DetailController.  Create an MVC controller to manage the detail pane.
	 * 
	 * @param parent
	 * @param ui
	 */
	public DetailController(BrowserTabController parent, DbBrowserPane ui) {
		this.parent = parent;
		this.ui = ui;
        parentComposite = ui.getFieldArea();
        parentComposite.addDisposeListener(disposeListener);
        
        ui.getSaveButton().addSelectionListener(saveEdits);
        ui.getCancelButton().addSelectionListener(cancelEdits);
		
		parent.getSelectionChangedController().setDetailController(this);
		
		RefreshService.getDefault().addEditStateListener(refreshListener);
	}

	private SelectionListener saveEdits = new SelectionAdapter() {
		public void widgetSelected(SelectionEvent e) {
			if(objectViewer!=null) {
				try {
					objectViewer.commit();
				} catch (CannotSaveException e1) {
					// The user has already been informed...
				}
			}
			else {
				BrowserCore.getDefault().getDatabase(parent.getCurrentConnection()).commit();
			}
			ui.getCancelButton().setEnabled(false);
			ui.getSaveButton().setEnabled(false);
		}
	};
	
	private SelectionListener cancelEdits = new SelectionAdapter() {
		public void widgetSelected(SelectionEvent e) {
			if(objectViewer!=null) {
				objectViewer.rollback();
			}
			else {
				BrowserCore.getDefault().getDatabase(parent.getCurrentConnection()).rollback();
			}
			ui.getCancelButton().setEnabled(false);
			ui.getSaveButton().setEnabled(false);
			parent.reopen();
		};
	};

	private IEditStateListener refreshListener = new IEditStateListener() {
		public void stateChanged(IObjectViewer sender) {
			refresh(sender);
		}
	};

    protected void refresh(IObjectViewer sender) {
		if (sender != objectViewer && !sender.isDirty()) {
			setInput(input, selection);
		}
	}

	private DisposeListener disposeListener = new DisposeListener() {
		public void widgetDisposed(DisposeEvent e) {
	        if (objectViewer != null) {
	        	objectViewer.removeObjectListener(RefreshService.getDefault());
		        parent.getEditStateController().removeObjectViewer(objectViewer);
	        }
	        RefreshService.getDefault().removeEditStateListener(refreshListener);
	    }
	};

	/* (non-Javadoc)
	 * @see com.db4o.browser.gui.controllers.IBrowserController#setInputcom.db4o.objectmanager.model.IGraphIteratorr)
	 */
	public void setInput(IGraphIterator input, GraphPosition selection) {
        disposeChildren(parentComposite);
        
        this.input = input;
        this.selection = selection;
		if (selection != null) {
			input.setPath(selection);
            
            // If the current element has children, we actually want to display/
            // edit the children of the current element
            if (input.nextHasChildren()) {
				input.selectNextChild();
                buildUI(input);
				input.selectParent();
			} else {
                buildUI(input);
			}
            
		}
		else {
			discardObjectViewer();
		}
	}
    
	private void buildUI(IGraphIterator input) {
		if (!input.hasParent()) {
			return;
		}
		
        // Get the parent object of the fields that we are editing
        input.selectParent();
        
        if (!input.hasNext()) {
        	return;
        }
        
        IModelNode parent = (IModelNode) input.next();
        input.previous();
        input.selectNextChild();
        
        // Remove the old state
        discardObjectViewer(); 

        // Create an ObjectViewer on the parent if it can be edited
        if (parent.getEditValue() != null) {
            IDatabase db = parent.getDatabase();
            objectViewer = construct(db, parent.getEditValue());
            this.parent.getEditStateController().addObjectViewer(objectViewer);
            objectViewer.addObjectListener(RefreshService.getDefault());
        }
        
        // Build the layout: start with the container Composite
        
        Composite detailViewHolder = new Composite(parentComposite, SWT.NULL);
        detailViewHolder.setLayout(new GridLayout(2, false));
        detailViewHolder.setBackground(WHITE);

        // Build each row by iterating over the fields
        while (input.hasNext()) {
            IModelNode fieldToDisplay = (IModelNode) input.next();
            
            // The field name...
            Label fieldName = new Label(detailViewHolder, SWT.NULL);
            fieldName.setBackground(WHITE);
            fieldName.setText(fieldToDisplay.getName());
            
            // Create an editor or a Label for the value
            if (fieldToDisplay.isEditable()) {
            	// If there's a field name, good, bind it.
            	if (!fieldToDisplay.getName().equals("")) {
	                Text fieldValue = new Text(detailViewHolder, SWT.BORDER);
	                fieldValue.setLayoutData(new GridData(SWT.FILL, SWT.CENTER, true, false));
	                fieldValue.setBackground(WHITE);
	                if (objectViewer.bind(fieldValue, fieldToDisplay.getName()) == null) {
	                    Logger.log().debug(getClass(), "Unable to bind property: " + fieldToDisplay.getName());
	                    fieldValue.dispose();
	                    createReadonlyField(detailViewHolder, fieldToDisplay);
	                }
	            // If there's not a field name, we've got a collection.  Display the field read-only for now.
	            // (The only type we could edit inside a collection is java.lang.String, which won't happen that often.)
            	} else {
                    createReadonlyField(detailViewHolder, fieldToDisplay);
            	}
            } else {
                createReadonlyField(detailViewHolder, fieldToDisplay);
            }
            
        }
        
        // We have to manually compute and set the size because we're inside
        // a ScrolledComposite here...
        Point parentPreferredSize = detailViewHolder.computeSize(parentComposite.getParent().getSize().x, SWT.DEFAULT, true);
        Point preferredSize = detailViewHolder.computeSize(SWT.DEFAULT, SWT.DEFAULT, true);
        if (parentPreferredSize.x > preferredSize.x)
            parentComposite.setBounds(new Rectangle(0, 0, parentPreferredSize.x, parentPreferredSize.y));
        else
            parentComposite.setBounds(new Rectangle(0, 0, preferredSize.x, preferredSize.y));
        parentComposite.layout(true);
    }


    private IObjectViewer construct(IDatabase db, Object editValue) {
        IObjectViewer editor = getEditorFactory(db).construct();
        editor.setInput(editValue);
        return editor;
    }
    IObjectViewerFactory editorFactory;
    private IObjectViewerFactory getEditorFactory(IDatabase db) {
        // this cast isn't very good, but just doing it for refactoring to start on OM2 since Db4oDatabase is the only implementation of IDatabase right now
        Db4oDatabase db4oDatabase = (Db4oDatabase) db;
        if(editorFactory == null){
            editorFactory = new Db4oObjectEditorFactory(db4oDatabase.getObjectContainer());
        }
        if(editorFactory instanceof Db4oObjectEditorFactory){
            // just going to set it here each time to make sure it's fresh
            Db4oObjectEditorFactory editorFactory2 = (Db4oObjectEditorFactory) editorFactory;
            editorFactory2.setDatabase(db4oDatabase.getObjectContainer());
        }
        return editorFactory;
    }

    private void discardObjectViewer() {
		if (objectViewer != null) {
        	objectViewer.removeObjectListener(RefreshService.getDefault());
	        this.parent.getEditStateController().removeObjectViewer(objectViewer);
        }
        objectViewer = null;
	}

	private void createReadonlyField(Composite detailViewHolder, IModelNode fieldToDisplay) {
		Label fieldValue = new Label(detailViewHolder, SWT.NULL);
		fieldValue.setLayoutData(new GridData(SWT.FILL, SWT.CENTER, true, false));
		fieldValue.setBackground(WHITE);
		fieldValue.setText(fieldToDisplay.getValueString());
	}

    private void disposeChildren(Composite parent) {
    	if (objectViewer != null) {
	    	objectViewer.removeObjectListener(RefreshService.getDefault());
	        this.parent.getEditStateController().removeObjectViewer(objectViewer);
	        objectViewer = null;
    	}
        
        Control[] children = parent.getChildren();
        for (int i = 0; i < children.length; i++) {
            children[i].dispose();
        }
    }

    public void deselectAll() {
        if (input != null)
            setInput(input, null);
    }

    public boolean canSelectionChange() {
    	if (objectViewer == null) {
    		return true;
    	}
    	
    	try {
    		objectViewer.commit();
    	} catch (CannotSaveException e) {
    		return false;
    	}
        return true;
    }

	public boolean canClose() {
		return canSelectionChange();
	}

}

