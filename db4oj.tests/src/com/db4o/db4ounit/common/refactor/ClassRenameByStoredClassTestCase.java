package com.db4o.db4ounit.common.refactor;

import com.db4o.ext.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;
import db4ounit.extensions.util.*;

public class ClassRenameByStoredClassTestCase extends AbstractDb4oTestCase implements OptOutNetworkingCS {

	private static String NAME = "test";
	
	public static void main(String[] args) {
		new ClassRenameByStoredClassTestCase().runAll();
	}
	
    public static class Original {
    	public String _name;
    	public Original(String name) {
    		this._name = name;
		}
    }
    
    public static class Changed {
    	public String _name;
    	public String _otherName;
    	public Changed(String name) {
    		_name = name;
    		_otherName = name;
		}
    }
    
    protected void store() throws Exception {
    	store(new Original(NAME));
    }
    
    public void testWithReopen() throws Exception {
    	assertRenamed(true);
	}

    public void testWithoutReopen() throws Exception {
    	assertRenamed(false);
	}

	private void assertRenamed(boolean doReopen) throws Exception {
		StoredClass originalClazz = db().ext().storedClass(Original.class);
    	originalClazz.rename(CrossPlatformServices.fullyQualifiedName(Changed.class));
    	if(doReopen) {
    		reopen();
    	}
    	Changed changedObject = (Changed) retrieveOnlyInstance(Changed.class);
    	Assert.areEqual(NAME, changedObject._name);
    	Assert.isNull(changedObject._otherName);
	}

}
