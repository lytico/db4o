package com.db4o.devtools.ant;


import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.Node;

public class CSharp2003Project extends CSharpProject {
	
	public CSharp2003Project(Document document) throws Exception {
		super(document);
	}

	protected Element resetFilesContainerElement() throws Exception {
		Element files = selectElement(getXPathExpression());
		if (null == files) {
			invalidProjectFile();
		}
		if (files.hasChildNodes()) {
			Node old = files;
			files = createElement("Include");
			old.getParentNode().replaceChild(files, old);
		}
		return files;
	}

	protected Node createFileNode(String file) {
		Element node = createElement("File");
		node.setAttribute("RelPath", file);
		node.setAttribute("SubType", "Code");
		node.setAttribute("BuildAction", "Compile");
		return node;
	}
	
	protected String getXPathExpression() {
		return "VisualStudioProject/CSHARP/Files/Include";
	}	
}
