package com.db4o.util.file;

import java.io.*;

import javax.xml.parsers.*;

import org.w3c.dom.*;
import org.xml.sax.*;

final class XMLParserImpl implements XMLParser {
	private final IFile realFile;

	XMLParserImpl(IFile realFile) {
		this.realFile = realFile;
	}

	public Element root() {
		try {
			DocumentBuilderFactory dbf = DocumentBuilderFactory.newInstance();
			DocumentBuilder db = dbf.newDocumentBuilder();
			InputStream in = this.realFile.openInputStream();
			Document doc = db.parse(in);
			in.close();
			return doc.getDocumentElement();
		} catch (ParserConfigurationException e) {
			throw new RuntimeException(e);
		} catch (SAXException e) {
			throw new RuntimeException(e);
		} catch (IOException e) {
			throw new RuntimeException(e);
		}

	}
}