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
 * CallMethodExpr represents the invocation of an object's method. In addition
 * to knowing what method is being called and its parameters, it also knows what
 * "kind" of method call it is (<tt>VIRTUAL</tt>, <tt>NONVIRTUAL</tt>, or
 * <tt>INTERFACE</tt>) and the object that is the reciever of this method
 * call.
 * 
 * @see CallStaticExpr
 */
public class CallMethodExpr extends CallExpr {
	// Different kinds of methods to call...
	public static final int VIRTUAL = 0; // invokevirtual

	public static final int NONVIRTUAL = 1; // invokespecial

	public static final int INTERFACE = 2; // invokeinterface

	Expr receiver;

	int kind;

	/**
	 * Constructor.
	 * 
	 * @param kind
	 *            The kind (VIRTUAL, NONVIRTUAL, or INTERFACE) of method that is
	 *            being called.
	 * @param receiver
	 *            The expression (object) whose method is being called.
	 * @param params
	 *            Parameters to the method.
	 * @param method
	 *            The method being called.
	 * @param type
	 *            The type of this expression.
	 */
	public CallMethodExpr(final int kind, final Expr receiver,
			final Expr[] params, final MemberRef method, final Type type) {
		super(params, method, type);
		this.receiver = receiver;
		this.kind = kind;

		receiver.setParent(this);
	}

	public int kind() {
		return kind;
	}

	public Expr receiver() {
		return receiver;
	}

	public void visitForceChildren(final TreeVisitor visitor) {
		if (visitor.reverse()) {
			for (int i = params.length - 1; i >= 0; i--) {
				params[i].visit(visitor);
			}

			receiver.visit(visitor);
		} else {
			receiver.visit(visitor);

			for (int i = 0; i < params.length; i++) {
				params[i].visit(visitor);
			}
		}
	}

	public void visit(final TreeVisitor visitor) {
		visitor.visitCallMethodExpr(this);
	}

	public int exprHashCode() {
		int v = 5 + kind ^ receiver.exprHashCode();

		for (int i = 0; i < params.length; i++) {
			v ^= params[i].exprHashCode();
		}

		return v;
	}

	public boolean equalsExpr(final Expr other) {
		return false;
	}

	public Object clone() {
		final Expr[] p = new Expr[params.length];

		for (int i = 0; i < params.length; i++) {
			p[i] = (Expr) params[i].clone();
		}

		return copyInto(new CallMethodExpr(kind, (Expr) receiver.clone(), p,
				method, type));
	}
}
