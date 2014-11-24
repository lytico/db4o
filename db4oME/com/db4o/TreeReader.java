/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

/**
 * @exclude
 */
public final class TreeReader  
{
	private final Readable i_template;
	private final YapReader i_bytes;
	private int i_current = 0;
	private int i_levels = 0;
	private int i_size;
	private boolean i_orderOnRead = false;
	
	TreeReader(YapReader a_bytes, Readable a_template){
		i_template = a_template;
		i_bytes = a_bytes;
	}
	
	public TreeReader(YapReader a_bytes, Readable a_template, boolean a_orderOnRead){
		this(a_bytes, a_template);
		i_orderOnRead = a_orderOnRead;
	}
	
	public Tree read() {
	    return read(i_bytes.readInt());
	}
	
	public Tree read(int a_size){
	    i_size = a_size;
		if(i_size > 0){
			if(i_orderOnRead){
				Tree tree = null;
				for (int i = 0; i < i_size; i++) {
				    tree = Tree.add(tree, (Tree)i_template.read(i_bytes));
                }
                return tree;
			}else{
				while ((1 << i_levels) < (i_size + 1)){
					i_levels ++;
				}
				return linkUp(null, i_levels);
			}
		}
		return null;
	}
	
	private final Tree linkUp(Tree a_preceding, int a_level){
		Tree node = (Tree)i_template.read(i_bytes);
		i_current++;
		node._preceding = a_preceding;
		node._subsequent = linkDown(a_level + 1);
		node.calculateSize();
		if(i_current < i_size){
			return linkUp(node, a_level - 1);
		}
		return node;

	}

	private final Tree linkDown(int a_level){
		if(i_current < i_size){
			i_current++;
			if(a_level < i_levels) {
				Tree preceding = linkDown(a_level + 1);
				Tree node = (Tree)i_template.read(i_bytes);
				node._preceding = preceding;
				node._subsequent = linkDown(a_level + 1);
				node.calculateSize();
				return node;
			}else {
			    return (Tree)i_template.read(i_bytes);
			}
		}
		return null;
	}
}
