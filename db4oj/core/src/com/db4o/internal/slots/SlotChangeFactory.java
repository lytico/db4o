/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.slots;

/**
 * @exclude
 */
public class SlotChangeFactory {
	
	private SlotChangeFactory(){
		
	}
	
	public SlotChange newInstance(int id){
		return new SlotChange(id);
	}
	
	public static final SlotChangeFactory USER_OBJECTS = new SlotChangeFactory();
	
	public static final SlotChangeFactory SYSTEM_OBJECTS = new SlotChangeFactory(){
		public SlotChange newInstance(int id) {
			return new SystemSlotChange(id);
		};
	};
	
	public static final SlotChangeFactory ID_SYSTEM = new SlotChangeFactory(){
		public SlotChange newInstance(int id) {
			return new IdSystemSlotChange(id);
		};
	};
	
	public static final SlotChangeFactory FREE_SPACE = new SlotChangeFactory(){
		public SlotChange newInstance(int id) {
			return new FreespaceSlotChange(id);
		};
	};
	
}
