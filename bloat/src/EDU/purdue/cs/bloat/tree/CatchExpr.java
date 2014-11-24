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

import EDU.purdue.cs.bloat.cfg.*;
import EDU.purdue.cs.bloat.editor.*;

/**
 * CatchExpr represents an expression that catches an exception. A CatchExpr is
 * used when evaluating a method's try-catch blocks when a control flow graph is
 * constructed.
 * 
 * @see TryCatch
 * @see FlowGraph#FlowGraph(MethodEditor)
 * @see MethodEditor
 */
public class CatchExpr extends Expr {
	Type catchType;

	/**
	 * Constructor.
	 * 
	 * @param catchType
	 *            The type of the exception that is being caught.
	 * @param type
	 *            The type of this expression.
	 */
	public CatchExpr(final Type catchType, final Type type) {
		super(type);
		this.catchType = catchType;
	}

	public void visitForceChildren(final TreeVisitor visitor) {
	}

	public void visit(final TreeVisitor visitor) {
		visitor.visitCatchExpr(this);
	}

	public Type catchType() {
		return catchType;
	}

	public int exprHashCode() {
		return 8 + type.simple().hashCode() ^ catchType.hashCode();
	}

	public boolean equalsExpr(final Expr other) {
		if (other instanceof CatchExpr) {
			final CatchExpr c = (CatchExpr) other;

			if (catchType != null) {
				return catchType.equals(c.catchType);
			} else {
				return c.catchType == null;
			}
		}

		return false;
	}

	public Object clone() {
		return copyInto(new CatchExpr(catchType, type));
	}
}
