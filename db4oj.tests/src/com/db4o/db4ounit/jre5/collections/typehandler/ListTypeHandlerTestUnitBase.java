package com.db4o.db4ounit.jre5.collections.typehandler;

import java.util.*;

import com.db4o.typehandlers.*;

/**
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class ListTypeHandlerTestUnitBase extends TypeHandlerTestUnitBase {
	
	protected AbstractItemFactory itemFactory() {
		return (AbstractItemFactory) ListTypeHandlerTestVariables.LIST_IMPLEMENTATION.value();
	}
	
	protected TypeHandler4 typeHandler() {
	    return (TypeHandler4) ListTypeHandlerTestVariables.LIST_TYPEHANDER.value();
	}
	
	protected void fillItem(Object item) {
		fillListItem(item);
	}

	protected void assertContent(Object item) {
		assertListContent(item);
	}

	protected void assertPlainContent(Object item) {
		assertPlainListContent((List) item);
	}

	protected ListTypeHandlerTestElementsSpec elementsSpec() {
		return (ListTypeHandlerTestElementsSpec) ListTypeHandlerTestVariables.ELEMENTS_SPEC.value();
	}    


}
