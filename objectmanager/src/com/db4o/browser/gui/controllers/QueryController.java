/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.browser.gui.controllers;

import java.io.File;
import java.util.HashMap;
import java.util.Iterator;

import org.eclipse.swt.SWT;
import org.eclipse.swt.custom.CTabFolder;
import org.eclipse.swt.custom.CTabFolder2Adapter;
import org.eclipse.swt.custom.CTabFolderEvent;
import org.eclipse.swt.custom.CTabItem;
import org.eclipse.swt.graphics.Image;
import org.eclipse.swt.widgets.Display;

import com.db4o.browser.gui.views.DbBrowserPane;
import com.db4o.browser.query.controllers.QueryTabController;
import com.db4o.browser.query.view.QueryBrowserPane;
import com.db4o.reflect.ReflectClass;

/**
 * QueryController. Opens a new query tab and supplies information about the
 * opened database to the query browser in that tab.
 *
 * @author djo
 */
public class QueryController {

    private CTabFolder folder;
    private BrowserTabController browserController;
    protected HashMap queryTabControllerRegistry = new HashMap();

    public QueryController(CTabFolder folder) {
        this.folder = folder;

        folder.addCTabFolder2Listener(new CTabFolder2Adapter() {
        	public void close(CTabFolderEvent event) {
        		QueryTabController controller = (QueryTabController) queryTabControllerRegistry.get(event.item);
        		if (!controller.canClose()) {
        			event.doit = false;
        			return;
        		}
        		queryTabControllerRegistry.remove(event.item);
        	}
        });
    }
    
	/**
	 * Forcably close the specified query tab.  Unsaved changes will be lost.
	 * 
	 * @param item The CTabItem referring to the query tab.
	 */
	public void close(CTabItem item) {
		queryTabControllerRegistry.remove(item);
		item.getControl().dispose();
		item.dispose();
	}

	public void setBrowserController(BrowserTabController browserController) {
        this.browserController = browserController;
    }
    
    public void open(ReflectClass clazz, String connectionName) {
        QueryBrowserPane ui = new QueryBrowserPane(folder, SWT.NULL);
        CTabItem queryTab = new CTabItem(folder, SWT.CLOSE);
        queryTab.setControl(ui);
        queryTab.setImage(new Image(Display.getCurrent(),
                DbBrowserPane.class.getResourceAsStream("icons/etool16/query.gif")));
        queryTab.setText(unqualifyFile(connectionName) + "::" + unqualifyClass(clazz.getName()));
        folder.setSelection(queryTab);
        
        QueryTabController controller = new QueryTabController(this, folder, ui, clazz);
        controller.setInput(clazz);
        queryTabControllerRegistry.put(queryTab, controller);
    }

    private String unqualifyFile(String fileName) {
        File file = new File(fileName);
        fileName = file.getName();
        final int lastDot = fileName.lastIndexOf('.');
        if (lastDot >= 1) {
            fileName = fileName.substring(0, lastDot);
        }
        return fileName;
    }

    private String unqualifyClass(String name) {
        name = name.substring(name.lastIndexOf('.')+1);
        int commaPos = name.indexOf(',');
        if (commaPos >= 0) {
            name = name.substring(0, commaPos);
        }
        return name;
    }

    public boolean canClose() {
    	for (Iterator queryControllerIter = queryTabControllerRegistry.keySet().iterator(); queryControllerIter.hasNext();) {
			QueryTabController controller = (QueryTabController) queryTabControllerRegistry.get(queryControllerIter.next());
			if (!controller.canClose()) {
				return false;
			}
		}
    	return true;
    }

    /**
     * @return Returns the browserController.
     */
    public BrowserTabController getBrowserController() {
        return browserController;
    }

    

}
