import System.IO
import Ionic.Zip
import System
import SnippetGenerator from "SnippetGenerator"


if(Environment.GetCommandLineArgs().Length == 3 and Environment.GetCommandLineArgs()[2] == '-rebuild'):
	print "full rebuild"
	Directory.CreateDirectory("../DB4OFlare/Content/CodeExamples").Delete(true)
	Directory.CreateDirectory("../DRSFlare/Content/CodeExamples").Delete(true)
	File.Delete(TimeStampStorage.TimeStampFile)
else:
	print "Update the snippets. For a full rebuild use the argument -rebuild"

zipFileGenerator = CodeZip()
tsCondition = TimeStampStorage()

# regular examples
snippetGenerator = CodeToSnippets("../SnippetGenerator/SnippetGenerator/CodeSnippetTemplate.flsnp",zipFileGenerator,tsCondition)
snippetGenerator.CreateCodeSnippets("java/src/com/db4odoc","../DB4OFlare/Content/CodeExamples","java")
snippetGenerator.CreateCodeSnippets("java/","../DB4OFlare/Content/CodeExamples","java")
snippetGenerator.CreateCodeSnippets("java/src/META-INF","../DB4OFlare/Content/CodeExamples/META-INF","java")
snippetGenerator.CreateCodeSnippets("dotNet/CSharpExamples/Code","../DB4OFlare/Content/CodeExamples","csharp")
snippetGenerator.CreateCodeSnippets("silverlight/silverlight/Code","../DB4OFlare/Content/CodeExamples","csharp")

# DRS
snippetGenerator.CreateCodeSnippets("javaDRS/src/com/db4odoc","../DRSFlare/Content/CodeExamples","java")
snippetGenerator.CreateCodeSnippets("javaDRS","../DRSFlare/Content/CodeExamples/javaDRS","java")
snippetGenerator.CreateCodeSnippets("dotNetDRS/CSharpExamples/Code","../DRSFlare/Content/CodeExamples","csharp")

# enhancement-examples
snippetGenerator.CreateCodeSnippets("javaEnhancement","../DB4OFlare/Content/CodeExamples","java")
snippetGenerator.CreateCodeSnippets("dotNetEnhancement/","../DB4OFlare/Content/CodeExamples","csharp")

# mini-example-applications
snippetGenerator.CreateCodeSnippets("javaAppExamples/","../DB4OFlare/Content/CodeExamples","java")
snippetGenerator.CreateCodeSnippets("dotNetAppExamples/","../DB4OFlare/Content/CodeExamples","csharp")

# Stuff which is shared between .NET and Java
snippetGenerator.CreateCodeSnippets("sharedAndMixed/java/src/com/db4odoc","../DB4OFlare/Content/CodeExamples","")

# vb-stuff with other template:
snippetGenerator.CreateCodeSnippets("dotNet/VisualBasicExamples/Code","../DB4OFlare/Content/CodeExamples","vb")
snippetGenerator.CreateCodeSnippets("dotNetDRS/VisualBasicExamples/Code","../DRSFlare/Content/CodeExamples","vb")
snippetGenerator.CreateCodeSnippets("silverlight/silverlightVB/Code","../DB4OFlare/Content/CodeExamples","vb")

aggreatedSnippet = SnippetAggregator(File.ReadAllText("../SnippetGenerator/SnippetGenerator/AggregateSnippetTemplate.flsnp"),tsCondition)
aggreatedSnippet.BuildAggregateSnippets(Directory.CreateDirectory("../DB4OFlare/Content/CodeExamples"))
aggreatedSnippet.BuildAggregateSnippets(Directory.CreateDirectory("../DRSFlare/Content/CodeExamples"))

tsCondition.PersistInfo()


