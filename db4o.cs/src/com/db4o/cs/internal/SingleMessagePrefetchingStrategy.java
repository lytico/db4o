/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.cs.internal;

import java.util.*;

import com.db4o.foundation.*;
import com.db4o.internal.*;

/**
 * Prefetchs multiples objects at once (in a single message).
 * 
 * @exclude
 */
public class SingleMessagePrefetchingStrategy implements PrefetchingStrategy {

	public static final PrefetchingStrategy INSTANCE = new SingleMessagePrefetchingStrategy();
	
	private SingleMessagePrefetchingStrategy() {
	}

	public int prefetchObjects(ClientObjectContainer container, 
			Transaction trans, 
			IntIterator4 ids, Object[] prefetched, int prefetchCount) {
		int count = 0;

		List<Pair<Integer, Integer>> idsToGet = new ArrayList<Pair<Integer, Integer>>();
		while (count < prefetchCount) {
			if (!ids.moveNext()) {
				break;
			}
			int id = ids.currentInt();
			if (id > 0) {
                Object obj = trans.objectForIdFromCache(id);
                if(obj != null){
                    prefetched[count] = obj;
                }else{
					idsToGet.add(Pair.of(id, count));
				}
				count++;
			}
		}

		if (idsToGet.size() > 0) {
		    final ByteArrayBuffer[] buffers = container.readObjectSlots(trans, idArrayFor(idsToGet));
		    for (int i=0; i<buffers.length; i++) {
		    	final Pair<Integer, Integer> pair = idsToGet.get(i);
		    	final int id = pair.first;
		    	final int position = pair.second;
				Object obj = trans.objectForIdFromCache(id);
		    	if(obj != null){
		    		prefetched[position] = obj;
		    	}else{
		    		prefetched[position] = new ObjectReference(id).readPrefetch(trans, buffers[i], Const4.ADD_TO_ID_TREE);
				}
			}
		}
		return count;
	}

	private int[] idArrayFor(List<Pair<Integer, Integer>> idsToGet) {
	    final int[] idArray = new int[idsToGet.size()];
	    for (int i=0; i<idArray.length; ++i) {
	    	idArray[i] = idsToGet.get(i).first;
	    }
	    return idArray;
    }

}
