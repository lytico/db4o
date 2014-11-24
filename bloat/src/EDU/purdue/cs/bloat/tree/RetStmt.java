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

import EDU.purdue.cs.bloat.cfg.*;
import EDU.purdue.cs.bloat.editor.*;

/**
 * RetStmt represents the <i>ret</i> opcode which returns from a subroutine.
 * Recall that when a subroutine returns, the <i>ret</i> opcode's argument
 * specifies a local variable that stores the return address of
 */
public class RetStmt extends JumpStmt {
	// ret

	Subroutine sub; // Subroutine from which to return.

	/**
	 * Constructor.
	 * 
	 * @param sub
	 *            The subroutine in which the return statement resides. That is,
	 *            from where the program control is returning.
	 * 
	 * @see Tree#addInstruction(Instruction, Subroutine)
	 */
	public RetStmt(final Subroutine sub) {
		this.sub = sub;
	}

	public void visitForceChildren(final TreeVisitor visitor) {
	}

	public void visit(final TreeVisitor visitor) {
		visitor.visitRetStmt(this);
	}

	public Subroutine sub() {
		return sub;
	}

	public Object clone() {
		return copyInto(new RetStmt(sub));
	}
}
