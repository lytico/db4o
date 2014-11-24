
using NUnit.Framework;

namespace WixBuilder.Tests
{
	[TestFixture]
	public class WixScriptBuilderNativeFileSystemIntegrationTestCase : WixScriptBuilderTestCase
	{
		protected override IFolderBuilder CreateFolderBuilder()
		{
			return NativeFileSystem.FolderBuilderFor(TestUtils.UniqueTempFolder());
		}
	}
}