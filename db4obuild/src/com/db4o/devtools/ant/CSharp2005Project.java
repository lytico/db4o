package com.db4o.devtools.ant;

import javax.xml.xpath.XPathExpressionException;

import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;
import org.w3c.dom.Text;

public class CSharp2005Project extends CSharpProject {

	protected CSharp2005Project(Document document) throws Exception {
		super(document);
	}
	
	@Override
	public String getHintPathFor(String assemblyName) throws Exception {
		Text found = getHintPathNodeFor(assemblyName);
		return null != found ? found.getTextContent() : null;
	}
	
	@Override
	public void setHintPathFor(String assemblyName, String hintPath) throws Exception {
		Text found = getHintPathNodeFor(assemblyName);
		if (null == found) throw new IllegalArgumentException("Project has no reference to '" + assemblyName + "'.");
		found.setTextContent(hintPath);
	}

	private Text getHintPathNodeFor(String assemblyName) throws XPathExpressionException {
		NodeList nodes = selectNodes("//*[local-name()='HintPath']/text()");
		for (int i = 0; i<nodes.getLength(); ++i) {
			Text node = (Text)nodes.item(i);
			if (node.getTextContent().endsWith(assemblyName)) return node;
		}
		return null;
	}

	@Override
	protected Node createFileNode(String file) {
		return createNode("Compile", file);
	}

	@Override
	protected Node createResourceNode(String resource) {
		return createNode("EmbeddedResource", resource);
	}

	private Node createNode(final String nodeName, String path) {
	    Element node = createElement(nodeName);
		node.setAttribute("Include", path);
		return node;
    }
	
	@Override
	protected Element resetFilesContainerElement() throws Exception {
		Element compile = selectElement("//*[local-name()='ItemGroup']/*[local-name()='Compile']");
		if (null == compile) invalidProjectFile();
		
		Element container = (Element)compile.getParentNode();
		if (container.hasChildNodes()) {
			Element old = container;
			container = createElement("ItemGroup");
			old.getParentNode().replaceChild(container, old);
		}
		return container;
	}

}