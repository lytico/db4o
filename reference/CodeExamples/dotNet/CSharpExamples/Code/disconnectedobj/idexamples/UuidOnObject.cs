using System;
using System.Linq;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Linq;

namespace Db4oDoc.Code.DisconnectedObj.IdExamples
{
    public class UuidOnObject : IIdExample<Guid>
    {
        public static IIdExample<Guid> Create()
        {
            return new UuidOnObject();
        }

        public Guid IdForObject(object obj, IObjectContainer container)
        {
            // #example: get the uuid
            IDHolder uuidHolder = (IDHolder) obj;
            Guid uuid = uuidHolder.ObjectId;
            // #end example
            return uuid;
        }

        public object ObjectForID(Guid idForObject, IObjectContainer container)
        {
            // #example: get an object its UUID
            var instance = (from IDHolder o in container
                                where o.ObjectId.Equals(idForObject)
                                select o).Single();
            // #end example
            return instance;
        }

        public void Configure(IEmbeddedConfiguration configuration)
        {
            // #example: index the uuid-field
            configuration.Common.ObjectClass(typeof (IDHolder)).ObjectField("guid").Indexed(true);
            // #end example
        }

        public void RegisterEventOnContainer(IObjectContainer container)
        {
            // no events required for internal ids
        }
    }
}