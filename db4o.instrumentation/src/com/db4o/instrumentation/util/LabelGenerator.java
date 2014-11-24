/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.instrumentation.util;

import EDU.purdue.cs.bloat.editor.*;

/**
 * @exclude
 */
public class LabelGenerator {

	private int _id = 0;

	public Label createLabel(boolean startsBlock) {
		Label label = new Label(_id,startsBlock);
		_id++;
		return label;
	}
}
