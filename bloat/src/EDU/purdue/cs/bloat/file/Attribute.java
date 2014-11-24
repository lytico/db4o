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

package EDU.purdue.cs.bloat.file;

import java.io.*;

/**
 * Attribute is an abstract class for an attribute defined for a method, field,
 * or class. An attribute consists of its name (represented as an index into the
 * constant pool) and its length. Attribute is extended to represent a constant
 * value, code, exceptions, etc.
 * 
 * @see Code
 * @see ConstantValue
 * @see Exceptions
 * 
 * @author Nate Nystrom (<a
 *         href="mailto:nystrom@cs.purdue.edu">nystrom@cs.purdue.edu</a>)
 */
public abstract class Attribute {
	protected int nameIndex;

	protected int length;

	/**
	 * Constructor.
	 * 
	 * @param nameIndex
	 *            The index into the constant pool of the name of the attribute.
	 * @param length
	 *            The length of the attribute, excluding the header.
	 */
	public Attribute(final int nameIndex, final int length) {
		this.nameIndex = nameIndex;
		this.length = length;
	}

	/**
	 * Write the attribute to a data stream.
	 * 
	 * @param out
	 *            The data stream of the class file.
	 */
	public abstract void writeData(DataOutputStream out) throws IOException;

	/**
	 * Returns a string representation of the attribute.
	 */
	public String toString() {
		return "(attribute " + nameIndex + " " + length + ")";
	}

	/**
	 * Returns the index into the constant pool of the name of the attribute.
	 */
	public int nameIndex() {
		return nameIndex;
	}

	/**
	 * Returns the length of the attribute, excluding the header.
	 */
	public int length() {
		return length;
	}

	public Object clone() {
		throw new UnsupportedOperationException("Cannot clone Attribute! "
				+ " (subclass: " + this.getClass() + ")");
	}
}
