using System;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;

namespace Db4oDoc.Code.Indexing.Check
{
    public class CheckForAndIndex
    {
        public static void Main(string[] args)
        {
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.ObjectClass(typeof(IndexedClass)).ObjectField("id").Indexed(true);
            using (IObjectContainer container = Db4oEmbedded.OpenFile(configuration, "database.db4o"))
            {
                container.Store(new IndexedClass(1));
                // #example: Check for a index
                IStoredClass metaInfo = container.Ext().StoredClass(typeof(IndexedClass));
                // list a fields and check if they have a index
                foreach (IStoredField field in metaInfo.GetStoredFields())
                {
                    if (field.HasIndex())
                    {
                        Console.WriteLine("The field '" + field.GetName() + "' is indexed");
                    }
                    else
                    {
                        Console.WriteLine("The field '" + field.GetName() + "' isn't indexed");
                    }
                }
                // #end example
            }
        }

        private class IndexedClass
        {
            private int id;
            private string data;

            public IndexedClass(int id)
            {
                this.id = id;
            }
        }
    }
}