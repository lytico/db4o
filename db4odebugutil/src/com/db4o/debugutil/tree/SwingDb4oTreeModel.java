/* Copyright (C) 2007  db4objects Inc.  http://www.db4o.com */

package com.db4o.debugutil.tree;

import java.io.*;
import java.util.*;

import javax.swing.*;
import javax.swing.event.*;
import javax.swing.tree.*;

import com.db4o.foundation.*;
import com.db4o.internal.*;

/**
 * Simple Swing TreeModel for com.db4o.foundation.Tree instances.
 * 
 * To use during debugging, ensure that this project is registered
 * as a dependency in the executing project's properties and execute
 * 
 * com.db4o.debugutil.tree.SwingDb4oTreeModel.show(treeInst)
 * 
 * from the Eclipse Display view, where "treeInst" is a reference
 * to a Tree instance known in the currently selected stack frame.
 * (Note: Display view autocompletion for this class may not work,
 * that's ok.)
 */
public class SwingDb4oTreeModel implements TreeModel {

	private final Tree _tree;
	private final Vector _listeners;	
	
	private final static Object LEFT_NIL = new Object() {
		public String toString() {
			return "NIL";
		}
	};

	private final static Object RIGHT_NIL = new Object() {
		public String toString() {
			return "NIL";
		}
	};

	public SwingDb4oTreeModel(Tree tree) {
		_tree = tree;
		_listeners = new Vector();
	}

	public void addTreeModelListener(TreeModelListener listener) {
		_listeners.addElement(listener);
	}

	public Object getChild(Object parent, int index) {
		Tree tree = (Tree)parent;
		switch(index) {
			case 0:
				return ((tree._preceding == null) ? LEFT_NIL : tree._preceding);
			case 1:
				return ((tree._subsequent == null) ? RIGHT_NIL : tree._subsequent);
			default:
				throw new NoSuchElementException();
		}
	}

	public int getChildCount(Object parent) {
		return (nil(parent) ? 0 : 2);
	}

	public int getIndexOfChild(Object parent, Object child) {
		Tree tree = (Tree)parent;
		if(child == tree._preceding) {
			return 0;
		}
		if(child == tree._subsequent) {
			return 1;
		}
		return -1;
	}

	public Object getRoot() {
		return _tree;
	}

	public boolean isLeaf(Object node) {
		return nil(node);
	}

	public void removeTreeModelListener(TreeModelListener listener) {
		_listeners.removeElement(listener);
	}

	public void valueForPathChanged(TreePath path, Object newValue) {
		// TODO
		System.out.println("PATH CHANGED" + path + " / " + newValue);
	}

	public boolean nil(Object obj) {
		return (obj == LEFT_NIL) || (obj == RIGHT_NIL);
	}
	
	public static void show(Tree tree) {
		TreeModel model = new SwingDb4oTreeModel(tree);
		JTree treeView = new JTree(model);
		JScrollPane treePane = new JScrollPane(treeView);
		JFrame frame = new JFrame("Tree " + System.identityHashCode(tree));
		frame.getContentPane().add(treePane);
		frame.pack();
		frame.setVisible(true);
	}
	
	public static void main(String[] args) throws IOException {
		Random rnd = new Random();
		TreeInt tree = null;
		for(int i = 0; i < 10; i++) {
			int value = rnd.nextInt(100);
			tree = TreeInt.add(tree, value);
			System.out.println("Added: " + value);
		}
		show(tree);
	}
}
