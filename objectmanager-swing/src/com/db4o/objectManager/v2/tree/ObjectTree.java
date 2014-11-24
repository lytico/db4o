package com.db4o.objectManager.v2.tree;

import javax.swing.*;
import javax.swing.tree.TreePath;

/**
 * User: treeder
 * Date: Sep 29, 2006
 * Time: 2:39:38 PM
 */
public class ObjectTree extends JTree {
    public ObjectTree(ObjectTreeModel objectTreeModel) {
        super(objectTreeModel);
    }

    public boolean isPathEditable(TreePath path) {
        ObjectTreeModel model = (ObjectTreeModel) getModel();
        boolean ret = model.isPathEditable(path);
        return ret;
    }
}
