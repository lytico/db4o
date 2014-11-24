package com.db4o.objectManager.v2;

import com.db4o.ObjectContainer;
import com.db4o.reflect.ReflectClass;
import com.db4o.reflect.ReflectField;
import com.db4o.objectManager.v2.uif_lite.component.Factory;
import com.db4o.objectManager.v2.uif_lite.panel.SimpleInternalFrame;
import com.db4o.objectManager.v2.util.Log;
import com.db4o.objectManager.v2.util.DateFormatter;
import com.db4o.objectManager.v2.query.QueryResultsPanel;
import com.db4o.objectManager.v2.query.QueryBarPanel;
import com.db4o.objectManager.v2.shortcuts.ShortcutsListener;
import com.db4o.objectManager.v2.shortcuts.TabShortCutsListener;
import com.db4o.objectManager.v2.resources.ResourceManager;
import com.db4o.objectManager.v2.connections.ConnectionHelper;
import com.db4o.objectmanager.api.DatabaseInspector;
import com.db4o.objectmanager.api.impl.DatabaseInspectorImpl;
import com.db4o.objectmanager.api.prefs.Preferences;
import com.db4o.objectmanager.model.Db4oConnectionSpec;
import com.jgoodies.looks.Options;
import com.jgoodies.looks.plastic.PlasticLookAndFeel;
import com.jgoodies.looks.windows.WindowsLookAndFeel;
import com.jgoodies.forms.factories.Borders;
import com.spaceprogram.db4o.sql.util.ReflectHelper;

import javax.swing.*;
import javax.swing.tree.TreeModel;
import javax.swing.tree.DefaultMutableTreeNode;
import javax.swing.tree.DefaultTreeModel;
import javax.swing.tree.DefaultTreeCellRenderer;
import javax.swing.border.EmptyBorder;
import java.awt.BorderLayout;
import java.awt.Component;
import java.awt.Dimension;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.KeyEvent;
import java.awt.event.MouseListener;
import java.util.List;

/**
 * <p/>
 * Most of the main items related to this connection are in here, this is mainly so that this panel could be embedded in
 * a different top level container (ie: doesn't have to be in MainFrame).
 * </p>
 * User: treeder
 * Date: Aug 18, 2006
 * Time: 1:12:30 PM
 */
public class MainPanel extends JPanel implements UISession {

	public static DateFormatter dateFormatter = new DateFormatter();

	private MainFrame mainFrame;
	private Settings settings;
	private ObjectContainer objectContainer;
	private Db4oConnectionSpec connectionSpec;
	private DatabaseInspector databaseInspector;
	//2private QueryResultsPanel queryResultsPanel;
    private QueryBarPanel queryBarPanel;
	private JTabbedPane tabbedPane;

	private int queryCounter;
	//private TreeModel classTreeModel;
    private JTree classTree;
	private DatabaseSummaryPanel databaseSummaryPanel;
	private static final int MAX_TABS = 8;


	public MainPanel(MainFrame mainFrame, Settings settings, Db4oConnectionSpec connectionSpec, ObjectContainer oc) {
        super(new BorderLayout());
        this.mainFrame = mainFrame;
        this.settings = settings;
        this.connectionSpec = connectionSpec;
		this.objectContainer = oc;
		build();
        initClassTree();
        mainFrame.addKeyListener(new ShortcutsListener());

    }

    public void build() {
        add(buildMainPanel());
    }

    private Component buildMainPanel() {
        JPanel panel = new JPanel(new BorderLayout());
        //panel.add(buildToolBar(), BorderLayout.NORTH);
        panel.add(buildQueryPanel(), BorderLayout.CENTER);
        return panel;
    }

    private Component buildQueryPanel() {
        JPanel panel = new JPanel(new BorderLayout());
        panel.add(buildQueryBar(), BorderLayout.NORTH);
        JSplitPane splitPane = Factory.createStrippedSplitPane(
                JSplitPane.HORIZONTAL_SPLIT,
                buildMainLeftPanel(),
                buildTabbedPane(),
                0.2f);

        panel.add(splitPane, BorderLayout.CENTER);
        return panel;
    }


    private Component buildQueryBar() {
        queryBarPanel = new QueryBarPanel(this);
        return queryBarPanel;
    }


    private JComponent buildMainLeftPanel() {
     /*   JTabbedPane tabbedPane = new JTabbedPane(SwingConstants.BOTTOM);
        tabbedPane.putClientProperty(Options.EMBEDDED_TABS_KEY, Boolean.TRUE);
        tabbedPane.addTab("Tree", buildTree());
        tabbedPane.addTab("Help", Factory.createStrippedScrollPane(buildHelp()));
*/
        SimpleInternalFrame sif = new SimpleInternalFrame("Stored Classes");
        sif.setPreferredSize(new Dimension(200, 300));
        sif.setBorder(Borders.DIALOG_BORDER);
        sif.add(buildTree());
        return sif;
    }


    private JScrollPane buildTree() {
        classTree = new JTree(createClassTreeModel());
		classTree.putClientProperty(Options.TREE_LINE_STYLE_KEY,
                Options.TREE_LINE_STYLE_NONE_VALUE);
        classTree.setToggleClickCount(2);
		DefaultTreeCellRenderer renderer = new DefaultTreeCellRenderer();
		renderer.setLeafIcon(ResourceManager.createImageIcon(ResourceManager.ICONS_16X16 + "element.png"));
		Icon icon = ResourceManager.createImageIcon(ResourceManager.ICONS_16X16 + "coffeebean.png");
		renderer.setOpenIcon(icon);
		renderer.setClosedIcon(icon);
		classTree.setCellRenderer(renderer);
		return new JScrollPane(classTree);

    }

    private TreeModel createClassTreeModel() {
        DefaultMutableTreeNode root = new DefaultMutableTreeNode("Stored Classes");
        TreeModel classTreeModel = new DefaultTreeModel(root);
        return classTreeModel;
    }

    public void initClassTree() {

		DefaultMutableTreeNode root = (DefaultMutableTreeNode) classTree.getModel().getRoot();
		DefaultMutableTreeNode parent;

        DatabaseInspector inspector = getDatabaseInspector();
        List<ReflectClass> classesStored = inspector.getClassesStored();
		for (int i = 0; i < classesStored.size(); i++) {
            ReflectClass storedClass = classesStored.get(i);
            parent = new DefaultMutableTreeNode(storedClass.getName());
            root.add(parent);
            ReflectField[] fields = ReflectHelper.getDeclaredFieldsInHeirarchy(storedClass);
            for (int j = 0; j < fields.length; j++) {
                ReflectField field = fields[j];
                //TODO: do not show transient fields
                //if(!field.isTransient())
                parent.add(new DefaultMutableTreeNode(field.getName()));
            }
        }
        addClassTreeListener(new ClassTreeListener(queryBarPanel));
		classTree.setRootVisible(true); // have to do this for some reason, when reopen is called and this is repopulated, it won't show anything unless this is called first
		classTree.expandRow(0);
		classTree.setRootVisible(false);
		classTree.setShowsRootHandles(true);
	}


    public void addClassTreeListener(MouseListener classTreeListener) {
        classTree.addMouseListener(classTreeListener);
    }


    private Preferences getPreferences() {
        return Preferences.getDefault();
    }

    public void setPreference(String key, Object pref) {
        getPreferences().setPreference(key, pref);
    }

    public Object getPreferenceForDatabase(String key) {
        return getPreferences().getPreference(connectionSpec.getFullPath() + "/" + key);
    }
    public void setPreferenceForDatabase(String key, Object pref) {
        getPreferences().setPreference(connectionSpec.getFullPath() + "/" + key, pref);
    }


    public Object getPreference(String key) {
        return getPreferences().getPreference(key);
    }

    public ObjectContainer getObjectContainer() {
        return objectContainer;
    }

    public void closeObjectContainer() {
        if (objectContainer != null) {
            objectContainer.close();
            objectContainer = null;
        }
    }

    public Db4oConnectionSpec getConnectionSpec() {
        return connectionSpec;
    }


    private Component buildTabbedPane() {

        tabbedPane = new JTabbedPane(SwingConstants.TOP);
        //tabbedPane.setTabLayoutPolicy(JTabbedPane.SCROLL_TAB_LAYOUT);
        tabbedPane.setBorder(new EmptyBorder(10, 10, 10, 10));
        tabbedPane.addKeyListener(new TabShortCutsListener(tabbedPane));
        tabbedPane.addMouseListener(new CloseTabListener(tabbedPane));


        addTabs(tabbedPane);

        return tabbedPane;
    }

    /**
     * This makes the initial tabset
     * @param tabbedPane
     */
    private void addTabs(JTabbedPane tabbedPane) {
        databaseSummaryPanel = new DatabaseSummaryPanel(this, connectionSpec, getDatabaseInspector());
        tabbedPane.addTab("Home", createTabIcon(TabType.home.icon()), databaseSummaryPanel);
    }

	private Icon createTabIcon(String iconImg) {
		return ResourceManager.createImageIcon(ResourceManager.ICONS_16X16 + iconImg);
	}

	public void addTab(TabType tabType, String name, Component p) {
        tabbedPane.addTab(name, createTabIcon(tabType.icon()), p);
        tabbedPane.setSelectedComponent(p);
        // todo: remove tabs based on usage time rather than FIFO
        if (tabbedPane.getTabCount() > MAX_TABS) {
            tabbedPane.remove(1); // 1 so that Home tab always stays
        }
    }

	public boolean isLocal() {
		return !connectionSpec.isRemote();
	}


	private Component buildToolBar() {
        JToolBar toolBar = new JToolBar();
        toolBar.setFloatable(true);
        toolBar.putClientProperty("JToolBar.isRollover", Boolean.TRUE);
        // Swing
        toolBar.putClientProperty(
                Options.HEADER_STYLE_KEY,
                settings.getToolBarHeaderStyle());
        toolBar.putClientProperty(
                PlasticLookAndFeel.BORDER_STYLE_KEY,
                settings.getToolBarPlasticBorderStyle());
        toolBar.putClientProperty(
                WindowsLookAndFeel.BORDER_STYLE_KEY,
                settings.getToolBarWindowsBorderStyle());
        toolBar.putClientProperty(
                PlasticLookAndFeel.IS_3D_KEY,
                settings.getToolBar3DHint());

        AbstractButton button;

        toolBar.add(createToolBarButton("backward.gif", "Back"));
        button = createToolBarButton("forward.gif", "Next");
        button.setEnabled(false);
        toolBar.add(button);
        toolBar.add(createToolBarButton("home.gif", "Home"));
        toolBar.addSeparator();

        ActionListener openAction = new OpenFileActionListener();
        button = createToolBarButton("open.gif", "Open", openAction, KeyStroke.getKeyStroke(KeyEvent.VK_O, KeyEvent.CTRL_DOWN_MASK));
        button.addActionListener(openAction);
        toolBar.add(button);
        toolBar.add(createToolBarButton("print.gif", "Print"));
        toolBar.add(createToolBarButton("refresh.gif", "Update"));
        toolBar.addSeparator();

        button = createToolBarButton("help.gif", "Open Help");
        button.addActionListener(createHelpActionListener());
        toolBar.add(button);

        return toolBar;
    }

    protected AbstractButton createToolBarButton(String iconName, String toolTipText) {
        JButton button = new JButton(ResourceManager.createImageIcon(iconName));
        button.setToolTipText(toolTipText);
        button.setFocusable(false);
        return button;
    }

    private AbstractButton createToolBarButton(String iconName, String toolTipText, ActionListener action, KeyStroke keyStroke) {
        AbstractButton button = createToolBarButton(iconName, toolTipText);
        button.registerKeyboardAction(action, keyStroke, JComponent.WHEN_IN_FOCUSED_WINDOW);
        return button;
    }

    protected AbstractButton createToolBarRadioButton(String iconName, String toolTipText) {
        JToggleButton button = new JToggleButton(ResourceManager.createImageIcon(iconName));
        button.setToolTipText(toolTipText);
        button.setFocusable(false);
        return button;
    }

    public void setConnectionSpec(Db4oConnectionSpec connectionSpec) {
        this.connectionSpec = connectionSpec;
        initClassTree();
    }

    public DatabaseInspector getDatabaseInspector() {
        if (databaseInspector == null) {
            databaseInspector = new DatabaseInspectorImpl(getObjectContainer());
        }
        return databaseInspector;
    }

    public void displayResults(String query) {
        QueryResultsPanel p = new QueryResultsPanel(this);
        addTab(TabType.query, "Query " + (++queryCounter), p);
        p.displayResults(query);
    }



    public void showClassSummary(String className) {
        // make sure class summary isn't already there
        for (int i = 0; i < tabbedPane.getTabCount(); i++) {
            Component c = tabbedPane.getComponentAt(i);
            if (c instanceof ClassSummaryPanel) {
                ClassSummaryPanel csp = (ClassSummaryPanel) c;
                if (csp.getClassName().equals(className)) {
                    tabbedPane.setSelectedIndex(i);
                    return;
                }
            }
        }
        ClassSummaryPanel panel = new ClassSummaryPanel(this, getDatabaseInspector(), className);
        addTab(TabType.classSummary, "Class: " + className, panel);
    }

	public void reopen() {
		for (int i = 0; i < tabbedPane.getTabCount(); i++) {
            Component c = tabbedPane.getComponentAt(i);
            if (c instanceof QueryResultsPanel) {
				tabbedPane.remove(c);
				i--;
			}
		}
		objectContainer.close();
		databaseInspector = null;
		try {
			objectContainer = ConnectionHelper.connect(mainFrame, connectionSpec);
		} catch (Exception e) {
			Log.addException(e);
			e.printStackTrace();
			// close the window, something went bad
			mainFrame.close();
			return;
		}
		// reset class tree
		TreeModel treeModel = createClassTreeModel();
		classTree.setModel(treeModel);
		initClassTree();

	}


	private final class OpenFileActionListener implements ActionListener {
        public void actionPerformed(ActionEvent e) {
            new JFileChooser().showOpenDialog(MainPanel.this);
        }
    }

    protected ActionListener createHelpActionListener() {
        return null;
    }
}
