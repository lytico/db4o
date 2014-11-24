package com.db4o.objectManager.v2.tree;

import java.awt.*;
import java.util.*;

import javax.swing.*;

import com.db4o.objectManager.v2.*;

/**
 * User: treeder
 * Date: Sep 29, 2006
 * Time: 3:11:48 PM
 */
public class ObjectTreeCellEditor extends DefaultCellEditor {

	public ObjectTreeCellEditor(final JTextField textField) {
		// from superclass
		super(textField);
		delegate = new EditorDelegate() {
			public void setValue(Object value) {
//               System.out.println("value: " + value + " class=" + value.getClass() + " value=" + value);
				if(value instanceof ObjectTreeNode) {
					ObjectTreeNode node = (ObjectTreeNode) value;
					setValueOnTextField(node.getObject(), textField);
				} else {
					setValueOnTextField(value, textField);
				}
			}

			public Object getCellEditorValue() {
//				System.out.println("getCellEditorValue: " + textField.getText());
				return textField.getText();
			}
		};
		textField.addActionListener(delegate);
	}

	private void setValueOnTextField(Object value, JTextField textField) {
		String text = "";
		if(value != null) {
			if(value instanceof Date) {
				text = MainPanel.dateFormatter.edit((Date) value);
			} else {
				text = value.toString();
			}
		}
		textField.setText(text);
	}

	public ObjectTreeCellEditor(JCheckBox checkBox) {
		super(checkBox);
	}

	public ObjectTreeCellEditor(JComboBox comboBox) {
		super(comboBox);
	}


	public Component getTreeCellEditorComponent(JTree tree, Object value, boolean isSelected, boolean expanded, boolean leaf, int row) {
//        System.out.println("value: " + value + " " + value.getClass());
		delegate.setValue(value);
		return editorComponent;
	}
}
