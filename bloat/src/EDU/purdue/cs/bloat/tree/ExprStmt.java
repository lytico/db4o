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

/**
 * ExprStmt is a statement consisting of an expression.
 * 
 * @see Expr
 */
public class ExprStmt extends Stmt {
	Expr expr; // Expression contained in this statement

	/**
	 * Constructor.
	 * 
	 * @param expr
	 *            The expression contained in this statement.
	 */
	public ExprStmt(final Expr expr) {
		this.expr = expr;
		expr.setParent(this);
	}

	public Expr expr() {
		return expr;
	}

	public void visitForceChildren(final TreeVisitor visitor) {
		expr.visit(visitor);
	}

	public void visit(final TreeVisitor visitor) {
		visitor.visitExprStmt(this);
	}

	public Object clone() {
		return copyInto(new ExprStmt((Expr) expr.clone()));
	}
}
