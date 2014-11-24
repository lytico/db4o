using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;

namespace Db4oDoc.Code.DisconnectedObj.IdExamples
{
    public class Db4oUuidExample : IIdExample<Db4oUUID>
    {
        public static Db4oUuidExample Create()
        {
            return new Db4oUuidExample();
        }

        public Db4oUUID IdForObject(object obj, IObjectContainer container)
        {
            // #example: get the db4o-uuid
            Db4oUUID uuid = container.Ext().GetObjectInfo(obj).GetUUID();
            // #end example
            return uuid;
        }

        public object ObjectForID(Db4oUUID idForObject, IObjectContainer container)
        {
            // #example: get an object by a db4o-uuid
            object objectForId = container.Ext().GetByUUID(idForObject);
            // getting by uuid doesn't activate the object
            // so you need to do it manually
            container.Ext().Activate(objectForId);
            // #end example
            return objectForId;
        }


        public void Configure(IEmbeddedConfiguration configuration)
        {
            // #example: db4o-uuids need to be activated
            configuration.File.GenerateUUIDs = ConfigScope.Globally;
            // #end example
        }

        public void RegisterEventOnContainer(IObjectContainer container)
        {
            // no events required for internal ids
        }
    }
}