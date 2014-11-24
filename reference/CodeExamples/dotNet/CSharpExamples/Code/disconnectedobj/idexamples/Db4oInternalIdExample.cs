using Db4objects.Db4o;
using Db4objects.Db4o.Config;

namespace Db4oDoc.Code.DisconnectedObj.IdExamples
{
    public class Db4oInternalIdExample : IIdExample<long>
    {
        public static Db4oInternalIdExample Create()
        {
            return new Db4oInternalIdExample();
        }

        public long IdForObject(object obj, IObjectContainer container)
        {
            // #example: get the db4o internal ids
            long interalId = container.Ext().GetID(obj);
            // #end example
            return interalId;
        }

        public object ObjectForID(long idForObject, IObjectContainer container)
        {
            // #example: get an object by db4o internal id
            long internalId = idForObject;
            object objectForID = container.Ext().GetByID(internalId);
            // getting by id doesn't activate the object
            // so you need to do it manually
            container.Ext().Activate(objectForID);
            // #end example
            return objectForID;
        }

        public void Configure(IEmbeddedConfiguration configuration)
        {
            // no configuration required for internal ids  
        }

        public void RegisterEventOnContainer(IObjectContainer container)
        {
            // no events required for internal ids  
        }
    }
}