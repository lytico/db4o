package db4ounit;

public interface TestExecutor {
	void execute(Test test);
	void fail(Test test, Throwable exc);
}
