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
 * <tt>MultiArrayOperand</tt> encapsulates the operands to the
 * <tt>multianewarray</tt> instruction. Each <tt>MultiArrayOperand</tt>
 * contains the type descriptor of the new multidimensional array the
 * instruction creates, as well as the number of dimensions in the array.
 * 
 * @author Nate Nystrom (<a
 *         href="mailto:nystrom@cs.purdue.edu">nystrom@cs.purdue.edu</a>)
 */
public class MultiArrayOperand {
	private Type type;

	private int dim;

	/**
	 * Constructor.
	 * 
	 * @param type
	 *            The element type of the array.
	 * @param dim
	 *            The number of dimensions of the array.
	 */
	public MultiArrayOperand(final Type type, final int dim) {
		this.type = type;
		this.dim = dim;
	}

	/**
	 * Get the element type of the array.
	 * 
	 * @return The element type of the array.
	 */
	public Type type() {
		return type;
	}

	/**
	 * Get the number of dimensions of the array.
	 * 
	 * @return The number of dimensions of the array.
	 */
	public int dimensions() {
		return dim;
	}

	/**
	 * Convert the operand to a string.
	 * 
	 * @return A string representation of the operand.
	 */
	public String toString() {
		return type + " x " + dim;
	}
}
