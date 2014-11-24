/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.browser.gui.controllers;

import org.eclipse.swt.events.KeyAdapter;
import org.eclipse.swt.events.KeyEvent;
import org.eclipse.swt.events.SelectionAdapter;
import org.eclipse.swt.widgets.Button;
import org.eclipse.swt.widgets.Text;
import org.eclipse.swt.widgets.Tree;
import org.eclipse.swt.widgets.TreeItem;

import com.db4o.browser.gui.views.DbBrowserPane;
import com.db4o.objectmanager.model.GraphPosition;
import com.db4o.objectmanager.model.IGraphIterator;

public class SearchController implements IBrowserController {

    private BrowserTabController parent;
    private DbBrowserPane ui;
    private Tree tree;
    private Button searchButton;
    private Text searchText;

    public SearchController(BrowserTabController parent, DbBrowserPane ui, NavigationController navigationController) {
        this.parent = parent;
        this.ui = ui;
        this.tree = ui.getObjectTree();
        this.searchText = ui.getSearch();
        this.searchButton = ui.getSearchButton();
        
        searchText.addKeyListener(new KeyAdapter() {
            public void keyReleased(KeyEvent e) {
                if (e.character == '\r' || e.character == '\n') {
                    search();
                }
            }
        });
        
        searchButton.addSelectionListener(new SelectionAdapter() {
            public void widgetSelected(org.eclipse.swt.events.SelectionEvent e) {
                search();
            }
        });
    }

    protected TreeItem getSelection() {
        TreeItem[] selection = tree.getSelection();
        if (selection.length == 0)
            return null;
        return selection[0];
    }

    private void search() {
        startingWith = getSelection();
        found = false;
        findNext(tree.getItems(), searchText.getText());
    }

    private TreeItem startingWith = null;
    private boolean found = false;
    
    protected void findNext(TreeItem[] items, String text) {
        for (int i = 0; i < items.length && !found; i++) {
            if (startingWith != null) {
                if (items[i].equals(startingWith)) {
                    startingWith = null;
                }
                TreeItem[] subitems = items[i].getItems();
                if (subitems.length > 0)
                    findNext(subitems, text);
            } else {
                if (items[i].getText().indexOf(text) >= 0) {
                    found = true;
                    tree.setSelection(new TreeItem[] {items[i]});
                } else {
                    TreeItem[] subitems = items[i].getItems();
                    if (subitems.length > 0)
                        findNext(subitems, text);
                }
            }
        }
    }
    
    public void setInput(IGraphIterator input, GraphPosition selection) {
        // Nothing needed here
    }
}
