/*This file is free software; you can redistribute it and/or modify it under
the terms of version 2 of the GNU General Public License.

You should have received a copy of the GNU General Public License along
with this program; if not, write to the Free Software Foundation, Inc.,
59 Temple Place - Suite 330, Boston, MA  02111-1307, USA. */

package com.db4o.devtools.ant;

import java.io.*;
import java.net.*;
import java.util.*;

import javax.xml.parsers.*;
import javax.xml.transform.*;
import javax.xml.transform.dom.*;
import javax.xml.transform.stream.*;
import javax.xml.xpath.*;

import org.apache.tools.ant.*;
import org.w3c.dom.*;
import org.xml.sax.*;

/**
 * @author $Author: Tetyana Loskutova $
 * @version $Revision: 1.0.0.0 $ (based on JReptator by christianhujer v1.2)
 * 
 */
public class LinkCheckTask extends Task {

	/** List containing the starting points where to check. */
	private ArrayList<String> startingPoints = new ArrayList<String>();

	/** The Hosts that are okay to follow when linked to. */
	private ArrayList<String> okayHosts = new ArrayList<String>();

	/** File extensions that can be checked for broken links */
	private static ArrayList<String> okayExtensions = new ArrayList<String>();

	/** The protocols that are okay to follow when linked to. */
	private static ArrayList<String> okayProtocols = new ArrayList<String>();

	static {
		okayProtocols.add("http");
		okayProtocols.add("file");
	}

	static {
		okayExtensions.add("htm");
		okayExtensions.add("html");
	}

	/** The list of resources already checked. */
	private ArrayList<String> checkedResources = new ArrayList<String>();

	/** The list of resources that have to be checked. */
	private ArrayList<String> uncheckedResources = new ArrayList<String>();

	/** Non-responsive links in format parent_link: broken_link */
	private ArrayList<String> brokenLinks = new ArrayList<String>();

	/** The DocumentBuilderFactory to create DocumentBuilders. */
	private DocumentBuilderFactory dbf;

	/** The DocumentBuilder to create Documents. */
	private DocumentBuilder db;

	/** The filename for the protocol. */
	private String protocolFilename;

	/** Proxy address if any. */
	private String proxyAddress;

	/** Proxy port if any. */
	private int proxyPort;

	/** Proxy login name. */
	private String proxyLogin;

	/** Proxy password. */
	private String proxyPassword;

	/**
	 * Sets the filename for the protocol.
	 * 
	 * @param protocolFilename
	 *            filename for protocol
	 */
	public void setProtocolFilename(final String protocolFilename) {
		this.protocolFilename = protocolFilename;
	}

	/**
	 * Sets the proxy address
	 * 
	 * @param proxy
	 *            proxy address
	 */
	public void setProxy(String proxy) {
		this.proxyAddress = proxy;
	}

	/**
	 * Sets the proxy port
	 * 
	 * @param port
	 *            proxy port
	 */
	public void setProxyPort(int port) {
		this.proxyPort = port;
	}

	/**
	 * Sets the proxy login
	 * 
	 * @param login
	 *            login name
	 */
	public void setProxyLogin(String login) {
		this.proxyLogin = login;
	}

	/**
	 * Sets the proxy password
	 * 
	 * @param password
	 *            password
	 */
	public void setProxyPassword(String password) {
		this.proxyPassword = password;
	}

	/**
	 * Sets the starting point for this task.
	 * 
	 * @param s
	 *            Start URI
	 */
	public void setStartURI(final String s) {
		startingPoints.add(s);
	}

	/***************************************************************************
	 * Extracts the directory path, from which the checking will occur For
	 * example for "http://www.site.com/index.html" the result will be
	 * "http://www.site.com".
	 * 
	 * @param path
	 *            start element path
	 * @return start directory path
	 */
	private String getStartFolder(String path) {
		int index1 = path.lastIndexOf("\\");
		int index2 = path.lastIndexOf("/");
		index1 = index1 > index2 ? index1 : index2;
		return path.substring(0, index1);
	}

	/**
	 * Entry point for the Ant task
	 */
	public void execute() throws BuildException {
		try {
			if (startingPoints.isEmpty()) {
				throw new BuildException("No starting points");
			}
			if (proxyPort == 0) {
				proxyPort = -1;
			}
			if (proxyAddress != null) {
				System.getProperties().put("proxySet", "true");
				System.getProperties().put("proxyHost", proxyAddress);
				System.getProperties().put("proxyPort", proxyPort);
			}

			if (protocolFilename == null || protocolFilename.length() == 0) {
				protocolFilename = "CheckResults.xml";
			}
			dbf = DocumentBuilderFactory.newInstance();
			dbf.setNamespaceAware(true);
			dbf.setIgnoringComments(true);
			dbf.setIgnoringElementContentWhitespace(true);
			db = dbf.newDocumentBuilder();
			for (int i = 0; i < startingPoints.size(); i++) {
				String sp = (String) startingPoints.get(i);
				if (!(uncheckedResources.contains(sp))) {
					uncheckedResources.add(sp);
				}
				String host = getStartFolder(sp);
				okayHosts.add(host);
			}
			check();
			writeProtocol();
		} catch (ParserConfigurationException e) {
			throw new BuildException(e);
		}
		catch(Exception e) {
			throw new BuildException(e);
		}
	}

	/**
	 * Writes the protocol of the task to the file specified as ProtocolFilename
	 * 
	 * @throws BuildException when broken links are found
	 * @throws IOException 
	 * @throws TransformerException 
	 * @throws TransformerFactoryConfigurationError 
	 */
	private void writeProtocol() throws BuildException, XPathExpressionException, SAXException, IOException, TransformerFactoryConfigurationError, TransformerException {
		try {
			Document doc = db.newDocument();
			Element root = doc.createElement("linkcheckresults");
			doc.appendChild(root);
			Text nl = doc.createTextNode("\n");
			Text indent = doc.createTextNode("    ");
			Element resourcesEl = doc.createElement("resources");
			root.appendChild(nl.cloneNode(false));
			root.appendChild(indent.cloneNode(false));
			root.appendChild(resourcesEl);
			for (int i = 0; i < checkedResources.size(); i++) {
				String r = (String) checkedResources.get(i);
				resourcesEl.appendChild(nl.cloneNode(false));
				resourcesEl.appendChild(indent.cloneNode(false));
				resourcesEl.appendChild(indent.cloneNode(false));
				Element resourcesLink = doc.createElement("link");
				resourcesEl.appendChild(resourcesLink);
				resourcesLink.appendChild(doc.createTextNode(r));
			}
			resourcesEl.appendChild(nl.cloneNode(false));
			resourcesEl.appendChild(indent.cloneNode(false));
			Element linksEl = doc.createElement("brokenlinks");
			root.appendChild(nl.cloneNode(false));
			root.appendChild(indent.cloneNode(false));
			root.appendChild(linksEl);
			for (int i = 0; i < brokenLinks.size(); i++) {
				String link = (String) brokenLinks.get(i);
				linksEl.appendChild(nl.cloneNode(false));
				linksEl.appendChild(indent.cloneNode(false));
				linksEl.appendChild(indent.cloneNode(false));
				Element resourcesLink = doc.createElement("link");
				linksEl.appendChild(resourcesLink);
				String link1 = link.substring(0, link.indexOf(": "));
				String link2 = link.substring(link.indexOf(": ") + 2);
				Element resourcesParent = doc.createElement("parent");
				resourcesLink.appendChild(resourcesParent);
				resourcesParent.appendChild(doc.createTextNode(link1));
				Element resourcesChild = doc.createElement("child");
				resourcesLink.appendChild(resourcesChild);
				resourcesChild.appendChild(doc.createTextNode(link2));
			}
			linksEl.appendChild(nl.cloneNode(false));
			linksEl.appendChild(indent.cloneNode(false));
			root.appendChild(nl.cloneNode(false));
			TransformerFactory.newInstance().newTransformer().transform(
					new DOMSource(doc),
					new StreamResult(new File(protocolFilename)));

		} catch (Exception e) {
			System.err.println(e);
		}
		if (brokenLinks.size() > 0) {
			System.err.println(readFile(protocolFilename));
			throw new BuildException("Broken links exist. See report file: " + protocolFilename);
		}
	}

	private String readFile(String fileName) throws SAXException, XPathExpressionException, IOException, TransformerFactoryConfigurationError, TransformerException {
		final Document doc = db.parse(fileName);
		final XPath xpath = XPathFactory.newInstance().newXPath();
		final Node brokenLinksNode = (Node) xpath.evaluate("linkcheckresults/brokenlinks", doc, XPathConstants.NODE);
		
		if (brokenLinksNode == null) {
			return "";
		}
		
		final Transformer transformer = TransformerFactory.newInstance().newTransformer();
		transformer.setOutputProperty(OutputKeys.METHOD, "xml");
		StringWriter sw = new StringWriter();
		transformer.transform(new DOMSource(brokenLinksNode), new StreamResult(sw));
		
		return sw.toString();
	}

	/**
	 * Checks the location for broken links
	 */
	private void check() {
		while (!uncheckedResources.isEmpty()) {
			doCheck();
		}
	}

	/**
	 * Checks the location for broken links
	 */
	private void doCheck() {
		String strURL = (String) uncheckedResources.get(0);

		URL url;
		try {
			if (proxyAddress != null) {
				url = new URL("http", proxyAddress, proxyPort, strURL);
			} else {
				url = new URL(strURL);
			}
		} catch (MalformedURLException e) {
			brokenLinks.add(strURL + ": " + strURL);
			uncheckedResources.remove(0);
			return;
		}

		uncheckedResources.remove(0);
		checkedResources.add(strURL);
		System.out.println("Checking: " + strURL);

		if (!okayProtocols.contains(url.getProtocol())) {
			return;
		}

		try {
			// try opening the URL
			URLConnection urlConnection = url.openConnection();
			authConnection(urlConnection);

			BufferedReader in = new BufferedReader(new InputStreamReader(
					urlConnection.getInputStream()));

			String content = "";
			String newContent;

			while ((newContent = in.readLine()) != null) {
				content += newContent;
			}
			in.close();

			// System.out.println(content);
			String lowerCaseContent = content.toLowerCase();

			int index = 0;
			while ((index = lowerCaseContent.indexOf("<a", index)) != -1) {
				if ((index = lowerCaseContent.indexOf("href", index)) == -1)
					break;
				if ((index = lowerCaseContent.indexOf("=", index)) == -1)
					break;

				index++;
				String remaining = content.substring(index);

				StringTokenizer st = new StringTokenizer(remaining, "\t\n\r\"'>");
				String strLink = st.nextToken();
				if (isMailTo(strLink)) {
					continue;
				}
				
				if (!strLink.startsWith("http")/* local links */) {
					if (strLink.contains("..") || strLink.contains("&quot")
							|| strLink.startsWith("/")
							|| strLink.startsWith("javascript")) {
						continue;
					}
					strLink = getStartFolder(strURL) + "/" + strLink;
				}
				strLink = encode(strLink);
				System.out.println("Checking: " + strLink);

				URL urlLink;
				try {
					urlLink = new URL(strLink);
					strLink = urlLink.toString();
				} catch (MalformedURLException e) {
					brokenLinks.add(strURL + ": " + strLink + " Exception caught: " + e);
					continue;
				}

				if (!okayProtocols.contains(urlLink.getProtocol())) {
					continue;
				}

				try {
					// try opening the URL
					URLConnection urlLinkConnection = urlLink.openConnection();
					authConnection(urlLinkConnection);
					BufferedReader inLink = new BufferedReader(
							new InputStreamReader(urlLinkConnection
									.getInputStream()));

					inLink.close();

					if ((!checkedResources.contains(strLink))
							&& (!uncheckedResources.contains(strLink))) {

						boolean okHost = false;
						for (int i = 0; i < okayHosts.size(); i++) {
							if (strLink.contains(okayHosts.get(i).toString())) {
								okHost = true;
								continue;
							}
						}
						String extension = getExtension(strLink);
						
						if (okHost && okayExtensions.contains(extension)) {
							uncheckedResources.add(strLink);
						}
					}

				} catch (IOException e) {
					brokenLinks.add(strURL + ": " + strLink + "\r\nException caught:" + e);
					continue;
				}
			}
		} catch (IOException e) {
			brokenLinks.add(strURL + "\r\nException caught:" + e);
		}

	}

	private String encode(String link) throws UnsupportedEncodingException {
		 return link.replaceAll(" ", "%20"); 
	}

	private boolean isMailTo(String link) {
		return link.startsWith("mailto:");
	}

	private String getExtension(String strLink) {
		String extension = strLink.lastIndexOf(".") >= 0 ? strLink
				.substring(strLink.lastIndexOf(".") + 1) : "";
		return extension;
	}

	private void authConnection(URLConnection urlConnection) {
		if (proxyLogin != null) {
			String password = proxyLogin + ":" + proxyPassword;
			
//			String encodedPassword = new sun.misc.BASE64Encoder().encode(password.getBytes());
//			urlConnection.setRequestProperty("Proxy-Authorization", encodedPassword);
		}
	}

}

/*
 * $Log: LinkCheckTask.java,v $ c-link: Revision 1.0.0.0 2007/08/09 19:31:00
 * Tetyana Loskutova Extracted from JReptator project, changed to work with html
 * instead of xml, simplified to a small script.
 * 
 * JReptator: Revision 1.2 2003/07/20 16:48:53 christianhujer New overall
 * revision. Prepared for supporting multiple and user defined document types.
 * 
 * Revision 1.1.1.1 2003/05/03 07:53:58 christianhujer Import of Prototype.
 * 
 * 
 */