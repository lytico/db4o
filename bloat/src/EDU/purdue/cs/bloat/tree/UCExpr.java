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
 * UPExpr represents an update check opcode which checks the persistent store to
 * determine if a variable needs to be updated.
 */
public class UCExpr extends CheckExpr {
	public static final int POINTER = 1;

	public static final int SCALAR = 2;

	int kind;

	/**
	 * Constructor.
	 * 
	 * @param expr
	 *            The expression to check to see if it needs to be updated.
	 * @param kind
	 *            The kind of expression (POINTER or SCALAR) to be checked.
	 * @param type
	 *            The type of this expression.
	 */
	public UCExpr(final Expr expr, final int kind, final Type type) {
		super(expr, type);
		this.kind = kind;
	}

	public int kind() {
		return kind;
	}

	public void visit(final TreeVisitor visitor) {
		visitor.visitUCExpr(this);
	}

	public boolean equalsExpr(final Expr other) {
		return (other instanceof UCExpr) && super.equalsExpr(other)
				&& (((UCExpr) other).kind == kind);
	}

	public Object clone() {
		return copyInto(new UCExpr((Expr) expr.clone(), kind, type));
	}
}
