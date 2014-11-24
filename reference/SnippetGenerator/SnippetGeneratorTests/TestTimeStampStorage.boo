namespace SnippetGeneratorTests

import System
import System.IO
import NUnit.Framework
import SnippetGenerator

class TestInfo:
	public static final TestFilesLocation = """SnippetGeneratorTests\TestFiles\"""

[TestFixture]
class TestTimeStampStorage:
	testInstance as TimeStampStorage

	[SetUp]
	def Setup():
		File.Delete(TimeStampStorage.TimeStampFile)	
		testInstance = TimeStampStorage()

	[Test]
	def NeedsToAdd():
		result = testInstance.NeedToAdd(FileInfo(TestInfo.TestFilesLocation+"testFile1.txt"))
		assert result

	[Test]
	def AfterUsingNoRebuildIsNeeded():
		testInstance.NeedToAdd(FileInfo(TestInfo.TestFilesLocation+"testFile1.txt"))
		result = testInstance.NeedToAdd(FileInfo(TestInfo.TestFilesLocation+"testFile1.txt"))
		assert not result

	[Test]
	def InfoIsPersistent():
		testInstance.NeedToAdd(FileInfo(TestInfo.TestFilesLocation+"testFile1.txt"))
		testInstance.PersistInfo()
		newInstance = TimeStampStorage()
		result = newInstance.NeedToAdd(FileInfo(TestInfo.TestFilesLocation+"testFile1.txt"))
		assert not result