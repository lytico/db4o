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

/**
 * SCStmt represents a swizzle check on an element in an array.
 */
public class SCStmt extends Stmt {

	Expr array;

	Expr index;

	boolean redundant;

	/**
	 * Constructor.
	 * 
	 * @param a
	 *            The array on which to place the swizzle check.
	 * @param i
	 *            The element in array a to swizzle.
	 */
	public SCStmt(final Expr a, final Expr i) {
		this.array = a;
		this.index = i;
		this.redundant = false;
		array.setParent(this);
		index.setParent(this);
	}

	public Expr array() {
		return array;
	}

	public Expr index() {
		return index;
	}

	/**
	 * @return True, if the swizzle check is redundent.
	 */
	public boolean redundant() {
		return redundant;
	}

	public void set_redundant(final boolean val) {
		this.redundant = val;
	}

	public void visit(final TreeVisitor visitor) {
		visitor.visitSCStmt(this);
	}

	public Object clone() {
		return copyInto(new SCStmt((Expr) array.clone(), (Expr) index.clone()));
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

}
