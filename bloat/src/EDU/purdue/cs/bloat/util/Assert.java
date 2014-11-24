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

/**
 * Mechanism for making assertions about things in BLOAT. If an assertion fails,
 * an <tt>IllegalArgumentException</tt> is thrown.
 */
public abstract class Assert {
	public static void isTrue(boolean test, final String msg) {
		if (!test) {
			throw new IllegalArgumentException("Assert.isTrue: " + msg);
		}
	}

	public static void isFalse(final boolean test, final String msg) {
		if (test) {
			throw new IllegalArgumentException("Assert.isFalse: " + msg);
		}
	}

	public static void isNotNull(final Object test, final String msg) {
		if (test == null) {
			throw new IllegalArgumentException("Assert.isNotNull: " + msg);
		}
	}

	public static void isNull(final Object test, final String msg) {
		if (test != null) {
			throw new IllegalArgumentException("Assert.isNull: " + msg);
		}
	}

	public static void isTrue(boolean test) {
		if (!test) {
			throw new IllegalArgumentException("Assert.isTrue failed");
		}
	}

	public static void isFalse(final boolean test) {
		if (test) {
			throw new IllegalArgumentException("Assert.isFalse failed");
		}
	}

	public static void isNotNull(final Object test) {
		if (test == null) {
			throw new IllegalArgumentException("Assert.isNotNull failed");
		}
	}

	public static void isNull(final Object test) {
		if (test != null) {
			throw new IllegalArgumentException("Assert.isNull failed");
		}
	}
}
