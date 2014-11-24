using System;
using System.IO;
using Db4objects.Db4o;


namespace OMNTest
{
    class CreateEnumDatabase
    {
        static String FILE = "enum.db4o";
        public void Run()
        {
            File.Delete(FILE);
            IObjectContainer objectContainer = Db4oEmbedded.OpenFile(FILE);
            Store(objectContainer);
            objectContainer.Close();
        }

        private void Store(IObjectContainer objectContainer)
        {
            Item item = new Item();
            item._name = "First";
            item._enumAsInteger = EnumAsInteger.First;
            objectContainer.Store(item);

            item = new Item();
            item._name = "Second";
            item._enumAsInteger = EnumAsInteger.Second;
            objectContainer.Store(item);
        }

        public class Item
        {
            public String _name;

            public EnumAsInteger _enumAsInteger;


        }

        public enum EnumAsInteger
        {
            First,
            Second,
        }

    }


}
