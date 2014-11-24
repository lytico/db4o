/* Copyright (C) 2004 - 2008  Versant Inc.  http://www.db4o.com

This file is part of the db4o open source object database.

db4o is free software; you can redistribute it and/or modify it under
the terms of version 2 of the GNU General Public License as published
by the Free Software Foundation and as clarified by db4objects' GPL 
interpretation policy, available at
http://www.db4o.com/about/company/legalpolicies/gplinterpretation/
Alternatively you can write to db4objects, Inc., 1900 S Norfolk Street,
Suite 350, San Mateo, CA 94403, USA.

db4o is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or
FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
for more details.

You should have received a copy of the GNU General Public License along
with this program; if not, write to the Free Software Foundation, Inc.,
59 Temple Place - Suite 330, Boston, MA  02111-1307, USA. */
package com.db4o.drs.inside.traversal;

public interface Traverser {

	/**
	 * Traversal will only stop when visitor.visit(...) returns false, EVEN IN
	 * THE PRESENCE OF CIRCULAR REFERENCES. So it is up to the visitor to detect
	 * circular references if necessary. Transient fields are not visited. The
	 * fields of second class objects such as Strings and Dates are also not visited.
	 */
	void traverseGraph(Object object, Visitor visitor);

	/**
	 * Should only be called during a traversal. Will traverse the graph
	 * for the received object too, using the current visitor. Used to
	 * extend the traversal to a possibly disconnected object graph.
	 */
	void extendTraversalTo(Object disconnected);

}
