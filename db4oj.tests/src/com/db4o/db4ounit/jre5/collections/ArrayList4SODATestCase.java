/**
 * @sharpen.if !SILVERLIGHT
 */
package com.db4o.db4ounit.jre5.collections;

import com.db4o.*;
import com.db4o.db4ounit.common.ta.*;
import com.db4o.query.*;

import db4ounit.*;

/**
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class ArrayList4SODATestCase extends TransparentActivationTestCaseBase {
	
	private static final Product PRODUCT_BATERY = new Product("BATE", "Batery 9v");
	private static final Product PRODUCT_KEYBOARD = new Product("KEYB", "Wireless keyboard");
	private static final Product PRODUCT_CHOCOLATE = new Product("CHOC", "Chocolate");
	private static final Product PRODUCT_MOUSE = new Product("MOUS", "Wireless Mouse");
	private static final Product PRODUCT_NOTE = new Product("NOTE", "Core Quad notebook with 1 Tb memory");
	
	private static final Product[] products = new Product[] {PRODUCT_BATERY, PRODUCT_CHOCOLATE, PRODUCT_KEYBOARD, PRODUCT_MOUSE, PRODUCT_NOTE};

	public void testSODAAutodescend() {
	
		for(int i = 0; i < products.length; i++) {
			assertCount(i);
		}
	}
	
	private void assertCount(int index) {
		
		Query query = db().query();
		query.constrain(Order.class);
		query.descend("_items").descend("_product").descend("_code").constrain(products[index].code());
		
		ObjectSet results = query.execute();
		Assert.areEqual(products.length - index, results.size());

		for (Object item : results) {
			Order order = (Order) item;
			for (int j = 0; j < order.size(); j++) {
				Assert.areEqual(products[j].code(),	order.item(j).product().code());
			}
		}
	}
	
	protected void store() {
		for(int i = 0; i < products.length; i++) {
			store(createOrder(i));
		}
	}
	
	private Order createOrder(int itemIndex) {
		Order o = new Order();
		
		for(int i = 0; i <= itemIndex; i++) {
			o.addItem(new OrderItem(products[i], i));
		}
		
		return o;
	}
}
