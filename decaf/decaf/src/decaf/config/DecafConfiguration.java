package decaf.config;

import java.util.*;

public class DecafConfiguration {
	
	private final Map<String, String> _mapping;

	public DecafConfiguration() {
		_mapping = Collections.emptyMap();
	}

	DecafConfiguration(Map<String, String> mapping) {
		_mapping = mapping;
	}
	
	public Iterable<String> mappedTypeKeys() {
		return _mapping.keySet();
	}

	public Iterable<String> mappedTypeValues() {
		return _mapping.values();
	}

	public String typeNameMapping(String typeName) {
		return _mapping.get(typeName);
	}

	public String reverseTypeNameMapping(String typeName) {
		for (Map.Entry<String,String> entry : _mapping.entrySet()) {
			if(entry.getValue().equals(typeName)) {
				return entry.getKey();
			}
		}
		return null;
	}
	
	public static DecafConfiguration forJDK11() {
		Map<String, String> mapping = new HashMap<String, String>();
		mapping.put(Map.class.getName(), "com.db4o.foundation.Map4");
		mapping.put(HashMap.class.getName(), "com.db4o.foundation.Hashtable4");
		mapping.put(List.class.getName(), "com.db4o.foundation.Sequence4");
		mapping.put(ArrayList.class.getName(), "com.db4o.foundation.Collection4");
		mapping.put(Collections.class.getName(), "com.db4o.foundation.Collections4");
		mapping.put(Comparator.class.getName(), "com.db4o.foundation.Comparison4");
		mapping.put(Iterator.class.getName() , "com.db4o.foundation.Iterator4");
		mapping.put(Iterable.class.getName() , "com.db4o.foundation.Iterable4");
		mapping.put(Set.class.getName(), "com.db4o.foundation.Set4");
		mapping.put(HashSet.class.getName(), "com.db4o.foundation.HashSet4");
		mapping.put(Arrays.class.getName(), "com.db4o.foundation.Arrays4");
		
		mapping.put(ThreadLocal.class.getName(), "com.db4o.foundation.ThreadLocal4");
		
		return create(mapping);
	}

	public static DecafConfiguration forJDK12() {
		Map<String, String> mapping = new HashMap<String, String>();
		mapping.put(Iterable.class.getName() , "com.db4o.foundation.IterableBase");

		return create(mapping);
	}
	
	public static DecafConfiguration create(Map<String, String> mapping) {
		return new DecafConfiguration(mapping);
	}
}
