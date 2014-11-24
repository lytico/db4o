/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.foundation;

import java.util.*;

import com.db4o.foundation.*;

import db4ounit.*;

@decaf.Remove(unlessCompatible=decaf.Platform.JDK15)
public class Algorithms4TestCase implements TestCase {
	
	public static class QuickSortableIntArray implements Sortable4{
		
		private int[] ints;
		
		public QuickSortableIntArray(int[] ints) {
			this.ints = ints;
		}

		public int compare(int leftIndex, int rightIndex) {
			return ints[leftIndex] - ints[rightIndex]; 
		}

		public int size() {
			return ints.length;
		}

		public void swap(int leftIndex, int rightIndex) {
			int temp = ints[leftIndex];
			ints[leftIndex] = ints[rightIndex];
			ints[rightIndex] = temp;
		}
	}
	
	public void testUnsortedSmall(){
		assertQSort(3 , 5, 2 , 1, 4);
	}
	
	public void testUnsortedBig() {
		assertQSort(3, 5, 7, 1, 2, 4, 6, 9, 11, 8, 10, 12);
	}
	
	public void testSingleElement(){
		assertQSort(42);
	}
	
	public void testTwoElements() {
		assertQSort(1, 42);
		assertQSort(42, 1);
	}
	
	public void testDuplicates() {
		assertQSort(2, 2, 1, 1, 5, 5, 5, 5, 5, 3, 3, 3);
	}

	public void testStackUsage(){
		int[] ints = new int[50000];
		for(int i=0;i<ints.length;i++) {
			ints[i]=i+1;
		}
		assertQSort(ints);
	}

	private void assertQSort(int... ints) {
		final int[] copy = Arrays4.copyOf(ints, ints.length);
		QuickSortableIntArray sample = new QuickSortableIntArray(copy);
		Algorithms4.sort(sample);
		Arrays.sort(ints);
		ArrayAssert.areEqual(ints, copy);
	}


}
