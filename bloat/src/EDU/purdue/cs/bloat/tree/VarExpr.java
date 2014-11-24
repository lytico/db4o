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
 * VarExpr represents an expression that accesses a local variable or a variable
 * on the stack.
 * 
 * @see StackExpr
 * @see LocalExpr
 * 
 * @see DefExpr
 */
public abstract class VarExpr extends MemExpr {
	int index;

	/**
	 * Constructor.
	 * 
	 * @param index
	 *            Index giving location of expression. For instance, the number
	 *            local variable represented or the position of the stack
	 *            variable represented.
	 * @param type
	 *            Type (descriptor) of this expression.
	 */
	public VarExpr(final int index, final Type type) {
		super(type);
		this.index = index;
	}

	public void setIndex(final int index) {
		this.index = index;
	}

	public int index() {
		return index;
	}

	/**
	 * Returns the expression that defines this expression.
	 */
	public DefExpr def() {
		if (isDef()) {
			return this;
		}

		return super.def();
	}
}
