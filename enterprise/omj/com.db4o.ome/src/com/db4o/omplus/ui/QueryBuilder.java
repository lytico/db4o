package com.db4o.omplus.ui;


import java.util.*;
import java.util.List;

import org.eclipse.jface.action.*;
import org.eclipse.jface.viewers.*;
import org.eclipse.swt.*;
import org.eclipse.swt.custom.*;
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
import com.db4o.omplus.datalayer.propertyViewer.*;
import com.db4o.omplus.datalayer.propertyViewer.classProperties.*;
import com.db4o.omplus.datalayer.queryBuilder.*;
import com.db4o.omplus.datalayer.queryresult.*;
import com.db4o.omplus.ui.customisedcontrols.queryBuilder.*;
import com.db4o.omplus.ui.interfaces.*;
import com.db4o.omplus.ui.model.*;

public class QueryBuilder extends ViewPart implements IChildObserver,IDropValidator
{
	private static final int[] sashWeights = new int[] {5,50,45};
	private Composite parentComposite = null;
	private ArrayList<QueryGroupComposite> querygroupList = null ;
	private OMQuery constraintList;
	private RecentQueries recentQueries;
	
	private SashForm mainSashForm = null;
	
	private Composite recentQueryComposite = null;
	private Combo recentQueriesCombo = null;
	
	//////////// QueryBuilder Group ////////////////////////////////////////////////////////////////
	private ScrolledComposite scrollComposite = null;
	private Composite childComposite = null;
	
	private int compositeStyle = SWT.BORDER;
	
		
	//////////////////////////////////// Attribute Viewer
	private ScrolledComposite attributeViewerScrollComposite = null;
	private Composite mainAttributeViewerComposite = null;
	private Composite attributeViewerComposite = null;
	private TableViewer attributeListViewer = null;
	private Label attributeLabel = null;
	private Action deleteAction = null;
	private Button runQueryBtn = null;
	private int ATTRIBUTE_VIEWER_TABLE_HEIGHT = 150;
	//private int ATTRIBUTE_VIEWER_TABLE_WIDTH = 300;
	
	private Composite buttonCompositeForAttributeViewer = null;	
	private Button addGroupBtn = null;	
	private Button clearAllGroupsBtn = null;	
	
	@Override
	public void createPartControl(final Composite parent) 
	{
		
		parentComposite = parent;
		mainSashForm = new SashForm(parentComposite,SWT.VERTICAL|SWT.SMOOTH);
		
		recentQueries = new RecentQueries();
		setComponentsInSashForm();
	}
	
	/**
	 * Show completely new QueryBuilder with just one group
	 */
	public void showNewQueryGroupComposite()
	{
		for(Control control: scrollComposite.getChildren())
		{
			control.dispose();
		}
		setChildComponentsInQueryGroups();
		refreshQueryGroup();		
	}
	
	
	/**
	 * Reset the queryBuilder to show groups acc to result set being shown
	 * @param queryForQueryResultTab
	 */
	public void resetQueryBuilder(OMQuery queryForQueryResultTab) 
	{
		constraintList = queryForQueryResultTab;
		querygroupList = new ArrayList<QueryGroupComposite>();
		
		for(Control control: scrollComposite.getChildren())
		{
			control.dispose();
		}

		childComposite = new Composite(scrollComposite,SWT.NONE);
		scrollComposite.setContent(childComposite);
		scrollComposite.setExpandHorizontal(true);
		scrollComposite.setExpandVertical(true);
		scrollComposite.setMinSize(childComposite.computeSize(SWT.DEFAULT, SWT.DEFAULT));		
		
		
		// If this code is commented a blank Quer Builder is shown for ViewAllObjs
		int constraintsSize = constraintList.getConstraintList().size();
		if(constraintsSize == 0){
			addNewGroup();
		} // commented above
		for(int i = 0; i < constraintsSize ;i++)
		{
			QueryGroupComposite groupComposite = new QueryGroupComposite(queryModel(), childComposite,compositeStyle,
															nextQueryGroupIndex(),
															constraintList.getQueryGroup(nextQueryGroupIndex()),
															this,//Observer
															this);//dropValidator
			querygroupList.add(groupComposite);
			
		}
		setLayoutForQueryGroups();
		refreshQueryGroup();
		
		//deselect everything set in Recent Queries
		//recentQueriesCombo.deselectAll();
		
		resetAttributeViewer();
	}
	
	/**
	 * Set the components in main SplitPane(SashForm)
	 */
	private void setComponentsInSashForm() 
	{
		buildRecentQueriesComposite();		
		buildQueryGroupsComposite();
		buildAtrributeViewerComposite();
		ConnectionStatus connStatus= new ConnectionStatus();
		if(connStatus.isConnected()){
			enableBtn(true);
		}else
			enableBtn(false);
		mainSashForm.setWeights(sashWeights);	
		
		mainSashForm.setLayout(new FillLayout());
	}

	private void enableBtn(boolean enable){
		runQueryBtn.setEnabled(enable);
		addGroupBtn.setEnabled(enable);
		clearAllGroupsBtn.setEnabled(enable);
	}
	/**
	 * Recent queries pane building
	 */
	private void buildRecentQueriesComposite()
	{
		recentQueryComposite = new Composite(mainSashForm, SWT.BORDER);
		
		recentQueriesCombo = new Combo(recentQueryComposite,SWT.READ_ONLY);
		recentQueriesCombo.setItems(getRecentQueriesFromDB());
//		recentQueriesCombo.select(0);
		
		addListenerForCombo();
		setLayoutForRecentQueries();
	}
	
	private String[] getRecentQueriesFromDB() {
		List<OMQuery> list = recentQueries.getRecentQueriesForDB();
		int size = list.size();  
		String [] queryStrings = new String[size];
		if(size > 0){
			int count = size - 1;
			ListIterator<OMQuery> iterator = list.listIterator();
			while(iterator.hasNext()){
				queryStrings[count--] = ((OMQuery)iterator.next()).toString();
			}
		}
		return queryStrings;
	}
	
	private void addListenerForCombo() {
		recentQueriesCombo.addSelectionListener(new SelectionListener(){

			public void widgetDefaultSelected(SelectionEvent e) {
				
			}

			public void widgetSelected(SelectionEvent e)
			{
				excecuteSelectedQuery();
				if(recentQueriesCombo.getSelectionIndex()>=0)
					recentQueriesCombo.setToolTipText(recentQueriesCombo.getItem
							 					(recentQueriesCombo.getSelectionIndex()));
			}
			
		});
		
	}

	private void excecuteSelectedQuery() {
//		TODO: compare length of the selected and in list and run app OMQuery
//		getQuery
		int queryIdx = recentQueriesCombo.getSelectionIndex();
		if(queryIdx > -1){
			List<OMQuery> list = recentQueries.getRecentQueriesForDB();
			int size = list.size();
			OMQuery query = null;
			for(int i= size -1, j=0;  (j <= queryIdx )&& (i >= 0);i--, j++){
				query = list.get(i);
			}
			query = createQuery(query);
			resetQueryBuilder(query);
		}
	}

	/**
	 * Set layout for Recent queries pane
	 */
	private void setLayoutForRecentQueries() 
	{
		//RecentQuery componet should be Fill layout
		recentQueryComposite.setLayout(new FillLayout());	
		
		recentQueryComposite.layout(true);
	}

	/**
	 * QueryGroup main compsite set
	 */
	private void buildQueryGroupsComposite()
	{
		scrollComposite = new ScrolledComposite(mainSashForm,SWT.H_SCROLL|SWT.V_SCROLL|SWT.BORDER);
		setChildComponentsInQueryGroups();
		//System.out.println("values "+childComposite.computeSize(SWT.DEFAULT, SWT.DEFAULT));
		scrollComposite.setMinSize(childComposite.computeSize(SWT.DEFAULT, SWT.DEFAULT));
		
	}
	
	/**
	 * Show query groups and set them in its parent
	 */
	private void setChildComponentsInQueryGroups()
	{
		childComposite = new Composite(scrollComposite,SWT.BORDER);
		scrollComposite.setContent(childComposite);
		scrollComposite.setExpandHorizontal(true);
		scrollComposite.setExpandVertical(true);
		querygroupList = new  ArrayList<QueryGroupComposite>();
		constraintList = new OMQuery();
		addNewGroup();		
		
		setLayoutForQueryGroups();
	}
	
	/**
	 * Set layout for all items in a group
	 */
	private void setLayoutForQueryGroups()
	{
		childComposite.setLayout(new FormLayout());
		// Added temporarily to avoid AIOB exception for ViewAllObjs
		if(querygroupList.size() == 0)
			return;
		QueryGroupComposite composite = querygroupList.get(0);
		FormData data = new FormData();
		data.top = new FormAttachment(2,2);
		data.left = new FormAttachment(1,1);
		data.right = new FormAttachment(100,-5);
		//data.width = composite.getWidth();
		data.height = composite.getHeight();
		composite.setLayoutData(data);
		
		for(int i = 1; i < querygroupList.size(); i++)
		{
			composite = querygroupList.get(i);
			data = new FormData();
			data.top = new FormAttachment(querygroupList.get(i-1),5);
			data.left = new FormAttachment(1,1);
			data.right = new FormAttachment(100,-5);
			//data.width = composite.getWidth();
			data.height = composite.getHeight();
			composite.setLayoutData(data);
			
		}
		childComposite.layout(true);		
	}
	
	/**
	 * Resizing on a particular group
	 * @param index
	 * @param groupComposite
	 */
	private void setLayoutForQueryGroups(int index, QueryGroupComposite groupComposite) 
	{
		if(index == 0)
		{
			FormData compData = new FormData();
			compData.top = new FormAttachment(2,5);
			compData.left = new FormAttachment(0,2);
			compData.right = new FormAttachment(100,-5);
			compData.height = groupComposite.getHeight();
			//compData.width = groupComposite.getWidth();
			groupComposite.setLayoutData(compData);
		}
		else
		{	
			FormData compData = new FormData();
			compData.top = new FormAttachment(querygroupList.get(index-1),5);
			compData.left = new FormAttachment(0,2);
			compData.right = new FormAttachment(100,-5);
			compData.height = groupComposite.getHeight();
			//compData.width = groupComposite.getWidth();
			groupComposite.setLayoutData(compData);
		}
		
	}
	

	private boolean checkIfNewGroupCanBeAdded() 
	{
		//For every group check that previous group has been filled
		int toBeAddedGroupIndex = nextQueryGroupIndex();
		
		//NO check for teh first group
		if(toBeAddedGroupIndex!=0)
		{	
			int prevGroupIndex = toBeAddedGroupIndex - 1;
			QueryGroup group= constraintList.getQueryGroup(prevGroupIndex);
			ArrayList<QueryClause> list = group.getQueryList();
			if(list.size() == 0)
				return false;
			else
				return true;
		}
		return true;
		
		
		
	}
	
	/**
	 * Attribute Viewer pane
	 */
	private void buildAtrributeViewerComposite() 
	{
		attributeViewerScrollComposite = new ScrolledComposite(mainSashForm,SWT.H_SCROLL|SWT.V_SCROLL|SWT.BORDER);
		attributeViewerScrollComposite.setExpandHorizontal(true);
		attributeViewerScrollComposite.setExpandVertical(true);
		
		mainAttributeViewerComposite = new Composite(attributeViewerScrollComposite, SWT.NONE);		
		addQuerygroupModiferButtons();
		attributeViewerComposite = new Composite(mainAttributeViewerComposite, SWT.BORDER);
		
		addChildComponentsToAttributeViewer();
		
		attributeViewerScrollComposite.setContent(mainAttributeViewerComposite);
		attributeViewerScrollComposite.setMinSize(mainAttributeViewerComposite.computeSize(SWT.DEFAULT, SWT.DEFAULT));
	}
	
	/**
	 * Clear All and Add Group buttons
	 */
	private void addQuerygroupModiferButtons()
	{
		buttonCompositeForAttributeViewer = new Composite(mainAttributeViewerComposite, SWT.BORDER);
		
		addGroupBtn = new Button(buttonCompositeForAttributeViewer, SWT.PUSH);
		addGroupBtn.setText("Add Group");
		addGroupBtn.addListener(SWT.Selection, new Listener()
		{

			public void handleEvent(Event event)
			{
				if(checkIfNewGroupCanBeAdded())
					addNewQueryGroupDynamically();
									
			}		
			
		});
		
		
		clearAllGroupsBtn = new Button(buttonCompositeForAttributeViewer, SWT.PUSH);
		clearAllGroupsBtn.setText("Clear All");
		clearAllGroupsBtn.addListener(SWT.Selection, new Listener()
		{

			public void handleEvent(Event event)
			{
				recentQueriesCombo.deselectAll();
				recentQueriesCombo.setToolTipText("");
				showNewQueryGroupComposite();
				resetAttributeViewer();
			}
			
		});
		
	}

	/**
	 * Add child components to attRibuteViewer composite ATtribute viewer table and run query btn
	 */
	private void addChildComponentsToAttributeViewer()
	{
		attributeLabel = new Label(attributeViewerComposite, SWT.NONE);
		attributeLabel.setText("Attribute List");
		attributeListViewer = new TableViewer(attributeViewerComposite);
		
		int ops = DND.DROP_COPY | DND.DROP_MOVE | DND.DROP_LINK;
		Transfer[] transfers = new Transfer[] { TextTransfer.getInstance()};
		
		attributeListViewer.addDropSupport(ops, transfers, new DropTargetAdapter()
		{			
			public void dragOver(DropTargetEvent event) {
				event.feedback = DND.FEEDBACK_EXPAND | DND.FEEDBACK_SCROLL;
		        if (event.item != null) {
		        	TableItem item = (TableItem) event.item;
		        	Display display = getAttributeListTable().getDisplay();
		        	Point pt = display.map(null, getAttributeListTable(), event.x, event.y);
		        	Rectangle bounds = item.getBounds();
		        	if (pt.y < bounds.y + bounds.height / 3) {
		        		event.feedback |= DND.FEEDBACK_INSERT_BEFORE;
		        	} else if (pt.y > bounds.y + 2 * bounds.height / 3) {
		        		event.feedback |= DND.FEEDBACK_INSERT_AFTER;
		        	} else {
		        		event.feedback |= DND.FEEDBACK_SELECT;
		        	}
		        }
		    }
		
			public void drop(DropTargetEvent event){
				Object data = event.data;
				String field = null;
				if(data != null)
				{
					int length = 0;
					String items[] = getAttributeViewerFields();
					if(items != null)
						length = items.length;
					field =  data.toString();
					String []fieldsHierarchy = field.split(OMPlusConstants.REGEX);
					String className = fieldsHierarchy[0];
					int depthOfField = fieldsHierarchy.length;
					if(length == 0 ) // If Attribute List is empty
					{
						if( isDropValid(className)){
							if(depthOfField == 1){
								attributeListViewer.add(getFieldsForClazz(className));
							}
							else
								attributeListViewer.add(data);
						}
					}						
					else
					{
						String fieldName = (String)items[0];
						String name = getClassName(fieldName);
						if( className.equals(name))
						{
							if(depthOfField == 1){ // class is dragged
								for(Object classField : getFieldsForClazz(className))
								{
									boolean isPresent = false;
									for (String attribute : items){
										if(attribute.equals(classField)){
											isPresent = true;
											continue;
										}
									}
									if(!isPresent) {
										attributeListViewer.add(classField);
									}
								}
							}
							else{ // a field is dragged
								dropTableItem(event, (String)data);
							}
						}						
					}
				}
			}

			private void dropTableItem(DropTargetEvent event, String data) {
				Table table = getAttributeListTable();
				Display disp  = table.getDisplay();
				 for(int i =0;i < table.getItemCount();i++){
		        	  if(data.equals(table.getItem(i).getData())){
		        		  table.remove(i);
		        	  }
		          }
				Point point = disp.map(null, table, event.x, event.y);
		          TableItem dropItem = table.getItem(point);
//		          System.out.println(dropItem);
		          if(null != dropItem){
		        	  int index = dropItem == null ? table.getItemCount() : table
				              .indexOf(dropItem);
		        	  attributeListViewer.insert(data, index);
		          }
		          else{
		        	 attributeListViewer.add(data);
		          }
		        	  
			}
	
		});		
		
		//Menu for deletion
		makeActions(attributeListViewer);
		addContextMenu(attributeListViewer);
		
		addDragSupport();
		
		runQueryBtn = new Button(attributeViewerComposite, SWT.PUSH);
		runQueryBtn.setText("Run Query");
		runQueryBtn.addListener(SWT.Selection, new Listener()
		{
			public void handleEvent(Event event)
			{				
				//Exceute only if permissions peresent
				runQuery();
			}		
		});	
		
		setLayoutForAttributeViewer();
	}
	
	private void addDragSupport() {
		int ops = DND.DROP_COPY | DND.DROP_MOVE | DND.DROP_LINK;
		Transfer[] transfers = new Transfer[] { TextTransfer.getInstance()};
		attributeListViewer.addDragSupport(ops, transfers, new DragSourceListener(){

			TableItem dragSourceItem [] = new TableItem[1];
			public void dragFinished(DragSourceEvent event) {
				if (event.detail == DND.DROP_MOVE){
					dragSourceItem[0] = null;
				}
			}

			public void dragSetData(DragSourceEvent event) {
				event.data = dragSourceItem[0].getData();
				System.out.println(event.data);
			}

			public void dragStart(DragSourceEvent event) {
				TableItem [] selection = attributeListViewer.getTable().getSelection();
				if(selection.length == 1){
					 event.doit = true;
					   dragSourceItem[0] = selection[0];
				}
				else
					event.doit = false;
			}
			
		});
		
	}
	
	private Table getAttributeListTable() 
	{
		return attributeListViewer.getTable();
	}

	private Object[] getFieldsForClazz(String className)
	{
		PropertiesManager properties = Activator.getDefault().dbModel().props();
		ClassProperties classProperties = properties.getClassProperties(className);
		FieldProperties []fields = classProperties.getFields();
		Object []list = new Object[fields.length];
		int i = 0;
		for(FieldProperties field : fields)
		{
			StringBuilder sb = new StringBuilder(className);
			sb.append(OMPlusConstants.REGEX);
			sb.append(field.getFieldName());
			list[i++] = sb.toString();
		}
		return list;
	}
	
	/**
	 * Add right click options to Attribute viewer table
	 * @param tableViewer
	 */
	private void makeActions(TableViewer tableViewer) 
	{
		final Table table = tableViewer.getTable();
		deleteAction = new Action() 
		{
			public void run() 
			{
				//showMessage("Delete Action executed for "+index);
				int indices [] = table.getSelectionIndices();
				try 
				{
					table.remove(indices);				
				} 
				catch (RuntimeException e) 
				{
					e.printStackTrace();
				}
				
			}
		};
		deleteAction.setText("Delete");
		deleteAction.setToolTipText("Delete Action");
	}
	
	/**
	 * Add the context menu for attribute viewer table
	 * @param tableViewer
	 */
	private void addContextMenu(final TableViewer tableViewer) 
	{
		MenuManager menuMgr = new MenuManager("#PopupMenu");
		menuMgr.setRemoveAllWhenShown(true);
		menuMgr.addMenuListener(new IMenuListener() 
		{
			public void menuAboutToShow(IMenuManager manager) 
			{
					manager.add(deleteAction);
					manager.add(new Separator(IWorkbenchActionConstants.MB_ADDITIONS));
			}
		});
		Menu menu = menuMgr.createContextMenu(tableViewer.getControl());
		tableViewer.getControl().setMenu(menu);		
	}
	
	/**
	 * Set layout for Attribute Viewer
	 */
	private void setLayoutForAttributeViewer()
	{
		
		mainAttributeViewerComposite.setLayout(new FormLayout());
		
		FormData data = new FormData();
		data.top = new FormAttachment(0,1);
		data.left = new FormAttachment(2,2);	
		//data.width = 380;
		data.right = new FormAttachment(100,-5);
		buttonCompositeForAttributeViewer.setLayoutData(data);
		
		data = new FormData();
		data.top = new FormAttachment(buttonCompositeForAttributeViewer,2);
		data.left = new FormAttachment(2,2);
		data.right = new FormAttachment(100,-5);
		//data.width = 380;
		attributeViewerComposite.setLayoutData(data);
		
		//For buttons
		buttonCompositeForAttributeViewer.setLayout(new FormLayout());
		FormData btnData = new FormData();
		btnData.top = new FormAttachment(2,2);
		btnData.left = new FormAttachment(2,2);
		addGroupBtn.setLayoutData(btnData);
		
		btnData = new FormData();
		btnData.top = new FormAttachment(2,2);
		btnData.left = new FormAttachment(addGroupBtn,5);
		clearAllGroupsBtn.setLayoutData(btnData);
		
		//For table+runQuery btn
		attributeViewerComposite.setLayout(new FormLayout());
		
		data = new FormData();
		data.top = new FormAttachment(2,2);
		data.left = new FormAttachment(0,3);
		attributeLabel.setLayoutData(data);
		
		
		data = new FormData();
		data.top = new FormAttachment(attributeLabel,2);
		data.left = new FormAttachment(0,3);
		data.right = new FormAttachment(100,-5);
		data.height = ATTRIBUTE_VIEWER_TABLE_HEIGHT;
		//data.width = ATTRIBUTE_VIEWER_TABLE_WIDTH;		
		attributeListViewer.getTable().setLayoutData(data);
		
		data = new FormData();
		data.top = new FormAttachment(attributeListViewer.getTable(),2);
		//data.top = new FormAttachment(composite0,2);
		data.left = new FormAttachment(0,3);		
		runQueryBtn.setLayoutData(data);		
		
	}
	
	/**
	 * Reset the attributeViewer when the result set changes
	 */
	private void resetAttributeViewer()
	{
		for(Control control: attributeViewerComposite.getChildren())
		{
			control.dispose();
		}
		addChildComponentsToAttributeViewer();
		setAttributeViewerFields();		
		refreshAttributeViewer();
	}
	
	private void refreshAttributeViewer()
	{
		attributeViewerComposite.layout(true,true);
				
	}

	/**
	 * Exceute a query
	 */
	private void runQuery() 
	{
		if(!checkForValidityOfClauses())
		{
			return;
		}
		
		String[] fields = getAttributeViewerFields();
		
		QueryResultsManager results = new QueryResultsManager();
		constraintList.setAttributeList(fields);

		results.runQuery(constraintList);
		QueryResultList rList = results.getResultList();
		
		// add constraintList to recent Queries. Now adds queries with 0 results
		recentQueries.addNewDBQuery(createQuery(constraintList));
		recentQueriesCombo.setItems(getRecentQueriesFromDB());
		
		IViewPart view = ViewerManager.getView(this.getSite().getPage(), OMPlusConstants.QUERY_RESULTS_ID);
		if(view instanceof QueryResults)
		{
			((QueryResults)view).addNewQueryResults(rList);
		}
		ViewerManager.showView(this.getSite().getPage(), OMPlusConstants.QUERY_RESULTS_ID);
		
	}

	private OMQuery createQuery(OMQuery constraintList2) {
		if(constraintList2 != null)
		{
			OMQuery newQuery = new OMQuery();
			newQuery.setAttributeList(constraintList2.getAttributeList());
			ArrayList< QueryGroup> listGroup = newQuery.getConstraintList();
			ListIterator< QueryGroup> grpIterator = constraintList2.getConstraintList().listIterator();
			while(grpIterator.hasNext())
			{
				QueryGroup group = (QueryGroup)grpIterator.next();
				QueryGroup newGroup = new QueryGroup();
				newGroup.setGroupOperator(group.getGroupOperator());
				ArrayList< QueryClause> listClauses = newGroup.getQueryList();
				ListIterator< QueryClause> clauseIterator = group.getQueryList().listIterator();
				while (clauseIterator.hasNext())
				{
					QueryClause clause = (QueryClause) clauseIterator.next();
					QueryClause newClause = new QueryClause();
					newClause.setField(new String(clause.getField()));
					newClause.setCondition(clause.getCondition());
					newClause.setOperator(clause.getOperator());
					newClause.setValue(clause.getValue());
					listClauses.add(newClause);
				}
				newGroup.setQueryList(listClauses);
				listGroup.add(newGroup);
			}
			newQuery.setConstraintList(listGroup);
			return newQuery;
		}
		return null;
	}

	/**
	 * Check for Validity of every row filled in QueryGroups 
	 * @return
	 */
	private boolean checkForValidityOfClauses()
	{
		boolean errorFlag = false;
		if(validateBuiltQuery())
		{
			for(int i = 0; i < constraintList.getConstraintList().size(); i++)
			{
				QueryGroup group = constraintList.getQueryGroup(i);
				for(int j = 0; j < group.getQueryList().size(); j++)
				{
					QueryClause clause = group.getQueryList().get(j);						
					if(!validClause(clause.getValue(),clause.getField()))
					{
						errorFlag = true;
						break;
					}							
				}
				if(errorFlag) //ErrorFlag already set by some clause
					break;
			}
			
			return (errorFlag ?  false : true);
//			if(errorFlag) 
//				return false; 
//			else
//				 return true;			
		}
		else//Invalid Query
			return false;
	}
	
		
	

	/**
	 * Valiadtes and modifies the query that has been built
	 * @return
	 */
	private boolean validateBuiltQuery()
	{
		if(constraintList.getConstraintList().size()>0)
		{
			//The first queryGroup should have some data
			QueryGroup group0 = constraintList.getQueryGroup(0);
			if(group0.getQueryList().size()<=0)
			{
				showMessage("Expression Group 0 cannot be empty ");
				return false;					
			}
			
			//Remaining QueryGroups from 1..n if no clauses in them delet the group from OmQuery 
			for(int i = 1; i < constraintList.getConstraintList().size(); i++)
			{
				QueryGroup group = constraintList.getQueryGroup(i);
				if(group.getQueryList().size()<=0)
				{
					close(i);					
				}				
			}
			return true;
			
		}
		else
		{
			showMessage("No queries added ");
			return false;	
		}
	}

	/**
	 * Validate individual fields wrt their datatypes
	 * @param value
	 * @param field
	 * @return
	 */
	private boolean validClause(Object value, String field) 
	{
		if(value == null ||  value.toString().trim().length()==0)
		{
			showMessage("Values cannot be blank");
			return false;
		}
		else
			return true;
	}
		
	
	private void showMessage(String message)
	{
		new ShellErrorMessageSink(parentComposite.getShell()).showError(message);
	}

	/**
	 * Add a new QueryGroup in the model
	 */
	private void addNewGroup()
	{
		constraintList.initQueryGroup(nextQueryGroupIndex());
		QueryGroupComposite queryGroupComposite = new QueryGroupComposite(queryModel(), childComposite,compositeStyle,
				nextQueryGroupIndex(),constraintList.getQueryGroup(nextQueryGroupIndex()),
				this, this);

		querygroupList.add(queryGroupComposite);		
		
	}

	/**
	 * Returns the next index where the QueryGroup is to be added
	 * @return
	 */
	private int nextQueryGroupIndex()
	{
		return querygroupList.size();
	}
	

	@Override
	public void setFocus() {
		// Auto-generated method stub
		
	}

	/**
	 * A query group has been closed. reset the layout
	 */
	public void close(int index)
	{
		QueryGroupComposite composite =  querygroupList.get(index);
		composite.dispose();
		querygroupList.remove(index);
		
		constraintList.removeQueryGroup(index);
		
		//Modify indexes for all the remaining groups
		for(int i = 0; i < querygroupList.size(); i++)
		{
			composite =  querygroupList.get(i);
			composite.modifyIndex(i);
		}
		
		for(int i = 1; i < querygroupList.size(); i++)
		{
			composite =  querygroupList.get(i);
			setLayoutForQueryGroups();
		}
		refreshQueryGroup();
		
	}

	/**
	 * A query group has been resizes. reset the layout
	 */
	public void resized(int index) 
	{
		//addFirstLayout(querygroupList.get(0));
		for(int i = 0; i < querygroupList.size(); i++)
		{
			QueryGroupComposite composite = querygroupList.get(i);
			//setLayoutForGroups(i, composite);
			setLayoutForQueryGroups(i,composite);
		}
		//Reset teh min size of scrolled composite else loss of some UI
		scrollComposite.setMinSize(childComposite.computeSize(SWT.DEFAULT, SWT.DEFAULT));
		scrollComposite.layout(true);
		refreshQueryGroup();
		
	}
	
	/**
	 * Add new query group
	 */
	private void addNewQueryGroupDynamically()
	{
		try
		{
			constraintList.initQueryGroup(nextQueryGroupIndex());
			QueryGroupComposite groupComposite = new QueryGroupComposite(queryModel(), childComposite,compositeStyle,
															nextQueryGroupIndex(),
															constraintList.getQueryGroup(nextQueryGroupIndex()),
															this,this);
			querygroupList.add(groupComposite);
			setLayoutForQueryGroups();
			System.out.println("child height = "+childComposite.computeSize(SWT.DEFAULT, SWT.DEFAULT));
			scrollComposite.setMinSize(childComposite.computeSize(SWT.DEFAULT, SWT.DEFAULT));
			refreshQueryGroup();
			
			//Scroll, so that newly added group becomes visible
			Rectangle rectangle = groupComposite.getBounds();
			scrollComposite.setOrigin(rectangle.x, rectangle.y);
		}
		catch (Exception e) 
		{
			e.printStackTrace();
		}
	}
	
	/**
	 * Test code
	 * @return
	 */
			
	public  OMQuery showDataInModel()
	{
//		constraintList.printAllData();
		return constraintList;
		
	}
	
	/**
	 * Refresh the QueryGroup pane. Need to set the layouts after evry refresh
	 * Otherwise after selection of combo the query builder doen't show the hor and vert bars
	 */
	private void refreshQueryGroup()
	{
		setLayoutForQueryGroups();		
				
		scrollComposite.setMinSize(childComposite.computeSize(SWT.DEFAULT, SWT.DEFAULT));
		scrollComposite.layout(true);
		mainSashForm.layout(true);
	}

	public boolean isDropValid(String className) 
	{
		String cName = constraintList.getQueryClass();
		String attributes[] = getAttributeViewerFields();
		if(attributes == null ){
			if(cName == null){
				return true;
			}
			else
				return className.equals(cName);
		}else{
			String viewClass = getClassName(attributes[0]);
			if(cName == null)
				return viewClass.equals(className);
			else
				return ( className.equals(cName) && viewClass.equals(className));
		}
	}
	
	/**
	 * Get the attribute list from Attribute Viewer table
	 * @return
	 */
	public String[] getAttributeViewerFields(){
		TableItem []tableItems = attributeListViewer.getTable().getItems();
		int count = tableItems.length;
		if(count == 0)
			return null;
		String []fields = new String[count];
		for(int i = 0; i< count; i++)
		{
			fields[i] = (String)tableItems[i].getData();
		}
		return fields;
	}
	
	public void setAttributeViewerFields()
	{
		if(constraintList.getAttributeList()!=null)
		{
			attributeListViewer.add(constraintList.getAttributeList());
		}
		//attributeListViewer.refresh();
	}
	
	
	
	public String getClassName(String fieldName){
		return fieldName.split(OMPlusConstants.REGEX)[0];
	}
	
	/**
	 * Reset the QueryBuilder for a completely new DB
	 */
	public void resetQueryBuilderForNewDB()
	{
		enableBtn(true);
		//1. add code for adding diff recent queries
		recentQueriesCombo.setItems(getRecentQueriesFromDB());
		
		//2.reset the queryGroups
		showNewQueryGroupComposite();
		
		//3.reset the attribute viewer
		resetAttributeViewer();
	}
	
	/**
	 * Called when all tabs closed in Queryresults
	 */
	public void allResultsTabClosedInQueryResults() 
	{
		//1.reset the queryGroups
		showNewQueryGroupComposite();
		
		//2.reset the attribute viewer
		resetAttributeViewer();		
	}
	
	
	/**
	 * The ClassViewer view has been activated, modify the QueryBuilder accordingly
	 */
	public void classViewerActivated() 
	{
		showNewQueryGroupComposite();
		resetAttributeViewer();
	}
	
	private QueryPresentationModel queryModel() {
		return Activator.getDefault().queryModel();
	}
}
