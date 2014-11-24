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
 * NewArrayExpr represents the <tt>newarray</tt> opcode which creates a new
 * array of a specified length and element type.
 */
public class NewArrayExpr extends Expr {
	// newarray

	Expr size;

	Type elementType;

	/**
	 * Constructor.
	 * 
	 * @param size
	 *            Expression representing the size of the array.
	 * @param elementType
	 *            The type of the elements in the array.
	 * @param type
	 *            The type of this expression.
	 */
	public NewArrayExpr(final Expr size, final Type elementType, final Type type) {
		super(type);
		this.size = size;
		this.elementType = elementType;

		size.setParent(this);
	}

	public Expr size() {
		return size;
	}

	public Type elementType() {
		return elementType;
	}

	public void visitForceChildren(final TreeVisitor visitor) {
		if (visitor.reverse()) {
			size.visit(visitor);
		} else {
			size.visit(visitor);
		}
	}

	public void visit(final TreeVisitor visitor) {
		visitor.visitNewArrayExpr(this);
	}

	public int exprHashCode() {
		return 15 + size.exprHashCode();
	}

	public boolean equalsExpr(final Expr other) {
		return false;
	}

	public Object clone() {
		return copyInto(new NewArrayExpr((Expr) size.clone(), elementType, type));
	}
}
