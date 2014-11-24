using System;

namespace Db4oTutorial.ExampleRunner
{
    public class CompilationException : Exception
    {
        public CompilationException(string message) : base(message)
        {
        }
    }
}