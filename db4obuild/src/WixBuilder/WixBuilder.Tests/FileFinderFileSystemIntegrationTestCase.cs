using NUnit.Framework;

namespace WixBuilder.Tests
{
	[TestFixture]
	public class FileFinderFileSystemIntegrationTestCase : FileFinderTestCase
	{
		protected override IFolderBuilder CreateFolderBuilder()
		{
			return NativeFileSystem.FolderBuilderFor(TestUtils.UniqueTempFolder());
		}
	}
}
