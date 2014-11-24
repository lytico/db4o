namespace SnippetGeneratorTests

import System
import System.IO
import NUnit.Framework
import SnippetGenerator
import Ionic.Zip

[TestFixture]
class TestZipFile:
	zipFileGenerator as CodeZip
	tsCondition as TimeStampStorage
	public def constructor():
		pass

	[SetUp]
	def Setup():		
		zipFileGenerator = CodeZip()
		tsCondition = TimeStampStorage()

	
	[Test]
	def IncludeAndNoDescent():		
		snippetGenerator = CodeToSnippets("../../../SnippetGenerator/CodeSnippetTemplate.flsnp",zipFileGenerator,tsCondition)
		snippetGenerator.CreateCodeSnippets("../../TestFiles/zipTest","./zipTest","cs")
		assert File.Exists("./zipTest/ExampleCode-Test.cs.flsnp")
		assert File.Exists("./zipTest/Example-TestFiles-zipTest-cs.zip")
		assert File.Exists("./zipTest/includeMe/ExpectMe-Tada.cs.flsnp")
		assert File.Exists("./zipTest/includeMe/Example-zipTest-includeMe-cs.zip")
		assert not Directory.Exists("./zipTest/doNotDescent")

		zipFile = ZipFile.Read("./zipTest/Example-TestFiles-zipTest-cs.zip")
		for entry as string in zipFile.EntryFileNames:
			assert not entry.EndsWith(".snippet-generator.nodescend")

			
	[Test]
	def IncludeParentDirectory():		
		snippetGenerator = CodeToSnippets("../../../SnippetGenerator/CodeSnippetTemplate.flsnp",zipFileGenerator,tsCondition)
		snippetGenerator.CreateCodeSnippets("../../TestFiles/includeTest","./includeTest","cs")
		assert File.Exists("./includeTest/subdir/subdir2/content-Content.txt.flsnp")

		zipFile = ZipFile.Read("./includeTest/subdir/subdir2/Example-subdir-subdir2-cs.zip")
		assert zipFile.EntryFileNames.Contains("TopLevelFile.txt")
		assert zipFile.EntryFileNames.Contains("MiddleLevelFile.txt")
		
