namespace SnippetGenerator


import System.IO
import Ionic.Zip
import System
import System.Linq;
import System.Globalization;
import System.Collections.Generic.IEnumerable
import System.Collections
import System.Linq.Enumerable from 'System.Core'
import System.Linq from 'System.Core'
import System.Web from 'System.Web'

# Generate snippets out of the source-code

class TimeStampStorage:
	public static final TimeStampFile = ".tsInfo"
	final timesStamps = {}
	
	def constructor():
		if(File.Exists(TimeStampFile)):
			lines = File.ReadAllLines(TimeStampFile)		
			for line in lines:
				splitted = line.Split(char('*'))
				file = splitted[0]
				fileDate = long.Parse(splitted[1])
				timesStamps[file] = fileDate
			File.Delete(TimeStampFile)
	
	def NeedToAdd(file as FileInfo):
		if not timesStamps.ContainsKey(file.FullName) or cast(long,timesStamps[file.FullName]) < file.LastWriteTime.Ticks:
			timesStamps[file.FullName] = file.LastWriteTime.Ticks
			return true
		else:
			return false
			
	def PersistInfo():
		tw = StreamWriter(".tsInfo");
		for keyValue in timesStamps:
			fileDate = cast(long,keyValue.Value);
			line = keyValue.Key.ToString() + "*" + fileDate
			tw.WriteLine(line);
		tw.Close();

class SnippetGenerator:
	final SnippetPrefix = "#example:"
	# For example in in a snippet from a XML-file we want to avoid the include the end-of-comment-symbols
	final SnippetPrefixEndMarker = "#"
	final SnippetEnd = "#end example"
	final Template as string
	final zipGenerator  as CodeZip
	final syntaxTransformer = SyntaxHighLighter()
	
	final tsCondition as TimeStampStorage
	
	final languageToCondition = {"java":"Primary.Java","csharp":"Primary.c#","vb":"Primary.VB.NET"}
	
	def constructor(template as string, zipGenerator as CodeZip,tsCondition as TimeStampStorage):
		self.Template = template
		self.zipGenerator = zipGenerator
		self.tsCondition = tsCondition

	def HandleCodeFile(file as FileInfo, snippetDir as DirectoryInfo, language as string):
		if tsCondition.NeedToAdd(file):
				DoHandleCodeFile(file,snippetDir,language)

	def DoHandleCodeFile(file as FileInfo, snippetDir as DirectoryInfo, language as string):
		allLines = File.ReadAllLines(file.FullName)
		inRecordingState = false
		snippetName = ""
		snippetText = []
		for line in allLines:
			if IsSnippetStart(line):
				snippetName = ExtractName(line)
				inRecordingState = true
				snippetText = []
			elif IsSnippetEnd(line):
				zipFile = zipGenerator.ZipDirectory(file.Directory,snippetDir,language)
				normalizedCodeLines = NormalizeText(array(string,snippetText))
				normalizedCode = join(normalizedCodeLines,"\n")
				syntaxColoredCode = syntaxTransformer.Translate(normalizedCode,file.Name)
				filePath = SnippetName(file.Name,snippetName.Replace(' ','-'),snippetDir.FullName)
				snippetTitle = file.Name+":${snippetName}"
				WriteSnippet(syntaxColoredCode, filePath, zipFile,language,snippetTitle)
			elif inRecordingState:
				snippetText.Add(line)
							
	def ExtractName(line as string):
		lineWithoutStart = line.Substring(line.IndexOf(SnippetPrefix)+SnippetPrefix.Length)
		return lineWithoutStart.Split(array(SnippetPrefixEndMarker),StringSplitOptions.None)[0]
			
	def IsSnippetStart(line as string):
		return line.Contains(SnippetPrefix)		
		
	def IsSnippetEnd(line as string):
		return line.Contains(SnippetEnd)
		
	def WriteSnippet(snippetText as string, filePath as string, zipFile as string, language as string, snippetTitle as string):
		content = string.Format(Template,snippetText,zipFile,languageToCondition[language],snippetTitle)
		File.WriteAllText(filePath,content)
	
	def SnippetName(originalFileName as string, snippetName as string, location as string):
		fileName = Path.GetFileNameWithoutExtension(originalFileName) + snippetName + Path.GetExtension(originalFileName) + ".flsnp"
		return Path.Combine(location,fileName)
	
	def NormalizeText(text  as (string)):
		numberOfIndent = int.MaxValue
		for line in text:
			if not string.IsNullOrEmpty(line):
				currentLineNumber = line.TakeWhile({c | c == char(' ') or c==char('\t')}).Count()
				numberOfIndent = Math.Min(currentLineNumber,numberOfIndent)
		result = []
		for  line in text:
			if numberOfIndent <= line.Length:
				result.Add(line.Substring(numberOfIndent))
			else:
				result.Add(line)
		return array(string,result)



class CodeToSnippets:
	final NoDescendDescription = ".snippet-generator.nodescend"
	final _SnippetGeneration as SnippetGenerator
	final IgnoredFilesWithEnding = (".jar",".dll",".class",".db4o",".dat")

	def constructor(templatePath as string, zipGenerator as CodeZip,tsCondition as TimeStampStorage):
		_SnippetGeneration = SnippetGenerator(File.ReadAllText(templatePath),zipGenerator,tsCondition)

	def HandleSubDirectory(sourceDir as DirectoryInfo, snippetDir as DirectoryInfo, language as string):
		directoryInSnippets=snippetDir.CreateSubdirectory(sourceDir.Name)
		CreateCodeSnippets(sourceDir,directoryInSnippets,language)
		if directoryInSnippets.GetFiles().Length == 0 and directoryInSnippets.GetDirectories().Length == 0:
			directoryInSnippets.Delete(false)

		
	def CreateCodeSnippets(sourceDir as DirectoryInfo, snippetDir as DirectoryInfo, language as string):
		for file in sourceDir.GetFiles():
			if not IsIgnored(file):
				_SnippetGeneration.HandleCodeFile(file,snippetDir,language)
		ignoredDirectories = ListIgnoredDirectories(sourceDir)
		for dir in sourceDir.GetDirectories():
			if not ignoredDirectories.ContainsKey(dir.FullName):
				HandleSubDirectory(dir,snippetDir,language)
	
	def ListIgnoredDirectories(sourceDir as DirectoryInfo):		
		files = sourceDir.GetFiles(NoDescendDescription)
		forbidden = {}
		if files.Length == 1:
			for dirName in File.ReadAllLines(files[0].FullName):
				fullName = Path.Combine(sourceDir.FullName,dirName)
				forbidden.Add(fullName,true)		
		return forbidden

	def CreateCodeSnippets(sourceDir as string, snippetDir as string, language as string):
		CreateCodeSnippets(Directory.CreateDirectory(sourceDir),Directory.CreateDirectory(snippetDir),language)

	def IsIgnored(file as FileInfo):
		fileName = file.Name
		for ending in IgnoredFilesWithEnding:
			if fileName.EndsWith(ending):
				return true
		return false
		
		
class SnippetAggregator:
	final Template as string
	final tsCondition as TimeStampStorage
	final Ending = ".all.flsnp" 
			
	def constructor(template as string,tsCondition as TimeStampStorage):
		Template = template	
		self.tsCondition = tsCondition
		
	def CreateAggregateSnippet(name as string, files as List):
		if NeedNewTemplate(files):
			GenerateNewAggregateSnippet(name,files)
	
	def GenerateNewAggregateSnippet(name as string, files as List):
		linkString = ""
		for file as FileInfo in files:
			fileName = file.Name
			linkString += 	"""<MadCap:snippetBlock src="${fileName}"/>"""
		filePath = cast(FileInfo, files[0]).Directory.FullName +"\\${name}"+Ending
		content = string.Format(Template,linkString)
		File.WriteAllText(filePath,content)
	
	def NeedNewTemplate(files as List):
		needNewTemplate = false
		for file as FileInfo in files:
			if tsCondition.NeedToAdd(file) and IsNotAggretionItsefl(file):
				needNewTemplate = true
		return needNewTemplate
	
	def IsNotAggretionItsefl(file as FileInfo):
		return not file.FullName.EndsWith(Ending)
	
	def BuildAggregateSnippets(directory as DirectoryInfo):
		filesToConsider = {}
		for file in directory.GetFiles("*.flsnp"):
			name = file.Name.Split(char('.'))[0]
			if filesToConsider.Contains(name) and IsNotAggretionItsefl(file):
				(filesToConsider[name] as List).Add(file)
			elif IsNotAggretionItsefl(file):
				filesToConsider[name] = [file]
		for	entry in filesToConsider:
			CreateAggregateSnippet(entry.Key,entry.Value as List)
		for	subDir in directory.GetDirectories():
			BuildAggregateSnippets(subDir)
			
	def BuildAggregateSnippets(directory as string, snippteTemplate as string):
		BuildAggregateSnippets(Directory.CreateDirectory(directory))
	
	

