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
 * Modifiers is an interface containing constants used as modifiers of classes,
 * fields, and methods.
 * 
 * @author Nate Nystrom (<a
 *         href="mailto:nystrom@cs.purdue.edu">nystrom@cs.purdue.edu</a>)
 */
public interface Modifiers {
	/**
	 * The class, field, or method is declared public.
	 */
	public static final short PUBLIC = 0x0001;

	/**
	 * The class, field, or method is declared private.
	 */
	public static final short PRIVATE = 0x0002;

	/**
	 * The class, field, or method is declared protected.
	 */
	public static final short PROTECTED = 0x0004;

	/**
	 * The field or method is declared static.
	 */
	public static final short STATIC = 0x0008;

	/**
	 * The class, field, or method is declared final.
	 */
	public static final short FINAL = 0x0010;

	/**
	 * The class calls methods in the superclass.
	 */
	public static final short SUPER = 0x0020;

	/**
	 * The method is declared synchronized.
	 */
	public static final short SYNCHRONIZED = 0x0020;

	/**
	 * The field is declared volatile.
	 */
	public static final short VOLATILE = 0x0040;

	/**
	 * The field is declared transient.
	 */
	public static final short TRANSIENT = 0x0080;

	/**
	 * The method is declared native.
	 */
	public static final short NATIVE = 0x0100;

	/**
	 * The class is an interface.
	 */
	public static final short INTERFACE = 0x0200;

	/**
	 * The class or method is declared abstract.
	 */
	public static final short ABSTRACT = 0x0400;
}
