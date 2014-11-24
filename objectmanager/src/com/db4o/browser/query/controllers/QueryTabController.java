/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.browser.query.controllers;

import org.eclipse.swt.custom.CTabFolder;
import org.eclipse.swt.events.SelectionAdapter;
import org.eclipse.ve.sweet.CannotSaveException;

import com.db4o.ObjectSet;
import com.db4o.browser.gui.controllers.BrowserTabController;
import com.db4o.browser.gui.controllers.QueryController;
import com.db4o.objectmanager.model.BrowserCore;
import com.db4o.objectmanager.model.IDatabase;
import com.db4o.objectmanager.model.ObjectSetGraphIterator;
import com.db4o.objectmanager.model.query.QueryBuilderModel;
import com.db4o.browser.query.view.QueryBrowserPane;
import com.db4o.query.Query;
import com.db4o.reflect.ReflectClass;

/**
 * QueryTabController. Manages a single query tab. Creates the MVC relationship
 * for the query editor's model, view, and controller.
 * 
 * Upon request, gets the current query from the query model, runs it, and hands
 * the resulting IGraphIterator to the embedded browser's controller object.
 * 
 * @author djo
 */
public class QueryTabController extends BrowserTabController {
    
    private BrowserTabController databaseBrowserController;
    private IDatabase database; 
    
    private com.db4o.objectmanager.model.query.QueryBuilderModel queryModel;
    private QueryBuilderPaneController queryController;
    
    public QueryTabController(QueryController queryController, CTabFolder folder, QueryBrowserPane ui, ReflectClass clazz) {
        super(ui, queryController);
        this.databaseBrowserController = queryController.getBrowserController();
        currentConnection(databaseBrowserController.getCurrentConnection());
        this.database = BrowserCore.getDefault().getDatabase(databaseBrowserController.getCurrentConnection());
        
        // Also enable/disable the query button based on editor state
        getEditStateController().addControl(ui.getQueryButton(), false);
    }
    
    protected void addQueryButtonHandler() {
        ui.getQueryButton().addSelectionListener(new SelectionAdapter() {
            public void widgetSelected(org.eclipse.swt.events.SelectionEvent e) {
                try {
                    queryController.save();
                    databaseBrowserController.deselectAll();
                    runQuery();
                } catch (CannotSaveException e1) {
                    // We couldn't save, so do nothing.  The user has already
                    // been informed of the problem.
                }
            }
        });
    }

    protected void runQuery() {
        Query query = queryModel.getQuery();
        ObjectSet results = query.execute();
        setInput(new ObjectSetGraphIterator(database, results), null);
    }

    public void setInput(ReflectClass input) {
        queryModel = new QueryBuilderModel(input, database);
        queryController = new QueryBuilderPaneController(queryModel, (QueryBrowserPane)ui);
        
        runQuery();
    }

    protected boolean internalSetInput() {
    	runQuery();
    	return true;
    }
}
