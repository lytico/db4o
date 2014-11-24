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

package EDU.purdue.cs.bloat.tree;

import java.util.*;

/**
 * Type0Visitor searches up the tree, starting at a LocalExpr, looking for an
 * earlier instance of the same definition of that LocalExpr in a Type 0
 * relation.
 * 
 * @author Thomas VanDrunen
 */
public class Type0Visitor extends AscendVisitor {

	boolean found; // have we found an earlier occurence

	static boolean DEBUG = false;

	public Type0Visitor(final Hashtable defInfoMap, final Hashtable useInfoMap) {
		super(defInfoMap, useInfoMap);
	}

	public boolean search(final LocalExpr start) {
		this.start = start;
		previous = this.start;
		found = false;
		this.start.parent().visit(this);
		return found;
	}

	public void check(final Node node) {

		if (node instanceof ExprStmt) {
			check(((ExprStmt) node).expr()); // might be something we want
		}

		// the next conditional should be true if the node is a
		// Stmt but not an ExprStmt OR if it is an ExprStmt but the
		// above thing didn't find a match

		if (!found && (node instanceof Stmt)) {
			found = (new Type0DownVisitor(useInfoMap, defInfoMap)).search(node,
					start);
		}

		else if (node instanceof StoreExpr) { // if it's a StoreExpr, we need
			final StoreExpr n = (StoreExpr) node; // to see if the target
													// matches

			if (((n.target() instanceof LocalExpr // this funny condition))
			&& ((n.expr() instanceof LocalExpr // weeds out moves between))
			&& (((LocalExpr) n.target()).index() // identically colored
			== ((LocalExpr) n.expr()).index())))))) {
				; // local vars
			} else {
				check(n.target());
			}
		}

		else if (node instanceof InitStmt) { // if it's an InitStmt,
			final LocalExpr[] targets = ((InitStmt) node).targets(); // check
																		// the
																		// last
			if (targets.length > 0) {
				check(targets[targets.length - 1]);
			}
		}

		// if it's a LocalExpr...
		else if (node instanceof LocalExpr) {
			if (((((LocalExpr) node).index() == start.index() // compare
																// index))
			&& (((LocalExpr) node).def() == start.def())))) { // and def
				// we've found a match
				// update information

				((UseInformation) useInfoMap.get(start)).type = 0;
				((UseInformation) useInfoMap.get(node)).type0s++;
				found = true;
			}
		}

	}

}

class Type0DownVisitor extends DescendVisitor {

	public Type0DownVisitor(final Hashtable useInfoMap,
			final Hashtable defInfoMap) {
		super(useInfoMap, defInfoMap);
	}

	public void visitLocalExpr(final LocalExpr expr) {
		if ((expr.index() == start.index()) && (expr.def() == start.def())) {
			// we've found a match
			// update information
			((UseInformation) useInfoMap.get(start)).type = 0;
			final UseInformation ui = (UseInformation) useInfoMap.get(expr);
			ui.type0s++;
			if (exchangeFactor == 1) {
				ui.type0_x1s++;

			}
			if (exchangeFactor == 2) {
				ui.type0_x2s++;
			}

			found = true;
		}
	}

}
