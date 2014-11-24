/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.versant.cobra.qlin;

import com.db4o.qlin.*;
import com.versant.odbms.query.*;

public class CLinOrderBy<T> extends CLinSubNode<T> {
	
	private final Field _field;

	public CLinOrderBy(CLinRoot<T> root, Object expression, QLinOrderByDirection direction) {
		super(root);
		_field = new Field(QLinSupport.field(expression).getName());
		query().setOrderByExpression(new OrderByExpression[]{
					new OrderByExpression(new SubExpression(_field), direction.isDescending())
			});
		

		
	}
	

}
