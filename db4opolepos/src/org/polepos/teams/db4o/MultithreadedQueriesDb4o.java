package org.polepos.teams.db4o;

import org.polepos.circuits.multithreadedqueries.*;
import org.polepos.framework.*;
import org.polepos.runner.db4o.*;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.query.*;

public class MultithreadedQueriesDb4o extends Db4oDriver implements MultithreadedQueriesDriver {


	private static int maximumPayload;
	
	@Override
	public void configure(Configuration config) {
		indexField(config, MultithreadedQueries.class, fieldNext());
		indexField(config, MultithreadedQueries.class, fieldPayload());
	}

	public void write() {
		MultithreadedQueriesList list = MultithreadedQueriesList.generate(setup()
				.getObjectCount());
		maximumPayload = list.getPayload();
		begin();
		store(list);
		commit();
	}

	public void queryWithTwoWorkers() throws Exception {
		int workers = 2;
		runQuery(workers);
	}

	private void runQuery(int threadCount) throws InterruptedException, Exception {
		Worker[] workers = new Worker[threadCount];
		for (int wi = 0; wi < threadCount; wi++) {
			workers[wi] = new Worker();
			workers[wi].start();
		}
		for (int wi = 0; wi < threadCount; wi++) {
			workers[wi].join();
		}
		if (lastException != null)
			throw lastException;
	}
	
	public void queryWithFourWorkers() throws Exception {
		int workers = 4;
		runQuery(workers);
	}	

	private Exception lastException = null;

	public class Worker extends Thread {

		public void run() {
			try {
				int count = setup().getSelectCount();
				for (int i = 1; i <= count; i++) {
					Query q = newHungaroringListQuery();
					Constraint c1 = q.descend(fieldPayload()).constrain(
							new Integer(maximumPayload - 1)).greater();
					// Constraint c2 = q.descend(fieldNext()).constrain(null);
					// c1.and(c2);
					doQuery(q);
				}
			} catch (Exception ex) {
				lastException = ex;
			}
		}
	}

	private Query newHungaroringListQuery() {
		Query q = db().query();
		q.constrain(MultithreadedQueriesList.class);
		return q;
	}

	private String fieldNext() {
		return MultithreadedQueriesList.FIELD_NEXT;
	}

	private String fieldPayload() {
		return MultithreadedQueriesList.FIELD_PAYLOAD;
	}

}
