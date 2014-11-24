using System;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;

namespace Db4oDoc.Code.TypeHandling.Translator
{
    public class TranslatorExample
    {
        public static void Main(string[] args)
        {
            using(IObjectContainer container = CreateDB())
            {
                // #example: Store the non storable type
                container.Store(new NonStorableType("TestData"));
                // #end example
            }
            using(IObjectContainer container = CreateDB())
            {
                // #example: Load the non storable type
                NonStorableType data = container.Query<NonStorableType>()[0];
                // #end example
                Console.Out.WriteLine(data.Data);
            }
        }

        private static IObjectContainer CreateDB()
        {
            // #example: Register type translator for the NonStorableType-class
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.ObjectClass(typeof(NonStorableType)).Translate(new ExampleTranslator());
            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, "database.db4o");
            // #end example
            return container;
        }
    }


    // #example: An example translator
    internal class ExampleTranslator : IObjectConstructor
    {
        // This is called to store the object
        public object OnStore(IObjectContainer objectContainer, object objToStore)
        {
            NonStorableType notStorable = (NonStorableType) objToStore;
            return notStorable.Data;
        }

        // This is called when the object is activated
        public void OnActivate(IObjectContainer objectContainer, object targetObject, object storedObject)
        {
            NonStorableType notStorable = (NonStorableType) targetObject;
            notStorable.Data = (String) storedObject;
        }

        // Tell db4o which type we use to store the data
        public Type StoredClass()
        {
            return typeof (String);
        }

        // This method is called when a new instance is needed
        public object OnInstantiate(IObjectContainer objectContainer, object storedObject)
        {
            return new NonStorableType("");
        }
    }

// #end example

    ///
    /// This is our example class which represents a not storable type
    ///
    internal class NonStorableType
    {
        private string data;
        [NonSerialized] private int dataLength = 0;

        public NonStorableType(string data)
        {
            this.data = data;
            this.dataLength = data.Length;
        }

        public string Data
        {
            get { return data; }
            set { data = value; }
        }

        public int DataLength
        {
            get { return dataLength; }
            set { dataLength = value; }
        }
    }
}