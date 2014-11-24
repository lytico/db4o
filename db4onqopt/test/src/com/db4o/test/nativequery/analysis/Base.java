package com.db4o.test.nativequery.analysis;

import com.db4o.activation.*;
import com.db4o.ta.*;

class Base implements Activatable {
	int id;
	Integer idWrap;

	public int getId() {
		return id;
	}

	public Integer getIdWrapped() {
		return idWrap;
	}

	public int getIdPlusOne() {
		return id+1;
	}

	public void activate(ActivationPurpose purpose) {
	}

	public void bind(Activator activator) {
	}
}

