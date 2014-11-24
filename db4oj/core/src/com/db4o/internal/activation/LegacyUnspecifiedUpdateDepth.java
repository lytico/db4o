package com.db4o.internal.activation;

import com.db4o.internal.*;

public class LegacyUnspecifiedUpdateDepth extends UnspecifiedUpdateDepth {

	public final static LegacyUnspecifiedUpdateDepth INSTANCE = new LegacyUnspecifiedUpdateDepth();
	
	private LegacyUnspecifiedUpdateDepth() {
	}

	public boolean canSkip(ObjectReference ref) {
		return false;
	}

	@Override
	protected FixedUpdateDepth forDepth(int depth) {
		return new LegacyFixedUpdateDepth(depth);
	}

}
