/* This file is part of the db4o object database http://www.db4o.com

Copyright (C) 2004 - 2009  Versant Corporation http://www.versant.com

db4o is free software; you can redistribute it and/or modify it under
the terms of version 3 of the GNU General Public License as published
by the Free Software Foundation.

db4o is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or
FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
for more details.

You should have received a copy of the GNU General Public License along
with this program.  If not, see http://www.gnu.org/licenses/. */
package com.db4o.devtools.pdfdoc;


import java.io.*;
import java.util.*;

import javax.xml.parsers.*;
import javax.xml.xpath.*;

import org.w3c.dom.*;
import org.w3c.dom.Node;


public class HtmlFileListing {
    
    private static final String OPTION_OUTPUT_FILE_NAME = "-output-name";

	private static final String OPTION_CONTENT_FILE = "-content";

	private static final String OPTION_CONVERSION_DIR = "-conv";

	private static final String OPTION_REFERENCE = "-ref";

	static HashMap<String, String> options = new HashMap<String, String>();
    
    
    static {
        options.put(OPTION_REFERENCE, "");
        options.put(OPTION_CONVERSION_DIR, "");
        options.put(OPTION_CONTENT_FILE, "");
        options.put(OPTION_OUTPUT_FILE_NAME, "");
    }

    public static void main(String[] args) {
        for (int i = 0; i < args.length; i++) {
            String option = args[i];
            if (option.startsWith("-")) {
                if ("".equals(options.get(option.toLowerCase())) 
                        && (i < args.length - 1)) {
                    options.put(option, args[i+1]);
                }
            }
        }
        
        String ref = options.get(OPTION_REFERENCE);
        String conv = options.get(OPTION_CONVERSION_DIR);
        String content = options.get(OPTION_CONTENT_FILE);
        String outputFileName = options.get(OPTION_OUTPUT_FILE_NAME);
        
        if (ref.length() == 0
                || conv.length() == 0
                || content.length() == 0 
        		|| outputFileName.length() == 0) {
            System.err.println("Please specify the arguments!");
            System.err.println("java tools.HTML2PDF -ref <referencePath> \n " +
            		"-conv <conversionPath> \n " +
            		"-content <contentPath> \n " +
            		"-output-name <outputFileName>");
            System.exit(1);
        }
        
        HtmlFileListing task = new HtmlFileListing();
        task.setContentFile(content);
        task.setConversionPath(conv);
        task.setReferencePath(ref);
        task.setOutputFileName(outputFileName);
        task.execute();
    }




	private String _referencePath;

    private String _conversionPath;

    private String _contentFile;

	private String _outputFileName;
	
    private final String _expression = "html/body/div/ul//a";


    public void execute() {  
    	try {
			prepareDirectory();
			writeOutputFile();
        } catch (Exception e) {
			e.printStackTrace();
			System.exit(1);
		} 
    }



	private void prepareDirectory() throws RuntimeException {
		File conversionDir = new File(_conversionPath);
		if (!conversionDir.mkdirs() && !conversionDir.exists()) {
		    throw new RuntimeException("Cannot create dir: " + _conversionPath);
		}
	}


	private void writeOutputFile() throws FileNotFoundException {
		PrintWriter writer = new PrintWriter(_conversionPath + "/" + _outputFileName);
		String[] htmlFiles = getPageFiles(_contentFile);
		for (String fileName : htmlFiles) {
			fileName = _referencePath + fileName;
			fileName = fileName.replace('/', '\\');	// create windows pathnames for VB/Acrobat
			writer.println(fileName);
		}
		writer.close();
	}

	
    String[] getPageFiles(String contentFile) {
        try {
            DocumentBuilder builder = DocumentBuilderFactory.newInstance().newDocumentBuilder();
            Document document = builder.parse(new File(contentFile));

            XPath xpath = XPathFactory.newInstance().newXPath();
            NodeList nodeList = (NodeList) xpath.evaluate(_expression, document, XPathConstants.NODESET);
            String[] pages = new String[nodeList.getLength()];
            for (int i = 0; i < nodeList.getLength(); i++) {
                Node node = nodeList.item(i);
                String href = node.getAttributes().getNamedItem("href").getTextContent();
                pages[i] = href;
            }
            return pages;
        } catch (Exception e) {
            e.printStackTrace();
            return null;
        }
    }
    
    
    public void setReferencePath(String path) {
        _referencePath = path;
    }

    public void setConversionPath(String path) {
        _conversionPath = path;
    }

    public void setContentFile(String file) {
        _contentFile = file;
    }
    
    private void setOutputFileName(String outputFileName) {
		_outputFileName = outputFileName;
	}
}
