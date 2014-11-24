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

package EDU.purdue.cs.bloat.cfg;

import java.util.*;

import EDU.purdue.cs.bloat.editor.*;

/**
 * <tt>Handler</tt> represents a try-catch block. It containes a set of
 * protected <tt>Block</tt>s (the "try" blocks), a catch <tt>Block</tt>,
 * and the <tt>Type</tt> of exception that is caught by the catch block.
 * 
 * @see Block
 * @see EDU.purdue.cs.bloat.reflect.Catch
 * @see EDU.purdue.cs.bloat.editor.TryCatch
 */
public class Handler {
	Set protectedBlocks;

	Block catchBlock;

	Type type;

	/**
	 * Constructor.
	 * 
	 * @param catchBlock
	 *            The block of code that handles an exception
	 * @param type
	 *            The type of exception that is thrown
	 */
	public Handler(final Block catchBlock, final Type type) {
		this.protectedBlocks = new HashSet();
		this.catchBlock = catchBlock;
		this.type = type;
	}

	/**
	 * Returns a <tt>Collection</tt> of the "try" blocks.
	 */
	public Collection protectedBlocks() {
		return protectedBlocks;
	}

	public void setCatchBlock(final Block block) {
		catchBlock = block;
	}

	public Block catchBlock() {
		return catchBlock;
	}

	public Type catchType() {
		return type;
	}

	public String toString() {
		return "try -> catch (" + type + ") " + catchBlock;
	}
}
