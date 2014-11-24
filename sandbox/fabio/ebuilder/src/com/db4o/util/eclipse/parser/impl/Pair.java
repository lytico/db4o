package com.db4o.util.eclipse.parser.impl;

public class Pair<F, S> {

	public static <F, S> Pair<F, S> of(F first, S second) {
		return new Pair<F, S>(first, second);
	}

	public F first;
	public S second;

	public Pair(F first, S second) {
		this.first = first;
		this.second = second;
	}

	@Override
	public String toString() {
		return "Pair.of(" + first + ", " + second + ")";
	}

	@Override
	public int hashCode() {
		final int prime = 31;
		int result = 1;
		result = prime * result + ((first == null) ? 0 : first.hashCode());
		result = prime * result + ((second == null) ? 0 : second.hashCode());
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
		@SuppressWarnings("rawtypes")
		Pair other = (Pair) obj;
		if (first == null) {
			if (other.first != null)
				return false;
		} else if (!first.equals(other.first))
			return false;
		if (second == null) {
			if (other.second != null)
				return false;
		} else if (!second.equals(other.second))
			return false;
		return true;
	}

}
