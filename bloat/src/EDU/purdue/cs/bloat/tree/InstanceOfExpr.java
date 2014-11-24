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
 * InstanceOfExpr represnts the <i>instanceof</i> opcode which determine if an
 * object is of a given type.
 */
public class InstanceOfExpr extends CondExpr {
	// instanceof

	Expr expr; // Expression (object) whose type we verify

	Type checkType; // Type to verify against

	/**
	 * Constructor.
	 * 
	 * @param expr
	 *            Expression (object) whose type is to be verified.
	 * @param checkType
	 *            Type to verify against (That is, is expr of type checkType?)
	 * @param type
	 *            Type of this expression.
	 */
	public InstanceOfExpr(final Expr expr, final Type checkType, final Type type) {
		super(type);
		this.expr = expr;
		this.checkType = checkType;

		expr.setParent(this);
	}

	public Expr expr() {
		return expr;
	}

	public Type checkType() {
		return checkType;
	}

	public void visitForceChildren(final TreeVisitor visitor) {
		if (visitor.reverse()) {
			expr.visit(visitor);
		} else {
			expr.visit(visitor);
		}
	}

	public void visit(final TreeVisitor visitor) {
		visitor.visitInstanceOfExpr(this);
	}

	public int exprHashCode() {
		return 12 + expr.exprHashCode();
	}

	public boolean equalsExpr(final Expr other) {
		return (other != null) && (other instanceof InstanceOfExpr)
				&& ((InstanceOfExpr) other).checkType.equals(checkType)
				&& ((InstanceOfExpr) other).expr.equalsExpr(expr);
	}

	public Object clone() {
		return copyInto(new InstanceOfExpr((Expr) expr.clone(), checkType, type));
	}
}
