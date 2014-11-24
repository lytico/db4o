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
 * StackExpr represents an expression that is stored on the stack.
 */
public class StackExpr extends VarExpr {
	/**
	 * Constructor.
	 * 
	 * @param index
	 *            Location (offset) in stack of the information to which the
	 *            expression refers. Index 0 represents the bottom of the stack.
	 * @param type
	 *            The type of this expression.
	 */
	public StackExpr(final int index, final Type type) {
		super(index, type);
	}

	public void visitForceChildren(final TreeVisitor visitor) {
	}

	public void visit(final TreeVisitor visitor) {
		visitor.visitStackExpr(this);
	}

	public int exprHashCode() {
		return 20 + index + type.simple().hashCode();
	}

	public boolean equalsExpr(final Expr other) {
		return (other instanceof StackExpr)
				&& ((StackExpr) other).type.simple().equals(type.simple())
				&& (((StackExpr) other).index == index);
	}

	public Object clone() {
		return copyInto(new StackExpr(index, type));
	}
}
