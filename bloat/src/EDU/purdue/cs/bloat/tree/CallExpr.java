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
 * <tt>CallExpr</tt> is a superclass of expressions that represent the
 * invocation of a method. It consists of an array of <tt>Expr</tt> that
 * represent the arguments to a method and a <tt>MemberRef</tt> that
 * represents the method itself.
 * 
 * @see CallMethodExpr
 * @see CallStaticExpr
 */
public abstract class CallExpr extends Expr {
	Expr[] params; // The parameters to the method

	MemberRef method; // The method to be invoked

	public int voltaPos; // used for placing swaps and stuff

	/**
	 * Constructor.
	 * 
	 * @param params
	 *            Parameters to the method. Note that these parameters do not
	 *            contain parameter 0, the "this" pointer.
	 * @param method
	 *            The method that is to be invoked.
	 * @param type
	 *            The type of this expression (i.e. the return type of the
	 *            method being called).
	 */
	public CallExpr(final Expr[] params, final MemberRef method, final Type type) {
		super(type);
		this.params = params;
		this.method = method;

		for (int i = 0; i < params.length; i++) {
			params[i].setParent(this);
		}
	}

	public MemberRef method() {
		return method;
	}

	public Expr[] params() {
		return params;
	}
}
