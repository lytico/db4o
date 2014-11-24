package com.db4o.omplus.ui;

import java.util.*;
import java.util.List;

import org.eclipse.jface.action.*;
import org.eclipse.jface.resource.*;
import org.eclipse.jface.viewers.*;
import org.eclipse.swt.*;
import org.eclipse.swt.dnd.*;
import org.eclipse.swt.events.*;
import org.eclipse.swt.graphics.*;
import org.eclipse.swt.layout.*;
import org.eclipse.swt.widgets.*;
import org.eclipse.ui.*;
import org.eclipse.ui.part.*;

import com.db4o.omplus.*;
import com.db4o.omplus.connection.*;
import com.db4o.omplus.datalayer.*;
import com.db4o.omplus.datalayer.classviewer.*;
import com.db4o.omplus.datalayer.queryresult.*;
import com.db4o.omplus.ui.model.*;


public class ClassViewer extends ViewPart
{	
	public static final String FLAT_VIEW = "Class View";
	public static final String HIERARCHICAL_VIEW = "Package View";
	
	private final String VIEW_ALL_OBJS = "View All Objects";
//	private final String ARRAYS = "(GA)";
	private final String SEARCH_STR = "Search";
//	private final int HEIGHT = 6;
//	private final int WIDTH = 6;

	private StringPattern strPattern;
	private RecentSearchKeys recentSearchKeys;

	// ALL Jface And SWT Widgets
	private TreeViewer	classTree;
	private Composite	parentComp;
	private Composite	searchComp;
	private Combo	searchCombo;
	private Button searchBtn;
	private Button	clearButton;
	private Button	nextButton;
	private Button	prevButton;
	private Label	filterlabel;
	
	// Pop Up Actions for ClassViewer
	private Action showPackageViewAction;
	private Action showClassViewAction;
	private Action viewAllObjsAction;
	
	private String currentView;

	@Override
	public void createPartControl(Composite parent) 
	{
		parentComp = new Composite(parent, SWT.BORDER);
		searchComp = new Composite(parentComp, SWT.NONE);
		classTree = new TreeViewer(parentComp,SWT.V_SCROLL | SWT.H_SCROLL | SWT.BORDER);
		
		strPattern = new StringPattern();
		recentSearchKeys = new RecentSearchKeys();

		// For the search function
		filterlabel = new Label(searchComp, SWT.NONE);
		filterlabel.setText(" Filter ");
		searchCombo = new Combo(searchComp, SWT.DROP_DOWN);
		searchCombo.setToolTipText("Filter");
		int size = setItemsForCombo();
		
		searchBtn = new Button(searchComp, SWT.PUSH);
		searchBtn.setText(SEARCH_STR);
		searchBtn.setToolTipText(SEARCH_STR);
		
		clearButton = new Button(searchComp, SWT.NONE);
		clearButton.setToolTipText(" Clear ");clearButton.setText("Clear");

		prevButton = new Button(searchComp, SWT.NONE);
		prevButton.setToolTipText(" Previous ");
		prevButton.setImage(ImageUtility.getImage(OMPlusConstants.SEARCH_PREV_ICON));
		
		nextButton = new Button(searchComp, SWT.NONE);
		nextButton.setToolTipText(" Next ");
		nextButton.setImage(ImageUtility.getImage(OMPlusConstants.SEARCH_NEXT_ICON));
		
		enableSrchBtns(size);
		setViewLayout(parentComp);
		
		//TODO: move to a diff function
		ClassViewerContentProvider classViewerContentProvider = new ClassViewerContentProvider(strPattern);
		ClassViewerLabelProvider labelProvider = new ClassViewerLabelProvider(currentView);
		classTree.setContentProvider(classViewerContentProvider);
		classTree.setLabelProvider(labelProvider);
		makeActions();
//		TODO: remove this
		ConnectionStatus status = new ConnectionStatus();
		if(status.isConnected()){
			setCurrentView();
			setInputForClassTree(currentView);
		}else
			enableButtons(false);
		
		int ops = DND.DROP_COPY | DND.DROP_MOVE;
	    Transfer[] transfers = new Transfer[] { TextTransfer.getInstance()};
		classTree.addDragSupport(ops, transfers, new TreeDragListener(classTree));
		
		//Listener for Text
		addListenerForSearch();
					
		addListenerForBtns();
		
		addContextMenu();
		IMenuManager mgr = getViewSite().getActionBars().getMenuManager();
		mgr.add(showClassViewAction);
		mgr.add(showPackageViewAction);

		addListenerForTree();
	}

	private void enableButtons(boolean enable){
		searchBtn.setEnabled(enable);
		clearButton.setEnabled(enable);
		nextButton.setEnabled(enable);
		prevButton.setEnabled(enable);
	}
	
	private String[] getSearchKeywords() {
		List<String> list = recentSearchKeys.getSearchKeysForDB();
		int size = list.size();  
		String [] queryStrings = new String[size];
		if(size > 0){
			int count = size - 1;
			ListIterator<String> iterator = list.listIterator();
			while(iterator.hasNext()){
				queryStrings[count--] = (iterator.next()).toString();
			}
		}
		return queryStrings;
	}

	private boolean setInputForClassTree(String currentView) {
		try{
			classTree.setInput(currentView);
			return true;
		}catch(Exception ex){
			Activator.getDefault().queryModel().err().error(ex);
			return false;
		}
	}

	private void addListenerForSearch() {

		searchCombo.addKeyListener(new KeyAdapter(){

			public void keyPressed(KeyEvent e) {
				if(e.character == SWT.CR){
					valueChanged();
				}
			}
		});
		
		searchCombo.addSelectionListener(new SelectionListener(){

			public void widgetDefaultSelected(SelectionEvent e) {
			}

			public void widgetSelected(SelectionEvent e) {
				valueChanged();
			}
			
		});
		
		searchBtn.addSelectionListener(new SelectionListener(){

			public void widgetDefaultSelected(SelectionEvent e) {
			}

			public void widgetSelected(SelectionEvent e) {
				valueChanged();
			}
			
		});
		
	}

	private void addListenerForBtns() {
		clearButton.addSelectionListener(new SelectionListener(){

			public void widgetDefaultSelected(SelectionEvent e) {
			}

			public void widgetSelected(SelectionEvent e) {
//				Requires both for UI clear
				searchCombo.deselect(searchCombo.getSelectionIndex());
				searchCombo.setText("");
				strPattern.setPattern("");
				classTree.refresh();
				enableSrchBtns(searchCombo.getItemCount());
				updatePropertiesView(null, false);
			}
			
		});
		
		nextButton.addSelectionListener(new SelectionListener(){

			public void widgetDefaultSelected(SelectionEvent e) {
			}

			public void widgetSelected(SelectionEvent e) {
				int selectedIndex = searchCombo.getSelectionIndex() + 1;
				actionForBtn(selectedIndex);
				updatePropertiesView(null, false);
			}
	
		});
		
		prevButton.addSelectionListener(new SelectionListener(){

			public void widgetDefaultSelected(SelectionEvent e) {
			}

			public void widgetSelected(SelectionEvent e) {
				int selectedIndex = searchCombo.getSelectionIndex() - 1;
				actionForBtn(selectedIndex);
				updatePropertiesView(null, false);
			}
			
		});
		
	}
	
	private void actionForBtn(int selectedIndex) {
		int length = getSearchKeywords().length;
		if(length > 0){
			if(selectedIndex > -1 && selectedIndex < length ){
				
				strPattern.setPattern(searchCombo.getItem(selectedIndex));
				classTree.refresh();
				select(selectedIndex);
				enableSrchBtns(length);
				updatePropertiesView(null, false);
//				valueChanged();
			}
		}
	}
	
	private void select(int selectedIndex) {
		searchCombo.select(selectedIndex);
	}
	
	private void enableSrchBtns(int length) {
		if(length > 1){
			int selectedIdx = searchCombo.getSelectionIndex();
			if(selectedIdx <= 0){
				enableBtn(nextButton, true);
				enableBtn(prevButton, false);
			}
			else if(selectedIdx == length-1) {
				enableBtn(nextButton, false);
				enableBtn(prevButton, true);
			}
			else{
				enableBtn(nextButton, true);
				enableBtn(prevButton, true);
			}
			
		} else {
			enableBtn(nextButton, false);
			enableBtn(prevButton, false);
		}
	}
	
	private void enableBtn(Button button, boolean enable) {
		button.setEnabled(enable);
	}

	private void setCurrentView() {
		if(currentView == null || currentView.equals(FLAT_VIEW)){
			currentView = FLAT_VIEW;
			showPackageViewAction.setEnabled(true);
			showClassViewAction.setEnabled(false);
		} else {
			showPackageViewAction.setEnabled(false);
			showClassViewAction.setEnabled(true);
		}
	}

	private void makeActions() {
		addViewAllObjsAction();
		addShowPkgViewAction();
		addShowClsViewAction();
	}

	private void addShowClsViewAction() {
		showClassViewAction = new Action() 
		{
			public void run() {		
				setInputForClassTree(FLAT_VIEW);
				updatePropertiesView(null, false);
				showPackageViewAction.setEnabled(true);
				showClassViewAction.setEnabled(false);
			}
		};
		showClassViewAction.setText(FLAT_VIEW);
		showClassViewAction.setToolTipText(FLAT_VIEW);
		showClassViewAction.setImageDescriptor(
				getImageDescriptor(OMPlusConstants.FLAT_VIEW_ICON));
//		classTree.refresh();
	}

	private void addShowPkgViewAction() {
		showPackageViewAction = new Action() 
		{
			public void run() {		
				setInputForClassTree(HIERARCHICAL_VIEW);
				updatePropertiesView(null, false);
				showPackageViewAction.setEnabled(false);
				showClassViewAction.setEnabled(true);
			}
		};
		showPackageViewAction.setText(HIERARCHICAL_VIEW);
		
		showPackageViewAction.setImageDescriptor(getImageDescriptor(OMPlusConstants.HIERARCHICAL_VIEW_ICON));
		showPackageViewAction.setToolTipText(HIERARCHICAL_VIEW);
	}
	
	private ImageDescriptor getImageDescriptor(int index){
		Image image = ImageUtility.getImage(index);
		ImageDescriptor desc = ImageDescriptor.createFromImage(image);
		return desc;
	}

	private void addListenerForTree() {
		final Tree tree = getClassTree();
		tree.addSelectionListener(new SelectionListener(){

			public void widgetDefaultSelected(SelectionEvent e) {
				
			}

			public void widgetSelected(SelectionEvent e) 
			{
				TreeItem [] items = tree.getSelection();
				if(items != null)
				{
					int size = items.length;
					if(size == 1)
					{
						ClassTreeNode node = (ClassTreeNode)items[0].getData();
						if(node.getNodeType() == OMPlusConstants.CLASS_NODE)
						{
							//UPdate the properties view
							updatePropertiesView(node.getName(), false);
						}else
						{

							if(node.getNodeType() == OMPlusConstants.CLASS_FIELD_NODE)
							{
								updatePropertiesView(getClsName(node.getName()), false);
							}
						}
					}
				}
			}
		});
		
		tree.addMouseListener(new MouseAdapter() {
//		for expanding and collapsing items
				public void mouseDoubleClick(MouseEvent e) {
				Object obj = e.getSource();
				if(obj instanceof Tree){
					TreeItem[] item = ((Tree)obj).getSelection();
					if(item != null && item.length == 1) {
						TreeItem[] children = item[0].getItems();
						if( !item[0].getExpanded()) { 
							if(children != null && children.length > 0){
								((Tree)obj).showItem(children[0]);
							}
						} else{
							item[0].setExpanded(false);
						}
					}
				}
			}
		});
		
		tree.addTreeListener(new TreeListener()
		{
			public void treeCollapsed(TreeEvent e) {
				TreeItem item = (TreeItem)e.item;
				if(item != null){
					setPropertiesForNodeSelected(item);
				}
			}

			public void treeExpanded(TreeEvent e) {
				TreeItem item = (TreeItem)e.item;
				if(item != null){
					setPropertiesForNodeSelected(item);
				}
			}
		});
	}
	
	private void setPropertiesForNodeSelected(TreeItem item) {
		String className = null;
		ClassTreeNode node = (ClassTreeNode)item.getData();
		if(node != null)
		{
			int nodeType  = node.getNodeType();
			if(nodeType == OMPlusConstants.PACKAGE_NODE)
				return;
			else if(nodeType == OMPlusConstants.CLASS_NODE)
			{
				className = node.getName();
				getClassTree().setSelection(item);
			}
			else if(nodeType == OMPlusConstants.CLASS_FIELD_NODE)
			{
				className = getClsName(node.getName());
				int count = node.getName().split(OMPlusConstants.REGEX).length;
				int i = 0;
				TreeItem classItem = item;
				if(count == 2)
					classItem = item.getParentItem();
				else{
					while(i < (count - 1) && classItem != null){
						classItem = classItem.getParentItem();
						i++;
					}
				}
				if(classItem != null)
					getClassTree().setSelection(classItem);
			}
			updatePropertiesView(className, false);
		}
	}

	private void setViewLayout(Composite parent) {
		FormLayout formLayout = new FormLayout();
		parent.setLayout(formLayout);
		setLayoutForControls();
	}

	private void setLayoutForControls() {
		
		FormData data = new FormData();
		data.left = new FormAttachment(0);
		data.right = new FormAttachment(100);
		data.top = new FormAttachment(0);
		searchComp.setLayoutData(data);
		
		Tree tree = classTree.getTree();
		FormData treeData = new FormData();
		treeData.left = new FormAttachment(0);
		treeData.top = new FormAttachment(searchComp, 3);
		treeData.right = new FormAttachment(100);
		treeData.bottom = new FormAttachment(100);
		tree.setLayoutData(treeData);

		// Layout for searchComp
		searchComp.setLayout(new FormLayout());
		
		FormData textData = new FormData();
		textData.left = new FormAttachment(0, 3);
		textData.bottom = new FormAttachment(100);
		textData.top = new FormAttachment(0, 10);
		filterlabel.setLayoutData(textData);
		
		data = new FormData();
		data.left = new FormAttachment(filterlabel, 3);
		data.bottom = new FormAttachment(100);
		data.right = new FormAttachment(40);
		data.top = new FormAttachment(0, 7);
		searchCombo.setLayoutData(data);
		
		FormData findData = new FormData();
		findData.left = new FormAttachment(searchCombo, 5);
		findData.bottom = new FormAttachment(80);
		findData.top = new FormAttachment(0, 5);
		searchBtn.setLayoutData(findData);
		
		findData = new FormData();
		findData.left = new FormAttachment(searchBtn, 10);
		findData.bottom = new FormAttachment(80);
		findData.top = new FormAttachment(0, 5);
		clearButton.setLayoutData(findData);
	
		FormData nxtData = new FormData();
		nxtData.left = new FormAttachment(clearButton, 10);
		nxtData.bottom = new FormAttachment(80);
//		nxtData.right = new FormAttachment(15);
		nxtData.top = new FormAttachment(0, 5);
		prevButton.setLayoutData(nxtData);
		
		FormData prevData = new FormData();
		prevData.left = new FormAttachment(prevButton, 5);
		prevData.bottom = new FormAttachment(80);
//		prevData.right = new FormAttachment(15);
		prevData.top = new FormAttachment(0, 5);
		nextButton.setLayoutData(prevData);
		
	}
	
	private void valueChanged() {
		String str = searchCombo.getText();
		if(str == null)
			return;
		int length = str.length();
		if(length > 0)
		{
			recentSearchKeys.addNewSearchKey(str);
			int size = setItemsForCombo();
			searchCombo.select(0);
			enableSrchBtns(size);
		}
		strPattern.setPattern(str);
		classTree.refresh();
	}
	

	@Override
	public void setFocus() 
	{
		
	}
	
	private void addViewAllObjsAction() 
	{
		final Tree tree = getClassTree();
		viewAllObjsAction = new Action() 
		{
			public void run()
			{		
				TreeItem [] items = tree.getSelection();
				if(items != null)
				{
					int size = items.length;
					if(size == 1)
					{
						ClassTreeNode node = (ClassTreeNode)items[0].getData();
						if(node.getNodeType() == OMPlusConstants.CLASS_NODE){
							runQuery(node.getName());
						}
					}
				}
			}
		};
		viewAllObjsAction.setText(VIEW_ALL_OBJS);
		viewAllObjsAction.setToolTipText(VIEW_ALL_OBJS);
	}
	
	private void runQuery(String name)
	{		
		QueryResultsManager results = new QueryResultsManager();
		
		// get Attribute list
		String [] fieldNames = getAttributeList();
		
		// TODO: Check if fieldName and className match.
		if(fieldNames != null && fieldNames.length > 0){
			String className = fieldNames[0].split(OMPlusConstants.REGEX)[0];
			if(!className.equals(name))
				fieldNames = null;
		}
		results.runQuery(name, fieldNames);
		QueryResultList rList = results.getResultList();
		
		IViewPart view = ViewerManager.getView(this.getSite().getPage(), OMPlusConstants.QUERY_RESULTS_ID);
		if(view instanceof QueryResults)
		{
			((QueryResults)view).addNewQueryResults(rList);
		}
		ViewerManager.showView(this.getSite().getPage(), OMPlusConstants.QUERY_RESULTS_ID);
	}
	
	private String[] getAttributeList() {
		IViewPart view = ViewerManager.getView(this.getSite().getPage(), OMPlusConstants.QUERY_BUILDER_ID);
		if(view instanceof QueryBuilder)
		{
			return ((QueryBuilder)view).getAttributeViewerFields();
		}
		return null;
	}

	private void addContextMenu() 
	{
		MenuManager menuMgr = new MenuManager("#PopupMenu");
		menuMgr.setRemoveAllWhenShown(true);
		menuMgr.addMenuListener(new IMenuListener() 
		{
			public void menuAboutToShow(IMenuManager manager) 
			{
				TreeItem [] items = getClassTree().getSelection();
				if(items != null)
				{
					int size = items.length;
					if(size == 1)
					{
						ClassTreeNode node = (ClassTreeNode)items[0].getData();
						if(node.getNodeType() == OMPlusConstants.CLASS_NODE){
							manager.add(viewAllObjsAction);
							manager.add(new Separator(IWorkbenchActionConstants.MB_ADDITIONS));
						}
					}
				}
			}
		});
		Tree tree = getClassTree();
		Menu menu = menuMgr.createContextMenu(tree);
		tree.setMenu(menu);		
	}
	
	
	//TODO: Inner class as of now move
	class TreeDragListener extends DragSourceAdapter{

		TreeViewer classTree;
		final TreeItem[] dragSourceItem = new TreeItem[1];
		
		 public TreeDragListener(TreeViewer viewer) {
		      this.classTree = viewer;
		   }
		 
		   /**
		    * Method declared on DragSourceListener
		    */
		   public void dragFinished(DragSourceEvent event) {
			   if (event.detail == DND.DROP_MOVE){
				   dragSourceItem[0] = null;
			   }
		   }
		   
		   /**
		    * Method declared on DragSourceListener
		    */
		   public void dragSetData(DragSourceEvent event) {
			   ClassTreeNode node = (ClassTreeNode)dragSourceItem[0].getData();
			   if(node != null)
				   event.data = node.getName();
		   }
		   
		   /**
		    * Method declared on DragSourceListener
		    */
		   public void dragStart(DragSourceEvent event) {
			   TreeItem[] selection = getClassTree().getSelection();
			   event.doit = false;
			   if (selection.length == 1 ) 
			   {
				   ClassTreeNode node = (ClassTreeNode)selection[0].getData();
				   if( node != null && node.getNodeType() != OMPlusConstants.PACKAGE_NODE )
				   {
					   event.doit = true;
					   dragSourceItem[0] = selection[0];
				   }
			  }
		 }
	}

	public void refreshClassViewerWithNewDB()
	{
		boolean refreshClassTree = false;
		if(Activator.getDefault().dbModel().connected()) {	
			setCurrentView();
			int size = setItemsForCombo();
			refreshClassTree = setInputForClassTree(currentView);
			enableButtons(true);
			enableSrchBtns(size);
		} else
			searchCombo.removeAll();
		if(refreshClassTree)
			classTree.refresh();
	}
	
	private int setItemsForCombo() {
		String []keywords = getSearchKeywords();
		searchCombo.setItems(keywords);
		return keywords.length;
	}

	private Tree getClassTree(){
		return classTree.getTree();
	}
	
	/**
	 * Update the property view with new class
	 * @param className
	 */
	public void updatePropertiesView(String className, boolean toUpdateObjectPropertiesTab) 
	{
		IViewPart view = ViewerManager.getView(getViewSite().getPage(),OMPlusConstants.PROPERTY_VIEWER_ID);
		if(view instanceof PropertyViewer)
		{
			((PropertyViewer)view).updateClassProperties(className, toUpdateObjectPropertiesTab);
		}
		
	}
	
	/**
	 * The class Viewer has been activated
	 */
	public void activated()
	{
		try{
			//TODO: get the selected item from the class tree and call 
			//updatePropertiesView(String className, boolean toUpdateObjectPropertiesTab)
			TreeItem[] items = getClassTree().getSelection();
			boolean updateProperties = false;
			String selectedClass = null;
			if(items != null && items.length > 0){
				int size = items.length;
				if(size == 1) {
					ClassTreeNode node = (ClassTreeNode)items[0].getData();
					if(node.getNodeType() == OMPlusConstants.CLASS_NODE) {
						selectedClass = node.getName();
						updateProperties = true;
					} else if(node.getNodeType() == OMPlusConstants.CLASS_FIELD_NODE) {
						selectedClass = getClsName(node.getName());
						updateProperties = true;
					} /*else if(node.getNodeType() == OMPlusConstants.PACKAGE_NODE) {
						ClassTreeNode node = (ClassTreeNode)items[0].getData();
					}*/
				}
			}
			else{
				if(getClassTree().getItemCount() > 0){
					ClassTreeNode node = (ClassTreeNode)getClassTree().getItem(0).getData();
					if(node.getNodeType() == OMPlusConstants.CLASS_NODE) {
						selectedClass = node.getName();
						updateProperties = true;
					}
				}
			}
			
			//1. Update teh PropertiesViewer
			
			IViewPart part =  ViewerManager.getView(getViewSite().getPage(), OMPlusConstants.PROPERTY_VIEWER_ID);
			if(part instanceof PropertyViewer)
			{
				if(updateProperties)
					((PropertyViewer)part).classViewerActivated(selectedClass);
				else
					((PropertyViewer)part).classViewerActivated();
			}	

		}catch(Exception ex){
			
		}
	}

	private String getClsName(String name) {
		String fieldName = name.split(OMPlusConstants.REGEX)[0];
		return fieldName;
	}

}
