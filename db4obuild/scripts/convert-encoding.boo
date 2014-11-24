import Useful.CommandLine from Boo.Lang.Useful
import System
import System.Text
import System.IO

def convert(fname as string, fromEncoding as Encoding, toEncoding as Encoding):
	writeAll(fname, toEncoding, readAll(fname, fromEncoding))
	
def writeAll(fname as string, encoding as Encoding, contents as string):
	using writer=StreamWriter(fname, false, encoding):
		writer.Write(contents)
		
def readAll(fname as string, encoding as Encoding):
	using reader=StreamReader(fname, encoding):
		return reader.ReadToEnd()
		
fromEncoding = Encoding.GetEncoding("utf-8")
toEncoding = Encoding.GetEncoding("iso-8859-1")
for fname in argv:
	print fname
	convert(fname, fromEncoding, toEncoding)