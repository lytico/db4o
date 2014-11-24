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

package EDU.purdue.cs.bloat.util;

import java.util.*;

/**
 * ResizableArrayList is the same as ArrayList except that ensureSize not only
 * increases the size of the array (super.ensureCapacity), but it also fills the
 * empty space with null. This way, the size method will return the length of
 * the array and not just the number of elements in it. I guess.
 */
public class ResizeableArrayList extends ArrayList implements List, Cloneable,
		java.io.Serializable {
	/**
	 * This constructor is no longer supported in JDK1.2 public
	 * ResizeableArrayList(int initialCapacity, int capacityIncrement) {
	 * super(initialCapacity, capacityIncrement); }
	 */
	public ResizeableArrayList(final int initialCapacity) {
		super(initialCapacity);
	}

	public ResizeableArrayList() {
		super();
	}

	public ResizeableArrayList(final Collection c) {
		super(c);
	}

	public void ensureSize(final int size) {
		ensureCapacity(size);

		while (size() < size) {
			add(null);
		}
	}

	public Object clone() {
		return super.clone();
	}
}
