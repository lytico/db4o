package com.db4o.drs.test.hibernate;

import com.db4o.drs.test.*;
import com.db4o.foundation.*;

import db4ounit.*;

public class RdbmsDrsTestSuiteBuilder implements TestSuiteBuilder {
	
	public static void main(String[] args) {
		new ConsoleTestRunner(new RdbmsDrsTestSuiteBuilder()).run();
	}


	public Iterator4 iterator() {
		return Iterators.concat(

//		new DrsTestSuiteBuilder(new HsqlMemoryFixture("hsql-a"), new HsqlMemoryFixture("hsql-b"), RdbmsDrsTestSuite.class),

			new DrsTestSuiteBuilder(new Db4oDrsFixture("db4o-a"), new HsqlMemoryFixture("hsql-b"), RdbmsDrsTestSuite.class),
			new DrsTestSuiteBuilder(new HsqlMemoryFixture("hsql-a"), new Db4oDrsFixture("db4o-b"), RdbmsDrsTestSuite.class)

//			new DrsTestSuiteBuilder(new Db4oClientServerDrsFixture("db4o-cs-a", 9587), new HsqlMemoryFixture("hsql-b"), RdbmsDrsTestSuite.class),
//			new DrsTestSuiteBuilder(new HsqlMemoryFixture("hsql-a"), new Db4oClientServerDrsFixture("db4o-cs-b", 9587), RdbmsDrsTestSuite.class),
//			new DrsTestSuiteBuilder(new OracleFixture("Oracle-a"), new Db4oClientServerDrsFixture("db4o-cs-b", 9587), RdbmsDrsTestSuite.class),
//			new DrsTestSuiteBuilder(new MySQLFixture("MySQL-a"), new Db4oClientServerDrsFixture("db4o-cs-b", 9587), RdbmsDrsTestSuite.class),
//			new DrsTestSuiteBuilder(new PostgreSQLFixture("PostgreSQL-a"), new Db4oClientServerDrsFixture("db4o-cs-b", 9587), RdbmsDrsTestSuite.class),
//			new DrsTestSuiteBuilder(new MsSqlFixture("MsSql"), new Db4oClientServerDrsFixture("db4o-cs-b", 9587), RdbmsDrsTestSuite.class),
//			new DrsTestSuiteBuilder(new Db2Fixture("Db2"), new Db4oClientServerDrsFixture("db4o-cs-b", 9587), RdbmsDrsTestSuite.class),
//			new DrsTestSuiteBuilder(new DerbyFixture("Derby"), new Db4oClientServerDrsFixture("db4o-cs-b", 9587), RdbmsDrsTestSuite.class)

		).iterator();
	}

}
