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
 * SRStmt represents the swizzle of a range of elements in an array.
 */
public class SRStmt extends Stmt {
	Expr array;

	Expr start; // starting value of range

	Expr end; // terminating value of range

	/**
	 * Constructor.
	 * 
	 * @param a
	 *            The array to swizzle.
	 * @param s
	 *            The starting value of the swizzle range.
	 * @param t
	 *            The terminating value of the swizzle range.
	 */
	public SRStmt(final Expr a, final Expr s, final Expr t) {
		this.array = a;
		this.start = s;
		this.end = t;
		array.setParent(this);
		start.setParent(this);
		end.setParent(this);
	}

	public Expr array() {
		return array;
	}

	public Expr start() {
		return start;
	}

	public Expr end() {
		return end;
	}

	public void visit(final TreeVisitor visitor) {
		visitor.visitSRStmt(this);
	}

	public Object clone() {
		return copyInto(new SRStmt((Expr) array.clone(), (Expr) start.clone(),
				(Expr) end.clone()));
	}

	public void visitForceChildren(final TreeVisitor visitor) {
		if (visitor.reverse()) {
			end.visit(visitor);
			start.visit(visitor);
			array.visit(visitor);
		} else {
			array.visit(visitor);
			start.visit(visitor);
			end.visit(visitor);
		}
	}
}
