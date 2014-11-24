using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using Db4objects.Db4o;
using System.Reflection;
using System.Text.RegularExpressions;
using System.IO;

namespace LinqConsole
{
	public interface IInteractiveQuery
	{
		void Run();
	}

	class Program
	{
		static void Main(string[] args)
		{
            new Program().Run();
        }

        public void Run()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (object sender, ResolveEventArgs args) => Assembly.LoadFrom(modelAssembly);
            Console.WriteLine(@"LinqConsole .02

Type a linq query followed by a semicolon to see its results.
Type :quit to quit the application.
Type :help to get a list of valid commands

Example:
>
> :database ""c:\temp\myDataSource.odb""
> :load ""MyProject.ObjectModel""
> from Item item in database select new { item.Code };
> :quit
");
            EvalLoop();
		}

        //Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        //{
        //    throw new NotImplementedException();
        //}

        private static string ImportsFor(HashSet<string> namespaces)
        {
            StringBuilder builder = new StringBuilder();
            namespaces.All(s => { builder.AppendFormat("using {0};{1}", s, Environment.NewLine); return true; });
            
            return builder.ToString();
        }

        private string GetNameSpaces(string assemblyPath)
        {
            Assembly assembly = Assembly.LoadFrom(assemblyPath);
            
            HashSet<string> namespaces = new HashSet<string>();
            foreach (Type type in assembly.GetTypes())
            {
                if (String.IsNullOrEmpty(type.Namespace)) continue;
                namespaces.Add(type.Namespace);
            }

            return ImportsFor(namespaces);
        }

        private void LoadModelAssembly(string assemblyPath)
        {
            if (File.Exists(assemblyPath))
            {
                modelAssembly = assemblyPath;
            }
            else
            {
                ConsoleHelper.With(
                    ConsoleColor.White, ConsoleColor.Red, () => Console.WriteLine("Unable to load {0}.", assemblyPath));
            }
        }

        private void ExecuteQuery(string querySpec)
        {
            try
            {
                IInteractiveQuery query = CompileQuery(querySpec, databasePath, modelAssembly);
                if (query != null)
                {
                    query.Run();
                }
            }
            catch (Exception ex)
            {
                ConsoleHelper.With(ConsoleColor.White, ConsoleColor.Red, () => Console.WriteLine(ex.ToString()));
            }
        }

        private void SetDatabasePath(string path)
        {
            if (!File.Exists(path))            
            {
                ConsoleHelper.With(ConsoleColor.White, ConsoleColor.Red, () => Console.WriteLine("Database file '{0}' not found.", path));
                return;
            }
             
            databasePath = path;
        }

        private IList<Command> InitializeCommands()
        {
            IList<Command> commands = new List<Command>();

            commands.Add(new Command("help", "Displays this help.", dummy => 
            {
                ConsoleHelper.With(ConsoleColor.Black, ConsoleColor.DarkGreen, () =>
                {
                    Console.WriteLine("{0}Command  \tDescription{0}" + 
                                         "===================================================================={0}", Environment.NewLine);
                    foreach (Command cmd in commands)
                    {
                        Console.WriteLine("{0,-10}\t{1}", cmd.Name, cmd.Description);
                    }
                    Console.WriteLine();
                });
            }));

            commands.Add(new Command("load", "Loads an assembly containing types stored in the database.", LoadModelAssembly));
            commands.Add(new Command("database", "Sets the database file's path to be used.", SetDatabasePath));
            commands.Add(quitCommand = new Command("quit", "Quits the application.", noOp => { }));
            commands.Add(new Command("query", "", ExecuteQuery));

            return commands;
        }

        private void EvalLoop()
        {
            IList<Command> commands = InitializeCommands();
            Command command = null;
            do
            {
                string line = ReadInput(s => s.StartsWith(":") || s.EndsWith(";"));
                command = ProcessCommand(commands, line);                
            } while (command != quitCommand);
        }

        private static Command ProcessCommand(IList<Command> commands, string line)
        {
            Regex commandParser = new Regex(@":(?<quit>quit)$|" +
                                            @":database \""(?<database>([a-z]\:){0,1}(\w|\\|\.)+)\""$|" +
                                            @":load \""(?<load>([a-z]\:){0,1}(\w|\\|\.)+)\""$|" +
                                            @":(?<help>help)$|" +
                                            @"(?<query>from ((.*)));$");
                         
            if (commandParser.IsMatch(line))
            {
                Match match = commandParser.Match(line);
                foreach (Command command in commands)
                {
                    string commandArgs = match.Groups[command.Name].Value;
                    if (!String.IsNullOrEmpty(commandArgs))
                    {
                        command.Action(commandArgs);
                        return command;
                    }
                }
            }

            return null;
         }

        private static string EscapePath(string path)
        {
            return path.Replace(@"\", @"\\");
        }
        private IInteractiveQuery CompileQuery(string queryString, string databasePath, string modelAssembly)
		{
            string referencedNameSpaces = GetNameSpaces(modelAssembly);
			string code = @"
using System;
using System.Linq;
using LinqConsole;
using Db4objects.Db4o.Linq;
using Db4objects.Db4o;" +
referencedNameSpaces +
@"

public class TheQuery : IInteractiveQuery
{
	public void Run()
	{
        using(IObjectContainer database = Db4oFactory.OpenFile(""" + EscapePath(databasePath) + @"""))
        {
		    var result = " + queryString + @";
		    result.PrettyPrint();
        }
	}
}";			
			var compiled = CompileType("TheQuery", code, modelAssembly);
			if (null == compiled) return null;
			return (IInteractiveQuery) Activator.CreateInstance(compiled);
		}

        private static CompilerParameters CompilerParameters(string modelAssembly)
        {
            CompilerParameters parameters = new CompilerParameters();
            parameters.IncludeDebugInformation = false;
            parameters.GenerateInMemory = true;
            parameters.ReferencedAssemblies.Add(AssemblyFor(typeof(IInteractiveQuery)));
            parameters.ReferencedAssemblies.Add(AssemblyFor(typeof(System.Linq.Enumerable)));
            parameters.ReferencedAssemblies.Add(AssemblyFor(typeof(Db4objects.Db4o.Linq.ObjectContainerExtensions)));
            parameters.ReferencedAssemblies.Add(AssemblyFor(typeof(Db4objects.Db4o.IObjectContainer)));
            parameters.ReferencedAssemblies.Add(modelAssembly);
            
            return parameters;
        }

        private static Type CompileType(string typeName, string type, string modelAssembly)
		{
			using (var provider = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v3.5" } }))
			{
                var fname = SaveToFile(type);
				CompilerResults results = provider.CompileAssemblyFromFile(CompilerParameters(modelAssembly), fname);
				if (results.Errors.Count > 0)
				{	
					foreach (var e in results.Errors)
					{
                        ConsoleHelper.With(ConsoleColor.Yellow, ConsoleColor.Red, () => Console.WriteLine(e));
					}
					return null;
				}
				return results.CompiledAssembly.GetType(typeName);
			}
		}

		private static string SaveToFile(string code)
		{
			var fname = System.IO.Path.GetTempFileName() + ".cs";
			System.IO.File.WriteAllText(fname, code);
			return fname;
		}

		private static string AssemblyFor(Type type)
		{
			return type.Assembly.ManifestModule.FullyQualifiedName;
		}

        private static void WritePrompt()
        {
            ConsoleHelper.With(ConsoleColor.White, ConsoleColor.DarkBlue, () => Console.Write(">"));
            Console.Write(" ");
        }

        private static string ReadInput(Func<string, bool> inputFinished)
        {
            WritePrompt();
            StringBuilder builder = new StringBuilder();

            ConsoleHelper.With(ConsoleColor.Black, ConsoleColor.Green,
                () =>
                {

                    string line = null;
                    while (null != (line = Console.ReadLine()))
                    {
                        line = line.Trim();

                        builder.Append(line);
                        if (inputFinished(line)) break;

                        builder.Append(" ");
                        WritePrompt();
                    }

                });
            return builder.ToString();
        }

        private Command quitCommand;
        private string databasePath;
        private string modelAssembly;
	}

    internal class Command
    {
        public Command(string name, string description, Action<string> action)
        {
            Name = name;
            Description = description;
            Action = action;
        }

        public string Name
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public Action<string> Action
        {
            get;
            set;
        }
    }
}