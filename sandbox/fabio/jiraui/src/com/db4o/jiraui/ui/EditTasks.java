package com.db4o.jiraui.ui;

import java.awt.*;
import java.net.*;
import java.text.*;
import java.util.*;
import java.util.List;

import org.eclipse.swt.*;
import org.eclipse.swt.dnd.*;
import org.eclipse.swt.events.*;
import org.eclipse.swt.graphics.Point;
import org.eclipse.swt.graphics.Rectangle;
import org.eclipse.swt.layout.*;
import org.eclipse.swt.layout.GridLayout;
import org.eclipse.swt.widgets.*;
import org.eclipse.swt.widgets.Button;
import org.eclipse.swt.widgets.Composite;
import org.eclipse.swt.widgets.Event;
import org.eclipse.swt.widgets.Label;
import org.eclipse.swt.widgets.Menu;
import org.eclipse.swt.widgets.MenuItem;
import org.eclipse.swt.widgets.Tree;

import com.db4o.foundation.*;
import com.db4o.jiraui.*;
import com.db4o.jiraui.api.*;

public final class EditTasks extends Composite {
	
	private static final int MAX_HISTORY_SIZE = 20;
	private final EditTasksController controller;
	private Tree tree;
	private TreeItem[] dragSourceItems;
	private Map<String, TreeItem> taskToItem = new HashMap<String, TreeItem>();
	private Label selectionInfo;
	private List<Resource> resources = new ArrayList<Resource>();
	private Map<Resource, MenuItem> resourceMenuItems = new HashMap<Resource, MenuItem>();
	private Map<Integer, MenuItem> iterationMenuItems = new HashMap<Integer, MenuItem>();
	private Menu resourcesMenu;
	private Menu iterationMenu;
	protected boolean selectingResources;
	private Menu menu;
	private Text search;
	private List<Pair<Integer, Integer>> history = new ArrayList<Pair<Integer, Integer>>();
	private Button back;
	private Pair<TreeItem, TreeItem> lastSelectedItem;
	private boolean goingBack;
	private SimpleDateFormat formatter = new SimpleDateFormat("yyyy-MM-dd");
	private List<Field> fields = new ArrayList<Field>();
	protected Field sortField;


	
	public EditTasks(Composite parent, int style, EditTasksController editTasksController) {
		
		super(parent, style);
		this.controller = editTasksController;
		
		setLayout(new GridLayout(1, true));
		
		addTopComposite();
		addTree();
		addBottomComposite();
	
		repopulate();
	}

	private void addTree() {
		
		tree = new Tree(this, SWT.BORDER | SWT.H_SCROLL | SWT.V_SCROLL | SWT.MULTI | SWT.FULL_SELECTION);
		tree.setLayoutData(new GridData(SWT.FILL, SWT.FILL, true, true));
		tree.setHeaderVisible(true);
		tree.setLinesVisible(true);
		
		addTreeKeyListener();
		addTreeColumns();
		addTreeContextMenu();
		addTreeMouseListener();
		addTreeSelectionListener();
		addTreeDragAndDropSupport();
		
	}

	private void addTreeKeyListener() {
		tree.addKeyListener(new KeyListener() {
			
			@Override
			public void keyReleased(KeyEvent arg0) {
			}
			
			@Override
			public void keyPressed(KeyEvent arg0) {
				if (arg0.keyCode == SWT.ESC) {
					search.setText("");
				}
			}
		});
	}

	private void addTreeDragAndDropSupport() {
		Transfer[] types = new Transfer[] {TextTransfer.getInstance()};
		int operations = DND.DROP_MOVE /*| DND.DROP_COPY | DND.DROP_LINK*/;
		
		final DragSource source = new DragSource (tree, operations);
		source.setTransfer(types);
		source.addDragListener (new DragSourceListener () {
			public void dragStart(DragSourceEvent event) {
				dragSourceItems = tree.getSelection();
				event.doit = true;
			};
			public void dragSetData (DragSourceEvent event) {
				String text = "";
				for(TreeItem item : dragSourceItems) {
					Task task = (Task)item.getData();
					if (!text.isEmpty()) {
						text += ", ";
					}
					text += task.getKey();
				}
				event.data = text;
			}
			public void dragFinished(DragSourceEvent event) {
//				if (event.detail == DND.DROP_MOVE)
//					dragSourceItem[0].dispose();
//				dragSourceItem[0] = null;
//				dragSourceItems = null;
			}
		});

		DropTarget target = new DropTarget(tree, operations);
		target.setTransfer(types);
		target.addDropListener (new DropTargetAdapter() {
			public void dragOver(DropTargetEvent event) {
				event.feedback = /*DND.FEEDBACK_EXPAND |*/ DND.FEEDBACK_SCROLL;
				if (event.item != null) {
					TreeItem item = (TreeItem)event.item;
					Point pt = getDisplay().map(null, tree, event.x, event.y);
					Rectangle bounds = item.getBounds();
					if (pt.y < bounds.y + bounds.height/2) {
						event.feedback |= DND.FEEDBACK_INSERT_BEFORE;
					} else /*if (pt.y > bounds.y + 2*bounds.height/2)*/ {
						event.feedback |= DND.FEEDBACK_INSERT_AFTER;
//					} else {
//						event.feedback |= DND.FEEDBACK_SELECT;
					}
				}
			}
			public void drop(DropTargetEvent event) {
				if (event.data == null) {
					event.detail = DND.DROP_NONE;
					return;
				}
				String text = (String)event.data;
				if (event.item == null) {
					TreeItem item = new TreeItem(tree, SWT.NONE);
					item.setText(text);
				} else {
					TreeItem item = (TreeItem)event.item;
					Point pt = getDisplay().map(null, tree, event.x, event.y);
					Rectangle bounds = item.getBounds();
					TreeItem[] items = tree.getItems();
					int index = 0;
					for (int i = 0; i < items.length; i++) {
						if (items[i] == item) {
							index = i;
							break;
						}
					}
					if (pt.y < bounds.y + bounds.height/2) {
					} else /*if (pt.y > bounds.y + 2*bounds.height/2)*/ {
						index++;
//					} else {
						// add as child
					}
					
					
					moveItems(items, dragSourceItems, index);
					
				}
			}
		});
		
	}

	private void addTreeSelectionListener() {
		tree.addSelectionListener(new SelectionListener() {
			
			@Override
			public void widgetSelected(SelectionEvent arg0) {
				updateSelectedInfo();
				updateHistory((TreeItem) arg0.item);
			}



			
			@Override
			public void widgetDefaultSelected(SelectionEvent arg0) {
			}
		});
	}

	private void accumulateHistory(Pair<TreeItem, TreeItem> selection) {
		int index = tree.indexOf(selection.second);
		if (!history.isEmpty() && history.get(history.size()-1).second == index) return;
		history.add(new Pair<Integer, Integer>(tree.indexOf(selection.first.isDisposed() ? tree.getTopItem() : selection.first), index));
		back.setEnabled(true);
		
		if (history.size() > MAX_HISTORY_SIZE) {
			history = new ArrayList<Pair<Integer,Integer>>(history.subList(history.size()-MAX_HISTORY_SIZE, history.size()));
		}
	}

	private void addTreeMouseListener() {
		tree.addMouseListener(new MouseListener() {
			
			@Override
			public void mouseUp(MouseEvent e) {
			}
			
			@Override
			public void mouseDown(MouseEvent e) {
			}
			
			@Override
			public void mouseDoubleClick(MouseEvent e) {
				if (tree.getSelection().length == 0) {
					return;
				}
				Task task = (Task) tree.getSelection()[0].getData();
				String url = "http://tracker.db4o.com/browse/"+task.getKey();
				try {
					Desktop.getDesktop().browse(new URI(url));
				} catch (Exception e1) {
					e1.printStackTrace();
				}
			}
		});
	}

	private void addTreeContextMenu() {
		
		menu = new Menu (getShell(), SWT.POP_UP);
		
		addMenuMoveToTop(menu);
		addMenuMoveToBottom(menu);
		addMenuEstimate(menu);
		addMenuAssign(menu);
		addMenuIteration(menu);
		addMenuDropTask(menu);
		addMenuLabelTask(menu);
		
		tree.setMenu (menu);
	}

	private void addMenuDropTask(Menu parent) {
		MenuItem menuItem = new MenuItem (parent, SWT.PUSH);
		menuItem.setText ("Drop task");
		
		menuItem.addSelectionListener(new SelectionListener() {
			
			@Override
			public void widgetSelected(SelectionEvent arg0) {
				final Set<Task> selectedTasks = new LinkedHashSet<Task>();
				TreeItem[] selectedItems = tree.getSelection();
				for(TreeItem item : selectedItems) {
					selectedTasks.add((Task) item.getData());
				}
				controller.dropTasks(selectedTasks);
				for(TreeItem item : selectedItems) {
					fillItem((Task)item.getData(), item);
				}
			}
			
			@Override
			public void widgetDefaultSelected(SelectionEvent arg0) {
			}
		});
	}
	
	private void addMenuLabelTask(Menu parent) {
		MenuItem menuItem = new MenuItem (parent, SWT.PUSH);
		menuItem.setText ("Label task...");
		
		menuItem.addSelectionListener(new SelectionListener() {
			
			@Override
			public void widgetSelected(SelectionEvent arg0) {
				final Set<Task> selectedTasks = new LinkedHashSet<Task>();
				TreeItem[] selectedItems = tree.getSelection();
				Set<String> labels = new LinkedHashSet<String>();
				for(TreeItem item : selectedItems) {
					Task data = (Task) item.getData();
					selectedTasks.add(data);
					for(String s : data.getLabel()) {
						labels.add(s);
					}
				}
				
				String label = formatLabel(labels);
				
				String newLabel = new LabelDialog(getDisplay(), label).open();
				
				if (newLabel == null) {
					return;
				}
				
				controller.setLabel(selectedTasks, newLabel);
				for(TreeItem item : selectedItems) {
					fillItem((Task)item.getData(), item);
				}
			}

			
			@Override
			public void widgetDefaultSelected(SelectionEvent arg0) {
			}
		});
	}
	
	private String formatLabel(Iterable<String> labels) {
		String label = "";
		for(String s : labels) {
			if (!label.isEmpty()) {
				label += " ";
			}
			label += s;
		}
		return label;
	}
	private void addMenuAssign(Menu parent) {
		MenuItem menuItem = new MenuItem (parent, SWT.CASCADE);
		menuItem.setText ("Assign to");
		
		resourcesMenu = new Menu(parent);
		menuItem.setMenu(resourcesMenu);
		
		resourcesMenu.addMenuListener(new MenuListener() {
			
			@Override
			public void menuShown(MenuEvent arg0) {
				checkSelectedTaskResources();
			}
			
			@Override
			public void menuHidden(MenuEvent arg0) {
			};
		});
	}
	
	public static int currentWeek() {
		TimeZone utc = TimeZone.getTimeZone("UTC");
		Calendar calendar1 = Calendar.getInstance(utc);
		Calendar calendar2 = Calendar.getInstance(utc);
		calendar1.set(2011, 1, 22, 15, 0, 0);
		calendar2.setTime(new Date());
		long milliseconds1 = calendar1.getTimeInMillis();
		long milliseconds2 = calendar2.getTimeInMillis();
		long diff = milliseconds2 - milliseconds1;
		long diffWeeks = (long) (diff / (7 * 24 * 60 * 60 * 1000.));
		return (int) (diffWeeks + 188);
	}
	
	private void addMenuIteration(Menu parent) {
		MenuItem menuItem = new MenuItem (parent, SWT.CASCADE);
		menuItem.setText ("Iteration");
		
		iterationMenu = new Menu(parent);
		menuItem.setMenu(iterationMenu);
		
		iterationMenu.addMenuListener(new MenuListener() {
			
			@Override
			public void menuShown(MenuEvent arg0) {
				checkSelectedTaskIteration();
			}
			
			@Override
			public void menuHidden(MenuEvent arg0) {
			};
		});
		
		int currentWeek = currentWeek();
		
		for (int i=-1;i<2;i++) {
			final MenuItem item = new MenuItem (iterationMenu, SWT.RADIO);
			final int week = i==-1?0:currentWeek-i;
			item.setText (i==-1?"None":""+week);
			iterationMenuItems.put(week, item);
			item.addSelectionListener(new SelectionListener() {
				
				@Override
				public void widgetSelected(final SelectionEvent arg0) {
					final Set<Task> selectedTasks = new LinkedHashSet<Task>();
					TreeItem[] selectedItems = tree.getSelection();
					for(TreeItem item : selectedItems) {
						selectedTasks.add((Task) item.getData());
					}
					controller.setIteration(selectedTasks, week);
					for(TreeItem item : selectedItems) {
						fillItem((Task)item.getData(), item);
					}
				}
				
				@Override
				public void widgetDefaultSelected(SelectionEvent arg0) {
				}
			});
		}
	}

	protected void checkSelectedTaskResources() {
		for(MenuItem menu : resourceMenuItems.values()) {
			menu.setSelection(false);
		}
		for(TreeItem item : tree.getSelection()) {
			Task t = (Task)item.getData();
			for(Resource r : t.getResources()) {
				MenuItem menuItem = resourceMenuItems.get(r);
				if (menuItem == null) {
					System.out.println("---------------> couldnt find menu entry for resource: " + r.getId());
					continue;
				}
				menuItem.setSelection(true);
			}
		}
	}

	protected void checkSelectedTaskIteration() {
		for(MenuItem menu : iterationMenuItems.values()) {
			menu.setSelection(false);
		}
		for(TreeItem item : tree.getSelection()) {
			Task t = (Task)item.getData();
			Iteration it = t.getIteration();
			int week = it == null ? 0 : it.getId();
			MenuItem menuItem = iterationMenuItems.get(week);
			if (menuItem == null) {
				System.out.println("-------------> couldnt find menu item for iteration " + week);
				continue;
			}
			menuItem.setSelection(true);
		}
	}

	private void populateResourcesMenu() {

		for(MenuItem item : resourceMenuItems.values()) {
			item.dispose();
		}
		resourceMenuItems.clear();
		boolean addingFavorites = true;
		for(final Resource r : resources) {
			if (addingFavorites && !r.isFavorite()) {
				addingFavorites = false;
				new MenuItem(resourcesMenu, SWT.SEPARATOR);
			}
			final MenuItem item = new MenuItem (resourcesMenu, SWT.CHECK);
			item.setText (r.getName());
			resourceMenuItems.put(r, item);
			item.addSelectionListener(new SelectionListener() {
				
				@Override
				public void widgetSelected(final SelectionEvent arg0) {
					final Set<Task> selectedTasks = new LinkedHashSet<Task>();
					TreeItem[] selectedItems = tree.getSelection();
					for(TreeItem item : selectedItems) {
						selectedTasks.add((Task) item.getData());
					}
					if (item.getSelection()) {
						controller.addAssign(selectedTasks, r);
					} else {
						controller.removeAssign(selectedTasks, r);
					}
					for(TreeItem item : selectedItems) {
						fillItem((Task)item.getData(), item);
					}
				}
				
				@Override
				public void widgetDefaultSelected(SelectionEvent arg0) {
				}
			});
		}
	}
	
	private void addMenuEstimate(Menu parent) {
		MenuItem menuItem = new MenuItem (parent, SWT.CASCADE);
		menuItem.setText ("Estimate");
		
		Menu menu = new Menu(parent);
		menuItem.setMenu(menu);
		for(int i=0;i<10;i++) {
			final int estimate = i;
			MenuItem item = new MenuItem (menu, SWT.PUSH);
			item.setText (i == 0 ? "None" : ""+i);
			item.addSelectionListener(new SelectionListener() {
				
				@Override
				public void widgetSelected(SelectionEvent arg0) {
					final Set<Task> selectedTasks = new LinkedHashSet<Task>();
					TreeItem[] selectedItems = tree.getSelection();
					for(TreeItem item : selectedItems) {
						selectedTasks.add((Task) item.getData());
					}
					controller.estimate(selectedTasks, estimate);
					for(TreeItem item : selectedItems) {
						fillItem((Task)item.getData(), item);
					}
				}
				
				@Override
				public void widgetDefaultSelected(SelectionEvent arg0) {
				}
			});
		}
	}

	private void addMenuMoveToTop(Menu menu) {
		MenuItem moveToTop = new MenuItem (menu, SWT.CASCADE);
		moveToTop.setText ("Move to top of order");
		
		Menu subMenu = new Menu(menu);
		moveToTop.setMenu(subMenu);
		
		for(int i=1;i<=9;i++) {
			final int order = i;
			addSetOrderMenu(subMenu, i, true, new Predicate4<Integer>() {

				@Override
				public boolean match(Integer candidate) {
					return candidate >= order;
				}
			});
		}
	}
	
	private void addMenuMoveToBottom(Menu parent) {
		MenuItem menuItem = new MenuItem (parent, SWT.CASCADE);
		menuItem.setText ("Move to bottom of order");
		
		Menu subMenu = new Menu(parent);
		menuItem.setMenu(subMenu);
		
		for(int i=1;i<=9;i++) {
			final int order = i;
			addSetOrderMenu(subMenu, i, false, new Predicate4<Integer>() {

				@Override
				public boolean match(Integer candidate) {
					return candidate > order;
				}
			});
		}
	}

	private void addSetOrderMenu(Menu subMenu, int i, final boolean defaultTop, final Predicate4<Integer> predicate) {
		final int order = i;
		MenuItem item = new MenuItem (subMenu, SWT.PUSH);
		item.setText (""+i);
		item.addSelectionListener(new SelectionListener() {
			
			@Override
			public void widgetSelected(SelectionEvent arg0) {
				final Set<Task> selectedTasks = new LinkedHashSet<Task>();
				TreeItem[] selectedItems = tree.getSelection();
				for(TreeItem item : selectedItems) {
					selectedTasks.add((Task) item.getData());
				}
				final ByRef<Task> taskBefore = new ByRef<Task>();
				Task taskAfter = controller.acceptTaskVisitor(new Visitor<Task>() {
					
					@Override
					public Task visit(Task t) {
						if (selectedTasks.contains(t)) {
							return null;
						}
						if (predicate.match(t.getOrder())) {
							return t;
						} else {
							taskBefore.value = t;
						}
						return null;
					}
				});

				TreeItem[] items = tree.getItems();
				int index = 0;
				for(int i=0;i<items.length;i++) {
					TreeItem treeItem = items[i];
					Task task = (Task)treeItem.getData();
					if (selectedTasks.contains(task)) {
						continue;
					}
					if (predicate.match(task.getOrder())) {
						index = i;
						break;
					}
				}
				
				if (index == 0 && !predicate.match(((Task)items[items.length-1].getData()).getOrder())) {
					index = items.length;
				}
				
				moveItems(selectedItems, selectedTasks, index, taskBefore.value, taskAfter, order);
				
				
			}
			
			@Override
			public void widgetDefaultSelected(SelectionEvent arg0) {
			}
		});
	}

	protected <T> List<T> list(T... tasks) {
		return Arrays.asList(tasks);
	}
	
	
	public abstract static class Field implements Comparator<Task> {
		private final String columnName;
		private final int swtAligmnent;

		public Field(String columnName) {
			this(columnName, SWT.LEFT);
		}

		public Field(String columnName, int swtAligmnent) {
			this.columnName = columnName;
			this.swtAligmnent = swtAligmnent;
		}

		public abstract String toString(Task t);

		public String getColumnName() {
			return columnName;
		}

		public int getSwtAligmnent() {
			return swtAligmnent;
		}

		@Override
		public int compare(Task o1, Task o2) {
			return toString(o1).compareToIgnoreCase(toString(o2));
		}

	}
	
	public static int compareFloat(float first, float second) {
		if (first == second) return 0;
		return first > second ? 1 : -1;
	}


	private void addTreeColumns() {
		
		addColumn(new Field("Key") {

			@Override
			public int compare(Task o1, Task o2) {
				String k1 = (o1.isDirty() ? "A" : "B") + o1.getKey();
				String k2 = (o2.isDirty() ? "A" : "B") + o2.getKey();
				return k1.compareTo(k2);
			}

			@Override
			public String toString(Task t) {
				return t.getKey() + (t.isDirty() ? "*" : "");
			}
		});
		
		addColumn(new Field("Created") {

			@Override
			public int compare(Task o1, Task o2) {
				return o1.getCreated().compareTo(o2.getCreated());
			}

			@Override
			public String toString(Task t) {
				return formatter.format(t.getCreated());
			}
		});
		
		addColumn(new Field("Summary") {

			@Override
			public String toString(Task t) {
				return t.getSummary();
			}
		});
		
		addColumn(new Field("Order") {

			@Override
			public int compare(Task o1, Task o2) {
				return o1.getOrder()-o2.getOrder();
			}

			@Override
			public String toString(Task t) {
				return ""+t.getOrder();
			}
		});
		
		addColumn(new Field("Estimate") {

			@Override
			public int compare(Task o1, Task o2) {
				return o1.getEstimate()-o2.getEstimate();
			}

			@Override
			public String toString(Task t) {
				return t.getEstimate() == 0 ? "" : "" + t.getEstimate();
			}
		});
		
		addColumn(new Field("Iteration") {

			@Override
			public int compare(Task o1, Task o2) {
				int i1 = o1.getIteration() == null ? 0 : o1.getIteration().getId();
				int i2 = o2.getIteration() == null ? 0 : o2.getIteration().getId();
				return i1-i2;
			}

			@Override
			public String toString(Task t) {
					return t.getIteration() == null ? "" : "" + t.getIteration().getId(); 
			}
		});
		
		addColumn(new Field("Assignee") {

			@Override
			public String toString(Task t) {
				return formatAssignee(t.getResources());
			}
		});
		
		addColumn(new Field("Labels") {

			@Override
			public String toString(Task t) {
				return formatLabel(list(t.getLabel()));
			}
		});
		

		TreeColumn fineOrder = addColumn(new Field("Fine Order", SWT.RIGHT) {

			@Override
			public int compare(Task o1, Task o2) {
				return compareFloat(o1.getFineGrainedOrder(), o2.getFineGrainedOrder());
			}

			@Override
			public String toString(Task t) {
				return String.format("%.4f", t.getFineGrainedOrder());
			}
		});
		
		setSortField(fineOrder);

	}
	


		
	private TreeColumn addColumn(final Field field) {
		fields.add(field);
		final TreeColumn column = new TreeColumn(tree, field.getSwtAligmnent());
		column.setData(field);
		column.setText(field.getColumnName());
		
	    column.addListener(SWT.Selection, new Listener() {
			
			@Override
	        public void handleEvent(Event e) {
				setSortField(column);
	        }
	    });
		return column;

	}

	private void updateSelectedInfo() {
		selectionInfo.setText(tree.getSelectionCount() +"/"+tree.getItemCount());
	}

	private void moveItems(TreeItem[] items, TreeItem[] sourceItems, int index) {
		List<Task> tasks = new ArrayList<Task>();
		for(TreeItem dragged : sourceItems) {
			tasks.add((Task)dragged.getData());
		}
		Task taskBefore = index == 0 ? null : (Task) items[index-1].getData();
		Task taskAfter = (Task) items[index].getData();
		moveItems(sourceItems, tasks, index, taskBefore, taskAfter, -1);
	}

	private void moveItems(TreeItem[] sourceItems, Collection<Task> tasks, int index, Task taskBefore, Task taskAfter, int order) {
		updateHistory(sourceItems[0]);
		Collection<Task> modifiedTasks = controller.moveTasks(taskBefore, taskAfter, tasks, order);
		if (modifiedTasks != tasks) {
			List<TreeItem> selected = new ArrayList<TreeItem>();
			for(TreeItem item : tree.getItems()) {
				if (modifiedTasks.contains(item.getData())) {
					selected.add(item);
				}
			}
			Collections.sort(selected, new Comparator<TreeItem>() {

				@Override
				public int compare(TreeItem o1, TreeItem o2) {
					float t1 = ((Task)o1.getData()).getFineGrainedOrder();
					float t2 = ((Task)o2.getData()).getFineGrainedOrder();
					if (t1 == t2) return 0;
					return t1 > t2 ? 1 : -1;
				}
			});
			sourceItems = selected.toArray(new TreeItem[selected.size()]);
		}
		
		tree.setRedraw(false);
		List<TreeItem> newItems = new ArrayList<TreeItem>();
		lastSelectedItem = null;
		for(TreeItem dragged : sourceItems) {
			Task task = (Task)dragged.getData();
			TreeItem item = insertTask(tree, task, index++);
			newItems.add(item);
			if (lastSelectedItem == null) {
				lastSelectedItem = new Pair<TreeItem, TreeItem>(tree.getTopItem(), item);
			}
		}
		for(TreeItem dragged : sourceItems) {
			dragged.dispose();
		}
		tree.setSelection(newItems.toArray(new TreeItem[newItems.size()]));
		tree.setRedraw(true);
	}

	private void addTopComposite() {
		Composite c = new Composite(this, SWT.NONE);
		c.setLayoutData(new GridData(SWT.FILL, SWT.TOP, true, false));
		
		c.setLayout(new GridLayout(7, false));
		
		back = new Button(c, SWT.PUSH);
		back.setText("Back");
		back.setEnabled(false);
		back.addSelectionListener(new SelectionListener() {
			
			@Override
			public void widgetSelected(SelectionEvent arg0) {
				back();
			}
			
			@Override
			public void widgetDefaultSelected(SelectionEvent arg0) {
			}
		});
		
		Label label = new Label(c, SWT.NONE);
		label.setText("Search:");
		label.setLayoutData(new GridData(SWT.LEFT, SWT.CENTER, false, false));
		
		search = new Text(c, SWT.SEARCH | SWT.ICON_CANCEL);
		GridData layoutData = new GridData(SWT.LEFT, SWT.CENTER, true, false);
		layoutData.widthHint = 300;
		search.setLayoutData(layoutData);
		search.addModifyListener(new ModifyListener() {
			
			Runnable searcher = new Runnable() {
				public void run() {
					setFilter(search.getText());
				}
			};
			
			@Override
			public void modifyText(ModifyEvent e) {
				getDisplay().timerExec(200, searcher);
			}
		});
		search.addKeyListener(new KeyListener() {
			
			@Override
			public void keyReleased(KeyEvent arg0) {
			}
			
			@Override
			public void keyPressed(KeyEvent arg0) {
				if (arg0.keyCode == 27) {
					search.setText("");
				}
			}
		});
		
		Button clear = new Button(c, SWT.PUSH);
		clear.setLayoutData(new GridData(SWT.LEFT, SWT.CENTER, false, false));
		clear.setText("Clear");
		clear.addSelectionListener(new SelectionListener() {
			
			@Override
			public void widgetSelected(SelectionEvent arg0) {
				MessageBox msg = new MessageBox(getShell(), SWT.ICON_QUESTION | SWT.YES | SWT.NO);
				msg.setMessage("Confirm delete all local issues?");
				msg.setText("Clear Issues");
				if (msg.open() == SWT.NO) {
					return;
				}
				clear();
			}
			
			@Override
			public void widgetDefaultSelected(SelectionEvent arg0) {
			}
		});
		
		Button fetch = new Button(c, SWT.PUSH);
		fetch.setLayoutData(new GridData(SWT.LEFT, SWT.CENTER, false, false));
		fetch.setText("Fetch from Jira");
		fetch.addSelectionListener(new SelectionListener() {
			
			@Override
			public void widgetSelected(SelectionEvent arg0) {
				controller.fetch();
			}
			
			@Override
			public void widgetDefaultSelected(SelectionEvent arg0) {
			}
		});
		
		Button save = new Button(c, SWT.PUSH);
		save.setLayoutData(new GridData(SWT.LEFT, SWT.CENTER, false, false));
		save.setText("Send to Jira");
		save.addSelectionListener(new SelectionListener() {
			
			@Override
			public void widgetSelected(SelectionEvent arg0) {
				controller.checkIn();
			}
			
			@Override
			public void widgetDefaultSelected(SelectionEvent arg0) {
			}
		});
	}
	
	protected void back() {
		if (history.isEmpty()) new IllegalStateException("History is empty");
		goingBack = true;
		Pair<Integer, Integer> index = history.remove(history.size()-1);
		TreeItem selectedItem = tree.getItem(index.second);
		TreeItem topItem = tree.getItem(index.first);
		tree.setTopItem(topItem);
		tree.setSelection(selectedItem);
		back.setEnabled(!history.isEmpty());
		goingBack = false;
		lastSelectedItem = new Pair<TreeItem, TreeItem>(topItem, selectedItem);
	}

	private void addBottomComposite() {
		Composite c = new Composite(this, SWT.BORDER);
		c.setLayoutData(new GridData(SWT.FILL, SWT.BOTTOM, true, false));
		
		c.setLayout(new GridLayout(2, false));
		
		Label status = new Label(c, SWT.NONE);
		status.setLayoutData(new GridData(SWT.LEFT, SWT.CENTER, true, false));
		
		selectionInfo = new Label(c, SWT.NONE);
		GridData gridData = new GridData(SWT.LEFT, SWT.CENTER, false, false);
		gridData.widthHint = 200;
		selectionInfo.setLayoutData(gridData);
		
	}
	
	
	private void setFilter(String filter) {

		historyClear();
		lastSelectedItem = null;
		
		
		final Set<Task> selectedTasks = new HashSet<Task>();
		for(TreeItem item : tree.getSelection()) {
			selectedTasks.add((Task) item.getData());
		}
		
		tree.removeAll();
		
		final List<TreeItem> selectedItems = new ArrayList<TreeItem>();
		
		fillTree(filter, new Visitor<TreeItem>() {

			@Override
			public TreeItem visit(TreeItem t) {
				if (selectedTasks.contains(t.getData())) {
					selectedItems.add(t);
				}
				return null;
			}
		});
		tree.setSelection(selectedItems.toArray(new TreeItem[selectedItems.size()]));
		updateSelectedInfo();
	}
	
	private void historyClear() {
		history.clear();
		back.setEnabled(false);
		lastSelectedItem = null;
	}

	public int getSuggestedWidth() {
		int w = 0;
		for(TreeColumn c : tree.getColumns()) {
			w += c.getWidth();
		}
		
		return w;
	}
	
	private void fillTree(String filter, final Visitor<TreeItem> visitor) {
		
		final Set<String> filters = filter == null || filter.isEmpty() ? null : parseFilter(filter);
		final Set<String> orders = filterByKey(filters, "order:");
		final Set<String> assignees = filterByKey(filters, "assignee:", "peer:");
		final Set<String> iterations = filterByKey(filters, "it:", "iteration:");
		final Set<String> labels = filterByKey(filters, "label:");
		

		final Set<Resource> res = filter == null ? new HashSet<Resource>() : null;
		
		final List<Task> tasks = new ArrayList<Task>();
		
		controller.acceptTaskVisitor(new Visitor<Task>() {
			@Override
			public Task visit(Task t) {
				
				if (res != null && t.getResources() != null) {
					for(Resource r : t.getResources()) {
						res.add(r);
					}
				}
				
				if (orders != null && !orders.contains(""+t.getOrder())) {
					return null;
				}
				if (iterations != null && (t.getIteration()==null || !iterations.contains(""+t.getIteration().getId()))) {
					return null;
				}
				if (labels != null && (t.getLabel()==null || !containsAny(labels, t.getLabel()))) {
					return null;
				}
				if (assignees != null) {
					boolean found = false;
					for(Resource r : t.getResources()) {
						if (assignees.contains(r.getName())) {
							found = true;
							break;
						}
					}
					if (!found) return null;
				}
				if (filters != null && !accept(filters, t)) {
					return null;
				}
				tasks.add(t);
				
				return null;
			}

		});
		
		if (sortField != null) {
			Collections.sort(tasks, new ComparatorMultiplier<Task>(sortField, tree.getSortDirection()==SWT.UP?1:-1));
		}
		
		tree.setRedraw(false);

		for(Task t : tasks) {
			TreeItem item = insertTask(tree, t, -1);
			if (visitor != null) {
				visitor.visit(item);
			}
		}
		tree.setRedraw(true);
		
		if (res != null) {
			resources.clear();
			resources.addAll(res);
			Collections.sort(resources, new Comparator<Resource>() {
				@Override
				public int compare(Resource o1, Resource o2) {
//					if (o1.isFavorite() && o2.isFavorite()) return 0;
//					return o1.isFavorite() ? -1 : 1;
					int diff1 = 0;
					if (o1.isFavorite()) diff1 -= 1000;
					if (o2.isFavorite()) diff1 += 1000;
					return o1.getName().compareToIgnoreCase(o2.getName())+diff1;
				}
			});
			populateResourcesMenu();
			
		}

	}

	protected boolean containsAny(Collection<String> needles, String[] haystacks) {
		for(String haystack : haystacks) {
			for(String needle: needles) {
				if (haystack.contains(needle)) {
					return true;
				}
			}
		}
		return false;
	}

	private Set<String> filterByKey(final Set<String> filters, String... filterKeys) {
		if (filters == null) {
			return null;
		}
		Set<String> opt = null;
		for(Iterator<String> it = filters.iterator();it.hasNext();) {
			String f = it.next();
			boolean found = false;
			for(String filterKey : filterKeys) {
				if (f.startsWith(filterKey)) {
					found = true;
					break;
				}
			}
			if (!found) continue;
			it.remove();
			String substring = f.substring(f.indexOf(':')+1);
			if (substring.isEmpty()) continue;
			String[] sorders = substring.split(",");
			for (String s : sorders) {
				try {
					if (opt == null) {
						opt = new HashSet<String>();
					}
					opt.add(s);
				} catch (NumberFormatException e) {
					e.printStackTrace();
				}
			}
		}
		return opt;
	}
	
	protected boolean accept(Set<String> filters, Task t) {
		Set<String> task = parseFilter(t.getKey() + " " + t.getSummary());
		if (t.getLabel() != null) {
			task.addAll(Arrays.asList(t.getLabel()));
		}
		
		search1: for (String filter : filters) {
			for (String word : task) {
				if (word.contains(filter)) {
					continue search1;
				}
			}
			return false;
		}
		return true;

	}

	private Set<String> parseFilter(String filter) {
		String[] sa = filter.split(" ");
		Set<String> ret = new LinkedHashSet<String>();
		for (int i = 0; i < sa.length; i++) {
			ret.add(sa[i].toLowerCase());
		}
		return ret;
	}

	private TreeItem insertTask(final Tree tree, Task t, int index) {
		TreeItem item = index == -1 ? new TreeItem(tree, SWT.NONE) : new TreeItem(tree, SWT.NONE, index);
		fillItem(t, item);
		taskToItem.put(t.getKey(), item);
		return item;
	}

	private void fillItem(Task t, TreeItem item) {
		if (item.isDisposed()) return;
		int idx = 0;
		for(Field f : fields) {
			item.setText(idx++, f.toString(t));
		}
//		item.setText(new String[] {
//				t.getKey() + (t.isDirty() ? "*" : ""),
//				formatter.format(t.getCreated()),
//				t.getSummary(), 
//				t.getOrder() + "", 
//				t.getEstimate() == 0 ? "" : "" + t.getEstimate(),
//				t.getIteration() == null ? "" : "" + t.getIteration().getId(), 
//				formatAssignee(t.getResources()),
//				formatLabel(list(t.getLabel())),
//				String.format("%.4f", t.getFineGrainedOrder()),
//		});
		item.setData(t);
		item.setForeground(getDisplay().getSystemColor(t.isDirty() ? SWT.COLOR_BLUE : SWT.COLOR_LIST_FOREGROUND));
	}

	private String formatAssignee(Collection<Resource> resources) {
		if (resources == null) return "";
		String ret = "";
		for (Resource r : resources) {
			if (!ret.isEmpty()) {
				ret += ", ";
			}
			ret += r.getName();
		}
		return ret;
	}

	public void repopulate() {
		tree.removeAll();
		
		fillTree(null, null);
		
		TreeColumn[] column = tree.getColumns();
		
		for (int i = 0, n = column.length; i < n; i++) {
			column[i].pack();
		}
		
		if (column[1].getWidth() > 500) {
			column[1].setWidth(500);
		}
		
		updateSelectedInfo();
		
	}

	public void taskUpdated(Task t) {
		TreeItem item = taskToItem.get(t.getKey());
		fillItem(t, item);
	}

	private void updateHistory(TreeItem item) {
		if (goingBack) return;
		if (lastSelectedItem != null) {
			accumulateHistory(lastSelectedItem);
		}
		lastSelectedItem = new Pair<TreeItem, TreeItem>(tree.getTopItem(), item);
	}

	private void clear() {
		controller.clear();
		tree.removeAll();
		historyClear();
	}

	private void reorderTasksBy() {
		if (tree.getItemCount() == 0) return;
		tree.setRedraw(false);
		List<TreeItem> items = new ArrayList<TreeItem>(Arrays.asList(tree.getItems()));
		final int sortMultiplier = tree.getSortDirection() == SWT.UP ? 1 : -1;
		List<Task> tasks = new ArrayList<Task>();
		for(TreeItem item : items) {
			tasks.add((Task) item.getData());
		}
		Collections.sort(tasks, new ComparatorMultiplier<Task>(sortField, sortMultiplier));
		tree.removeAll();
		for(Task task : tasks) {
			insertTask(tree, task, -1);
		}
		tree.setRedraw(true);
		historyClear();
	}

	private void setSortField(TreeColumn column) {
		sortField = (Field) column.getData();
		int direction = tree.getSortDirection();
		if (direction == SWT.None) direction = SWT.UP;
		if (column == tree.getSortColumn()) {
			direction = direction == SWT.UP ? SWT.DOWN : SWT.UP; 
		}
		tree.setSortColumn(column);
		tree.setSortDirection(direction);
		reorderTasksBy();
	}
}
