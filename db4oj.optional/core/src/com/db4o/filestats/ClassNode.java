/* Copyright (C) 2010   Versant Inc.   http://www.db4o.com */

package com.db4o.filestats;

import java.util.*;

import com.db4o.internal.*;

/**
* @exclude
*/
@decaf.Ignore(decaf.Platform.JDK11)
public class ClassNode {
	
	public static Set<ClassNode> buildHierarchy(ClassMetadataRepository repository) {
		ClassMetadataIterator classIter = repository.iterator();
		Map<String, ClassNode> nodes = new HashMap<String, ClassNode>();
		Set<ClassNode> roots = new HashSet<ClassNode>();
		while(classIter.moveNext()) {
			ClassMetadata clazz = classIter.currentClass();
			ClassNode node = new ClassNode(clazz);
			nodes.put(clazz.getName(), node);
			if(clazz.getAncestor() == null) {
				roots.add(node);
			}
		}
		for (ClassNode node : nodes.values()) {
			ClassMetadata ancestor = node.classMetadata().getAncestor();
			if(ancestor != null) {
				nodes.get(ancestor.getName()).addSubClass(node);
			}
		}
		return roots;
	}
	
	private final ClassMetadata _clazz;
	private final Set<ClassNode> _subClasses = new HashSet<ClassNode>();

	public ClassNode(ClassMetadata clazz) {
		_clazz = clazz;
	}
	
	public ClassMetadata classMetadata() {
		return _clazz;
	}
	
	void addSubClass(ClassNode node) {
		_subClasses.add(node);
	}
	
	@Override
	public boolean equals(Object obj) {
		if(obj == this) {
			return true;
		}
		if(obj == null || getClass() != obj.getClass()) {
			return false;
		}
		return _clazz.getName().equals(((ClassNode)obj)._clazz.getName());
	}
	
	@Override
	public int hashCode() {
		return _clazz.getName().hashCode();
	}

	public Iterable<ClassNode> subClasses() {
		return _subClasses;
	}
}