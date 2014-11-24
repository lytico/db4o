package com.db4o.internal.activation;

import com.db4o.internal.*;
import com.db4o.ta.*;

public class TPUnspecifiedUpdateDepth extends UnspecifiedUpdateDepth {

	private final ModifiedObjectQuery _query;
	
	TPUnspecifiedUpdateDepth(ModifiedObjectQuery query) {
		_query = query;
	}

	public boolean canSkip(ObjectReference ref) {
		ClassMetadata clazz = ref.classMetadata();
		return clazz.reflector().forClass(Activatable.class).isAssignableFrom(clazz.classReflector()) && !_query.isModified(ref.getObject());
	}

	@Override
	protected FixedUpdateDepth forDepth(int depth) {
		return new TPFixedUpdateDepth(depth, _query);
	}

}
