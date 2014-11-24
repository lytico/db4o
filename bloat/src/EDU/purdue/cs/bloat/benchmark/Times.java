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

package EDU.purdue.cs.bloat.benchmark;

/**
 * This class allows Java to access the information obtained by the UNIX system
 * call <tt>times</tt>.
 */
public class Times {
	static {
		// Load native code from libbenchmark.so
		System.loadLibrary("times");
	}

	static float userTime;

	static float systemTime;

	/**
	 * Takes a "snapshot" of the system. Reads various items from the result of
	 * <tt>times</tt>.
	 * 
	 * @return <tt>true</tt> if everything is successful
	 */
	public static native boolean snapshot();

	/**
	 * Returns the user time used by this process in seconds.
	 */
	public static float userTime() {
		return (Times.userTime);
	}

	/**
	 * Returns the system time used by this process in seconds.
	 */
	public static float systemTime() {
		return (Times.systemTime);
	}

	/**
	 * Test program.
	 */
	public static void main(final String[] args) throws Exception {
		System.out.println("Starting Test");

		if (Times.snapshot() == false) {
			System.err.println("Error during snapshot");
			System.exit(1);
		}

		System.out.println("System time: " + Times.systemTime());
		System.out.println("User time: " + Times.userTime());

		System.out.println("Ending Test");
	}

}
