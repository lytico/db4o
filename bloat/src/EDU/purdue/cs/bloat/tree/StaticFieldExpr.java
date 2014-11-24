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
 * StaticFieldExpr represents the <tt>getstatic</tt> opcode which gets a
 * static (class) field from a class.
 */
public class StaticFieldExpr extends MemRefExpr {
	// getstatic

	MemberRef field;

	/**
	 * Constructor.
	 * 
	 * @param field
	 *            The field to access.
	 * @param type
	 *            The type of this expression.
	 */
	public StaticFieldExpr(final MemberRef field, final Type type) {
		super(type);
		this.field = field;
	}

	public MemberRef field() {
		return field;
	}

	public void visitForceChildren(final TreeVisitor visitor) {
	}

	public void visit(final TreeVisitor visitor) {
		visitor.visitStaticFieldExpr(this);
	}

	public int exprHashCode() {
		return 21 + field.hashCode() ^ type.simple().hashCode();
	}

	public boolean equalsExpr(final Expr other) {
		return (other != null) && (other instanceof StaticFieldExpr)
				&& ((StaticFieldExpr) other).field.equals(field);
	}

	public Object clone() {
		return copyInto(new StaticFieldExpr(field, type));
	}
}
