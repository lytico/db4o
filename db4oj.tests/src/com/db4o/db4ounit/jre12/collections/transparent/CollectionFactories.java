/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.jre12.collections.transparent;

import java.util.*;

import com.db4o.collections.*;
import com.db4o.foundation.*;

/**
 * @sharpen.remove
 */
@decaf.Remove(decaf.Platform.JDK11)
public final class CollectionFactories {

	
	private CollectionFactories() {
	}

	public static Closure4<ArrayList<CollectionElement>> plainArrayListFactory() {
		return new Closure4<ArrayList<CollectionElement>>() {
				public ArrayList<CollectionElement> run() {
					return new ArrayList<CollectionElement>();
				}
		};
	}

	public static Closure4<ArrayList<CollectionElement>> activatableArrayListFactory() {
		return new Closure4<ArrayList<CollectionElement>>() {
				public ArrayList<CollectionElement> run() {
					return new ActivatableArrayList<CollectionElement>();
				}
		};
	}

	public static Closure4<LinkedList<CollectionElement>> plainLinkedListFactory() {
		return new Closure4<LinkedList<CollectionElement>>() {
				public LinkedList<CollectionElement> run() {
					return new LinkedList<CollectionElement>();
				}
		};
	}

	public static Closure4<LinkedList<CollectionElement>> activatableLinkedListFactory() {
		return new Closure4<LinkedList<CollectionElement>>() {
				public LinkedList<CollectionElement> run() {
					return new ActivatableLinkedList<CollectionElement>();
				}
		};
	}

	public static Closure4<Stack<CollectionElement>> plainStackFactory() {
		return new Closure4<Stack<CollectionElement>>() {
				public Stack<CollectionElement> run() {
					return new Stack<CollectionElement>();
				}
		};
	}

	public static Closure4<Stack<CollectionElement>> activatableStackFactory() {
		return new Closure4<Stack<CollectionElement>>() {
			public Stack<CollectionElement> run() {
				return new ActivatableStack<CollectionElement>();
			}			
		};
	}

	public static Closure4<HashSet<CollectionElement>> plainHashSetFactory() {
		return new Closure4<HashSet<CollectionElement>>() {
				public HashSet<CollectionElement> run() {
					return new HashSet<CollectionElement>();
				}
		};
	}

	public static Closure4<HashSet<CollectionElement>> activatableHashSetFactory() {
		return new Closure4<HashSet<CollectionElement>>() {
			public HashSet<CollectionElement> run() {
				return new ActivatableHashSet<CollectionElement>();
			}			
		};
	}
	
	public static Closure4<TreeSet<CollectionElement>> plainTreeSetFactory() {
		return new Closure4<TreeSet<CollectionElement>>() {
				public TreeSet<CollectionElement> run() {
					return new TreeSet<CollectionElement>();
				}
		};
	}

	public static Closure4<TreeSet<CollectionElement>> activatableTreeSetFactory() {
		return new Closure4<TreeSet<CollectionElement>>() {
			public TreeSet<CollectionElement> run() {
				return new ActivatableTreeSet<CollectionElement>();
			}			
		};
	}


}
