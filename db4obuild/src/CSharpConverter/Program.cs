using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Parser;
using ICSharpCode.NRefactory.Parser.AST;
using ICSharpCode.NRefactory.PrettyPrinter;

namespace CSharpConverter
{	
	public class Program
	{	
		public static void Main(string[] args)
		{	
			new Program(Options.Parse(args)).Run();
		}
		
		delegate void ConversionStep(IParser parser);
		
		Options _options;
		
		Program(Options options)
		{
			_options = options;	
		}
		
		void Run()
		{		
			foreach (string fname in ListFiles(_options.SourceDir, "*.cs"))
			{
				Console.Write(".");
				string targetFile = GetTargetFileName(fname);
				Directory.CreateDirectory(Path.GetDirectoryName(targetFile));
				WriteFile(targetFile, ConvertFile(fname));
			}
		}
		
		string GetTargetFileName(string fname)
		{
			string targetFile = Path.Combine(_options.TargetDir, fname.Substring(_options.SourceDir.Length + 1));
			if (_options.VisualBasicOutput) return Path.ChangeExtension(targetFile, ".vb");
			return targetFile;
		}
		
		string ConvertFile(string fname)
		{
			using (StreamReader reader = File.OpenText(fname))
			{
				return ConvertReader(reader);
			}
		}
		
		IParser ParseReader(System.IO.TextReader reader)
		{
			IParser parser = ParserFactory.CreateParser(SupportedLanguage.CSharp, reader);
			parser.Parse();
			return parser;
		}
		
		string ConvertReader(System.IO.TextReader reader)
		{
			IParser parser = ParseReader(reader);
			
			ConvertToPascalCase(parser.CompilationUnit);
			
			if (_options.VisualBasicOutput)
			{
				ConvertToVBNet(parser.CompilationUnit);
			}
			
			return Output(parser);
		}
		
		static void ConvertToPascalCase(CompilationUnit cu)
		{
			cu.AcceptVisitor(new PascalCaseConverter(), null);
		}
		
		static void ConvertToVBNet(CompilationUnit cu)
		{
			ToVBNetConvertVisitor converter = new ToVBNetConvertVisitor();
			cu.AcceptVisitor(converter, null);
		}
		
		string Output(IParser parser)
		{
			IOutputASTVisitor visitor = CreateOutputVisitor();
			visitor.Options = CreatePrettyPrintOptions();
			
			List<ISpecial> specials = parser.Lexer.SpecialTracker.CurrentSpecials;
			
			SpecialNodesInserter sni = new SpecialNodesInserter(specials,
			                                                    new SpecialOutputVisitor(visitor.OutputFormatter));
			visitor.NodeTracker.NodeVisiting += sni.AcceptNodeStart;			
			visitor.NodeTracker.NodeVisited += sni.AcceptNodeEnd;
			visitor.NodeTracker.NodeChildrenVisited += sni.AcceptNodeEnd;
			visitor.NodeTracker.NodeVisited += delegate(INode node) {
				if (IsMemberDeclaration(node))
				{
					visitor.OutputFormatter.NewLine();
				}
			};
			parser.CompilationUnit.AcceptVisitor(visitor, null);
			sni.Finish();
			return visitor.Text;
		}
		
		static bool IsMemberDeclaration(INode node)
		{	
			return node is MethodDeclaration
				|| node is FieldDeclaration
				|| node is PropertyDeclaration
				|| node is EventDeclaration
				|| node is ConstructorDeclaration;
		}
		
		IOutputASTVisitor CreateOutputVisitor()
		{
			if (_options.VisualBasicOutput) return new VBNetOutputVisitor();
			return new CSharpOutputVisitor();
		}
		
		static PrettyPrintOptions CreatePrettyPrintOptions()
		{
			PrettyPrintOptions options = new PrettyPrintOptions();
			options.PropertyBraceStyle = BraceStyle.NextLine;
			options.PropertyGetBraceStyle = BraceStyle.NextLine;
			options.PropertySetBraceStyle = BraceStyle.NextLine;
			options.ClassBraceStyle = BraceStyle.NextLine;
			options.IndentationChar = ' ';
			options.IndentSize = 4;
			options.TabSize = 4;			
			return options;
		}
		
		static IEnumerable<string> ListFiles(string dir, string glob)
		{
			foreach (string fname in Directory.GetFiles(dir, glob))
			{
				yield return fname;
			}
			foreach (string subDir in Directory.GetDirectories(dir))
			{
				foreach (string fname in ListFiles(subDir, glob))
				{
					yield return fname;
				}
			}
		}
		
		static void WriteFile(string fname, string contents)
		{
			using (StreamWriter writer = new StreamWriter(fname, false, System.Text.Encoding.ASCII))
			{
				writer.Write(contents);
			}
		}
	}
}
