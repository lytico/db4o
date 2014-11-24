package com.db4o.objectManager.v2;

import com.db4o.objectManager.v2.query.QueryBuilder;
import com.db4o.objectManager.v2.query.QueryBarPanel;

import javax.swing.*;
import javax.swing.tree.DefaultMutableTreeNode;
import javax.swing.tree.TreePath;
import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;

/**
 * User: treeder
 * Date: Aug 20, 2006
 * Time: 2:49:02 PM
 */
public class ClassTreeListener extends MouseAdapter {
    private JTextArea queryText;
    private QueryBarPanel queryBarPanel;

    public ClassTreeListener(QueryBarPanel queryBarPanel) {
        this.queryBarPanel = queryBarPanel;
        this.queryText = queryBarPanel.getQueryText();
    }

    public void mousePressed(MouseEvent e) {
        JTree tree = (JTree) e.getSource();
        int selRow = tree.getRowForLocation(e.getX(), e.getY());
        TreePath selPath = tree.getPathForLocation(e.getX(), e.getY());
        if (selRow != -1) {
            DefaultMutableTreeNode node = (DefaultMutableTreeNode) selPath.getLastPathComponent();
            if (node == null) return;
            if (e.getClickCount() == 1) {

            } else if (e.getClickCount() == 2) {
                String nodeInfo = (String) node.getUserObject();
                if (!node.isLeaf() && !node.isRoot()) {
                    queryText.setText(QueryBuilder.addClass(queryText.getText(), nodeInfo));
                    queryBarPanel.showClassSummary(nodeInfo);
                } else {
                    DefaultMutableTreeNode parent = (DefaultMutableTreeNode) node.getParent();
                    String nodeObject = (String) parent.getUserObject();
                    queryText.setText(QueryBuilder.addField(queryText.getText(), nodeObject, nodeInfo));
                }
            }
        }
    }
}
