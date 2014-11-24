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
 * FieldExpr represents the <i>getfield</i> opcode which fetches a field from
 * an object.
 * 
 * @see MemberRef
 */
public class FieldExpr extends MemRefExpr {
	// getfield

	Expr object; // The object whose field we are fetching

	MemberRef field; // The field to fetch

	/**
	 * Constructor.
	 * 
	 * @param object The object (result of an Expr) whose field is to be
	 *        fetched.
	 * @param field
	 *            The field of object to be fetched.
	 * @param type
	 *            The type of this expression.
	 */
	public FieldExpr(final Expr object, final MemberRef field, final Type type) {
		super(type);
		this.object = object;
		this.field = field;

		object.setParent(this);
	}

	public Expr object() {
		return object;
	}

	public MemberRef field() {
		return field;
	}

	public void visitForceChildren(final TreeVisitor visitor) {
		if (visitor.reverse()) {
			object.visit(visitor);
		} else {
			object.visit(visitor);
		}
	}

	public void visit(final TreeVisitor visitor) {
		visitor.visitFieldExpr(this);
	}

	public int exprHashCode() {
		return 11 + object.exprHashCode() ^ type.simple().hashCode();
	}

	public boolean equalsExpr(final Expr other) {
		return (other != null) && (other instanceof FieldExpr)
				&& ((FieldExpr) other).field.equals(field)
				&& ((FieldExpr) other).object.equalsExpr(object);
	}

	public Object clone() {
		return copyInto(new FieldExpr((Expr) object.clone(), field, type));
	}
}
