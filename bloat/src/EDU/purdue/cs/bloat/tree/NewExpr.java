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
 * NewExpr represents the <tt>new</tt> opcode that creates a new object of a
 * specified type.
 */
public class NewExpr extends Expr {
	// new
	Type objectType;

	/**
	 * Constructor.
	 * 
	 * @param objectType
	 *            The type of the object to create.
	 * @param type
	 *            The type of this expression.
	 */
	public NewExpr(final Type objectType, final Type type) {
		super(type);
		this.objectType = objectType;
	}

	/**
	 * Returns the <tt>Type</tt> of the object being created.
	 */
	public Type objectType() {
		return objectType;
	}

	public void visitForceChildren(final TreeVisitor visitor) {
	}

	public void visit(final TreeVisitor visitor) {
		visitor.visitNewExpr(this);
	}

	public int exprHashCode() {
		return 16 + objectType.hashCode();
	}

	public boolean equalsExpr(final Expr other) {
		return false;
	}

	public Object clone() {
		return copyInto(new NewExpr(objectType, type));
	}
}
