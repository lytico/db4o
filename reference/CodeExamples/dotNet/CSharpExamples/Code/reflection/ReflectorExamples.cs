using System;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Reflect.Net;

namespace Db4oDoc.Code.Reflection
{
    public class ReflectorExamples
    {
        public static void Main(string[] args)
        {
            UseLoggerReflector();
        }

        private static void UseLoggerReflector()
        {
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.ReflectWith(new LoggerReflector());
            using (IObjectContainer container = Db4oEmbedded.OpenFile("database.db4o"))
            {
            }
        }
    }

    // #example: Logging reflector
    internal class LoggerReflector : IReflector
    {
        private readonly IReflector readReflector;

        public LoggerReflector()
        {
            this.readReflector = new NetReflector();
        }

        public LoggerReflector(IReflector readReflector)
        {
            this.readReflector = readReflector;
        }

        public void Configuration(IReflectorConfiguration reflectorConfiguration)
        {
            readReflector.Configuration(reflectorConfiguration);
        }

        public IReflectArray Array()
        {
            return readReflector.Array();
        }

        public IReflectClass ForClass(Type type)
        {
            Console.WriteLine("Reflector.forClass({0})", type);
            return readReflector.ForClass(type);
        }

        public IReflectClass ForName(string className)
        {
            Console.WriteLine("Reflector.forName({0})", className);
            return readReflector.ForName(className);
        }

        public IReflectClass ForObject(object o)
        {
            Console.WriteLine("Reflector.forObject(" + o + ")");
            return readReflector.ForObject(o);
        }

        public bool IsCollection(IReflectClass reflectClass)
        {
            return readReflector.IsCollection(reflectClass);
        }

        public void SetParent(IReflector reflector)
        {
            readReflector.SetParent(reflector);
        }

        public object DeepClone(object o)
        {
            return new LoggerReflector((IReflector) readReflector.DeepClone(o));
        }
    }
    // #end example
}