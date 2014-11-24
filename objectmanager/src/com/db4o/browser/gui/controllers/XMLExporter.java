package com.db4o.browser.gui.controllers;

import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.PrintStream;
import java.util.HashMap;

import com.db4o.objectmanager.model.GraphPosition;
import com.db4o.objectmanager.model.IGraphIterator;
import com.db4o.objectmanager.model.nodes.IModelNode;

public class XMLExporter {

	private String fileName;
	private IGraphIterator input;
	private PrintStream out;

	public XMLExporter(String file, IGraphIterator input) {
		this.fileName = file;
		this.input = input;
	}

	public void export() throws FileNotFoundException {
		out = null;
		GraphPosition oldState = input.getPath();

		try {
			out = new PrintStream(new FileOutputStream(fileName));
			out.println("<root>");
			input.reset();
			export(input);
			out.println("</root>");
		} finally {
			if (out != null)
				out.close();
			input.setPath(oldState);
		}
	}
	
	private final int TABSIZE=3;
	private int indent = 0;
	private HashMap visitedNodes = new HashMap();
	
	public void indent() {
		StringBuffer b = new StringBuffer();
		for (int spaces = indent*TABSIZE; spaces > 0; --spaces) {
			b.append(" ");
		}
		out.print(b.toString());
	}
	
	public PrintStream getOut() {
		return out;
	}
		
	private void export(IGraphIterator input) {
		while (input.hasNext()) {
			boolean hasChildren = false;
			input.selectNextChild();
			if (input.numChildren() > 0) {
				hasChildren = true;
			}
			input.selectParent();

			// If this node is an object in its own right
			IModelNode node = (IModelNode) input.next();
			long id = node.getId();
			if(id==0) {
				continue;
			}
			if (id > 0) {

				// If we have seen this object before, print a reference
				Long oldID = (Long) visitedNodes.get(new Long(id));
				if (oldID != null) {
					// If this a top-level node and we've seen it before, don't bother repeating it
					if (indent == 0) {
						continue;
					}
					
					// Print a reference to the node we saw before
					indent();
					node.printXmlReferenceNode(out);
					continue;
				} else {
					// We haven't seen it yet; remember it
					visitedNodes.put(new Long(id), new Long(id));
				}
			}
			
			// If this node has children, print a begin tag, the children, then 
			// an end tag.  Otherwise, just print it as a single node.
			if (hasChildren) {
				indent();

				node.printXmlStart(out);

				input.selectPreviousChild();
				if (node.shouldIndent()) {
					++indent;
					export(input);
					--indent;
				} else {
					export(input);
				}
				input.selectParent();
				input.next();
				indent();

				node.printXmlEnd(out);
				
			} else {
				indent();
				node.printXmlValueNode(out);
				out.println();
			}
		}
	}

}


