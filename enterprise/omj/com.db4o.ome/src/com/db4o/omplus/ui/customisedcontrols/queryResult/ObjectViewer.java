package com.db4o.omplus.ui.customisedcontrols.queryResult;

import java.util.*;

import org.eclipse.jface.action.*;
import org.eclipse.jface.dialogs.*;
import org.eclipse.jface.viewers.*;
import org.eclipse.swt.*;
import org.eclipse.swt.custom.*;
import org.eclipse.swt.events.*;
import org.eclipse.swt.widgets.*;
import org.eclipse.ui.*;

import com.db4o.omplus.*;
import com.db4o.omplus.datalayer.*;
import com.db4o.omplus.datalayer.queryresult.*;
import com.db4o.omplus.ui.interfaces.*;
import com.db4o.omplus.ui.model.*;
import com.db4o.omplus.ui.model.queryResults.*;

@SuppressWarnings("unused")
public class ObjectViewer extends CTabFolder
{
	private final String SET_NULL_MESSAGE = "This will set the object field to Null "
											+"and commit. Do you want to continue?";
	
	private final String SAVE_CHANGES_MESSAGE = "Do you want to save the changes made." +
												"Selecting 'No' will discard the changes?";
	
	private Hashtable<Object, Integer> objectViewerHashmap = new Hashtable<Object, Integer>(10);
	private QueryResultList queryResultList;
	
	private Action setToNullAction;
	
	private Composite parent;
	
	private QueryPresentationModel queryModel;

	public static final String TAB_NAME_BEGINS_WITH = "Object ";
	
	private final String SET_TO_NULL = "Set to Null";
	
	private ObjectTreeBuilder objectTreeBuilder;
	
	/**
	 * On editing a tree ask the parent to enable its Save button stored in the parent
	 */
	private IChildModifier childModifier;
	
	public ObjectViewer(QueryPresentationModel queryModel, Composite parent, int style,QueryResultList qList, IChildModifier childModifier) 
	{
		super(parent, style);
		this.queryModel = queryModel;
		this.parent = parent;
		this.addCTabFolder2Listener(new ObjectViewerTabCloseAdapter());
		queryResultList = qList;
		objectTreeBuilder = new ObjectTreeBuilder(Activator.getDefault().dbModel().db());
		this.childModifier = childModifier;
		
		
		//Add selection listener for eac tab item that would be added here
		this.addSelectionListener(new SelectionListener()
		{
			public void widgetDefaultSelected(SelectionEvent e) 
			{
			}

			public void widgetSelected(SelectionEvent e) 
			{
				saveEditedTab(true);
				ObjectViewerTab tab = (ObjectViewerTab)ObjectViewer.this.getSelection();
				String tabName = tab.getText();
				tabName = tabName.substring(TAB_NAME_BEGINS_WITH.length(), tabName.length());
				Integer integer = new Integer(tabName.trim());//System.out.println(integer.intValue());
				ObjectViewer.this.childModifier.objectTabSelectedInTree(integer.intValue());
			}
			
		});
		
	}
	
	private void saveEditedTab(boolean updateTree){
		if(isModified()){ // Commented for OMJ-126
//			if(showMessage(SAVE_CHANGES_MESSAGE)) {
				updateTheQueryListonSaveClick();
		/*	}
			else {
				clearModifiedlist();
			}*/
			((QueryResultTab)childModifier).modifyObjectViewerSaveButton(false);
		}
	}
	
	public void addTabsInObjectViewer(QueryResultRow row, String classname, String tabname)
	{
		//Check if new tab has to be added
		int index = checkIfObjectAlreadyDisplayed(row.getResultObj());
		if(index != -1)
		{
			this.setSelection(index);
			return;
		}
		
		Object object = row.getResultObj();	
		ObjectTreeNode treeNodes [] =  new ObjectTreeNode[]{objectTreeBuilder.getObjectTreeRootNode(classname, object)};	
		
		ObjectViewerTab tabItem = new ObjectViewerTab(this,SWT.CLOSE|SWT.BORDER, object,treeNodes);
		tabItem.setText(TAB_NAME_BEGINS_WITH + row.getId());
		addTableTree(tabItem, row,classname, treeNodes);
		objectViewerHashmap.put(row.getResultObj(), this.getItemCount()-1);
		tabItem.addDisposeListener(new DisposeListener(){

			public void widgetDisposed(DisposeEvent e) {
				saveEditedTab(false);
			}
			
		});
	}
	
	
	private int checkIfObjectAlreadyDisplayed(Object resultObj) 
	{
		Integer index = objectViewerHashmap.get(resultObj); 
		if(index!=null)
			return index.intValue();
		else
			return -1;
		
	}

	private void addTableTree( ObjectViewerTab tabItem, QueryResultRow row, String classname,ObjectTreeNode treeNodes [])
	{
		Tree tree = tabItem.getTree();
		TreeViewer treeViewer = tabItem.getTreeViewer();
		
		TextCellEditor textCellEditor = new TextCellEditor(treeViewer.getTree());
		String [] columnList = {"Field","Value","Type"};
		
		
		TreeViewerFocusCellManager focusCellManager = new TreeViewerFocusCellManager(treeViewer,new FocusCellOwnerDrawHighlighter(treeViewer));
		
		ColumnViewerEditorActivationStrategy actSupport = new ColumnViewerEditorActivationStrategy(treeViewer) 
		{
			protected boolean isEditorActivationEvent(ColumnViewerEditorActivationEvent event) 
			{
				return event.eventType == ColumnViewerEditorActivationEvent.MOUSE_DOUBLE_CLICK_SELECTION;
			}
		};
		
		
		TreeViewerEditor.create(treeViewer, focusCellManager, actSupport, 
								ColumnViewerEditor.TABBING_HORIZONTAL | ColumnViewerEditor.KEYBOARD_ACTIVATION);
		
		
		TreeViewerColumn column1 = null;
		for(int i = 0; i < columnList.length; i++)
		{
			column1= new TreeViewerColumn(treeViewer, SWT.NONE);
			String str = columnList[i];
	    	column1.getColumn().setText(str);
	    	column1.getColumn().setWidth(200);
	    	
	    	if(i==1 && !Activator.getDefault().dbModel().db().readOnly())//Allow editions only for value
	    	{
	    		column1.setEditingSupport(new ObjectTreeEditor(queryModel, this,tabItem,treeViewer,
	    									textCellEditor, queryResultList.getClassName(),
	    									queryResultList.getFieldNames(), row,
	    									childModifier));
	    	}
		}
		
		//Providers
		ObjectTreeContentProvider contentProvider = new ObjectTreeContentProvider(this,tabItem);
		treeViewer.setContentProvider(contentProvider);
		ObjectTreeLabelProvider labelProvider = new ObjectTreeLabelProvider(this,tabItem);
		treeViewer.setLabelProvider(labelProvider);
		treeViewer.setInput(treeNodes);
		
		// add drag support
	/*	int ops = DND.DROP_COPY | DND.DROP_MOVE;
	    Transfer[] transfers = new Transfer[] { TextTransfer.getInstance()};
	    treeViewer.addDragSupport(ops, transfers, new ObjectTreeDragListener(treeViewer));*/
		
		tabItem.setControl(tree);
		this.setSelection(this.getItemCount()-1);
		
		addSetToNullAction(treeViewer);
		addContextMenu(treeViewer);
	}
	
	private void addSetToNullAction(final TreeViewer objTree) 
	{
		if(Activator.getDefault().dbModel().db().readOnly()) {
			return;
		}
		final Tree tree = objTree.getTree();
		setToNullAction = new Action() 
		{
			public void run()
			{		
				TreeItem [] items = tree.getSelection();
				if(items != null)
				{
					int size = items.length;
					if(size == 1)
					{
						ObjectTreeNode node = (ObjectTreeNode)items[0].getData();
						int nodeType = node.getNodeType();
						String type = node.getType();
						if( nodeType != OMPlusConstants.PRIMITIVE || 
								type.equals(String.class.getName()) ||
								type.equals(Date.class.getName()) )
						{
							if(showMessage(SET_NULL_MESSAGE))
							{
								objectTreeBuilder.setNodeToNull(node);
								childModifier.objectTreeModified(false);
								objTree.refresh();
							}

						}
					}
				}
			}
		};
		setToNullAction.setText(SET_TO_NULL);
		setToNullAction.setToolTipText(SET_TO_NULL);
	}
	
	private boolean showMessage(String msg) {
		return MessageDialog.openQuestion(this.getShell(), OMPlusConstants.DIALOG_BOX_TITLE, msg);
	}
	
	private void addContextMenu(final TreeViewer objViewer) 
	{
		MenuManager menuMgr = new MenuManager("#TreeMenu");
		menuMgr.setRemoveAllWhenShown(true);
		menuMgr.addMenuListener(new IMenuListener() 
		{
			public void menuAboutToShow(IMenuManager manager) 
			{
				TreeItem [] items = objViewer.getTree().getSelection();
				if(items != null){
					int size = items.length;
					if(size == 1){
						ObjectTreeNode node = (ObjectTreeNode)items[0].getData();
						if( node == null || node.getParent() == null || 
								node.getParent().getNodeType() == OMPlusConstants.COLLECTION)
							return;
						ObjectTreeNode parent = node.getParent();
						int nodeType = node.getNodeType();
						String type = node.getType();
						if( nodeType != OMPlusConstants.PRIMITIVE ||
								type.equals(String.class.getName()) ||
								type.equals(Date.class.getName()) ){
							manager.add(setToNullAction);
							manager.add(new Separator(IWorkbenchActionConstants.MB_ADDITIONS));
						}
					}
				}
			}
		});
		Tree tree = objViewer.getTree();
		Menu menu = menuMgr.createContextMenu(tree);
		tree.setMenu(menu);		
	}
	
	public ObjectTreeBuilder getObjectTreeBuilder()
	{
		return objectTreeBuilder;
	}
	
	public boolean isModified(){
		return getObjectTreeBuilder().isObjectTreeModified();
	}
	
	public void updateTheQueryListonSaveClick()
	{
		objectTreeBuilder.writeToDB();
	}
	
	/**
	 * Some object deleted on QueryResult table. remove that particular item tab if shown
	 * @param resultObject
	 */
	public void refresh(Object resultObject) 
	{
		int index = checkIfObjectAlreadyDisplayed(resultObject);
		if(index != -1)
		{
			ObjectViewerTab item = (ObjectViewerTab)this.getItem(index);
			item.refresh();
		}		
	}
	
	// Currently added for Refresh functionality. Check if this can be avoided
	public void refresh() 
	{
		CTabItem []items = this.getItems();
		if(items != null){
			for(CTabItem tab : items){
				((ObjectViewerTab)tab).refresh();
			}
		}
	}
	
	public void clearModifiedlist() {
		getObjectTreeBuilder().refresh();		
	}
	
	/**
	 * Deelte a displayed tab
	 * @param resultObject
	 */
	public void deleteDisplayedObject(Object resultObject) 
	{
//		saveEditedTab(false);
		int index = checkIfObjectAlreadyDisplayed(resultObject);
		String deletedTabTxt  = null;
		if(index != -1)
		{
			ObjectViewerTab item = (ObjectViewerTab)this.getItem(index);
			deletedTabTxt = item.getText();
			item.dispose();
			objectViewerHashmap.remove(resultObject);
			//return;
		}
		int length = TAB_NAME_BEGINS_WITH.length(); 
		int delIdx = new Integer(deletedTabTxt.substring(length)).intValue();
		CTabItem [] items = this.getItems();
		String tabStr = null;
		for(CTabItem tab : items) {
			tabStr = tab.getText();
			Integer integer = new Integer(tabStr.substring(length)).intValue();
			if(integer > delIdx){
				tab.setText(TAB_NAME_BEGINS_WITH + (integer - 1));
			}
			
		}
		
	}
	////////////////////////////////////////InnerClass: for handling closing tab items
	class ObjectViewerTabCloseAdapter extends CTabFolder2Adapter
	{
		public void close(CTabFolderEvent event) 
		{
			ObjectViewerTab tab = (ObjectViewerTab) event.item;			
			objectViewerHashmap.remove(tab.getResultObject());
		}
	}
	/////////////////////////////////////////// End of inner class
			
}
