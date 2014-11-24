using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;
using Db4oTutorial.ExampleRunner.Utils;
using Mono.CSharp;



namespace Db4oTutorial.ExampleRunner
{
    class CodeCompiler
    {
        public OperationResult<Func<IObjectContainer,String>> Compile(string theCode)
        {
            var errors = new Printer();
            var eval = CreateEvaluator(errors);
            AddReferencedAssemblies(eval);
            AddDefaultNamespaces(eval);
            CreateEvaluationObject(eval, ReplaceConsoleOut(theCode));
            var result = eval.Evaluate("new CodeHolder();");
            var runMethod = result.GetType().GetMethod("Run");
            if(errors.Errors.Length!=0)
            {
                return OperationResult.Failure<Func<IObjectContainer, String>>(
                    new CompilationException("Compilation error: "+errors.Errors));
            }
            return OperationResult.Success<Func<IObjectContainer, String>>(
                (container) => UnboxExceptions(runMethod, result, container));
        }

        /// <summary>
        /// On Silverlight we cannot use Console.SetOut() to redirect the standard out.
        /// Therefore we replace it with our implementation.
        /// 
        /// </summary>
        /// <param name="theCode"></param>
        /// <returns></returns>
        private string ReplaceConsoleOut(string theCode)
        {
            return theCode.Replace("System.Console.", "Console.")
                .Replace("Console.Write","Console.Out.Write");
        }

        private string UnboxExceptions(MethodInfo runMethod,
            object result, IObjectContainer container)
        {
            try
            {
                return ConsoleOutReplacement.RunInContext(
                    () => runMethod.Invoke(result, new[] {container}));

            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException;
            }
        }

        private void CreateEvaluationObject(Evaluator eval, string theCode)
        {
            var code = CreateCode(theCode);
            eval.Run(code);
        }

        private static string CreateCode(string theCode)
        {
            return "" +
                   "using System.Linq;"
                   + "class CodeHolder{" +
    "public void Run(IObjectContainer container){"
+ theCode + "}};";
        }

        private void AddDefaultNamespaces(Evaluator eval)
        {
            eval.Run("using System;");
            eval.Run("using System.Collections.Generic;");
            eval.Run("using System.Linq;");
            eval.Run("using Db4objects.Db4o;");
            eval.Run("using Db4objects.Db4o.Linq;");
            eval.Run("using Db4oTutorial.ExampleRunner.Demos;");
            eval.Run("using Console = Db4oTutorial.ExampleRunner.ConsoleOutReplacement;");
        }

        private void AddReferencedAssemblies(Evaluator eval)
        {
            eval.ReferenceAssembly(typeof (CodeCompiler).Assembly);
            eval.ReferenceAssembly(typeof (IObjectContainer).Assembly);
            eval.ReferenceAssembly(typeof (ObjectContainerExtensions).Assembly);
            eval.ReferenceAssembly(typeof(Enumerable).Assembly);
        }

        private Evaluator CreateEvaluator(Printer errors)
        {
            var report = new Report(errors);
            var settings = new CommandLineParser(report).ParseArguments(new string[] {});
            return new Evaluator(settings, report);
        }


        public class Printer : ReportPrinter
        {
            private readonly StringBuilder messages = new StringBuilder();

            public override void Print(AbstractMessage msg)
            {
                messages.Append(msg.Text);
            }

            public string Errors
            {
                get { return messages.ToString(); }
            }
        }
    }
}