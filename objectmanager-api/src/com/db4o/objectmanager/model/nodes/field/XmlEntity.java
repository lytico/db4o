package com.db4o.objectmanager.model.nodes.field;

public class XmlEntity {

	public static String encode(String string) {
		string = string.replaceAll("\\&", "&amp;");
		string = string.replaceAll("\\'", "&apos;");
		string = string.replaceAll("\\<", "&lt;");
		string = string.replaceAll("\\>", "&gt;");
		string = string.replaceAll("\\\"", "&quot;");
		return string;
	}

}
