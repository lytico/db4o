package com.db4o.db4ounit.jre12.collections.map;

/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class MapContent {
	String name;
	
	public MapContent() {
		
	}
	
	public MapContent(String name) {
		this.name = name;
	}

	public int hashCode() {
		final int PRIME = 31;
		int result = 1;
		result = PRIME * result + ((name == null) ? 0 : name.hashCode());
		return result;
	}

	public boolean equals(Object obj) {
		if (this == obj)
			return true;
		if (obj == null)
			return false;
		if (getClass() != obj.getClass())
			return false;
		final MapContent other = (MapContent) obj;
		if (name == null) {
			if (other.name != null)
				return false;
		} else if (!name.equals(other.name))
			return false;
		return true;
	}
	
	public String toString() {
		return name;
	}
}
