using System;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Constraints;

namespace Db4oDoc.Code.Features.UniqueConstrain
{
    public class UniqueConstrainExample
    {
        public static void Main(string[] args)
        {
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            // #example: Add the index the field and then the unique constrain
            configuration.Common.ObjectClass(typeof (UniqueId)).ObjectField("id").Indexed(true);
            configuration.Common.Add(new UniqueFieldValueConstraint(typeof (UniqueId), "id"));
            // #end example
            using (IObjectContainer container = Db4oEmbedded.OpenFile(configuration, "database.db4o"))
            {
                container.Store(new UniqueId(44));
                // #example: Violating the constrain throws an exception
                container.Store(new UniqueId(42));
                container.Store(new UniqueId(42));
                try
                {
                    container.Commit();
                }
                catch (UniqueFieldValueConstraintViolationException e)
                {
                    Console.Out.WriteLine(e.StackTrace);
                }
                // #end example
            }
        }

        private class UniqueId
        {
            private readonly int id;

            public UniqueId(int id)
            {
                this.id = id;
            }
        }
    }
}