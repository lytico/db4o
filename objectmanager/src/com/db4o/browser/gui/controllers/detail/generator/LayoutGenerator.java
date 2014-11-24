/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.browser.gui.controllers.detail.generator;

import java.io.DataInputStream;
import java.io.IOException;
import java.util.ArrayList;

import org.apache.oro.text.perl.Perl5Util;

import com.db4o.objectmanager.model.GraphPosition;
import com.db4o.objectmanager.model.IGraphIterator;
import com.db4o.objectmanager.model.nodes.IModelNode;

/**
 * LayoutGenerator.
 *
 * @author djo
 */
public class LayoutGenerator {
	
	public static String resourceFile(String fileName) {
		DataInputStream input = new DataInputStream(LayoutGenerator.class.getResourceAsStream(fileName));
		
		StringBuffer result = new StringBuffer();
		byte[] buffer = new byte[4096];
		int bytesRead = -1;
		do {
			try {
				bytesRead = input.read(buffer);
				if (bytesRead > 0) {
					char chars[] = new char[bytesRead];
					for (int i = 0; i < chars.length; i++) {
						chars[i] = (char) buffer[i];
					}
					result.append(chars);
				}
			} catch (IOException e) {
				throw new RuntimeException("Error reading resource file", e);
			}
		} while (bytesRead >= 0);
		
		return result.toString();
	}
	
	private static final String errorTemplate = resourceFile("invalidTemplate.xswt");
	
    private static final String FIELDS_TOKEN = "%%fields";
	private static final String FIELDS_REGEX = "/" + FIELDS_TOKEN + "/";
	private static final String FIELD_NO_TOKEN = "%fieldNumber";
	private static final String FIELD_NAME_TOKEN = "%fieldName";
	private static final String FIELD_VALUE_TOKEN = "%fieldValue";
	
	private static final Perl5Util perl = new Perl5Util();
	
	private static String substitute(String token, String replaceWith, String in) {
		replaceWith = perl.substitute("s%/%\\\\/%g", replaceWith);	// Automatically quote '/' characters
		String perlCommand = "s/" + token + "/" + replaceWith + "/g";
		String result = perl.substitute(perlCommand, in);
		return result;
	}
	
	public static String fillTemplateString(IGraphIterator input, String layoutTemplate) {
		GraphPosition initialState = input.getPath();
		
		try {
			// Break it up into header, body template, and footer
			ArrayList parts = new ArrayList();
			perl.split(parts, FIELDS_REGEX, layoutTemplate);
			
			// If the template is invalid, return an Invalid Template part
			if (parts.size() != 4) {
				layoutTemplate = errorTemplate;
				return layoutTemplate;
			} else {
				// Build the real layout
				String header = (String) parts.get(0);
                String row_readonly = (String) parts.get(1);
                String row_editable = (String) parts.get(2);
				String footer = (String) parts.get(3);
				
				// Go to the beginning
				while (input.hasPrevious()) input.previous();
				
				// Build the layout: start with the header
				StringBuffer contents = new StringBuffer(header);
				
				// Each object gets a copy of the "row" fragment
				int i=0;
				while (input.hasNext()) {
					++i;
					IModelNode node = (IModelNode) input.next();
					
                    String currentRow;
                    if (node.isEditable()) {
//                    if (false) {
                        // Substitute for all tokens from the editor row template
                        currentRow = row_editable;
                        currentRow = substitute(FIELD_NO_TOKEN, Integer.toString(i), currentRow);
                        String fieldName = node.getName();
                        if (!fieldName.equals(""))
                            fieldName += ": ";
                        currentRow = substitute(FIELD_NAME_TOKEN, fieldName, currentRow);
                        currentRow = substitute(FIELD_VALUE_TOKEN, node.getValueString(), currentRow);
                    } else {
    					// Substitute for all tokens from the read-only template
    					currentRow = row_readonly;
    					currentRow = substitute(FIELD_NO_TOKEN, Integer.toString(i), currentRow);
    					String fieldName = node.getName();
    					if (!fieldName.equals(""))
    						fieldName += ": ";
    					currentRow = substitute(FIELD_NAME_TOKEN, fieldName, currentRow);
    					currentRow = substitute(FIELD_VALUE_TOKEN, node.getValueString(), currentRow);
                    }
					
					contents.append(currentRow);
				}
				
				// Add the footer and return the result
				contents.append(footer);
				return contents.toString();
			}
		} finally {
			input.setPath(initialState);
		}
	}
}
