/*
 * All files in the distribution of BLOAT (Bytecode Level Optimization and
 * Analysis tool for Java(tm)) are Copyright 1997-2001 by the Purdue
 * Research Foundation of Purdue University.  All rights reserved.
 * 
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
 */

package EDU.purdue.cs.bloat.tree;

import EDU.purdue.cs.bloat.cfg.*;

/**
 * IfZeroStmt evaluates an expression and executes one of its two branches
 * depending on whether or not the expression evaluated to zero.
 */
public class IfZeroStmt extends IfStmt {
	Expr expr; // Expression to evaluate

	/**
	 * Constructor.
	 * 
	 * @param comparison
	 *            Comparison operator.
	 * @param expr
	 *            An expression to be evaluated.
	 * @param trueTarget
	 *            Basic Block that is executed if the expression evaluates to
	 *            zero.
	 * @param falseTarget
	 *            Basic Block that is executed if the expression evaluates to
	 *            non-zero.
	 */
	public IfZeroStmt(final int comparison, final Expr expr,
			final Block trueTarget, final Block falseTarget) {
		super(comparison, trueTarget, falseTarget);
		this.expr = expr;
		expr.setParent(this);
	}

	/**
	 * @return The expression that is evaluated.
	 */
	public Expr expr() {
		return expr;
	}

	public void visitForceChildren(final TreeVisitor visitor) {
		expr.visit(visitor);
	}

	public void visit(final TreeVisitor visitor) {
		visitor.visitIfZeroStmt(this);
	}

	public Object clone() {
		return copyInto(new IfZeroStmt(comparison, (Expr) expr.clone(),
				trueTarget, falseTarget));
	}
}
