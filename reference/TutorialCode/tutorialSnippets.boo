import System.IO
import Ionic.Zip
import System
import SnippetGenerator from "SnippetGenerator"


if(Environment.GetCommandLineArgs().Length == 3 and Environment.GetCommandLineArgs()[2] == '-rebuild'):
	print "full rebuild"
	Directory.CreateDirectory("../DB4OTutorial/Content/CodeExamples").Delete(true)
	File.Delete(TimeStampStorage.TimeStampFile)
else:
	print "Update the snippets. For a full rebuild use the argument -rebuild"

zipFileGenerator = CodeZip()
tsCondition = TimeStampStorage()

# regular examples
snippetGenerator = CodeToSnippets("../SnippetGenerator/SnippetGenerator/CodeSnippetTemplate.flsnp",zipFileGenerator,tsCondition)
snippetGenerator.CreateCodeSnippets("java/db4o-tutorial/src/main/com/db4odoc/tutorial","../DB4OTutorial/Content/CodeExamples","java")
snippetGenerator.CreateCodeSnippets("java/db4o-tutorial/","../DB4OTutorial/Content/CodeExamples","")
snippetGenerator.CreateCodeSnippets("dotNet/CSharpExamples/Code","../DB4OTutorial/Content/CodeExamples","csharp")
snippetGenerator.CreateCodeSnippets("dotNet/VisualBasicExamples/Code","../DB4OTutorial/Content/CodeExamples","vb")


aggreatedSnippet = SnippetAggregator(File.ReadAllText("../SnippetGenerator/SnippetGenerator/AggregateSnippetTemplate.flsnp"),tsCondition)
aggreatedSnippet.BuildAggregateSnippets(Directory.CreateDirectory("../DB4OTutorial/Content/CodeExamples"))

tsCondition.PersistInfo()


