package com.db4o.objectManager.v2;

import com.db4o.objectManager.v2.custom.BorderedFormPanel;
import com.db4o.objectManager.v2.custom.BorderedPanel;
import com.db4o.objectmanager.api.DatabaseInspector;
import com.db4o.objectmanager.model.Db4oConnectionSpec;
import com.db4o.objectmanager.model.Db4oFileConnectionSpec;
import com.db4o.objectmanager.model.Db4oSocketConnectionSpec;
import com.db4o.reflect.ReflectClass;
import com.jgoodies.forms.factories.Borders;

import javax.swing.*;
import javax.swing.table.TableColumn;
import javax.swing.table.TableModel;
import java.awt.BorderLayout;
import java.awt.Component;
import java.awt.Point;
import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;

/**
 * User: treeder
 * Date: Aug 28, 2006
 * Time: 2:51:41 PM
 */
public class DatabaseSummaryPanel extends JPanel {
	private MainPanel mainPanel;
	private Db4oConnectionSpec connectionSpec;
	private DatabaseInspector databaseInspector;
	private ClassStatsTableModel classStatsTableModel;


	public DatabaseSummaryPanel(MainPanel mainPanel, Db4oConnectionSpec connectionSpec, DatabaseInspector databaseInspector) {
		super(new BorderLayout());
		this.mainPanel = mainPanel;
		this.connectionSpec = connectionSpec;
		this.databaseInspector = databaseInspector;
		setOpaque(false);
		setBorder(Borders.DIALOG_BORDER);
		add(buildMain());
	}

	private Component buildMain() {
		Box box = new Box(BoxLayout.PAGE_AXIS);

		box.add(buildConnectionInfo());
		box.add(buildMainDatabaseStats());
		box.add(buildClassStats());
		//box.add(buildReplicationStats());

		return box;
	}


	private Component buildConnectionInfo() {
		BorderedFormPanel builder = new BorderedFormPanel("Connected to db4o Database");

		if (connectionSpec instanceof Db4oFileConnectionSpec) {
			Db4oFileConnectionSpec spec = (Db4oFileConnectionSpec) connectionSpec;
			builder.append("File:", new JLabel(spec.getFullPath()));
		} else if (connectionSpec instanceof Db4oSocketConnectionSpec) {
			Db4oSocketConnectionSpec spec = (Db4oSocketConnectionSpec) connectionSpec;
			builder.append("Hostname: ", new JLabel(spec.getHost()));
			builder.append("Port: ", new JLabel(Integer.toString(spec.getPort())));
			builder.append("Username: ", new JLabel(spec.getUser()));
		}

		return builder.getPanel();
	}

	private Component buildMainDatabaseStats() {
		BorderedFormPanel builder = new BorderedFormPanel("Database Statistics");

		builder.append("Size:", new JLabel(databaseInspector.getSize() + " bytes"));
		//builder.append("Used Space: ", new JLabel(databaseInspector.getSpaceUsed() + " bytes"));
		//builder.append("Free Space: ", new JLabel(databaseInspector.getSpaceFree() + " bytes"));
		//builder.append("Lost Space: ", new JLabel(databaseInspector.getSpaceLost() + " bytes"));

		return builder.getPanel();
	}

	private Component buildClassStats() {
		BorderedPanel builder = new BorderedPanel("Stored Classes");

		TableModel classModel = createClassTableModel();
		final JTable table = new JTable(classModel);
		table.addMouseListener(new MouseAdapter() {
			public void mouseClicked(MouseEvent e) {
				if (e.getClickCount() == 2) {
					Point p = e.getPoint();
					int row = table.rowAtPoint(p);
					if(classStatsTableModel.getStoredClasses() != null && classStatsTableModel.getStoredClasses().size() > row){
						ReflectClass clazz = classStatsTableModel.getStoredClasses().get(row);
						// now show this class's info
						mainPanel.showClassSummary(clazz.getName());
					}
				}
			}
		});
		TableColumn col = table.getColumnModel().getColumn(0);
		int width = 200;
		col.setPreferredWidth(width);
		JScrollPane scrollPane = new JScrollPane(table);
		builder.add(scrollPane);

		return builder.getPanel();
	}

	private TableModel createClassTableModel() {
		classStatsTableModel = new ClassStatsTableModel(databaseInspector);
		return classStatsTableModel;
	}

	/*private Component buildReplicationStats() {
			BorderedFormPanel builder = new BorderedFormPanel("Replication Info");

			builder.append("To/From Database: ", new JLabel("hostname or filename if possible"));
			builder.append("Last Replication: ", new JLabel("Date"));
			builder.append("To/From Database: ", new JLabel("hostname or filename if possible"));
			builder.append("Last Replication: ", new JLabel("Date"));

			return builder.getPanel();
		}*/

}
