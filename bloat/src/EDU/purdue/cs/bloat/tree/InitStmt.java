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

/**
 * <tt>InitStmt</tt> groups together the initialization of local variables (<tt>LocalExpr</tt>).
 * 
 * @see LocalExpr
 * @see Tree#initLocals
 */
public class InitStmt extends Stmt implements Assign {
	LocalExpr[] targets;

	/**
	 * Constructor.
	 * 
	 * @param targets
	 *            The instances of LocalExpr that are to be initialized.
	 */
	public InitStmt(final LocalExpr[] targets) {
		this.targets = new LocalExpr[targets.length];

		for (int i = 0; i < targets.length; i++) {
			this.targets[i] = targets[i];
			this.targets[i].setParent(this);
		}
	}

	/**
	 * Returns the local variables (<tt>LocalExpr</tt>s) initialized by this
	 * <tt>InitStmt</tt>.
	 */
	public LocalExpr[] targets() {
		return targets;
	}

	/**
	 * Returns the local variables (<tt>LocalExpr</tt>s) defined by this
	 * <tt>InitStmt</tt>. These are the same local variables that are the
	 * targets of the <tt>InitStmt</tt>.
	 */
	public DefExpr[] defs() {
		return targets;
	}

	public void visitForceChildren(final TreeVisitor visitor) {
		for (int i = 0; i < targets.length; i++) {
			targets[i].visit(visitor);
		}
	}

	public void visit(final TreeVisitor visitor) {
		visitor.visitInitStmt(this);
	}

	public Object clone() {
		final LocalExpr[] t = new LocalExpr[targets.length];

		for (int i = 0; i < targets.length; i++) {
			t[i] = (LocalExpr) targets[i].clone();
		}

		return copyInto(new InitStmt(t));
	}
}
