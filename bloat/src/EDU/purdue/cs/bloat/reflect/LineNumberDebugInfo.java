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

package EDU.purdue.cs.bloat.reflect;

/**
 * LineNumberDebugInfo is used to map a range of instructions to a line number
 * in the original Java source file. An instance of
 * <tt>file.LineNumberTable</tt> contains an array of these guys.
 * 
 * @see EDU.purdue.cs.bloat.file.LineNumberTable
 * 
 * @author Nate Nystrom (<a
 *         href="mailto:nystrom@cs.purdue.edu">nystrom@cs.purdue.edu</a>)
 */
public class LineNumberDebugInfo {
	private int startPC;

	private int lineNumber;

	/**
	 * Constructor.
	 * 
	 * @param startPC
	 *            The start PC of the instructions for this line number.
	 * @param lineNumber
	 *            The line number for this group of instructions.
	 */
	public LineNumberDebugInfo(final int startPC, final int lineNumber) {
		this.startPC = startPC;
		this.lineNumber = lineNumber;
	}

	/**
	 * Get the start PC of the instructions for this line number.
	 * 
	 * @return The start PC.
	 */
	public int startPC() {
		return startPC;
	}

	/**
	 * Get the line number for this group of instructions.
	 * 
	 * @return The line number.
	 */
	public int lineNumber() {
		return lineNumber;
	}

	/**
	 * Convert the attribute to a string.
	 * 
	 * @return A string representation of the attribute.
	 */
	public String toString() {
		return "(line #" + lineNumber + " pc=" + startPC + ")";
	}

	public Object clone() {
		return (new LineNumberDebugInfo(this.startPC, this.lineNumber));
	}
}
