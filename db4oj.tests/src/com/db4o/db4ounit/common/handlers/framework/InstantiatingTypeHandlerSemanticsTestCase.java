package com.db4o.db4ounit.common.handlers.framework;

import static org.easymock.EasyMock.capture;
import static org.easymock.EasyMock.createMock;
import static org.easymock.EasyMock.expect;
import static org.easymock.EasyMock.expectLastCall;
import static org.easymock.EasyMock.isA;
import static org.easymock.EasyMock.replay;
import static org.easymock.EasyMock.verify;

import org.easymock.*;

import com.db4o.config.*;
import com.db4o.marshall.*;
import com.db4o.typehandlers.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;

@decaf.Remove(unlessCompatible=decaf.Platform.JDK15)
public class InstantiatingTypeHandlerSemanticsTestCase extends AbstractInMemoryDb4oTestCase implements OptOutDefragSolo {
	
	public static class Item {
	}

	final InstantiatingTypeHandler _typeHandlerMock = createMock(InstantiatingTypeHandler.class);
	
	@Override
	protected void configure(Configuration config) throws Exception {
	    config.registerTypeHandler(
	    		new SingleClassTypeHandlerPredicate(Item.class),
	    		_typeHandlerMock);
	}
	
	public void testStoreInstantiate() throws Exception {
		
		_typeHandlerMock.writeInstantiation(isA(WriteContext.class), isA(Item.class));
		expectLastCall();
	
		_typeHandlerMock.write(isA(WriteContext.class), isA(Item.class));
		expectLastCall();
		
		final Item item = new Item();
		expect(_typeHandlerMock.instantiate(isA(ReadContext.class)))
			.andReturn(item);
		
		Capture<ReferenceActivationContext> readContext = new Capture<ReferenceActivationContext>();
		_typeHandlerMock.activate(capture(readContext));
		expectLastCall();
		
		replay(_typeHandlerMock);
		
		store(item);
		reopen();
		
		Assert.areSame(item, retrieveOnlyInstance(Item.class));
		Assert.areSame(item, readContext.getValue().persistentObject());
		
		verify(_typeHandlerMock);
	}
}
