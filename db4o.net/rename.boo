import Useful.IO from Boo.Lang.Useful
import System.IO
import System.Text.RegularExpressions

callable StringFunction(contents as string) as string

class FileReplacer:
	
	[property(Path)]
	_path as string
	
	[property(Function)]
	_function as StringFunction

	def process(fname as string):
		original = TextFile.ReadFile(fname)
		contents = _function(original)
		if original != contents:
			print fname
			TextFile.WriteFile(fname, contents)	
	
	def run():
		for fname as string in listFiles(_path):
			if ".svn" in fname: continue
			if not fname.EndsWith(".cs"): continue
			process(fname)
			
class Globals:
	public static final interfaces = (
		"Db4oCollection",
		"Db4oCollections",
		"ObjectContainer", 
		"ExtObjectContainer",
		"DiagnosticListener",
		"ReflectArray",
		"ReflectClass",
		"ReflectConstructor",
		"ReflectField",
		"ReflectMethod",
		"Reflector",
		"Db4oList",
		"Db4oTypeImpl",
		"Evaluation",
		"ObjectTranslator",
		"ObjectConstructor",
		"Db4oMap",
		"TransactionListener",
		"ObjectSet",
		"QueryResult",
		"Configuration",
		"Candidate",
		"ExtObjectSet",
		"QueryComparator",
		"Closure4",
		"Visitor4",
		"Db4oEnhancedFilter",
	)	

def replaceInterfaces(contents as string):
	lines = /\n/.Split(contents)
	for line in lines:
		if line.StartsWith("using") or line.StartsWith("namespace"):
			continue
		for interfaceName in Globals.interfaces:			
			line = regex("\\b${interfaceName}\\b").Replace(line) do (match as Match):
				name = match.Groups[0].ToString()
				return "I${name}"
	return join(lines, "\n")
	
def toPascalCase(s as string):
	if len(s) < 3: return s.ToUpper()
	return s[:1].ToUpper() + s[1:]
	
def toPascalCaseNamespace(s as string):
	return join(toPascalCase(part) for part in /\./.Split(s), ".")
	
def replace(s as string, replacements as ((string))):
	for old, new in replacements:
		s = s.Replace(old, new)
	return s
	
def replaceNamespaces(contents as string):
	prefixes = (
		("com.db4o", "Db4objects.Db4o"),
		("j4o", "Sharpen"),
	)
	return /(?<fullname>(j4o|com\.db4o).*?)(?<suffix>\s|\(|\)|;)/.Replace(contents) do (match as Match):
		before = match.Groups["fullname"].ToString()
		after = toPascalCaseNamespace(replace(before, prefixes)) 
		suffix = match.Groups["suffix"].ToString()
#		print before, "=>", "${after}${suffix}"
		return "${after}${suffix}"
			
#FileReplacer(
#	Path: "../db4o.net/Db4objects.Db4o/native",
#	Function: replaceInterfaces).run()

