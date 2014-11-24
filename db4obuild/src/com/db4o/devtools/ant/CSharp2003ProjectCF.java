package com.db4o.devtools.ant;

import org.w3c.dom.Document;

public class CSharp2003ProjectCF extends CSharp2003Project {
	public CSharp2003ProjectCF(Document document) throws Exception {
		super(document);
	}
	
	protected String getXPathExpression() {
		return "VisualStudioProject/ECSHARP/Files/Include";
	}
}
