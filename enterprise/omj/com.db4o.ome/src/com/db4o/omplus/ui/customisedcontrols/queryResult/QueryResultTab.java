package com.db4o.omplus.ui.customisedcontrols.queryResult;

import java.util.*;

import org.eclipse.jface.action.*;
import org.eclipse.jface.dialogs.*;
import org.eclipse.jface.resource.*;
import org.eclipse.jface.viewers.*;
import org.eclipse.nebula.widgets.cdatetime.*;
import org.eclipse.swt.*;
import org.eclipse.swt.custom.*;
import org.eclipse.swt.events.*;
import org.eclipse.swt.graphics.*;
import org.eclipse.swt.layout.*;
import org.eclipse.swt.widgets.*;
import org.eclipse.ui.*;

import com.db4o.omplus.*;
import com.db4o.omplus.datalayer.*;
import com.db4o.omplus.datalayer.queryBuilder.*;
import com.db4o.omplus.datalayer.queryresult.*;
import com.db4o.omplus.ui.interfaces.*;
import com.db4o.omplus.ui.listeners.queryResult.*;
import com.db4o.omplus.ui.model.*;
import com.db4o.omplus.ui.model.queryResults.*;

public class QueryResultTab extends CTabItem implements IChildModifier
{
	private QueryPresentationModel queryModel;
	
	private final String DELETE_MESSAGE = "Do you want to delete this object ?";
	private final String SAVE_MESSAGE = "Do you want to save the changes made in this page. Selecting 'No' "+
	"will discard the changes made?";
	
	private final String ROW_ID = "Row id";

	private CTabFolder parentTab;
	/**
	 * The queryResult associated with this tab(item)
	 */
	private QueryResultList queryResultList;
	
	// for testing to be removed
	private QueryResultPage resultPage;
	
	private IViewUpdatorForQueryResults viewUpdatorForQueryResults;
	
	/**
	 * Main composite which is set in this tab
	 */
	private Composite queryResultTabComposite;
	
	private SashForm tableSashForm;
	private Composite tableButtonsComposite;
	private SashForm treeSashForm;
	private Composite treeButtonsComposite;
	
	private Table table;
	private TableViewer tableViewer;
	private ObjectViewer objectViewer;
	private ShowHideAction [] showHideAction;
	private ShowHideAction showAllColumnsAction;
	private HashMap<String, String> columnNameMap;
	
	/**
	 * Since buttons have to be enables/disabled for queries its moved here
	 */
	private Button resultObjectTableSaveButton;
	private Button resultObjectTableDeleteButton;
	private Button tableRefreshButton;
	private Button resultObjectTreeSaveButton;
	
	/**
	 * Pagination buttons
	 */
	private Button firstPageButton;
	private Button previousPageButton;
	private Button nextPageButton;
	private Button lastPageButton;
	private Label pageLabel;
	private Text pageNumberText;
	
	private QueryResultTableModifiedList modifiedObjList;
	

	public QueryResultTab(QueryPresentationModel model, CTabFolder parent, int style, QueryResultList qList, 
							IViewUpdatorForQueryResults viewUpdatorForQueryResults)
	{
		super(parent, style);
		this.queryModel = model;
		parentTab = parent;
		this.viewUpdatorForQueryResults = viewUpdatorForQueryResults;
		
		queryResultList = qList;
	}
	
	private void initForResultPages() {
		resultPage = new QueryResultPage();
		int size = queryResultList.size();
		if(size > 0) {
			int pages = size / 50 ;
			if((size%50) != 0)
				pages++;
			resultPage.setNumOfPages(pages);
			resultPage.setCurrentPage(1);
//			pageLabel.setText(" of "+resultPage.getNumOfPages());
		}
	}

	private void initializeChildComponents()
	{
		queryResultTabComposite = new Composite(parentTab,SWT.NONE);
		queryResultTabComposite.setLayout(new FillLayout(SWT.VERTICAL));
				
		//Table section
		tableSashForm = new SashForm(queryResultTabComposite, SWT.SMOOTH|SWT.VERTICAL);
		int style = SWT.SINGLE | SWT.BORDER | SWT.H_SCROLL | SWT.V_SCROLL | 
					SWT.FULL_SELECTION;// | SWT.HIDE_SELECTION;
		table = new Table(tableSashForm,style);
		tableButtonsComposite = new Composite(tableSashForm, SWT.NONE);
		resultObjectTableSaveButton = new Button(tableButtonsComposite,SWT.PUSH);
		resultObjectTableSaveButton.setEnabled(false);
		resultObjectTableDeleteButton = new Button(tableButtonsComposite,SWT.PUSH);
		resultObjectTableDeleteButton.setEnabled(!Activator.getDefault().dbModel().db().readOnly());
		tableRefreshButton = new Button(tableButtonsComposite, SWT.PUSH);
		tableRefreshButton.setText("Refresh");
		tableRefreshButton.setEnabled(!Activator.getDefault().dbModel().db().readOnly());
		
		
		//tree section
		treeSashForm = new SashForm(queryResultTabComposite, SWT.SMOOTH|SWT.VERTICAL);
		objectViewer = new ObjectViewer(queryModel, treeSashForm,SWT.TOP,queryResultList, this);
		treeButtonsComposite = new Composite(treeSashForm, SWT.NONE);
		resultObjectTreeSaveButton = new Button(treeButtonsComposite,SWT.PUSH);
		resultObjectTreeSaveButton.setEnabled(false);
		
		tableSashForm.setWeights(new int[] {80,20});
		treeSashForm.setWeights(new int[] {80,20});
		
		columnNameMap = new HashMap<String, String>();
		
		initializePaginationButtons();
	}

	private void initializePaginationButtons() 
	{
		firstPageButton = new Button(tableButtonsComposite,SWT.PUSH);
		firstPageButton.setText(" << ");
		firstPageButton.setToolTipText("First");
		previousPageButton = new Button(tableButtonsComposite,SWT.PUSH);
		previousPageButton.setText(" < ");
		previousPageButton.setToolTipText("Previous");
		nextPageButton = new Button(tableButtonsComposite,SWT.PUSH);
		nextPageButton.setText(" > ");
		nextPageButton.setToolTipText("Next");
		lastPageButton = new Button(tableButtonsComposite,SWT.PUSH);
		lastPageButton.setText(" >> ");
		lastPageButton.setToolTipText("Last");
		
		pageNumberText = new Text(tableButtonsComposite, SWT.BORDER | SWT.MULTI);
		pageLabel = new Label(tableButtonsComposite, SWT.NONE);
//		

		addListenersForPageButtons();
		addListenerForPageNumber();
	}

	private void addListenerForPageNumber() {
//		TODO: handle buttons disabling and add label
		pageNumberText.addKeyListener(new KeyListener(){

			public void keyPressed(KeyEvent e) {
				if(resultPage == null || resultPage.getNumOfPages() == 0){
					return;
				}
				if(e.character == SWT.CR){
					Integer num = 1;
					try {
						String s = pageNumberText.getText();
						num = new Integer(s.trim());
					}catch(NumberFormatException ex){
						pageNumberText.setText(""+resultPage.getCurrentPage());
						pageNumberText.setToolTipText(""+resultPage.getCurrentPage());
						return;
					}
					int pageNum = num.intValue();
					if(pageNum == resultPage.getCurrentPage()) {
						pageNumberText.setText(""+resultPage.getCurrentPage());
						pageNumberText.setToolTipText(""+resultPage.getCurrentPage());
						return;
					}
					int max = resultPage.getNumOfPages();
					beforePageSet();
					if(pageNum >0 && pageNum <= max){
						pageNumberText.setText(""+pageNum);
					}else if(pageNum <= 0){
						pageNum = 1;
						pageNumberText.setText(""+pageNum);
					}
					else if(pageNum > max){
						pageNum = max;
						pageNumberText.setText(""+max);
					}
					setPageBtn(pageNum, max);
					resultPage.setCurrentPage(pageNum);
					pageNumberText.setToolTipText(""+resultPage.getCurrentPage());
					tableViewer.refresh();
				}
			}

			public void keyReleased(KeyEvent e) {
				// TODO Auto-generated method stub
			}
		});
	}

	private void setPageBtn(int pageNum, int max) {
		if(pageNum == 1 && pageNum == max){
			firstPageButton.setEnabled(false);
			lastPageButton.setEnabled(false);
			nextPageButton.setEnabled(false);
			previousPageButton.setEnabled(false);
		}else if(pageNum > 1){
			firstPageButton.setEnabled(true);
			previousPageButton.setEnabled(true);
			if(pageNum < max){
				lastPageButton.setEnabled(true);
				nextPageButton.setEnabled(true);
			}else if(pageNum == max){
				lastPageButton.setEnabled(false);
				nextPageButton.setEnabled(false);
			}
		}else if(pageNum < max){
			lastPageButton.setEnabled(true);
			nextPageButton.setEnabled(true);
			if(pageNum > 1){
				firstPageButton.setEnabled(true);
				previousPageButton.setEnabled(true);
			}else if(pageNum == 1){
				firstPageButton.setEnabled(false);
				previousPageButton.setEnabled(false);
			}
		}else if(pageNum == 1){
			firstPageButton.setEnabled(false);
			lastPageButton.setEnabled(true);
			nextPageButton.setEnabled(true);
			previousPageButton.setEnabled(false);
		}
		else if(pageNum == max){
			firstPageButton.setEnabled(true);
			lastPageButton.setEnabled(false);
			nextPageButton.setEnabled(false);
			previousPageButton.setEnabled(true);
		}
	}
	
	private void addListenersForPageButtons() {
		firstPageButton.addSelectionListener(new SelectionListener(){

			public void widgetDefaultSelected(SelectionEvent e) {
			
			}

			public void widgetSelected(SelectionEvent e) {
				if(resultPage.getCurrentPage() != 1){
					beforePageSet();
					resultPage.setCurrentPage(1);
					tableViewer.refresh();
				}
				firstPageButton.setEnabled(false);
				lastPageButton.setEnabled(true);
				nextPageButton.setEnabled(true);
				previousPageButton.setEnabled(false);
				pageNumberText.setText(""+resultPage.getCurrentPage());
			}
			
		});
		
		nextPageButton.addSelectionListener(new SelectionListener(){

			public void widgetDefaultSelected(SelectionEvent e) {
			}

			public void widgetSelected(SelectionEvent e) {
				int pageNum = resultPage.getCurrentPage();
				int max = resultPage.getNumOfPages();
				if( pageNum != max){
					beforePageSet();
					pageNum++;
					resultPage.setCurrentPage(pageNum);
					
					tableViewer.refresh();
				}
				if(pageNum == max){
					lastPageButton.setEnabled(false);
					nextPageButton.setEnabled(false);
				} else {
					lastPageButton.setEnabled(true);
					nextPageButton.setEnabled(true);
				}
				firstPageButton.setEnabled(true);				
				previousPageButton.setEnabled(true);
				pageNumberText.setText(""+resultPage.getCurrentPage());
			}
			
		});
		
		previousPageButton.addSelectionListener(new SelectionListener(){

			public void widgetDefaultSelected(SelectionEvent e) {
			}

			public void widgetSelected(SelectionEvent e) {
				int pageNum = resultPage.getCurrentPage();
//				int max = resultPage.getNumOfPages();
				if( pageNum > 1){
					beforePageSet();
					pageNum--;
					resultPage.setCurrentPage(pageNum);
					tableViewer.refresh();
				}
				if(pageNum == 1){
					firstPageButton.setEnabled(false);				
					previousPageButton.setEnabled(false);
				} else {
					firstPageButton.setEnabled(true);				
					previousPageButton.setEnabled(true);
				}
				lastPageButton.setEnabled(true);
				nextPageButton.setEnabled(true);
				pageNumberText.setText(""+resultPage.getCurrentPage());
			}
			
		});
		
		lastPageButton.addSelectionListener(new SelectionListener(){

			public void widgetDefaultSelected(SelectionEvent e) {
			
			}

			public void widgetSelected(SelectionEvent e) {
				int pageNum = resultPage.getCurrentPage();
				int max = resultPage.getNumOfPages();
				if( pageNum != max){
					beforePageSet();
					resultPage.setCurrentPage(max);
					tableViewer.refresh();
				}
				lastPageButton.setEnabled(false);
				firstPageButton.setEnabled(true);
				nextPageButton.setEnabled(false);
				previousPageButton.setEnabled(true);
				pageNumberText.setText(""+resultPage.getCurrentPage());
			}
			
		});
		
	}
	
	public void beforePageSet() {
		if(checkForModifications()) {
			saveChanges();
		}
	}
	
	private boolean checkForModifications() {
		if(modifiedObjList != null){
			return (modifiedObjList.getModifiedObjList().size() > 0);
		}
		return false;
	}
	
	private void saveChanges() {
//		if(showMessage()) { // Commented for OMJ-126
			modifiedObjList.writeToDB();
/*		}
		else {
			modifiedObjList.refresh();
		}*/
		
	}
	
	private boolean showMessage(){
		return MessageDialog.openQuestion(parentTab.getShell(), 
				OMPlusConstants.DIALOG_BOX_TITLE, SAVE_MESSAGE);
	}

	/**
	 * enable/disable the SaveButton for the result table
	 * @param b
	 */
	public void modifyTableSaveButton(boolean b)
	{
		resultObjectTableSaveButton.setEnabled(b);
	}
	
	/**
	 * enable/disable the SaveButton for teh result tree of object viewer
	 * @param b
	 */
	public void modifyObjectViewerSaveButton(boolean b)
	{
		resultObjectTreeSaveButton.setEnabled(b);
	}
	
	/**
	 * Get the class table associated with this tab
	 * @return
	 */
	public String getClassNameForQueryResultTab()
	{
		return queryResultList.getClassName();
	}
	
	public OMQuery getOMQueryForQueryResultTab()
	{
		return queryResultList.getOMQuery();
	}

	public QueryResultTableModifiedList getModifiedObjList() {
		return modifiedObjList;
	}

	public void setModifiedObjList(QueryResultTableModifiedList modifiedObjList) {
		this.modifiedObjList = modifiedObjList;
	}

	
	/**
	 * Dispose all components and start afresh
	 */
	public void disposeAllComponents()
	{
		if(queryResultTabComposite!=null)
			for (Control control : queryResultTabComposite.getChildren()) 
			{
				control.dispose();
			}		
	}
	
	/**
	 * Add components to this tab
	 * Reinitalize the result list
	 */
	public void addComponentsToTab(QueryResultList list)
	{
		disposeAllComponents();
		initializeChildComponents();
		
		this.queryResultList = list;
		
		addResultTable();
		addObjectViewer();
			
		//Important Step: set this comppsoite to current tab item
		this.setControl(queryResultTabComposite);
		this.getControl().addDisposeListener(new DisposeListener(){

			public void widgetDisposed(DisposeEvent e) {
//				beforePageSet();
				beforeTabClose();
			}
			
		});
		
	}
	
	public void beforeTabClose(){
		if( checkForModifications() || objectViewer.isModified()){
			if(showMessage()) {
				modifiedObjList.writeToDB();
				objectViewer.updateTheQueryListonSaveClick();
			}
			else {
				modifiedObjList.refresh();
				objectViewer.clearModifiedlist();
			}
		}
	}
	
	/**
	 * Add the table
	 */
	private void addResultTable()
	{
		String [] columnList = queryResultList.getFieldNames();
		String [] uiColumnList = new String[columnList.length+1];
		
		uiColumnList[0] = "Row Id";
		for(int i = 0; i < columnList.length; i++)
		{
			uiColumnList[i+1]= columnList[i];
		}
		
		modifyDisplayNamesForColumns(uiColumnList);		
		fillColumnNamesMapping(columnList, uiColumnList);
		
		//TODO: check this...if not needed delete it
		/*GridData gridData = new GridData(GridData.FILL_BOTH);
		gridData.horizontalSpan = 3;
		table.setLayoutData(gridData);*/	
		table.setLinesVisible(true);
		table.setHeaderVisible(true);
				
		CellEditor[] editors = new CellEditor[uiColumnList.length];
		
		TableColumn column = null;
		showHideAction = new ShowHideAction[uiColumnList.length];
		for(int i = 0; i<uiColumnList.length; i++)
		{
			final int index = i;
			column = new TableColumn(table, SWT.CENTER | SWT.UP);	// , SWT.UP	
			column.setText(uiColumnList[i]);
			column.setWidth(100);
			column.setMoveable(true);
			
			//Set action 
			showHideAction[i] = new ShowHideAction(i);
			showHideAction[i].setText(uiColumnList[i]);
			Image image = ImageUtility.getImage(OMPlusConstants.COLUMN_VISIBLE_ICON);
			showHideAction[i].setImageDescriptor(ImageDescriptor.createFromImage(image));
			
			//TODO: ideally add custom editing because tomorrow you may need. As a quick fix just add ComboCellEditor
			//for boolean
			if(i!=0 && !Activator.getDefault().dbModel().db().readOnly())
			{
				int type = queryResultList.getDataType(columnList[i-1]);
				if(type == QueryBuilderConstants.DATATYPE_BOOLEAN)//0th is row id
					editors[i] = new ComboBoxCellEditor(table,new String[] {"true","false"},SWT.READ_ONLY);
				else if(type == QueryBuilderConstants.DATATYPE_DATE_TIME)
					editors[i] = new CDateTimeCellEditor(table, 
								CDT.DROP_DOWN | CDT.DATE_MEDIUM | CDT.TIME_MEDIUM );
				else
					editors[i] = new TextCellEditor(table);
					
//					Set sorting here
					column.addSelectionListener(new SelectionListener(){

						public void widgetDefaultSelected(SelectionEvent e) {
						}

						public void widgetSelected(SelectionEvent e) {
							int direction = ( table.getSortDirection() == SWT.UP )? 
								TableColumnComparator.DESCENDING : TableColumnComparator.ASCENDING;
							tableViewer.setComparator(new TableColumnComparator(index, direction) );
							if(table.getSortDirection() == SWT.UP)
								table.setSortDirection(SWT.DOWN);
							else
								table.setSortDirection(SWT.UP);
							table.setSortColumn(table.getColumn(index));
						}
						
					});
			}
			else
				editors[i] = null;
			
		}
		showAllColumnsAction = new ShowHideAction(-1);
		showAllColumnsAction.setText("Show all columns");

		
		tableViewer = new TableViewer(table);
		tableViewer.getTable().setSortDirection(SWT.UP);
		//Allow edition only when double clicked. Can add for keyPress
		ColumnViewerEditorActivationStrategy actSupport = new ColumnViewerEditorActivationStrategy(tableViewer) 
		{
			protected boolean isEditorActivationEvent(ColumnViewerEditorActivationEvent event) 
			{
				return event.eventType == ColumnViewerEditorActivationEvent.MOUSE_DOUBLE_CLICK_SELECTION;
				//|| event.eventType == ColumnViewerEditorActivationEvent.KEY_PRESSED 
				//					  && event.keyCode == SWT.CR;
			}
		};
		
		
		addContextMenu();
		
		initForResultPages();
		
		TableViewerEditor.create(tableViewer, actSupport, ColumnViewerEditor.DEFAULT);		
		tableViewer.setColumnProperties(uiColumnList);
		tableViewer.setCellEditors(editors);		
		modifiedObjList = new QueryResultTableModifiedList();
		
		ResultTableCellModifier modifier = new ResultTableCellModifier(Activator.getDefault().dbModel().db().reflectHelper(), queryModel, tableViewer, 
											   queryResultList, modifiedObjList,this,columnNameMap);
		tableViewer.setCellModifier(modifier);
		
		//Set Content Providers for Table
		tableViewer.setContentProvider(new QueryResultsContentProvider(resultPage) );
		tableViewer.setLabelProvider(new QueryResultsLabelProvider(Activator.getDefault().dbModel().db().reflectHelper()));
		try {
			tableViewer.setInput(queryResultList);
		}
		catch(Exception ex){
			err().error("Cannot display results.", ex);
		}
		
		
		ResultTableSelectionListener listener = new ResultTableSelectionListener(queryResultList,
															objectViewer, viewUpdatorForQueryResults);
		table.addSelectionListener(listener);
				
		addTableButtons();
		
		if(resultPage.getNumOfPages() <= 1){
			lastPageButton.setEnabled(false);
			firstPageButton.setEnabled(false);
			nextPageButton.setEnabled(false);
			previousPageButton.setEnabled(false);
		}
		else{
			lastPageButton.setEnabled(true);
			firstPageButton.setEnabled(false);
			nextPageButton.setEnabled(true);
			previousPageButton.setEnabled(false);
		}
		pageNumberText.setText(""+resultPage.getCurrentPage());
		pageLabel.setText(" of "+resultPage.getNumOfPages());
	}
	
	/**
	 * Fill the map for uiDisplayColumn names and ACtualColumNNames
	 * @param actualColumNames
	 * @param uiColumnList
	 */
	private void fillColumnNamesMapping(String[] actualColumNames,	String[] uiColumnList) 
	{		
		for(int i = 0; i < uiColumnList.length; i++)
		{
			//0th column = Row identifier
			if(i == 0)
				columnNameMap.put(uiColumnList[i], ROW_ID);
			else
				columnNameMap.put(uiColumnList[i], actualColumNames[i-1]);
		}			
	}

	/**
	 * Modify display names to retrieve package info
	 * @param uiColumnList
	 */
	private void modifyDisplayNamesForColumns(String[] uiColumnList)
	{
		//0th column has no class anme package anme etc 
		for(int i = 1; i < uiColumnList.length; i++)
		{
			int colonIndex = uiColumnList[i].indexOf(OMPlusConstants.REGEX);
//			String packageName = uiColumnList[i].substring(0,colonIndex);
			String fieldName = uiColumnList[i].substring(colonIndex+1);
			fieldName = fieldName.replace(':', '.');
//			String className = packageName.substring(packageName.lastIndexOf('.')+1);
			//LOGIC: if you want to show Classname.VarName use this.
			//uiColumnList[i] = className+"."+fieldName;
			
			uiColumnList[i] = fieldName;
		}		
	}
	
	/**
	 * Add menu to show/hide columns
	 */
	private void addContextMenu()
	{
		MenuManager menuMgr = new MenuManager("#PopupMenu");
		menuMgr.setRemoveAllWhenShown(true);
		menuMgr.addMenuListener(new IMenuListener() 
		{
			public void menuAboutToShow(IMenuManager manager) 
			{
				//Start from 0 if want to allow hiding column 0 too
				for(int i = 1; i < showHideAction.length; i++)
					manager.add(showHideAction[i]);
				manager.add(new Separator(IWorkbenchActionConstants.MB_ADDITIONS));
				manager.add(showAllColumnsAction); 
				
			}
		});
		Menu menu = menuMgr.createContextMenu(tableViewer.getControl());
		tableViewer.getControl().setMenu(menu);
	}
	
	/**
	 * Add the buttons below the table
	 */
	private void addTableButtons()
	{
		tableButtonsComposite.setLayout(new FormLayout());
		resultObjectTableSaveButton.setText("Save");
		FormData btnData = new FormData();
		btnData.top = new FormAttachment(0,5);
		btnData.left = new FormAttachment(0,5);		

		resultObjectTableSaveButton.setLayoutData(btnData);
		
		resultObjectTableSaveButton.addListener(SWT.Selection, new Listener()
		{
			public void handleEvent(Event event) 
			{
			//Call ModifyObjects class
			modifiedObjList.writeToDB();
			resultObjectTableSaveButton.setEnabled(false);
			}
		});
		
		resultObjectTableDeleteButton.setText("Delete");
		btnData = new FormData();
		btnData.top = new FormAttachment(0,5);
		btnData.left = new FormAttachment(resultObjectTableSaveButton,5);		

		resultObjectTableDeleteButton.setLayoutData(btnData);
		
		btnData = new FormData();
		btnData.top = new FormAttachment(0,5);
		btnData.left = new FormAttachment(resultObjectTableDeleteButton,5);
		tableRefreshButton.setLayoutData(btnData);
		
		addListenerForDelete();
		addListenerForRefresh();
		addPaginationButtonsLayout();
	}

	private void addListenerForRefresh() {
		tableRefreshButton.addSelectionListener(new SelectionAdapter(){
			public void widgetSelected(SelectionEvent e) {
//				beforePageSet(); // onlu checks for the changes in result table
				beforeTabClose();
				resultPage.refresh(true);
				tableViewer.refresh();
				resultPage.refresh(false);
				//check for valid Objs
				removeDeletedObjTabs();
				resultObjectTableSaveButton.setEnabled(false);
				resultObjectTreeSaveButton.setEnabled(false);
			}
		});		
	}

	private void removeDeletedObjTabs() {
		CTabItem []items = objectViewer.getItems();
		if(items != null)
		{
			for(CTabItem tab : items)
			{
				String tabText = tab.getText();
				Object tabObj = ((ObjectViewerTab)tab).getResultObject();
				tabText = tabText.substring(ObjectViewer.TAB_NAME_BEGINS_WITH.length());
				int tabIndex = new Integer(tabText).intValue();
				Object tableObj = resultPage.getObjectById(queryResultList.getLocalIdList().get(tabIndex -1), 
											Activator.getDefault().dbModel().db());
				if(tabObj != tableObj)
				{
					tab.dispose();
				}
				else
					((ObjectViewerTab)tab).refresh();
			}

		}		
	}
	
	private void addPaginationButtonsLayout() 
	{
		FormData btnData = new FormData();
		
		btnData = new FormData();
		btnData.top = new FormAttachment(0,5);
		btnData.right = new FormAttachment(100, -5); 
		lastPageButton.setLayoutData(btnData);
		
		btnData = new FormData();
		btnData.top = new FormAttachment(0,5);
		btnData.right = new FormAttachment(lastPageButton, -4);		
		nextPageButton.setLayoutData(btnData);
		
		
		btnData = new FormData(40,16);
		btnData.top = new FormAttachment(0,9);
		btnData.right = new FormAttachment(nextPageButton, -4);
//		pageNumber
		pageLabel.setLayoutData(btnData);
		
		btnData = new FormData(30,16);
		btnData.top = new FormAttachment(0,6);
		btnData.right = new FormAttachment(pageLabel, -4);
//		pageNumber
		pageNumberText.setLayoutData(btnData);
	
		
		btnData = new FormData();
		btnData.top = new FormAttachment(0,5);
		btnData.right = new FormAttachment(pageNumberText, -4);		
		previousPageButton.setLayoutData(btnData);
		
		btnData = new FormData();
		btnData.top = new FormAttachment(0,5);
		btnData.right = new FormAttachment(previousPageButton, -4);		
		firstPageButton.setLayoutData(btnData);		
		
	}

	private void addListenerForDelete() {
		if(Activator.getDefault().dbModel().db().readOnly()) {
			return;
		}
		resultObjectTableDeleteButton.addListener(SWT.Selection, new Listener() {
		// Right now handling single row delete. no multiple selection.
			public void handleEvent(Event event)
			{
				int index = table.getSelectionIndex();
				if(index > -1 && index < OMPlusConstants.MAX_OBJS_PAGE)
				{
					QueryResultRow row = (QueryResultRow)table.getItem(index).getData();
					if(row != null)
					{
						Object obj = row.getResultObj();
						if(showMessageDialog()==0)
	
						{
							modifiedObjList.addToDeleteList(obj);							
							modifyAssociatedObjectTreeItem(obj);
							modifiedObjList.deleteFromDB();
							queryResultList.removeFromList(row.getId() - 1);						
							tableViewer.getTable().remove(index);tableViewer.refresh();
						}
					}
				}
			}
		});		
	}

	private int showMessageDialog() {
		String [] buttonStr = new String[]{ "Ok","Cancel"};
		MessageDialog msgDialog = new MessageDialog(parentTab.getShell(), 
				OMPlusConstants.DIALOG_BOX_TITLE, null, DELETE_MESSAGE, 
				MessageDialog.QUESTION, buttonStr, 0);
		return msgDialog.open();
	}

	/**
	 * Add object viewer tree
	 */
	private void addObjectViewer()
	{
		treeButtonsComposite.setLayout(new FormLayout());
		resultObjectTreeSaveButton.setText("Save");
		FormData btnData = new FormData();
		btnData.top = new FormAttachment(objectViewer,5);
		btnData.left = new FormAttachment(0,5);		
		resultObjectTreeSaveButton.setLayoutData(btnData);
		
		resultObjectTreeSaveButton.addListener(SWT.Selection, new Listener()
		{
			public void handleEvent(Event event) 
			{
				objectViewer.updateTheQueryListonSaveClick();
				resultObjectTreeSaveButton.setEnabled(false);
			}			
		});		
	}

	public void modifyAssociatedObjectTreeItem(Object resultObject)
	{
		//objectViewer.refresh(resultObject);
		objectViewer.deleteDisplayedObject(resultObject);
	}
	
	
	/***************************** implementaion for IChildModifier interface ****************************/
	public void objectTableModified(Object resultObject) 
	{
		modifyTableSaveButton(true);
		objectViewer.refresh(resultObject);
				
	}

	public void objectTreeModified(boolean saveBtnEnabled) 
	{
		modifyObjectViewerSaveButton(saveBtnEnabled);
		tableViewer.refresh();
		
	}
	
	public void objectTabSelectedInTree(int tabIndex)
	{
		int pageNo = tabIndex/OMPlusConstants.MAX_OBJS_PAGE;
		if( ( ( tabIndex % OMPlusConstants.MAX_OBJS_PAGE ) != 0) )
			pageNo += 1;
		if(pageNo == 0)
			pageNo = 1;
		resultPage.setCurrentPage(pageNo);
		pageNumberText.setText(""+pageNo);
		setPageBtn(resultPage.getCurrentPage(),
							resultPage.getNumOfPages()) ;
		tableViewer.refresh();
		
		int tableIndex =  getTableindexFromRowId(tabIndex);
		if(tableIndex != -1)
			table.setSelection(table.getItem(tableIndex));
	}
	
	// accessing QueryResultRow currently. Need to change this
	private int getTableindexFromRowId(int index)
	{
		TableItem []items = table.getItems();
		int i = 0;
		for(TableItem tableItem : items)
		{
			QueryResultRow row = (QueryResultRow)tableItem.getData();
			if(index == row.getId())
			{
				return i;
			}
			i++;
		}
		return -1;
	}

	/****************************     END    IChildModifier interface  ****************************/              
	
	
	
	/////////////////////////////   INNER CLASS - for handling context menu of showing hiding columns in ResultTable
	
	class ShowHideAction extends Action
	{
		private int columnIndex;
		
				
		
		public ShowHideAction(int colIdx)
		{
			columnIndex = colIdx;
		}
		
		public void run() 
		{
			if(columnIndex == -1)//show all columns
			{
				for(int i = 0; i < table.getColumnCount(); i++)
				{
					TableColumn col = table.getColumn(i);
					col.setWidth(100);
					col.setResizable(true);
					Image image = ImageUtility.getImage(OMPlusConstants.COLUMN_VISIBLE_ICON);
					showHideAction[i].setImageDescriptor(ImageDescriptor.createFromImage(image));					
					
				}
			}
			else
			{	
				TableColumn col = table.getColumn(columnIndex);
				if(col.getWidth()==0)
				{
					col.setWidth(100);
					col.setResizable(true);
					Image image = ImageUtility.getImage(OMPlusConstants.COLUMN_VISIBLE_ICON);
					showHideAction[columnIndex].setImageDescriptor(ImageDescriptor.createFromImage(image));					
				}
				else
				{
					col.setWidth(0);
					col.setResizable(false);
					showHideAction[columnIndex].setImageDescriptor(null);
				}
			}
		}
	}
	
	private ErrorMessageHandler err() {
		return queryModel.err();
	}

}
