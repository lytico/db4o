/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.test.nativequery.analysis;

import EDU.purdue.cs.bloat.editor.*;

import com.db4o.nativequery.expr.*;
import com.db4o.nativequery.expr.cmp.*;

public class BooleanReturnValueTestCase extends
		NQOptimizationByteCodeTestCaseBase {

	private static final String FIELDNAME = "bool";

	protected void assertOptimization(Expression expression) throws Exception {
		NQOptimizationAssertUtil.assertComparison(expression, new String[]{ FIELDNAME }, Boolean.TRUE, ComparisonOperator.VALUE_EQUALITY, false);
	}

	protected void generateMethodBody(MethodEditor method) {
		Label falseLabel = createLabel();
		Label trueLabel = createLabel();
		Label returnLabel = createLabel(); 
		method.addInstruction(Opcode.opc_aload, new LocalVariable(1));
		method.addInstruction(Opcode.opc_getfield, createFieldReference(Type.getType(Data.class), FIELDNAME, Type.BOOLEAN));
		method.addInstruction(Opcode.opc_ldc, new Integer(1));
		method.addInstruction(Opcode.opc_if_icmpne, falseLabel);
		method.addLabel(trueLabel);
		method.addInstruction(Opcode.opc_ldc, new Integer(1));
		method.addInstruction(Opcode.opc_goto, returnLabel);
		method.addLabel(falseLabel);
		method.addInstruction(Opcode.opc_ldc, new Integer(0));
		method.addLabel(returnLabel);
		method.addInstruction(Opcode.opc_ireturn);
	}

}
