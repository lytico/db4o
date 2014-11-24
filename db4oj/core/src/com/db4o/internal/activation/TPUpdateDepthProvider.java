package com.db4o.internal.activation;

public class TPUpdateDepthProvider implements UpdateDepthProvider {

	public FixedUpdateDepth forDepth(int depth) {
		return new TPFixedUpdateDepth(depth, NullModifiedObjectQuery.INSTANCE);
	}

	public UnspecifiedUpdateDepth unspecified(ModifiedObjectQuery query) {
		return new TPUnspecifiedUpdateDepth(query);
	}

}
