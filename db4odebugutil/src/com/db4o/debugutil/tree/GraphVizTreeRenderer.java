/* Copyright (C) 2007  db4objects Inc.  http://www.db4o.com */

package com.db4o.debugutil.tree;

import java.io.*;
import java.util.*;

import com.db4o.foundation.*;
import com.db4o.internal.*;

/**
 * Simple graphviz renderer for com.db4o.foundation.Tree instances.
 * 
 * To use during debugging, ensure that this project is registered
 * as a dependency in the executing project's properties and execute
 * 
 * com.db4o.debugutil.tree.GraphVizTreeRenderer.renderToFile(treeInst, fileName)
 * 
 * from the Eclipse Display view, where "treeInst" is a reference
 * to a Tree instance known in the currently selected stack frame
 * and "fileName" is the path to a file the .dot representation should
 * be written to.
 * (Note: Display view autocompletion for this class may not work,
 * that's ok.)
 * 
 * The resulting file can be rendered with graphviz, for example
 * 
 * dot -otree.ps -Tps tree.dot
 * 
 * on the command line should generate a postscript rendering, given
 * "fileName" was "tree.dot" in the above example. (postscript is just
 * an example - graphviz supports a variety of image formats, please
 * check the graphviz docs for more details.)
 * 
 * Of course you need to have graphviz installed. :)
 */

public class GraphVizTreeRenderer {

	private final static String SEP = System.getProperty("line.separator", "\n");

	public static void renderToFile(Tree tree, String fileName) throws IOException {
		String rendered = render(tree);
		OutputStream fileStream = new FileOutputStream(fileName);
		Writer writer = new BufferedWriter(new OutputStreamWriter(fileStream));
		try {
			writer.write(rendered);
		}
		finally {
			writer.close();
		}
	}

	public static String render(final Tree tree) {
		final StringBuffer rendered = new StringBuffer();
		rendered.append("digraph G {")
			.append(SEP);
		Set seen = new HashSet();
		processNode(tree, rendered, seen);
		rendered.append("}")
			.append(SEP);
		return rendered.toString();
	}
	
	private static void processNode(final Tree tree, final StringBuffer rendered, Set seen) {
		if(tree == null) {
			return;
		}
		if(seen.contains(tree)) {
			System.err.println("CYCLE: " + tree);
			return;
		}
		seen.add(tree);
		renderNode(tree, rendered);
		processNode(tree._preceding, rendered, seen);
		processNode(tree._subsequent, rendered, seen);
		renderArc(tree, tree._preceding, "L", rendered);
		renderArc(tree, tree._subsequent, "R", rendered);
	}
	
	private static void renderNode(final Tree tree, final StringBuffer rendered) {
		rendered.append('\t')
			.append(id(tree))
			.append(" [label=\"")
			.append(label(tree))
			.append("\"];")
			.append(SEP);
	}

	private static void renderArc(final Tree from, final Tree to, String label, final StringBuffer rendered) {
		if(to == null) {
			return;
		}
		rendered.append('\t')
			.append(id(from))
			.append(" -> ")
			.append(id(to))
			.append(" [label=\"")
			.append(label)
			.append("\"]")
			.append(SEP);
	}

	private static String id(Tree tree) {
		return "node" + System.identityHashCode(tree);
	}

	private static String label(Tree tree) {
		return tree.toString();
	}
	
	public static void main(String[] args) throws IOException {
		Random rnd = new Random();
		TreeInt tree = null;
		for(int i = 0; i < 10; i++) {
			int value = rnd.nextInt(100);
			tree = TreeInt.add(tree, value);
			System.out.println("Added: " + value);
		}
		renderToFile(tree, "test.dot");
	}
}
