/**
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
 * StoreExpr represents a store of an expression into a memory location.
 * 
 * @see MemExpr
 */
public class StoreExpr extends Expr implements Assign {
	MemExpr target;

	Expr expr;

	/**
	 * Constructor.
	 * 
	 * @param target
	 *            The memory location (or local variable, etc.) into which expr
	 *            is stored.
	 * @param expr
	 *            An expression whose value is to be stored.
	 * @param type
	 *            The type of this expression.
	 */
	public StoreExpr(final MemExpr target, final Expr expr, final Type type) {
		super(type);

		this.target = target;
		this.expr = expr;

		target.setParent(this);
		expr.setParent(this);
	}

	/**
	 * Returns the <tt>MemExpr</tt> into which the expression is stored.
	 */
	public DefExpr[] defs() {
		return new DefExpr[] { target };
	}

	/**
	 * Returns the memory location (or local variable) into which the expression
	 * is stored.
	 */
	public MemExpr target() {
		return target;
	}

	/**
	 * Returns the expression being stored.
	 */
	public Expr expr() {
		return expr;
	}

	public void visitForceChildren(final TreeVisitor visitor) {
		if (visitor.reverse()) {
			target.visitOnly(visitor);
			expr.visit(visitor);
			target.visitChildren(visitor);
		} else {
			target.visitChildren(visitor);
			expr.visit(visitor);
			target.visitOnly(visitor);
		}
	}

	public void visit(final TreeVisitor visitor) {
		visitor.visitStoreExpr(this);
	}

	public int exprHashCode() {
		return 22 + target.exprHashCode() ^ expr.exprHashCode();
	}

	public boolean equalsExpr(final Expr other) {
		return (other instanceof StoreExpr)
				&& ((StoreExpr) other).target.equalsExpr(target)
				&& ((StoreExpr) other).expr.equalsExpr(expr);
	}

	public Object clone() {
		return copyInto(new StoreExpr((MemExpr) target.clone(), (Expr) expr
				.clone(), type));
	}
}
