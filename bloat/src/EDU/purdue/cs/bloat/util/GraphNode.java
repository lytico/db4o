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
 * GraphNode represents a node in a Graph. Each node has a set of predacessors
 * and successors associated with it as well as a pre-order and post-order
 * traversal index. This information is maintained by the Graph in which the
 * GraphNode resides.
 * 
 * @see Graph
 */
public abstract class GraphNode {
	protected HashSet succs;

	protected HashSet preds;

	protected int preIndex;

	protected int postIndex;

	/**
	 * Constructor.
	 */
	public GraphNode() {
		succs = new HashSet();
		preds = new HashSet();
		preIndex = -1;
		postIndex = -1;
	}

	/**
	 * @return This node's index in a pre-order traversal of the Graph that
	 *         contains it.
	 */
	int preOrderIndex() {
		return preIndex;
	}

	/**
	 * @return This node's index in a post-order traversal of the Graph that
	 *         contains it.
	 */
	int postOrderIndex() {
		return postIndex;
	}

	void setPreOrderIndex(final int index) {
		preIndex = index;
	}

	void setPostOrderIndex(final int index) {
		postIndex = index;
	}

	/**
	 * Returns the successor (or children) nodes of this GraphNode.
	 */
	protected Collection succs() {
		return succs;
	}

	/**
	 * Returns the predacessor (or parent) nodes of this GraphNode.
	 */
	protected Collection preds() {
		return preds;
	}
}
