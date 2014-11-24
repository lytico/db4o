using System;
using System.IO;
using Db4objects.Db4o;

namespace OMNTest
{
    class CreateDateTimeDatabase
    {
        static String FILE = "datetime.db4o";
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
            item._dateTime = new DateTime(2009,1,14);
            objectContainer.Store(item);

            item = new Item();
            item._name = "Second";
            item._dateTime = new DateTime(2009, 1, 15);
            objectContainer.Store(item);
        }

        public class Item
        {
            public String _name;

            public DateTime _dateTime;


        }

    }

}
