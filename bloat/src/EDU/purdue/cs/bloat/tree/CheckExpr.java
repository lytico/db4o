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

import EDU.purdue.cs.bloat.editor.*;

/**
 * CheckExpr is a superclass for classes representing a check on an expression.
 * For instance, a CheckExpr is inserted into the tree before the divisor of a
 * divide operation. The CheckExpr checks to make sure that the divisor is not
 * zero.
 * 
 * @see RCExpr
 * @see UCExpr
 * @see ZeroCheckExpr
 */
public abstract class CheckExpr extends Expr {
	Expr expr;

	/**
	 * Constructor.
	 * 
	 * @param expr
	 *            An expression that is to be checked.
	 * @param type
	 *            The type of this expression.
	 */
	public CheckExpr(final Expr expr, final Type type) {
		super(type);
		this.expr = expr;
		expr.setParent(this);
	}

	public void visitForceChildren(final TreeVisitor visitor) {
		if (visitor.reverse()) {
			expr.visit(visitor);
		} else {
			expr.visit(visitor);
		}
	}

	/**
	 * Returns the expression being checked.
	 */
	public Expr expr() {
		return expr;
	}

	public int exprHashCode() {
		return 9 + expr.exprHashCode() ^ type.simple().hashCode();
	}

	public boolean equalsExpr(final Expr other) {
		return (other != null) && (other instanceof CheckExpr)
				&& ((CheckExpr) other).expr.equalsExpr(expr);
	}
}
