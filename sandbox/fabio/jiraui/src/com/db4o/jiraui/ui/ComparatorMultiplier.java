package com.db4o.jiraui.ui;

import java.util.*;

public class ComparatorMultiplier<T> implements Comparator<T> {

	private final Comparator<T> comp;
	private final int sortMultiplier;

	public ComparatorMultiplier(Comparator<T> comp, int sortMultiplier) {
		this.comp = comp;
		this.sortMultiplier = sortMultiplier;
	}

	@Override
	public int compare(T o1, T o2) {
		return comp.compare(o1, o2) * sortMultiplier;
	}

}