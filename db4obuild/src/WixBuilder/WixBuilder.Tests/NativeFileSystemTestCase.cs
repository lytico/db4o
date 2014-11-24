using System;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace WixBuilder.Tests
{
	[TestFixture]
	public class NativeFileSystemTestCase
	{
		[Test]
		public void TestGetFolder()
		{
			var root = NativeFileSystem.FolderBuilderFor(TestUtils.UniqueTempFolder())
				.AddFiles("foo.txt")
				.EnterFolder("Bar")
					.AddFiles("bar1.txt", "bar2.txt")
				.LeaveFolder();

			Func<string, string> rebase = path => Path.Combine(root.FullPath, path);

			var folder = NativeFileSystem.GetFolder(root.FullPath);
			AssertPathAndName(root.FullPath, folder);

			var fooTxt = folder.Children.OfType<IFile>().AssertSingle();
			AssertPathNameAndParent(rebase("foo.txt"), folder, fooTxt);

			var bar = folder.Children.OfType<IFolder>().AssertSingle();
			AssertPathAndName(rebase("Bar"), bar);

			var barFiles = bar.Children.Cast<IFile>().OrderBy(file => file.Name).ToList();
			AssertPathAndName(rebase(@"Bar\bar1.txt"), barFiles[0]);
			AssertPathAndName(rebase(@"Bar\bar2.txt"), barFiles[1]);

		}

		private void AssertPathNameAndParent(string expectedPath, IFolder expectedParent, IFile actualFile)
		{
			AssertPathAndName(expectedPath, actualFile);
			Assert.AreSame(expectedParent, actualFile.Parent);
		}

		private void AssertPathAndName(string expectedPath, IFileSystemItem fsi)
		{
			Assert.AreEqual(expectedPath, fsi.FullPath);
			Assert.AreEqual(Path.GetFileName(expectedPath), fsi.Name);
		}
	}
}
