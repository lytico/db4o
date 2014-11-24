using System;

namespace Db4objects.Db4o.Tutorial.Console
{
	class App
	{
		[STAThread]
		static void Main(string[] args)
		{
            Db4odoc.Tutorial.F1.Chapter1.FirstStepsExample.Main(args);
            Db4odoc.Tutorial.F1.Chapter2.StructuredExample.Main(args);
            Db4odoc.Tutorial.F1.Chapter3.OMEExample.Main(args);
            Db4odoc.Tutorial.F1.Chapter4.CollectionsExample.Main(args);
            Db4odoc.Tutorial.F1.Chapter5.InheritanceExample.Main(args);
            Db4odoc.Tutorial.F1.Chapter6.ClientServerExample.Main(args);
            Db4odoc.Tutorial.F1.Chapter8.TransparentActivationExample.Main(args);
            Db4odoc.Tutorial.F1.Chapter9.TransparentPersistenceExample.Main(args);
        }
	}
}
