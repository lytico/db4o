/* Copyright (C) 2006  db4objects Inc.  http://www.db4o.com */

package demo.objectmanager.model;

import java.util.*;


/**
 * @exclude
 */
public class Collections {

	public static class Item implements Comparable {

		public String name;

		public Item() {

		}

		public Item(String name_) {
			if (name_ == null) {
				throw new IllegalArgumentException();
			}
			name = name_;
		}

		public int compareTo(Object o) {
			if (!(o instanceof Item)) {
				throw new IllegalArgumentException();
			}
			Item other = (Item) o;

			return name.compareTo(other.name);
		}

		public boolean equals(Object obj) {
			if (!(obj instanceof Item)) {
				return false;
			}
			Item other = (Item) obj;
			return name.equals(other.name);
		}

		public int hashCode() {
			return name.hashCode();
		}


		public String toString() {
			return "Item named: " + name;
		}
	}

	public List list;

	public ArrayList arrayList;

	public Vector vector;
	public Set set;
	public Queue queue;
	public Map map;
	public HashMap hashMap;

	public TreeMap treeMap;

	public Item[] array;

	public boolean[] primitiveArrayBoolean;
	public double[] primitiveArrayDouble;
	public Object[] objectArray;


	public Collections() {

	}

	public static Object forDemo() {
		Collections collections = new Collections();
		collections.createDemoCollections();
		return collections;
	}

	private void createDemoCollections() {
		list = new ArrayList();
		arrayList = new ArrayList();
		vector = new Vector();
		map = new HashMap();
		hashMap = new HashMap();
		treeMap = new TreeMap();
		set = new HashSet();
		queue = new LinkedList();

		array = new Item[2];
		primitiveArrayBoolean = new boolean[10];
		primitiveArrayDouble = new double[10];
		objectArray = new Object[20];

		fillList(list);
		fillList(arrayList);
		fillList(vector);
		fillSet(set);
		fillQueue(queue);

		fillMap(map);
		fillMap(hashMap);
		fillMap(treeMap);

		fillArray(array);

		fillPrimitiveBooleanArray(primitiveArrayBoolean);
		fillPrimitiveDoubleArray(primitiveArrayDouble);

		fillObjectArray(objectArray);
	}

	private void fillObjectArray(Object[] objectArray) {
		for (int i = 0; i < objectArray.length; i++) {
			objectArray[i] = DemoPopulator.getArbitraryObject(i);
		}
	}

	private void fillQueue(Queue queue) {
		queue.add("one");
		queue.add(new Item("two"));
	}

	private void fillSet(Set set) {
		set.add("one");
		set.add(new Item("two"));
	}

	private void fillArray(Item[] array) {
		array[0] = new Item("one");
		array[1] = new Item("two");
	}


	private void fillPrimitiveBooleanArray(boolean[] primitiveArrayBoolean) {
		for (int i = 0; i < primitiveArrayBoolean.length; i += 2) { // every second
			primitiveArrayBoolean[i] = true;
		}
	}

	private void fillPrimitiveDoubleArray(double[] primitiveArrayDouble) {
		for (int i = 0; i < primitiveArrayDouble.length; i++) { // every second
			primitiveArrayDouble[i] = i * 2.1;
		}
	}

	private void fillList(List list) {
		list.add("one");
		list.add(new Item("two"));
	}

	private void fillMap(Map map) {
		map.put(new Item("one-key"), new Item("one-value"));
		map.put(new Item("two-key"), new Item("two-value"));
		if (!(map instanceof TreeMap)) { // can't take different key types
			map.put(new Integer(123), "Some value");
			map.put("string-key", new Integer(456));
		}
	}

}
