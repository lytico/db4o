package com.db4o.db4ounit.jre5.enums;

import java.lang.reflect.*;

import com.db4o.*;
import com.db4o.activation.*;
import com.db4o.config.*;
import com.db4o.db4ounit.common.ta.*;
import com.db4o.ta.*;

import db4ounit.*;
import db4ounit.extensions.*;


@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class TAEnumsTestCase extends AbstractDb4oTestCase {

	public static void main(String[] args) {
		new TAEnumsTestCase().runSolo();
	}
	
	public static class Item extends ActivatableImpl {
		private TypeCountEnum _enum;
		private String _name;
		public Item(TypeCountEnum enum_, String name) {
			this._enum = enum_;
			this._name = name;
		}
		public TypeCountEnum get_enum() {
			activate(ActivationPurpose.READ);
			return _enum;
		}
		public String get_name() {
			activate(ActivationPurpose.READ);
			return _name;
		}
	}
	
	protected void configure(Configuration config) throws Exception {
		config.add(new TransparentActivationSupport());
	}
	
	protected void store() throws Exception {
		store(new Item(TypeCountEnum.A ,"A"));
		store(new Item(TypeCountEnum.B, "B"));
	}
	
	@SuppressWarnings("unchecked")
	public void test() throws Exception {
		ObjectSet<Item> set = newQuery(Item.class).execute();
		while(set.hasNext()){
			Item item = (Item)set.next();
			assertItemIsNotActivated(item);
			assertItemEnum(item);
		}
	}

	private void assertItemIsNotActivated(Item item) throws Exception {
		Field[] declaredFields = Item.class.getDeclaredFields();
		for (int i = 0; i < declaredFields.length; i++) {
			declaredFields[i].setAccessible(true);
			Assert.isNull(declaredFields[i].get(item));
		}
	}

	private void assertItemEnum(Item item) {
		if(item.get_name().equals("A")){
			Assert.areSame(TypeCountEnum.A, item.get_enum());
		}
		if(item.get_name().equals("B")){
			Assert.areSame(TypeCountEnum.B, item.get_enum());
		}
	}
	
	
	
}
