package com.db4o.nativequery.main;

import com.db4o.instrumentation.ant.*;
import com.db4o.instrumentation.core.*;

public class NQAntClassEditFactory implements AntClassEditFactory {

	public BloatClassEdit createEdit(ClassFilter clazzFilter) {
		return new TranslateNQToSODAEdit();
	}

}
