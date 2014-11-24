package com.db4o.internal.activation;

public interface UpdateDepthProvider {

	UnspecifiedUpdateDepth unspecified(ModifiedObjectQuery query);
	FixedUpdateDepth forDepth(int depth);
	
}
