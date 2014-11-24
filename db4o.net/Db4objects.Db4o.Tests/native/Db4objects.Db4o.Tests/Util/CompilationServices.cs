/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
namespace Db4objects.Db4o.Tests.Util
{
#if !CF && !SILVERLIGHT
	using System;
	using System.CodeDom.Compiler;
	using System.IO;
	using System.Reflection;
	using System.Text;

	/// <summary>
	/// Compilation helper.
	/// </summary>
	public class CompilationServices
	{
		public static void EmitAssembly(string assemblyFileName, params string[] code)
		{
			string[] references = {
				typeof(Db4oFactory).Module.FullyQualifiedName,
				typeof(CompilationServices).Module.FullyQualifiedName 
			};
			EmitAssembly(assemblyFileName, references, code);
		}

		public static void EmitAssembly(string assemblyFileName, string[] references, params string[] code)
		{
			string basePath = Path.GetDirectoryName(assemblyFileName);
			CreateDirectoryIfNeeded(basePath);

			string[] sourceFiles = WriteSourceFiles(Path.GetTempPath(), code);
			CompileFiles(assemblyFileName, references, sourceFiles);
		}

		public static void CreateDirectoryIfNeeded(string directory)
		{
			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}
		}

		static string[] WriteSourceFiles(string basePath, string[] code)
		{
			string[] sourceFiles = new string[code.Length];
			for (int i=0; i<code.Length; ++i)
			{
				string sourceFile = Path.Combine(basePath, "source" + i + ".cs");
				WriteFile(sourceFile, code[i]);
				sourceFiles[i] = sourceFile;
			}
			return sourceFiles;
		}

		static void WriteFile(string fname, string contents)
		{
			using (StreamWriter writer = new StreamWriter(fname))
			{
				writer.Write(contents);
			}
		}

#if !CF
		static CompilerInfo GetCSharpCompilerInfo()
		{
			return CodeDomProvider.GetCompilerInfo(CodeDomProvider.GetLanguageFromExtension(".cs"));
		}
#endif

		static CodeDomProvider GetCSharpCodeDomProvider()
		{
#if !CF && !MONO
			return GetCSharpCompilerInfo().CreateProvider();
#else
			Type provider = typeof(System.Uri).Assembly.GetType("Microsoft.CSharp.CSharpCodeProvider");
			return (CodeDomProvider)Activator.CreateInstance(provider);
#endif
		}

		static CompilerParameters CreateDefaultCompilerParameters()
		{
#if !CF && !MONO
			return GetCSharpCompilerInfo().CreateDefaultCompilerParameters();
#else
			return new CompilerParameters();
#endif
		}

		public static void CompileFiles(string assemblyFName, string[] references, string[] files)
		{
			using (CodeDomProvider provider = GetCSharpCodeDomProvider())
			{
				CompilerParameters parameters = CreateDefaultCompilerParameters();
				parameters.IncludeDebugInformation = false;
				parameters.OutputAssembly = assemblyFName;
				parameters.GenerateExecutable = ".exe" == Path.GetExtension(assemblyFName).ToLower();
				parameters.ReferencedAssemblies.AddRange(references);

				ICodeCompiler compiler = provider.CreateCompiler();
				CompilerResults results = compiler.CompileAssemblyFromFileBatch(parameters, files);
                if(ContainsErrors(results.Errors))
                {
                    throw new ApplicationException(GetErrorString(results.Errors));
                }
			}
		}

        private static Boolean ContainsErrors(CompilerErrorCollection errors)
        {
            foreach (CompilerError error in errors)
            {
                if (! error.IsWarning)
                {
                    return true;
                }
            }
            return false;
        }

		static string GetErrorString(CompilerErrorCollection errors)
		{
			StringBuilder builder = new StringBuilder();
			foreach (CompilerError error in errors)
			{
				builder.Append(error.ToString());
				builder.Append(Environment.NewLine);
			}
			return builder.ToString();
		}

		private CompilationServices()
		{
		}
	}
#endif
}
