/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.nativequery.optimization;

import EDU.purdue.cs.bloat.cfg.*;
import EDU.purdue.cs.bloat.editor.*;
import EDU.purdue.cs.bloat.file.*;
import EDU.purdue.cs.bloat.tree.*;

import com.db4o.instrumentation.bloat.*;
import com.db4o.instrumentation.core.*;
import com.db4o.nativequery.*;
import com.db4o.nativequery.analysis.*;
import com.db4o.nativequery.expr.*;
import com.db4o.nativequery.instrumentation.*;

public class NativeQueryEnhancer {
	
	public boolean enhance(BloatLoaderContext bloatUtil,ClassEditor classEditor,String methodName,Type[] argTypes,ClassSource classSource) throws Exception {
		if(NQDebug.LOG) {
			System.err.println("Enhancing "+classEditor.name());
		}
		Expression expr = analyze(bloatUtil, classEditor, methodName, argTypes);
		if(expr==null) {
			return false;
		}
		new SODAMethodBuilder(new BloatTypeEditor(classEditor, bloatUtil.references())).injectOptimization(expr);
		
		classEditor.commit();
		return true;
	}
	
	public Expression analyze(BloatLoaderContext bloatUtil, ClassEditor classEditor, String methodName) {
		return analyze(bloatUtil, classEditor, methodName,null);
	}
	
	public Expression analyze(BloatLoaderContext bloatUtil, ClassEditor classEditor, String methodName, Type[] argTypes) {
		FlowGraph flowGraph=null;
		try {
			flowGraph=bloatUtil.flowGraph(classEditor,methodName, argTypes);
		} catch (ClassNotFoundException e) {
			return null;
		}
		if(flowGraph!=null) {
			BloatExprBuilderVisitor builder = new BloatExprBuilderVisitor(bloatUtil);
			if(NQDebug.LOG) {
				System.out.println("FLOW GRAPH:");
				flowGraph.visit(new PrintVisitor());
			}
			flowGraph.visit(builder);
			Expression expr=builder.expression();
			if(NQDebug.LOG) {
				System.out.println("EXPRESSION TREE:");
				System.out.println(expr);
			}
			return expr;
		}
		return null;
	}
}
