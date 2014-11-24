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
 * ArrayRefExpr represents an expression that references an element in an array.
 */
public class ArrayRefExpr extends MemRefExpr {
	Expr array;

	Expr index;

	Type elementType;

	/**
	 * Constructor.
	 * 
	 * @param array
	 *            The array whose element we are indexing.
	 * @param index
	 *            The index into the array.
	 * @param elementType
	 *            The type of elements in array.
	 * @param type
	 *            The type of this expression.
	 */
	public ArrayRefExpr(final Expr array, final Expr index,
			final Type elementType, final Type type) {
		super(type);
		this.array = array;
		this.index = index;
		this.elementType = elementType;

		array.setParent(this);
		index.setParent(this);
	}

	public Expr array() {
		return array;
	}

	public Expr index() {
		return index;
	}

	public Type elementType() {
		return elementType;
	}

	public void visitForceChildren(final TreeVisitor visitor) {
		if (visitor.reverse()) {
			index.visit(visitor);
			array.visit(visitor);
		} else {
			array.visit(visitor);
			index.visit(visitor);
		}
	}

	public void visit(final TreeVisitor visitor) {
		visitor.visitArrayRefExpr(this);
	}

	public int exprHashCode() {
		return 4 + array.exprHashCode() ^ index.exprHashCode();
	}

	public boolean equalsExpr(final Expr other) {
		return (other != null) && (other instanceof ArrayRefExpr)
				&& ((ArrayRefExpr) other).array.equalsExpr(array)
				&& ((ArrayRefExpr) other).index.equalsExpr(index);
	}

	public Object clone() {
		return copyInto(new ArrayRefExpr((Expr) array.clone(), (Expr) index
				.clone(), elementType, type));
	}
}
