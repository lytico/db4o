using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace WixBuilder.Tests
{
    [TestFixture]
    public class FileFinderTestCase
    {
    	private IFolder root;

    	private FileFinder finder;

		[SetUp]
		public void SetUpFolderLayout()
		{
			root = CreateFolderBuilder()
				.EnterFolder("Bin")
					.AddFiles("OMNSetup.msi", "OtherFile.txt")
				.LeaveFolder()
				.EnterFolder("Doc")
					.EnterFolder("Api")
						.AddFiles("api.chm")
					.LeaveFolder()
				.LeaveFolder()
				.GetFolder();
			finder = new FileFinder(root);
		}

    	protected virtual IFolderBuilder CreateFolderBuilder()
    	{
    		return new FolderMock("root");
    	}

    	[Test]
		public void TestRelativePathFor()
		{
			Assert.AreEqual(@"Bin\OMNSetup.msi", finder.RelativePathFor(Bin["OMNSetup.msi"]));
		}

    	private IFolder Bin
    	{
    		get { return (IFolder) root["Bin"]; }
    	}

		[Test]
		public void TestWithSpecificFileSet()
		{
			var found = finder.FindAllIn(Bin.Children.OfType<IFile>(), Patterns.Include(@"Bin\OMNSetup.msi"));
			Assert.AreEqual(Bin["OMNSetup.msi"], found.Single());
		}

    	[Test]
        public void TestIncludeSingleFile()
        {
        	var found = finder.FindAll(Patterns.Include(@"Bin\OMNSetup.msi"));
        	Assert.AreEqual(Bin["OMNSetup.msi"], found.Single());
        }

		[Test]
		public void TestIncludeSingleFileWithBackslash()
		{
			var found = finder.FindAll(Patterns.Include(@"Bin/OMNSetup.msi"));
			Assert.AreEqual(Bin["OMNSetup.msi"], found.Single());
		}

		[Test]
		public void TestIncludeMultipleFiles()
		{
			var found = finder.FindAll(Patterns.Include(@"Bin\*.*"));
			AssertFiles(Bin.Children.Cast<IFile>(), found);
		}

    	[Test]
		public void TestIncludeAllRecursively()
		{
    		var found = finder.FindAll(Patterns.Include(@"**\*.*"));
			AssertFiles(AllFiles(), found);
		}

    	[Test]
		public void TestIncludeAll()
		{
			var found = finder.FindAll(Patterns.Include(@"*.*"));
			AssertFiles(new IFile[0], found);
		}

		[Test]
		public void TestIncludeAllRecursivelyExcludingNonExistingSingle()
		{
			var found = finder.FindAll(Patterns.And(Patterns.Include(@"**\*.*"), Patterns.Exclude(@"Bin\NonExisting")));
			AssertFiles(AllFiles(), found);
		}

    	[Test]
		public void TestIncludeAllRecursivelyExcludingSingle()
		{
			var found = finder.FindAll(Patterns.And(Patterns.Include(@"**\*.*"), Patterns.Exclude(@"Bin\OMNSetup.msi")));
			AssertFiles(AllFiles().Except(new[] { (IFile)Bin["OMNSetup.msi"] }), found);
		}

		private IEnumerable<IFile> AllFiles()
		{
			return root.GetAllFilesRecursively();
		}

		private void AssertFiles(IEnumerable<IFile> expected, IEnumerable<IFile> actual)
		{
			Assert.AreEqual(SortedByName(expected), SortedByName(actual));
		}

    	private IFile[] SortedByName(IEnumerable<IFile> source)
    	{
    		return source.OrderBy(file => file.Name).ToArray();
    	}
    }
}
