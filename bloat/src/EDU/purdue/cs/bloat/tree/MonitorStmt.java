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
 * MonitorStmt represents the <tt>monitorenter</tt> and <tt>monitorexit</tt>
 * opcodes, which gain and release ownership of the monitor associated with a
 * given object.
 */
public class MonitorStmt extends Stmt {
	public static final int ENTER = 0;

	public static final int EXIT = 1;

	int kind;

	Expr object;

	/**
	 * Constructor.
	 * 
	 * @param kind
	 *            The kind of monitor statement: ENTER or EXIT.
	 * @param object
	 *            The expression (object) whose monitor is being entered or
	 *            exited.
	 */
	public MonitorStmt(final int kind, final Expr object) {
		this.kind = kind;
		this.object = object;

		object.setParent(this);
	}

	public Expr object() {
		return object;
	}

	public int kind() {
		return kind;
	}

	public void visitForceChildren(final TreeVisitor visitor) {
		object.visit(visitor);
	}

	public void visit(final TreeVisitor visitor) {
		visitor.visitMonitorStmt(this);
	}

	public Object clone() {
		return copyInto(new MonitorStmt(kind, (Expr) object.clone()));
	}
}
