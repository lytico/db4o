using System;
using Db4oTutorial.ExampleRunner.Demos;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;
using Console = Db4oTutorial.ExampleRunner.ConsoleOutReplacement;

namespace AnoNp
{
    class CodeHolder { public void Run(IObjectContainer container) { var cars = from Car c in container select c; Console.Out.Write(cars.Count()); } };
}