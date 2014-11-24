namespace SnippetGenerator

import System.IO
import Ionic.Zip
import System
import System.Linq;
import System.Globalization;
import System.Collections.Generic.IEnumerable
import System.Linq.Enumerable from 'System.Core'
import System.Linq from 'System.Core'
import System.Web from 'System.Web'

class CodeZip:
	final allreadyGenerated = {}
	final IgnoreFile = ".snippet-generator.include"
	final RemoveFromZip = ".snippet-generator.nodescend"
	
	
	def ZipDirectory(directoryToZip as DirectoryInfo, directoryToStore as DirectoryInfo, name as string):		
		fileName = directoryToStore.FullName+"/"+ BuildName(directoryToZip)+"-${name}.zip"
		if allreadyGenerated.Contains(directoryToZip.FullName):
			return allreadyGenerated[directoryToZip.FullName]
		File.Delete(fileName)
		allreadyGenerated.Add(directoryToZip.FullName,Path.GetFileName(fileName));
		zipFile= ZipFile(fileName)
		AddFiles(zipFile,directoryToZip)
		RemoveMagicFiles(zipFile);
		zipFile.Save()
		zipFile.Dispose()
		return Path.GetFileName(fileName)
	
	def AddFiles(zipFile as ZipFile,directoryToZip as DirectoryInfo):
		explicitIncludes = directoryToZip.GetFiles(IgnoreFile)		
		if explicitIncludes.Length > 0:
			for line in File.ReadAllLines(explicitIncludes[0].FullName):
				path = Path.Combine(directoryToZip.FullName,line)
				if File.Exists(path):
					zipFile.AddFile(path,"")
				elif line.Contains(".."):
					AddParentFiles(zipFile, path)
				else:
					zipFile.AddDirectory(path,line)
		else:
			zipFile.AddDirectory(directoryToZip.FullName)

	def AddParentFiles(zipFile as ZipFile, path as string):
		files = Directory.CreateDirectory(path).GetFiles()
		for file as FileInfo in files:
			zipFile.AddFile(file.FullName,"")
				
		

	def RemoveMagicFiles(zipFile	 as ZipFile):
		for entry as string in zipFile.EntryFileNames.ToList():
			if entry.EndsWith(RemoveFromZip):
				zipFile.RemoveEntry(entry)	
	
	def BuildName(directoryToZip as DirectoryInfo):
		return "Example-"+directoryToZip.Parent.Name +"-"+ directoryToZip.Name
