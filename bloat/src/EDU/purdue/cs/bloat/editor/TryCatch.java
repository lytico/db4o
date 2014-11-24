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
 * TryCatch holds the labels for the start and end of a protected block and the
 * beginning of a catch block and the type of the exception to catch.
 * 
 * @author Nate Nystrom (<a
 *         href="mailto:nystrom@cs.purdue.edu">nystrom@cs.purdue.edu</a>)
 */
public class TryCatch {
	private Label start;

	private Label end;

	private Label handler;

	private Type type;

	/**
	 * Constructor.
	 * 
	 * @param start
	 *            The start label of the protected block.
	 * @param end
	 *            The label of the instruction after the end of the protected
	 *            block.
	 * @param handler
	 *            The start label of the exception handler.
	 * @param type
	 *            The type of exception to catch.
	 */
	public TryCatch(final Label start, final Label end, final Label handler,
			final Type type) {
		this.start = start;
		this.end = end;
		this.handler = handler;
		this.type = type;
	}

	/**
	 * Get the start label of the protected block.
	 * 
	 * @return The start label.
	 */
	public Label start() {
		return start;
	}

	/**
	 * Get the end label of the protected block.
	 * 
	 * @return The end label.
	 */
	public Label end() {
		return end;
	}

	/**
	 * Get the start label of the catch block.
	 * 
	 * @return The handler label.
	 */
	public Label handler() {
		return handler;
	}

	/**
	 * Set the start label of the catch block.
	 */
	public void setHandler(final Label handler) {
		this.handler = handler;
	}

	/**
	 * Get the type of the exception to catch.
	 * 
	 * @return The type of the exception to catch.
	 */
	public Type type() {
		return type;
	}

	public String toString() {
		return "try " + start + ".." + end + " catch (" + type + ") " + handler;
	}
}
