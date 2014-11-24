namespace SnippetGeneratorTests

import System
import NUnit.Framework
import SnippetGenerator

[TestFixture]
class TestSyntaxHighLighter:
	final Java = """ 
	Pilot pilot = new Pilot("Joe");
    container.store(pilot);
	"""
	final CSharpCode = """ 
	Pilot pilot = new Pilot("Joe");
    container.Store(pilot);
	"""
	final VBNet = """
	Dim pilot As New Pilot("Joe")
    container.Store(pilot)
	"""
	final XML = """
	<hixml><element></element></hixml>
	"""
	testInstance as SyntaxHighLighter
	
	[SetUp]
	def Setup():
		testInstance = SyntaxHighLighter()

	[Test]
	def CSharp():
		result = testInstance.Translate(CSharpCode,"TheFile.cs")
		print result
		assert result.Contains("Pilot pilot = <span style")
		assert not result.Contains("<div")
		assert result.StartsWith("Pi")

	[Test]
	def VB():
		result = testInstance.Translate(VBNet,"TheFile.vb")
		print result
		assert result.Contains("Dim</span> pilot")
		assert not result.Contains("<div")
		assert result.StartsWith("<span")
		
	[Test]
	def Xml():
		result = testInstance.Translate(XML,"TheFile.xml")
		print result
		assert result.Contains("element")
		assert not result.Contains("<div")

	[Test]
	def AssumeJDOIsXML():
		result = testInstance.Translate(XML,"TheFile.jdo")
		print result
		assert result.Contains("element")
		assert not result.Contains("<div")

	[Test]
	def JavaTranslation():
		result = testInstance.Translate(Java,"TheFile.java")
		print result
		assert result.Contains("Pilot pilot = <span style")
		assert not result.Contains("<div")
		
	[Test]
	def AnyCodeIsPossible():
		result = testInstance.Translate(CSharpCode,"zomg")
		print result
		assert result.Contains("Pilot")

		
	[Test]
	def OtherCodeIsEncoded():
		result = testInstance.Translate(XML,"zomg")
		print result
		assert result.Contains("&lt;")
		assert result.Contains("&gt;")
		


