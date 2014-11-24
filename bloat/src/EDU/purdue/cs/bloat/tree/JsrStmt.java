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

/**
 * JsrStmt represents a <i>jsr</i> instruction that jumps to a subroutine.
 * Recall that a subroutine is used to implement the finally cause in exception
 * handlers. The <i>ret</i> instruction is used to return from a subroutine.
 * 
 * @see RetStmt
 * @see Subroutine
 */
public class JsrStmt extends JumpStmt {
	Subroutine sub; // Subroutine to which to jump

	Block follow; // Basic Block to execute upon returning

	// from the subroutine

	/**
	 * Constructor.
	 * 
	 * @param sub
	 *            Subroutine that this statement jumps to.
	 * @param follow
	 *            Basic Block following the jump statement.
	 */
	public JsrStmt(final Subroutine sub, final Block follow) {
		this.sub = sub;
		this.follow = follow;
	}

	public void setFollow(final Block follow) {
		this.follow = follow;
	}

	public Block follow() {
		return follow;
	}

	public Subroutine sub() {
		return sub;
	}

	public void visitForceChildren(final TreeVisitor visitor) {
	}

	public void visit(final TreeVisitor visitor) {
		visitor.visitJsrStmt(this);
	}

	public Object clone() {
		return copyInto(new JsrStmt(sub, follow));
	}
}
