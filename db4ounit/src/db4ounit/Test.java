package db4ounit;

import com.db4o.foundation.*;

public interface Test extends Runnable {
	
	String label();
	boolean isLeafTest();
	Test transmogrify(Function4<Test, Test> fun);
	
}
