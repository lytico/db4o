using System;
using Db4oTutorial.ExampleRunner.Utils;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.IO;

namespace Db4oTutorial.ExampleRunner
{
    class SnippetExecutor
    {
        private readonly CodeCompiler compiler = new CodeCompiler();
        public string RunSnippet(string codeToRun)
        {
            var result = compiler.Compile(codeToRun);
            if(result.IsFailure)
            {
                return result.Exception.Message+"\n"+result.Exception.StackTrace;
            }
            else
            {
                using(var container = OpenContainer())
                {
                    return CatchException(container, result);
                }
            }
        }

        private static string CatchException(IObjectContainer container, OperationResult<Func<IObjectContainer, string>> result)
        {
            try
            {
                return result.Result(container);
            }
            catch (Exception e)
            {
                return e.StackTrace;
            }
        }

        private IObjectContainer OpenContainer()
        {
            var config = Db4oEmbedded.NewConfiguration();
            config.File.Storage = new MemoryStorage();
            return Db4oEmbedded.OpenFile(config, "In:Memory!");
        }
    }
}