/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.browser.gui.controllers;

import java.io.*;
import java.util.*;
import java.util.List;

import org.eclipse.swt.*;
import org.eclipse.swt.events.*;
import org.eclipse.swt.widgets.*;
import org.eclipse.ve.sweet.controllers.*;
import org.eclipse.ve.sweet.controllers.swt.*;
import org.eclipse.ve.sweet.metalogger.*;

import com.db4o.browser.gui.controllers.tree.*;
import com.db4o.browser.gui.dialogs.*;
import com.db4o.browser.gui.views.*;
import com.db4o.objectmanager.model.nodes.*;
import com.db4o.objectmanager.model.IGraphIterator;
import com.db4o.objectmanager.model.GraphPosition;
import com.db4o.objectmanager.model.BrowserCore;
import com.db4o.objectmanager.model.Db4oConnectionSpec;
import com.db4o.browser.prefs.PreferencesCore;
import com.db4o.browser.prefs.RecentlyOpenedPreferences;
import com.db4o.reflect.*;

/**
 * BrowserTabController.  The root MVC Controller for a browser (root) tab.
 *
 * @author djo
 */
public class BrowserTabController implements IBrowserController {
    
    protected DbBrowserPane ui;
    protected QueryController queryController;  // Used for running queries

    private Db4oConnectionSpec currentConnection;
    
	private TreeController treeController;
	private DetailController detailController;
	private SelectionChangedController selectionChangedController;
	private NavigationController navigationController;
	private PathLabelController pathController;
    private SearchController searchController;
    private IEditStateController editStateController;

    private IGraphIterator input = null;
    private GraphPosition initialSelection;

	/**
     * Constructor BrowserController.  Create a BrowserController for a
     * particular user interface.
	 * @param ui The DbBrowserPane to use as for the user interface
	 * @param queryController The QueryController used for opening queries
	 */
	public BrowserTabController(DbBrowserPane ui, final QueryController queryController) {
        this.ui = ui;
        this.queryController = queryController;

		// Initialize the ObjectTree's controllers
		selectionChangedController = new SelectionChangedController();
		
		treeController = new TreeController(this, ui.getObjectTree(), ui.getDeleteButton());
		detailController = new DetailController(this, ui);
		navigationController = new NavigationController(ui.getLeftButton(), ui.getRightButton());
        searchController = new SearchController(this, ui, navigationController);
		pathController = new PathLabelController(ui.getPathLabel());
		
		editStateController = new SWTEditStateController();
		editStateController.addControl(ui.getCancelButton(), true);
		editStateController.addControl(ui.getSaveButton(), true);
		
        addQueryButtonHandler();
	}
	
	public void dirty() {
		ui.getSaveButton().setEnabled(true);
		ui.getCancelButton().setEnabled(true);
		reopen();
	}
	
	public IEditStateController getEditStateController() {
		return editStateController;
	}

	protected void addQueryButtonHandler() {
        ui.getQueryButton().addSelectionListener(new SelectionAdapter() {
            public void widgetSelected(SelectionEvent e) {
                newQuery();
            }
        });
    }
    
    public void newQuery() {
        if (!BrowserCore.getDefault().isOpen()) {
            MessageBox error = new MessageBox(ui.getShell(), SWT.ICON_ERROR);
            error.setMessage("Please open a database before querying.");
            error.setText("Error");
            error.open();
            return;
        }
        ReflectClass toOpen = chooseClass();
        if (toOpen != null) {
            queryController.open(toOpen, currentConnection.path());
        }
    }
    
    public boolean open(Db4oConnectionSpec spec){
        currentConnection(spec);
        PreferencesCore.getDefault().registerPreference(RecentlyOpenedPreferences.RECENTLY_OPENED_PREFERENCES_ID, spec);
        PreferencesCore.getDefault().commit();
        //System.out.println(PreferencesCore.getDefault().getPreference(RecentlyOpenedPreferences.RECENTLY_OPENED_PREFERENCES_ID));
        return internalSetInput();
    }

    public void reopen() {
        internalSetInput();
    }

    protected void currentConnection(Db4oConnectionSpec connection) {
    	currentConnection=connection;
    }
    
    protected boolean internalSetInput() {
        IGraphIterator i=null;
        try {
            i = BrowserCore.getDefault().iterator(currentConnection);
        } catch (Exception e) {
            Logger.log().error(e, "Unexpected exception opening connection");
            MessageBox messageBox = new MessageBox(ui.getShell(), SWT.ICON_ERROR);
            messageBox.setText("Error");
            messageBox.setMessage(e.getClass().getName() + ": " + e.getMessage());
            messageBox.open();
            return false;
        }
        setInput(i, null);
        return true;
    }

    /**
     * Displays a class selection dialog box and allows the user to select a
     * class.
     * 
     * @return The class the user chose or null if the user did not choosse one.
     */
    public ReflectClass chooseClass() {
        final IGraphIterator iterator = BrowserCore.getDefault().iterator(currentConnection);
        ListSelector dialog = new ListSelector(ui.getShell());
        dialog.setText("Query a type");
        final HashMap choices = new HashMap();
        dialog.setListPopulator(new IListPopulator() {
            public void populate(List list) {
                int position=0;
                while (iterator.hasNext()) {
                    ClassNode node = (ClassNode) iterator.next();
                    ReflectClass clazz = node.getReflectClass();
                    list.add(clazz.getName());
                    choices.put(new Integer(position), clazz);
                    ++position;
                }
            }
        });
        dialog.open();
        if (dialog.getSelection() >= 0) {
            return (ReflectClass) choices.get(new Integer(dialog.getSelection()));
        } else {
            return null;
        }
    }
	
	/**
     * @return Returns the initialSelection.
     */
    public GraphPosition getInitialSelection() {
        return initialSelection;
    }
    

    /**
     * @return Returns the input.
     */
    public IGraphIterator getInput() {
        return input;
    }
    

    /* (non-Javadoc)
	 * @see com.db4o.browser.gui.controllers.IBrowserController#ocom.db4o.objectmanager.model.IGraphIteratorator)
	 */
	public void setInput(IGraphIterator input, GraphPosition selection) {
        this.input = input;
        this.initialSelection = selection;
        
		// Set the various sub-controllers' inputs
		treeController.setInput(input, selection);
		detailController.setInput(input, selection);
		navigationController.setInput(input, selection);
		pathController.setInput(input, selection);
	}

	/**
	 * Returns the SelectionChangedController for this window.
	 * 
	 * @return SelectionChangedController the current SelectionChangedController
	 */
	public SelectionChangedController getSelectionChangedController() {
		return selectionChangedController;
	}

    /**
     * @return Returns the queryController.
     */
    public QueryController getQueryController() {
        return queryController;
    }
    
    public Db4oConnectionSpec getCurrentConnection() {
        return currentConnection;
    }

    /**
     * Remove any selection from the tree
     */
    public void deselectAll() {
        treeController.deselectAll();
        detailController.deselectAll();
        navigationController.resetUndoRedoStack();
    }

	public boolean canClose() {
		return detailController.canClose();
	}

	public void xmlExport(String file) {
		try {
			new XMLExporter(file, input).export();
		} catch (FileNotFoundException e) {
			MessageBox messageBox = new MessageBox(ui.getShell(), SWT.ICON_ERROR | SWT.OK);
			messageBox.setMessage("Unable to export to " + file + "\n" + e.getMessage());
			messageBox.open();
		}
	}

}
