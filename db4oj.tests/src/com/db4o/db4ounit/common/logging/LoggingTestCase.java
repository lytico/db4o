package com.db4o.db4ounit.common.logging;

import java.io.*;
import java.util.*;

import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.logging.*;

import db4ounit.*;

public class LoggingTestCase implements TestLifeCycle {
	
	@LogInterface
	public interface TestLogger {
		void msg();
	}

	public interface InvalidTestLogger {
	}
	
	@Override
	public void setUp() throws Exception {
		Logger.purgeCache();
	}

	
	@Override
	public void tearDown() {
	}

	public void testInvalidTestLogger() {
		try {
			Logger.get(InvalidTestLogger.class);
			Assert.fail("An exception should have been generated since our "+InvalidTestLogger.class.getSimpleName() +" is not annotated with @LogInterface");
		} catch (IllegalArgumentException expected) {
		}
	}
	
	public void testWithNoRootInterceptor() {
		
		Logging<TestLogger> logger = Logger.get(TestLogger.class);
		
		logger.debug().msg();

	}
	
	
	public void testLoggingLevels() {
		
		List<Pair<Level, String>> methodsCalled = setRootInterceptor();
		
		Logging<TestLogger> logger = Logger.get(TestLogger.class);
		
		
		logger.trace().msg();
		logger.debug().msg();
		logger.info().msg();
		logger.warn().msg();
		logger.error().msg();
		logger.fatal().msg();
		
		Assert.areEqual(Pair.of(Logger.TRACE, "msg"), popFirst(methodsCalled));
		Assert.areEqual(Pair.of(Logger.DEBUG, "msg"), popFirst(methodsCalled));
		Assert.areEqual(Pair.of(Logger.INFO, "msg"), popFirst(methodsCalled));
		Assert.areEqual(Pair.of(Logger.WARN, "msg"), popFirst(methodsCalled));
		Assert.areEqual(Pair.of(Logger.ERROR, "msg"), popFirst(methodsCalled));
		Assert.areEqual(Pair.of(Logger.FATAL, "msg"), popFirst(methodsCalled));
		
	}

	private List<Pair<Level, String>> setRootInterceptor() {
		final List<Pair<Level,String>> methodsCalled = new ArrayList<Pair<Level,String>>();
		
		Logger.intercept(new LoggingInterceptor() {
		
			@Override
			public void log(Level loggingLevel, String method, Object[] args) {
				methodsCalled.add(Pair.of(loggingLevel, method));
			}
		
		});
		return methodsCalled;
	}
	
	public void testSetLoggingLevel() {
		
		List<Pair<Level, String>> methodsCalled = setRootInterceptor();
		
		Logging<TestLogger> logger = Logger.get(TestLogger.class);
		
		
		Logger.loggingLevel(Logger.DEBUG);
		
		logger.trace().msg();
		logger.debug().msg();
		logger.info().msg();
		
		Assert.areEqual(Pair.of(Logger.DEBUG, "msg"), popFirst(methodsCalled));
		Assert.areEqual(Pair.of(Logger.INFO, "msg"), popFirst(methodsCalled));
		
		
		logger.loggingLevel(Logger.INFO);
		
		logger.debug().msg();
		logger.info().msg();
		logger.error().msg();
		
		Assert.areEqual(Pair.of(Logger.INFO, "msg"), popFirst(methodsCalled));
		Assert.areEqual(Pair.of(Logger.ERROR, "msg"), popFirst(methodsCalled));
		
	}
	
	public void testPrintWriterLogger() throws SecurityException, NoSuchMethodException {
		
		ByteArrayOutputStream bout = new ByteArrayOutputStream();
		
		PrintWriterLoggerInterceptor interceptor = new PrintWriterLoggerInterceptor(new PrintWriter(bout, true));
		Logger.intercept(interceptor);
		
		Logging<TestLogger> logger = Logger.get(TestLogger.class);
		
		logger.debug().msg();
		logger.info().msg();
		
		String actual = Platform4.asUtf8(bout.toByteArray());
		
		String debugMsg = PrintWriterLoggerInterceptor.formatMessage(Logger.DEBUG, "msg", null);
		String infoMsg = PrintWriterLoggerInterceptor.formatMessage(Logger.INFO, "msg", null);

		Assert.isTrue(actual.contains(debugMsg));
		Assert.isTrue(actual.contains(infoMsg));
		
	}
	
	public void testInterceptor() {

		Logging<TestLogger> logger = Logger.get(TestLogger.class);
		
		Logger.intercept(new LoggingInterceptor() {
			
			@Override
			public void log(Level loggingLevel, String method, Object[] args) {
				Assert.fail("The root interceptor should not be called");
			}
		});

		final ByRef<Level> called = new ByRef<Level>();
		
		logger.forward(new TestLogger() {
			
			@Override
			public void msg() {
				called.value = Logger.contextLoggingLevel(); 
			}
		});
		
		logger.debug().msg();
		
		Assert.areEqual(Logger.DEBUG, called.value);
		
		called.value = null;
		
		logger = Logger.get(TestLogger.class);
		logger.debug().msg();
		
		Assert.areEqual(Logger.DEBUG, called.value);

	}
	
	public static <T> T popFirst(List<T> list) {
		T first = list.get(0);
		list.remove(0);
		return first;
	}

}
