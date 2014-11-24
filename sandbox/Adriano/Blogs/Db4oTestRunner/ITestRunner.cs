namespace Db4oTestRunner
{
	public interface ITestRunner
	{
		void Run(ILogger logger);
		void TearDown();
	}
}
