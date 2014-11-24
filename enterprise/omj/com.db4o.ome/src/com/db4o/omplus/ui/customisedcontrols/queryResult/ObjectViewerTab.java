package com.db4o.omplus.ui.customisedcontrols.queryResult;

import org.eclipse.jface.viewers.TreeViewer;
import org.eclipse.swt.SWT;
import org.eclipse.swt.custom.CTabFolder;
import org.eclipse.swt.custom.CTabItem;
import org.eclipse.swt.widgets.Tree;

import com.db4o.omplus.datalayer.queryresult.ObjectTreeNode;

public class ObjectViewerTab extends CTabItem
{
	Object resultObject;
	/**
	 * resultObject in TreeNodes form
	 */
	ObjectTreeNode treeNodes [];
	
	private CTabFolder parent;
	private Tree tree;
	private TreeViewer treeViewer; 
	
	//TODO: as of now kept...if not needed delete
	boolean isEdited = false;
	
	public ObjectViewerTab(CTabFolder parent, int style, Object obj,ObjectTreeNode tn []) 
	{
		super(parent, style);
		this.resultObject = obj;		
		this.treeNodes = tn;	
		this.parent = parent;
		
		initializeComponents();		
	}

	/**
	 * Initialize the tree and tree viewer
	 */
	private void initializeComponents()
	{
		this.tree = new Tree(parent, SWT.BORDER | SWT.H_SCROLL | SWT.V_SCROLL|SWT.FULL_SELECTION);
		tree.setHeaderVisible(true);
		tree.setLinesVisible(true);		
		this.treeViewer = new TreeViewer(tree);		
	}
	
	/**
	 * Get teh tree
	 * @return
	 */
	public Tree getTree()
	{
		return this.tree;
	}
	
	/**
	 * get teh Tree viewer
	 * @return
	 */
	public TreeViewer getTreeViewer()
	{
		return this.treeViewer;
	}

	public Object getResultObject()
	{
		return resultObject;
	}

	public void setIsEdited(boolean b)
	{
		isEdited = b;		
	}
	
	public boolean getIsEdited()
	{
		return isEdited;
	}

	
	public void refresh()
	{
		treeViewer.refresh();
	}
}
