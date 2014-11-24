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

import EDU.purdue.cs.bloat.editor.*;

/**
 * An expression in which a definition occurs. Each instance has a unique
 * version number associated with it.
 */
public abstract class DefExpr extends Expr {
	Set uses; // Expressions in which the definition is used

	int version; // Which number DefExpr is this?

	static int next = 0; // Total number of DefExprs

	/**
	 * Constructor.
	 * 
	 * @param type
	 *            The Type (descriptor) of this expression
	 */
	public DefExpr(final Type type) {
		super(type);
		uses = new HashSet();
		version = DefExpr.next++;
	}

	/**
	 * Clean up this expression. Notify all the expressions that use this
	 * definition that it is no longer their defining expression.
	 */
	public void cleanupOnly() {
		super.cleanupOnly();

		final List a = new ArrayList(uses);

		uses.clear();

		final Iterator e = a.iterator();

		while (e.hasNext()) {
			final Expr use = (Expr) e.next();
			use.setDef(null);
		}
	}

	/**
	 * Returns Number DefExpr this is. This is also the SSA version number of
	 * the expression that this <tt>DefExpr</tt> defines.
	 */
	public int version() {
		return version;
	}

	/**
	 * Determines whether or not this <tt>DefExpr</tt> defines a local
	 * variable in its parent.
	 * 
	 * @see Assign#defs
	 */
	public boolean isDef() {
		if (parent instanceof Assign) {
			final DefExpr[] defs = ((Assign) parent).defs();

			if (defs != null) {
				for (int i = 0; i < defs.length; i++) {
					if (defs[i] == this) {
						return true;
					}
				}
			}
		}

		return false;
	}

	/**
	 * Returns the <tt>Expr</tt>s in which the variable defined by this are
	 * used.
	 */
	public Collection uses() {
		return new HashSet(uses);
	}

	public boolean hasUse(final Expr use) {
		return uses.contains(use);
	}

	protected void addUse(final Expr use) {
		uses.add(use);
	}

	protected void removeUse(final Expr use) {
		uses.remove(use);
	}
}
