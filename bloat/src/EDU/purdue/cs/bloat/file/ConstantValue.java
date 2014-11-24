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
 * The ConstantValue attribute stores an index into the constant pool that
 * represents constant value. A class's static fields have constant value
 * attributes.
 * 
 * @see Field
 * 
 * @author Nate Nystrom (<a
 *         href="mailto:nystrom@cs.purdue.edu">nystrom@cs.purdue.edu</a>)
 */
public class ConstantValue extends Attribute {
	private int constantValueIndex;

	/**
	 * Creates a new <code>ConstantValue</code> from scratch
	 * 
	 * @param nameIndex
	 *            The index in the constant pool of the UTF8 string
	 *            "ConstantValue"
	 * @param constantValueIndex
	 *            The index in the constant pool of the Constant containing the
	 *            constant value
	 */
	ConstantValue(final int nameIndex, final int length,
			final int constantValueIndex) {
		super(nameIndex, length);
		this.constantValueIndex = constantValueIndex;
	}

	/**
	 * Constructor. Create a ConstantValue attribute from a data stream.
	 * 
	 * @param in
	 *            The data stream of the class file.
	 * @param nameIndex
	 *            The index into the constant pool of the name of the attribute.
	 * @param length
	 *            The length of the attribute, excluding the header.
	 * @exception IOException
	 *                If an error occurs while reading.
	 */
	public ConstantValue(final DataInputStream in, final int nameIndex,
			final int length) throws IOException {
		super(nameIndex, length);
		constantValueIndex = in.readUnsignedShort();
	}

	/**
	 * Write the attribute to a data stream.
	 * 
	 * @param out
	 *            The data stream of the class file.
	 */
	public void writeData(final DataOutputStream out) throws IOException {
		out.writeShort(constantValueIndex);
	}

	/**
	 * Returns the index into the constant pool of the constant value.
	 */
	public int constantValueIndex() {
		return constantValueIndex;
	}

	/**
	 * Set the index into the constant pool of the constant value.
	 */
	public void setConstantValueIndex(final int index) {
		this.constantValueIndex = index;
	}

	/**
	 * Private constructor used for cloning.
	 */
	private ConstantValue(final ConstantValue other) {
		super(other.nameIndex, other.length);

		this.constantValueIndex = other.constantValueIndex;
	}

	public Object clone() {
		return (new ConstantValue(this));
	}

	/**
	 * Converts the attribute to a string.
	 */
	public String toString() {
		return "(constant-value " + constantValueIndex + ")";
	}
}
