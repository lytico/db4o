package org.polepos.circuits.multithreadedqueries;

public interface MultithreadedQueriesDriver {

    void write();
	
    void queryWithTwoWorkers() throws Exception;
    
    void queryWithFourWorkers() throws Exception;
    
}
