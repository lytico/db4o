/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.nativequery.analysis;

import java.util.*;

import EDU.purdue.cs.bloat.tree.*;

public class OpSymmetryUtil {

	private final static Map<Integer,Integer> OP_SYMMETRY = new HashMap<Integer,Integer>();

	static {
		OP_SYMMETRY.put(IfStmt.EQ, IfStmt.EQ);
		OP_SYMMETRY.put(IfStmt.NE, IfStmt.NE);
		OP_SYMMETRY.put(IfStmt.LT, IfStmt.GT);
		OP_SYMMETRY.put(IfStmt.GT, IfStmt.LT);
		OP_SYMMETRY.put(IfStmt.LE, IfStmt.GE);
		OP_SYMMETRY.put(IfStmt.GE, IfStmt.LE);
	}

	public static int counterpart(int op) {
		return OP_SYMMETRY.get(op);
	}
}
