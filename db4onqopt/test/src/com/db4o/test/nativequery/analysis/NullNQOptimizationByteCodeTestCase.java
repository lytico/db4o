/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.test.nativequery.analysis;

import EDU.purdue.cs.bloat.editor.MethodEditor;
import EDU.purdue.cs.bloat.editor.Opcode;

import com.db4o.nativequery.expr.Expression;

public class NullNQOptimizationByteCodeTestCase extends
		NQOptimizationByteCodeTestCaseBase {

	protected void assertOptimization(Expression expression) throws Exception {
		// TODO

	}

	protected void generateMethodBody(MethodEditor method) {
		method.addInstruction(Opcode.opc_ldc,new Integer(0));
		method.addInstruction(Opcode.opc_ireturn);
	}

}
