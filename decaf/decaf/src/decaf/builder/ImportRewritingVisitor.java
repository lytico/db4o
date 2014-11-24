/* Copyright (C) 2011 Versant Inc. http://www.db4o.com */
package decaf.builder;

import org.eclipse.jdt.core.dom.*;

public class ImportRewritingVisitor extends DecafVisitorBase {

	public ImportRewritingVisitor(DecafRewritingContext context) {
		super(context);
	}
	
	@Override
	public boolean visit(ImportDeclaration node) {
		
		if ("decaf".equals(node.resolveBinding().getName())) {
			rewrite().remove(node);
			return false;
		}
		else {
			return super.visit(node);
		}
	}

}
