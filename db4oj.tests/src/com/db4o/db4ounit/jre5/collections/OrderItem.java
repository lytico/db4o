package com.db4o.db4ounit.jre5.collections;

import com.db4o.activation.*;
import com.db4o.db4ounit.common.ta.*;


/**
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class OrderItem extends ActivatableImpl {
	private Product _product;
	private int _quantity;
	
	public OrderItem(Product product, int quantity) {
		_product = product;
		_quantity = quantity; 
	}
	
	public Product product() {
		activate(ActivationPurpose.READ);
		return _product;
	}
	
	public int quantity() {
		activate(ActivationPurpose.READ);
		return _quantity;
	}
}
