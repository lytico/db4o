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
 * ConstantExpr represents a constant expression. It is used when opcodes <i>ldc</i>,
 * <i>iinc</i>, and <i>getstatic</i> are visited. It value must be an Integer,
 * Long, Float, Double, or String.
 */
public class ConstantExpr extends Expr implements LeafExpr {
	// ldc

	Object value; // The operand to the ldc instruction

	/**
	 * Constructor.
	 * 
	 * @param value
	 *            The operand of the ldc instruction
	 * @param type
	 *            The Type of the operand
	 */
	public ConstantExpr(final Object value, final Type type) {
		super(type);
		this.value = value;
	}

	/**
	 * @return The operand of the ldc instruction
	 */
	public Object value() {
		return value;
	}

	public void visitForceChildren(final TreeVisitor visitor) {
	}

	public void visit(final TreeVisitor visitor) {
		visitor.visitConstantExpr(this);
	}

	/**
	 * @return A hash code for this expression.
	 */
	public int exprHashCode() {
		if (value != null) {
			return 10 + value.hashCode();
		}

		return 10;
	}

	/**
	 * Compare this ConstantExpr to another Expr.
	 * 
	 * @param other
	 *            An Expr to compare this to.
	 * 
	 * @return True, if this and other are the same (that is, have the same
	 *         contents).
	 */
	public boolean equalsExpr(final Expr other) {
		if (!(other instanceof ConstantExpr)) {
			return false;
		}

		if (value == null) {
			return ((ConstantExpr) other).value == null;
		}

		if (((ConstantExpr) other).value == null) {
			return false;
		}

		return ((ConstantExpr) other).value.equals(value);
	}

	public Object clone() {
		return copyInto(new ConstantExpr(value, type));
	}
}
