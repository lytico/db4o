import Useful.IO from Boo.Lang.Useful
import System
import System.IO
import System.Text

def isSourceFile(fname as string):
	if ".svn" in fname: return false
	if fname.EndsWith(".cs"): return true
	if fname.EndsWith(".java"): return true
	return false
	
def listSources(path as string):
	for fname as string in listFiles(path):
		if isSourceFile(fname):
			yield fname
			
def firstLine(fname as string):
	using reader=File.OpenText(fname):
		return reader.ReadLine()
		
def prependToFile(fname as string, prefix as string):
	writeAll(fname, prefix + readAll(fname))
	
def writeAll(fname as string, contents as string):
	using writer=StreamWriter(fname, false):
		writer.Write(contents)
		
def readAll(fname as string):
	using reader=StreamReader(fname):
		return reader.ReadToEnd()

header = readAll("config/copyright_comment.txt")

srcDirs = (
	"db4o.net/Db4objects.Db4o/native",
	"db4o.net/Db4objects.Db4o.Tests/native",
	"db4o.net/Db4objects.Db4o.TA.Tests/native",
	"db4o.net/Db4objects.Db4o.Instrumentation/native",
	"db4o.net/Db4objects.Db4o.NativeQueries/native",
	"db4o.net/Db4oTool",
	"db4o.net/Db4oUnit/native",
	"db4o.net/Db4oUnit.Extensions/native",
	"db4o.net/sharpen",
	"db4oj",
	"db4oj.tests",
	"db4ota",
	"db4otaj",)

for srcDir in srcDirs:
	for fname in listSources(Path.Combine("..", srcDir)):
		if "Copyright (C)" not in firstLine(fname):
			print fname #, "\n\t", firstLine(fname)
			prependToFile(fname, header)
