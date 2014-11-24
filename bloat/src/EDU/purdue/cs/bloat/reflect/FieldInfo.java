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
 * FieldInfo grants access to a field's name and type (represented as indices
 * into the constant pool), as well as its modifiers. FieldInfo is implemented
 * in <tt>file.Field</tt>.
 * 
 * @see EDU.purdue.cs.bloat.file.Field
 * 
 * @author Nate Nystrom (<a
 *         href="mailto:nystrom@cs.purdue.edu">nystrom@cs.purdue.edu</a>)
 */
public interface FieldInfo {
	/**
	 * Get the class which declared the field.
	 * 
	 * @return The ClassInfo of the class which declared the field.
	 */
	public ClassInfo declaringClass();

	/**
	 * Get the index into the constant pool of the name of the field.
	 * 
	 * @return The index into the constant pool of the name of the field.
	 */
	public int nameIndex();

	/**
	 * Get the index into the constant pool of the type of the field.
	 * 
	 * @return The index into the constant pool of the type of the field.
	 */
	public int typeIndex();

	/**
	 * Set the index into the constant pool of the name of the field.
	 * 
	 * @param index
	 *            The index into the constant pool of the name of the field.
	 */
	public void setNameIndex(int index);

	/**
	 * Set the index into the constant pool of the type of the field.
	 * 
	 * @param index
	 *            The index into the constant pool of the type of the field.
	 */
	public void setTypeIndex(int index);

	/**
	 * Set the modifiers of the field. The values correspond to the constants in
	 * the Modifiers class.
	 * 
	 * @param modifiers
	 *            A bit vector of modifier flags for the field.
	 * @see Modifiers
	 */
	public void setModifiers(int modifiers);

	/**
	 * Get the modifiers of the field. The values correspond to the constants in
	 * the Modifiers class.
	 * 
	 * @return A bit vector of modifier flags for the field.
	 * @see Modifiers
	 */
	public int modifiers();

	/**
	 * Get the index into the constant pool of the field's constant value, if
	 * any. Returns 0 if the field does not have a constant value.
	 * 
	 * @see ClassInfo#constants
	 */
	public int constantValue();

	/**
	 * Set the index into the constant pool of the field's constant value.
	 * 
	 * @see ClassInfo#constants
	 */
	public void setConstantValue(int index);
}
