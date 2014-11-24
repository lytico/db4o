package db4ounit;

import java.io.*;

import com.db4o.foundation.*;

public class ConsoleTestRunner {
	
	private final Iterable4 _suite;
	private final boolean _reportToFile;
	
	public ConsoleTestRunner(Iterator4 suite) {
		this(suite, true);
	}

	public ConsoleTestRunner(Iterator4 suite, boolean reportToFile) {
		if (null == suite) throw new IllegalArgumentException("suite");
		_suite = Iterators.iterable(suite);
		_reportToFile = reportToFile;
	}

	public ConsoleTestRunner(Iterable4 suite) {
		this(suite, true);
	}

	public ConsoleTestRunner(Iterable4 suite, final boolean reportToFile) {
		if (null == suite) throw new IllegalArgumentException("suite");
		_suite = suite;
		_reportToFile = reportToFile;
	}
	
	public ConsoleTestRunner(Class clazz) {
		this(new ReflectionTestSuiteBuilder(clazz));
	}	

	public int run() {
		return run(TestPlatform.getStdErr());
	}
	
	protected TestResult createTestResult() {
		return new TestResult();
	}

	public int run(Writer writer) {		
		
		TestResult result = createTestResult();
		
		new TestRunner(_suite).run(new CompositeTestListener(new ConsoleListener(writer), result));
		
		reportResult(result, writer);
		return result.failures().size();
	}

	private void report(Exception x) {
		TestPlatform.printStackTrace(TestPlatform.getStdErr(), x);
	}

	private void reportResult(TestResult result, Writer writer) {
		if(_reportToFile) {
			reportToTextFile(result);
		}
		report(result, writer);
	}

	private void reportToTextFile(TestResult result) {
		try {
			java.io.Writer writer = TestPlatform.openTextFile("db4ounit.log");
			try {
				report(result, writer);
			} finally {
				writer.close();
			}
		} catch (IOException e) {
			report(e);
		}
	}

	private void report(TestResult result, Writer writer) {
		try {
			result.print(writer);
			writer.flush();
		} catch (IOException e) {
			report(e);
		}
	}
}
