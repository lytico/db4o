using System;
using NUnit.Framework;

namespace WixBuilder.Tests
{
	[TestFixture]
	public class PatternsTestCase
	{
		[Test]
		public void TestRecursivePattern()
		{
			var pattern = Patterns.Include("**/*.txt");
			AssertMatches(pattern,
				"foo.txt", @"foo\foo.txt",
				"foo/foo.txt", "foo/bar/foo.txt",
				"foo folder/bar+file (yes).txt",
				"foo1/bar2.txt");
			AssertDoesNotMatch(pattern, "foo", "foo.bar", "foo.txt/bar.msi");
		}

		[Test]
		public void TestAllFilesPattern()
		{
			var pattern = Patterns.Include("**/*.*");
			AssertMatches(pattern, "foo.bar", @"foo\foo.txt", "foo/foo.msi", "foo/foo.bar/foo.txt");
			
		}

		private void AssertMatches(Predicate<string> pattern, params string[] valuesThatMustMatch)
		{
			foreach (var value in valuesThatMustMatch)
				Assert.IsTrue(pattern(value), "Match expected: " + value);
		}

		private void AssertDoesNotMatch(Predicate<string> pattern, params string[] valuesThatMustNotMatch)
		{
			foreach (var value in valuesThatMustNotMatch)
				Assert.IsFalse(pattern(value), "Unexpected match: " + value);
		}
	}
}
