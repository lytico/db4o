/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.polepos.continuous.filealgebra;

import java.util.*;

public class LenientIndexSelectingFileSource extends IndexSelectingFileSource {

	public LenientIndexSelectingFileSource(FileSource source, int[] indices) {
		super(source, indices);
	}

	@Override
	protected int[] indices(int count) {
		Set<Integer> indices = new HashSet<Integer>(Math.min(count, super.indices(count).length));
		int[] origIndices = super.indices(count);
		for (int i = 0; i < origIndices.length; i++) {
			int curIdx = origIndices[i];
			indices.add(curIdx >= count ? count - 1 : curIdx);
		}
		Iterator<Integer> indexIter = indices.iterator();
		int[] lenientIndices = new int[indices.size()];
		for (int i = 0; i < lenientIndices.length; i++) {
			lenientIndices[i] = indexIter.next();
		}
		return lenientIndices;
	}

}
