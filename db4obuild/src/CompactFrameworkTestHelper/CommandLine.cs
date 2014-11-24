/*
 * Command Line Argument Parser
 * written by GriffonRL
 * http://www.codeproject.com/csharp/command_line.asp
 * */

using System;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

// TODO: check license and include it here or better yet use Mono.GetOptions instead which we
// are already using for Db4oAdmin

namespace CommandLine.Utility {
	/// <summary>
	/// Arguments class
	/// </summary>
	/// <example>
	///	static void Main(string[] Args)
	///	{
	///		// Command line parsing
	///		Arguments CommandLine=new Arguments(Args);
	///	
	///		if(CommandLine["param1"] != null) 
	///			Console.WriteLine("Param1 value: " + 
	///				CommandLine["param1"]);
	///		else
	///			Console.WriteLine("Param1 not defined !");
	/// }
	/// </example>
	public class Arguments {
		// Variables
		private StringDictionary Parameters;

		// Constructor
		public Arguments(string[] Args) {
			Parameters = new StringDictionary();
			Regex Spliter = new Regex(@"^-{1,2}|^/|=|:",
					RegexOptions.IgnoreCase | RegexOptions.Compiled);

			Regex Remover = new Regex(@"^['""]?(.*?)['""]?$",
					RegexOptions.IgnoreCase | RegexOptions.Compiled);

			string Parameter = null;
			string[] Parts;

			// Valid parameters forms:
			// {-,/,--}param{ ,=,:}((",')value(",'))
			// Examples: 
			// -param1 value1 --param2 /param3:"Test-:-work" 
			//   /param4=happy -param5 '--=nice=--'
			foreach (string Txt in Args) {
				// Look for new parameters (-,/ or --) and a
				// possible enclosed value (=,:)
				Parts = Spliter.Split(Txt, 3);

				switch (Parts.Length) {
					// Found a value (for the last parameter 
					// found (space separator))
					case 1:
						if (Parameter != null) {
							if (!Parameters.ContainsKey(Parameter)) {
								Parts[0] =
										Remover.Replace(Parts[0], "$1");

								Parameters.Add(Parameter, Parts[0]);
							}
							Parameter = null;
						}
						// else Error: no parameter waiting for a value (skipped)
						break;

					// Found just a parameter
					case 2:
						// The last parameter is still waiting. 
						// With no value, set it to true.
						if (Parameter != null) {
							if (!Parameters.ContainsKey(Parameter))
								Parameters.Add(Parameter, "true");
						}
						Parameter = Parts[1].ToLowerInvariant();
						break;

					// Parameter with enclosed value
					case 3:
						// The last parameter is still waiting. 
						// With no value, set it to true.
						if (Parameter != null) {
							if (!Parameters.ContainsKey(Parameter))
								Parameters.Add(Parameter, "true");
						}

						Parameter = Parts[1];

						// Remove possible enclosing characters (",')
						if (!Parameters.ContainsKey(Parameter)) {
							Parts[2] = Remover.Replace(Parts[2], "$1");
							Parameters.Add(Parameter, Parts[2]);
						}

						Parameter = null;
						break;
				}
			}
			// In case a parameter is still waiting
			if (Parameter != null) {
				if (!Parameters.ContainsKey(Parameter))
					Parameters.Add(Parameter, "true");
			}
		}

		// Retrieve a parameter value if it exists 
		// (overriding C# indexer property)
		public string this[string Param] {
			get {
				return (Parameters[Param]);
			}
		}

		public bool HasKey(string key) {
			return this.Parameters.ContainsKey(key);
		}
	}
}
