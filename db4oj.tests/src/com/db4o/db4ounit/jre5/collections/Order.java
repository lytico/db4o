/**
 * @sharpen.if !SILVERLIGHT
 */
package com.db4o.db4ounit.jre5.collections;

import com.db4o.activation.*;
import com.db4o.collections.*;
import com.db4o.db4ounit.common.ta.*;


/**
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class Order extends ActivatableImpl {
	private ArrayList4<OrderItem> _items;

	public Order() {
		_items = new ArrayList4<OrderItem>();
	}
	
	public void addItem(OrderItem item) {
		activate(ActivationPurpose.READ);
		_items.add(item);
	}
	
	public OrderItem item(int i) {
		activate(ActivationPurpose.READ);
		return _items.get(i);
	}

	public int size() {
		activate(ActivationPurpose.READ);
		return _items.size();
	}
}
