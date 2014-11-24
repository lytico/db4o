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

import java.util.*;

import EDU.purdue.cs.bloat.tree.*;

public class UseMap {

	public Hashtable map;

	public UseMap() {
		map = new Hashtable();
	}

	public void add(final LocalExpr use, final Instruction inst) {

		final Node def = use.def();
		if (def != null) {
			map.put(inst, def);
		}
	}

	public boolean hasDef(final Instruction inst) {

		return map.containsKey(inst);
	}

	public boolean hasSameDef(final Instruction a, final Instruction b) {
		return map.containsKey(a) && map.containsKey(b)
				&& (map.get(a) == map.get(b));
	}

}
