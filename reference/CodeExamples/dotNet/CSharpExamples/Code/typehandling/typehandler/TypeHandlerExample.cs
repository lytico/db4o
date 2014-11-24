using System;
using System.Text;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Typehandlers;

namespace Db4oDoc.Code.TypeHandling.TypeHandler
{
    public class TypeHandlerExample
    {
        public static void Main(string[] args)
        {
            using (IObjectContainer container = OpenContainer())
            {
                // #example: Store the non storable type
                MyType testType = new MyType();
                container.Store(testType);
                // #end example
            }

            using (IObjectContainer container = OpenContainer())
            {
                // #example: Load the non storable type
                MyType builder = container.Query<MyType>()[0];
                Console.WriteLine(builder);
                // #end example
            }
        }

        private static IObjectContainer OpenContainer()
        {
            // #example: Register type handler
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.RegisterTypeHandler(
                new SingleClassTypeHandlerPredicate(typeof(StringBuilder)), new StringBuilderHandler());
            // #end example
            return Db4oEmbedded.OpenFile(configuration, "database.db4o");
        }


        private class MyType
        {
            private StringBuilder builder = new StringBuilder("TestData");

            public override string ToString()
            {
                return string.Format("Builder: {0}", builder);
            }
        }
    }
}