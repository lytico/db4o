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
 * Methods and fields are described by their name and type descriptor.
 * NameAndType represents exactly that.
 * 
 * @author Nate Nystrom (<a
 *         href="mailto:nystrom@cs.purdue.edu">nystrom@cs.purdue.edu</a>)
 */
public class NameAndType {
	private String name;

	private Type type;

	/**
	 * Constructor.
	 */
	public NameAndType(final String name, final Type type) {
		this.name = name;
		this.type = type;
	}

	/**
	 * Returns the name.
	 */
	public String name() {
		return name;
	}

	/**
	 * Returns the type.
	 */
	public Type type() {
		return type;
	}

	/**
	 * Returns a string representation of the name and type.
	 */
	public String toString() {
		return "<NameandType " + name + " " + type + ">";
	}

	/**
	 * Check if an object is equal to this name and type.
	 * 
	 * @param obj
	 *            The object to compare against.
	 * @return <tt>true</tt> if equal
	 */
	public boolean equals(final Object obj) {
		return (obj instanceof NameAndType)
				&& ((NameAndType) obj).name.equals(name)
				&& ((NameAndType) obj).type.equals(type);
	}

	/**
	 * Returns a hash of the name and type.
	 */
	public int hashCode() {
		return name.hashCode() ^ type.hashCode();
	}
}
