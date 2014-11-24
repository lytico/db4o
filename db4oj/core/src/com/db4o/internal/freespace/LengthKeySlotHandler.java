/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.freespace;

import com.db4o.foundation.*;
import com.db4o.internal.slots.*;
import com.db4o.marshall.*;


/**
 * @exclude
 */
public class LengthKeySlotHandler extends SlotHandler{
	
	public int compareTo(Object obj) {
		return _current.compareByLength((Slot)obj);
	}
	
	public PreparedComparison prepareComparison(Context context, Object slot) {
		final Slot sourceSlot = (Slot)slot;
		return new PreparedComparison() {
			public int compareTo(Object obj) {
				final Slot targetSlot = (Slot)obj;
				
				// FIXME: The comparison method in #compareByLength is the wrong way around.
				
				// Fix there and here after other references are fixed.
				
				return - sourceSlot.compareByLength(targetSlot);
			}
		};
	}



}
