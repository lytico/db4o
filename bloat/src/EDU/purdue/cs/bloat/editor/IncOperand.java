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

package EDU.purdue.cs.bloat.editor;

/**
 * IncOperand encapsulates the operands to the iinc instruction. It is necessary
 * because the <tt>iinc</tt> has two operands: a local variable and an integer
 * by which to increment the local variable.
 * 
 * @author Nate Nystrom (<a
 *         href="mailto:nystrom@cs.purdue.edu">nystrom@cs.purdue.edu</a>)
 */
public class IncOperand {
	private LocalVariable var;

	private int incr;

	/**
	 * Constructor.
	 * 
	 * @param var
	 *            The local variable to increment.
	 * @param incr
	 *            The amount to increment by.
	 */
	public IncOperand(final LocalVariable var, final int incr) {
		this.var = var;
		this.incr = incr;
	}

	/**
	 * Get the local variable to increment.
	 * 
	 * @return The local variable to increment.
	 */
	public LocalVariable var() {
		return var;
	}

	/**
	 * Get the amount to increment by.
	 * 
	 * @return The amount to increment by.
	 */
	public int incr() {
		return incr;
	}

	/**
	 * Convert the operand to a string.
	 * 
	 * @return A string representation of the operand.
	 */
	public String toString() {
		return "" + var + " by " + incr;
	}
}
