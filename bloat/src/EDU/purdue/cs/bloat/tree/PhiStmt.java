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

import java.util.*;

/**
 * A PhiStmt is inserted into a CFG in Single Static Assignment for. It is used
 * to "merge" uses of the same variable in different basic blocks.
 * 
 * @see PhiJoinStmt
 * @see PhiCatchStmt
 */
public abstract class PhiStmt extends Stmt implements Assign {
	VarExpr target; // The variable into which the Phi statement assigns

	/**
	 * Constructor.
	 * 
	 * @param target
	 *            A stack expression or local variable that is the target of
	 *            this phi-statement.
	 */
	public PhiStmt(final VarExpr target) {
		this.target = target;
		target.setParent(this);
	}

	public VarExpr target() {
		return target;
	}

	/**
	 * Return the expressions (variables) defined by this PhiStmt. In this case,
	 * only the target is defined.
	 */
	public DefExpr[] defs() {
		return new DefExpr[] { target };
	}

	public abstract Collection operands();

	public Object clone() {
		throw new RuntimeException();
	}
}
