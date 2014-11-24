using System.Collections.Generic;
using System.Xml;
using NUnit.Framework;
using WixBuilder.Tests.Wix;

namespace WixBuilder.Tests
{
	static class WixAssert
	{
		internal static void AssertFile(IFileSystemItem expected, WixElement actual)
		{
			Assert.AreEqual(expected.FullPath, actual.GetAttribute("Source"));
		}

		internal static void AssertWixDirectory(IFileSystemItem expected, XmlElement actual)
		{
			Assert.AreEqual("Directory", actual.Name);
			Assert.AreEqual(expected.Name, actual.GetAttribute("Name"));
		}

		internal static void AssertDirectoryComponent(IFolder expectedDirectoryLayout, WixComponent actualComponent)
		{
			XmlElement parentDirectory = actualComponent.ParentElement;
			AssertWixDirectory(expectedDirectoryLayout, parentDirectory);

			var actualFiles = actualComponent.Files.GetEnumerator();
			var expectedFiles = expectedDirectoryLayout.Children;
			foreach (var expectedFile in expectedFiles)
			{
				Assert.IsTrue(actualFiles.MoveNext(), "Expecting '" + expectedFile + "'");
				AssertFile(expectedFile, actualFiles.Current);
			}
			if (actualFiles.MoveNext())
				Assert.Fail("Unexpected '" + actualFiles.Current + "'");
		}

		internal static void AssertFeature(Feature expectedFeature, WixFeature featureElement)
		{
			Assert.AreEqual(expectedFeature.Id, featureElement.Id);
			Assert.AreEqual(expectedFeature.Title, featureElement.Title);
			Assert.AreEqual(expectedFeature.Description, featureElement.Description);
		}
	}
}
