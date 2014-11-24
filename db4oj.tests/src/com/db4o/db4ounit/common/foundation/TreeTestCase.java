/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.foundation;

import java.util.*;

import com.db4o.foundation.*;
import com.db4o.internal.*;

import db4ounit.*;

@decaf.Remove(decaf.Platform.JDK11)
public class TreeTestCase implements TestCase{
	
	private static final int COUNT = 21;
	
	
	public void testTraversalWithStartingPointEmpty(){
		Tree.traverse(null, new TreeInt(5), new CancellableVisitor4<TreeInt>() {
			public boolean visit(TreeInt node) {
				return true;
			}
		});
	}
	
	public void testCancelledTraversalWithStartingPointNotInTheTree(){
		final IntByRef visits = new IntByRef();
		TreeInt tree = createTree();
		Tree.traverse(tree, new TreeInt(5), new CancellableVisitor4<TreeInt>() {
			public boolean visit(TreeInt node) {
				visits.value++;
				Assert.areEqual(new TreeInt(6), node);
				return false; 
			}
		});
		Assert.areEqual(1, visits.value);
	}
	
	public void testCancelledTraversalWithStartingPointInTheTree(){
		final IntByRef visits = new IntByRef();
		TreeInt tree = createTree();
		Tree.traverse(tree, new TreeInt(6), new CancellableVisitor4<TreeInt>() {
			public boolean visit(TreeInt node) {
				visits.value++;
				Assert.areEqual(new TreeInt(6), node);
				return false; 
			}
		});
		Assert.areEqual(1, visits.value);
	}
	
	public void testUnCancelledTraversalWithStartingPointNotInTheTree(){
		final List<TreeInt> actual = new ArrayList<TreeInt>();
		TreeInt tree = createTree();
		Tree.traverse(tree, new TreeInt(5), new CancellableVisitor4<TreeInt>() {
			public boolean visit(TreeInt node) {
				actual.add(node);
				return true; 
			}
		});
		IteratorAssert.areEqual(createList(6).iterator(), actual.iterator());
	}
	
	public void testUnCancelledTraversalWithStartingPointInTheTree(){
		final List<TreeInt> actual = new ArrayList<TreeInt>();
		TreeInt tree = createTree();
		Tree.traverse(tree, new TreeInt(6), new CancellableVisitor4<TreeInt>() {
			public boolean visit(TreeInt node) {
				actual.add(node);
				return true; 
			}
		});
		IteratorAssert.areEqual(createList(6).iterator(), actual.iterator());
	}


	private List<TreeInt> createList(int start) {
		final List<TreeInt> expected = new ArrayList<TreeInt>();
		TreeInt expectedTree = createTree(start);
		Tree.traverse(expectedTree, new Visitor4<TreeInt>() {
			public void visit(TreeInt node) {
				expected.add(node);
			}
		});
		return expected;
	}


	private TreeInt createTree() {
		return createTree(0);
	}

	private TreeInt createTree(int start) {
		TreeInt tree = null;
		for (int i = start; i < COUNT; i+=3) {
			tree = Tree.add(tree, new TreeInt(i));
		}
		return tree;
	}
	
	

}
