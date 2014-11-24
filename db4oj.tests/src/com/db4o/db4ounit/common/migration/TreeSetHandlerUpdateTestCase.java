package com.db4o.db4ounit.common.migration;

import java.util.*;

import com.db4o.db4ounit.common.handlers.*;
import com.db4o.ext.*;

import db4ounit.*;

/**
 * @sharpen.remove
 */
@decaf.Remove(decaf.Platform.JDK11)
public class TreeSetHandlerUpdateTestCase extends HandlerUpdateTestCaseBase {

	public static final class Item {

		@Override
		public int hashCode() {
			final int prime = 31;
			int result = 1;
			result = prime * result
					+ ((_interface == null) ? 0 : _interface.hashCode());
			result = prime * result
					+ ((_typed == null) ? 0 : _typed.hashCode());
			result = prime * result
					+ ((_untyped == null) ? 0 : _untyped.hashCode());
			return result;
		}

		@Override
		public boolean equals(Object obj) {
			if (this == obj)
				return true;
			if (obj == null)
				return false;
			if (getClass() != obj.getClass())
				return false;
			Item other = (Item) obj;
			return Check.objectsAreEqual(_typed, other._typed)
				&& Check.objectsAreEqual(_untyped, other._untyped)
				&& Check.objectsAreEqual(_interface, other._interface);
		}

		public TreeSet _typed;
		public Object _untyped;
		public Set _interface;

		public Item(TreeSet treeSet) {
			_typed = treeSet;
			_untyped = treeSet;
			_interface = treeSet;
		}
	}

	@Override
	protected void assertArrays(ExtObjectContainer objectContainer, Object obj) {
		ArrayAssert.areEqual((TreeSet[])createArrays(), (TreeSet[])obj);
	}

	@Override
	protected void assertValues(ExtObjectContainer objectContainer, Object[] values) {
		ArrayAssert.areEqual(createValues(), values);
	}

	@Override
	protected Object createArrays() {
		return new TreeSet[] { };
	}

	@Override
	protected Object[] createValues() {
		return new Object[] {
			new Item(new TreeSet()),
			new Item(new TreeSet<String>(new Comparator<String>() {
				public int compare(String s1, String s2) {
					return s2.compareTo(s1);
				}
			}))
		};
	}

	@Override
	protected String typeName() {
		return TreeSet.class.getName();
	}

}
