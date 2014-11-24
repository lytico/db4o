using System;
using Db4objects.Db4o;
using Db4objects.Db4o.Config.Attributes;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;

namespace Db4oDoc.Code.Indexing.Traverse
{
    public class TraverseIndexExample
    {
        public static void Main(string[] args)
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile("database.db4o"))
            {
                StoreExampleObjects(container);

                TraverseIndex(container);
            }
        }

        private static void TraverseIndex(IObjectContainer container)
        {
            // #example: Traverse index
            IStoredField storedField = container.Ext()
                .StoredClass(typeof (Item)).StoredField("data", typeof (int));
            storedField.TraverseValues(new IndexVisitor());
            // #end example
        }

        // #example: The index visitor
        private class IndexVisitor : IVisitor4
        {
            public void Visit(object obj)
            {
                var value = (int) obj;
                Console.Out.WriteLine("Value {0}", value);
            }
        }
        // #end example

        private static void StoreExampleObjects(IObjectContainer container)
        {
            for (int i = 0; i < 100; i++)
            {
                container.Store(new Item(i));
            }
        }
    }

    internal class Item
    {
        [Indexed] private int data;

        public Item(int data)
        {
            this.data = data;
        }
    }
}