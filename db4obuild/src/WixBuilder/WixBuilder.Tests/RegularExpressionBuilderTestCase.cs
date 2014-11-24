using NUnit.Framework;

namespace WixBuilder.Tests
{
	[TestFixture]
	public class RegularExpressionBuilderTestCase
	{
		[Test]
		public void RecursiveFolderPattern()
		{
			var expected = @"(([^\\/]|\.)*(\\|/))*[^\\/]*\.txt";
			AssertPattern(expected, @"**/*.txt");
			AssertPattern(expected, @"**\*.txt");
		}

		private void AssertPattern(string expected, string pattern)
		{
			Assert.AreEqual(expected, RegularExpressionBuilder.For(pattern));
		}
	}
}
