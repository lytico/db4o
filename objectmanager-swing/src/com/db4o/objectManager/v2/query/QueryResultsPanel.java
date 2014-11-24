/*
 * Copyright (c) 2001-2006 JGoodies Karsten Lentzsch. All Rights Reserved.
 *
 * Redistribution and use in source and binary forms, with or without 
 * modification, are permitted provided that the following conditions are met:
 * 
 *  o Redistributions of source code must retain the above copyright notice, 
 *    this list of conditions and the following disclaimer. 
 *     
 *  o Redistributions in binary form must reproduce the above copyright notice, 
 *    this list of conditions and the following disclaimer in the documentation 
 *    and/or other materials provided with the distribution. 
 *     
 *  o Neither the name of JGoodies Karsten Lentzsch nor the names of 
 *    its contributors may be used to endorse or promote products derived 
 *    from this software without specific prior written permission. 
 *     
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, 
 * THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR 
 * PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR 
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, 
 * EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, 
 * PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; 
 * OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
 * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE 
 * OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, 
 * EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE. 
 */

package com.db4o.objectManager.v2.query;

import com.db4o.objectManager.v2.uif_lite.panel.SimpleInternalFrame;
import com.db4o.objectManager.v2.custom.FastScrollPane;
import com.db4o.objectManager.v2.tree.ObjectTreeModel;
import com.db4o.objectManager.v2.tree.ObjectTreeNode;
import com.db4o.objectManager.v2.tree.ObjectTree;
import com.db4o.objectManager.v2.tree.ObjectTreeCellEditor;
import com.db4o.objectManager.v2.results.DateRenderer;
import com.db4o.objectManager.v2.UISession;
import com.db4o.objectManager.v2.TabType;
import com.db4o.ObjectContainer;
import com.db4o.objectmanager.model.IGraphIterator;
import com.jgoodies.forms.factories.Borders;

import javax.swing.*;
import javax.swing.tree.DefaultMutableTreeNode;
import javax.swing.tree.DefaultTreeCellEditor;
import javax.swing.tree.DefaultTreeCellRenderer;
import javax.swing.table.TableModel;
import javax.swing.table.TableColumn;
import java.awt.BorderLayout;
import java.awt.Component;
import java.awt.Dimension;
import java.awt.Color;
import java.util.Date;

public class QueryResultsPanel extends JPanel {
	private JTable resultsTable;
	private TableModel tableModel;
	private SimpleInternalFrame resultsFrame;
	private JLabel statusLabel = new JLabel();
	private UISession session;

	public QueryResultsPanel(UISession session) {
		super(new BorderLayout());
		this.session = session;
		setOpaque(false);
		setBorder(Borders.DIALOG_BORDER);
		add(buildTablePanel());
		add(buildStatusBar(), BorderLayout.SOUTH);
	}

	private JPanel buildStatusBar() {
		JPanel p = new JPanel();
		p.add(statusLabel);
		return p;
	}


	private JComponent buildTablePanel() {
		Component table = buildResultsTable();
		resultsFrame = new SimpleInternalFrame("Results");
		resultsFrame.setPreferredSize(new Dimension(300, 100));
		resultsFrame.add(table);
		return resultsFrame;
	}

	private JScrollPane buildResultsTable() {
		resultsTable = new JTable();
		resultsTable.setAutoResizeMode(JTable.AUTO_RESIZE_OFF);
		resultsTable.setDefaultRenderer(Date.class, new DateRenderer());
		JTextField tf = new JTextField();
		resultsTable.setDefaultEditor(Date.class, new DateEditor(tf));
		JScrollPane scrollpane = new FastScrollPane(resultsTable);
		scrollpane.setHorizontalScrollBarPolicy(JScrollPane.HORIZONTAL_SCROLLBAR_ALWAYS);
		scrollpane.setVerticalScrollBarPolicy(JScrollPane.VERTICAL_SCROLLBAR_ALWAYS);
		QueryResultsTableSelectionListener listener = new QueryResultsTableSelectionListener(resultsTable, this);
		resultsTable.addMouseListener(listener);
		return scrollpane;
	}

	/**
	 * After a query executes, this will setup the table to display results.
	 *
	 * @param query
	 */
	public void displayResults(String query) {
		try {
			tableModel = new QueryResultsTableModel(query, this);
			resultsTable.setModel(tableModel);
			//resultsTable.setA
		} catch (Exception e) {
			// don't display if there was an error
			e.printStackTrace();
		}
		// todo: should grow columns depending on avg column length
		if (resultsTable.getColumnCount() > 0) {
			TableColumn column = resultsTable.getColumnModel().getColumn(QueryResultsTableModel.COL_TREE);
			column.setPreferredWidth(20); // icon cell
			for (int i = 1; i < resultsTable.getColumnCount(); i++) {
				column = resultsTable.getColumnModel().getColumn(i);
				column.setPreferredWidth(100);
			}
		}
	}

	/**
	 * This method will batch up any changed objects until the user closes this panel, or clicks the Commit/Apply button
	 * <p/>
	 * NOTE: Currently commits immediately
	 *
	 * @param o
	 */
	public void addObjectToBatch(Object o) {
		// todo: implement the batch apply button
		session.getObjectContainer().set(o);
		session.getObjectContainer().commit();
	}

	public ObjectContainer getObjectContainer() {
		return session.getObjectContainer();
	}

	public void setStatusMessage(String msg) {
		statusLabel.setForeground(Color.BLACK);
		statusLabel.setText(msg);
	}

	public void setErrorMessage(String s) {
		statusLabel.setForeground(Color.RED);
		statusLabel.setText(s);
	}

	/**
	 * Will display a tree of the object in a tab.
	 *
	 * @param o object tree to display
	 */
	public void showObjectTree(Object o) {
		ObjectTreeNode top = new ObjectTreeNode(null, null, o);
		//IGraphIterator graphIterator = new ObjectGraphIterator(o, Db4oDatabase.getForObjectContainer(mainPanel.getObjectContainer(), mainPanel.getConnectionSpec()));
		//createNodes(top, graphIterator);
		ObjectTreeModel objectTreeModel = new ObjectTreeModel(top, session);
		ObjectTree tree = new ObjectTree(objectTreeModel);
		tree.setEditable(true);
		JTextField tf = new JTextField();
		DefaultTreeCellEditor editor = new DefaultTreeCellEditor(tree, (DefaultTreeCellRenderer) tree.getCellRenderer(), new ObjectTreeCellEditor(tf));
		tree.setCellEditor(editor);
		JScrollPane treeView = new JScrollPane(tree);
		session.addTab(TabType.objectTree, "Object: " + o, treeView);
	}

	private void createNodes(DefaultMutableTreeNode top, IGraphIterator iter) {
		while (iter.hasNext()) {
			if (iter.nextHasChildren()) {
				DefaultMutableTreeNode node = new DefaultMutableTreeNode(iter.selectNextChild());
				top.add(node);
			} else {
				DefaultMutableTreeNode node = new DefaultMutableTreeNode(iter.next());
				top.add(node);
			}
		}
		// no more next, so see if we can go back up tree
		if (iter.hasParent()) {
			iter.selectParent();

			// continue up a level
			createNodes(new DefaultMutableTreeNode(iter.next()), iter);
		}
	}

}