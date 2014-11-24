package com.db4o.objectManager.v2;

import com.db4o.objectManager.v2.custom.BorderedFormPanel;
import com.db4o.objectManager.v2.custom.BorderedPanel;
import com.db4o.objectmanager.api.DatabaseInspector;
import com.jgoodies.forms.factories.Borders;

import javax.swing.*;
import javax.swing.table.TableColumn;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

/**
 * User: treeder
 * Date: Sep 3, 2006
 * Time: 4:37:35 PM
 */
public class ClassSummaryPanel extends JPanel {
	private DatabaseInspector databaseInspector;
	private UISession session;
	private String className;
	private FieldInfoTableModel classModel;
	private static final String EDIT = "edit";
	private static final String STOP_EDIT = "stop";
	private Button editButton;

	public ClassSummaryPanel(UISession session, DatabaseInspector databaseInspector, String className) {
		super(new BorderLayout());
		this.session = session;
		this.className = className;
		setOpaque(false);
		setBorder(Borders.DIALOG_BORDER);
		this.databaseInspector = databaseInspector;
		add(buildMain());
	}

	private Component buildMain() {
		Box box = new Box(BoxLayout.PAGE_AXIS);

		box.add(buildClassStats());
		box.add(buildFieldInfo());

		return box;
	}

	private Component buildClassStats() {
		BorderedFormPanel builder = new BorderedFormPanel("Class Statistics");

		builder.append("Number of Objects:", new JLabel(databaseInspector.getNumberOfObjectsForClass(className) + ""));

		return builder.getPanel();
	}

	private Component buildFieldInfo() {
		BorderedPanel builder = new BorderedPanel("Fields");

		JPanel verticalPanel = new JPanel();
		verticalPanel.setLayout(new BoxLayout(verticalPanel, BoxLayout.PAGE_AXIS));

		classModel = createFieldModel();
		JTable table = new JTable(classModel);
		TableColumn col = table.getColumnModel().getColumn(0);
		int width = 200;
		col.setPreferredWidth(width);
		JScrollPane scrollPane = new JScrollPane(table);
		verticalPanel.add(scrollPane);

		if (session.isLocal()) {
			editButton = new Button("Edit");
			editButton.setActionCommand(EDIT);
			editButton.addActionListener(new ActionListener() {
				public void actionPerformed(ActionEvent e) {
					if (e.getActionCommand().equals(EDIT)) {
						editButton.setActionCommand(STOP_EDIT);
						editButton.setForeground(new Color(0x009933)); // dark green
						editButton.setFont(editButton.getFont().deriveFont(Font.BOLD));
						editButton.setLabel("Stop Editing");
						classModel.setEditMode(true);

					} else {
						classModel.setEditMode(false);
						editButton.setForeground(Color.BLACK);
						editButton.setFont(editButton.getFont().deriveFont(Font.PLAIN));
						editButton.setLabel("Edit");
						editButton.setActionCommand(EDIT);
					}
				}
			});
			verticalPanel.add(editButton);
		}

		builder.add(verticalPanel);

		return builder.getPanel();
	}

	private FieldInfoTableModel createFieldModel() {
		FieldInfoTableModel tableModel = new FieldInfoTableModel(session, className);
		return tableModel;
	}

	public String getClassName() {
		return className;
	}
}
