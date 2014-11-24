package com.db4o.devtools.ant;

import java.io.*;

import javax.xml.parsers.*;
import javax.xml.transform.*;
import javax.xml.transform.dom.*;
import javax.xml.transform.stream.*;
import javax.xml.xpath.*;

import org.w3c.dom.*;
import org.xml.sax.*;

public abstract class CSharpProject {
	
	/**
	 * Loads the current project definition based on the file extension and file format.
	 * 
	 * @param projectFile the project file to load
	 * @return
	 * @throws Exception
	 */
	public static CSharpProject load(File projectFile) throws Exception {
		Document document = loadXML(projectFile);
		
		String fname = projectFile.getName();
		if (fname.endsWith(".mdp")) {
			return new MonoDevelopProject(document);
		}
		
		if (document.getDocumentElement().getNodeName().equals("Project")) {
			// VS 2005
			return new CSharp2005Project(document);
		}
		
		if (fname.endsWith(".csdproj")) {
			return new CSharp2003ProjectCF(document);
		}
		return new CSharp2003Project(document);
	}

	private static Document loadXML(File projectFile) throws SAXException,
			IOException, ParserConfigurationException {
		return DocumentBuilderFactory.newInstance().newDocumentBuilder().parse(projectFile);
	}
	
	private Document _document;
	private Element _files;
	
	protected CSharpProject(Document document) throws Exception {
		_document = document;
	}

	public void addFile(String file) throws Exception {
		String relativePath = prepareFileNameForNode(file);
		filesElement().appendChild(createFileNode(relativePath));
	}

	private Element filesElement() throws Exception {
	    if (null == _files) {
			_files = resetFilesContainerElement();
		}
	    return _files;
    }

	protected String prepareFileNameForNode(String file) {
		return file.replace('/', '\\');
	}

	public void addFiles(String[] files) throws Exception {
		for (int i=0; i<files.length; ++i) {
			addFile(files[i]);
		}
	}

	public void addResources(String[] resources) throws Exception {
		for (int i = 0; i < resources.length; i++) {
	        addResource(resources[i]);
        }
    }

	private void addResource(String resource) throws Exception {
		String relativePath = prepareFileNameForNode(resource);
		filesElement().appendChild(createResourceNode(relativePath));
    }

	public void writeToFile(File file) throws IOException {
		// write to temp file first to avoid problem with file
		// being locked
		final File tempFile = File.createTempFile("vsp", null);
		writeTo(tempFile.getAbsolutePath());
		copyFile(tempFile, file);
	}

	private void copyFile(File fromFile, File toFile) throws IOException {
		final FileInputStream in = new FileInputStream(fromFile);
		try {
			final FileOutputStream out = new FileOutputStream(toFile);
			try {
				byte[] buffer = new byte[32*1024];
				while (true) {
					int read = in.read(buffer);
					if (read <= 0) break;
					out.write(buffer, 0, read);
				}
			} finally {
				out.close();
			}
		} finally {
			in.close();
		}
	}

	private void writeTo(String uri) throws IOException {
		try {
			Transformer transformer = TransformerFactory.newInstance().newTransformer();
			transformer.setOutputProperty(OutputKeys.INDENT, "yes");

			StreamResult result = new StreamResult(new FileWriter(new File(uri)));
			transformer.transform(new DOMSource(_document), result);
			result.getWriter().close();
			
		} catch (TransformerException e) {
			throw new RuntimeException(e);
		}
	}
	
	protected abstract Element resetFilesContainerElement() throws Exception;

	protected abstract Node createFileNode(String file);
	
	protected Node createResourceNode(String resource) {
		throw new UnsupportedOperationException();
	}

	public Element selectElement(String xpath) throws XPathExpressionException {
		return (Element)newXPath().evaluate(xpath, _document, XPathConstants.NODE);
	}
	
	public NodeList selectNodes(String xpath) throws XPathExpressionException {
		return (NodeList)newXPath().evaluate(xpath, _document, XPathConstants.NODESET);
	}

	private XPath newXPath() {
		return XPathFactory.newInstance().newXPath();
	}
	
	public Element createElement(String tagName, String value) {
		final Element element = createElement(tagName);
		element.setTextContent(value);
		return element;
	}
	
	protected Element createElement(String tagName) {
		return _document.createElement(tagName);
	}

	protected void invalidProjectFile() {
		throw new RuntimeException("Invalid project file");
	}

	public String getHintPathFor(String assemblyName) throws Exception {
		throw new UnsupportedOperationException();
	}

	public void setHintPathFor(String assemblyName, String hintPath) throws Exception {
		throw new UnsupportedOperationException();
	}

}
