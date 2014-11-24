using Db4objects.Db4o;
using Db4objects.Db4o.Config;

namespace Db4oDoc.Code.DisconnectedObj.IdExamples
{

    public interface IIdExample<TId>
    {
        TId IdForObject(object obj, IObjectContainer database);
        object ObjectForID(TId idForObject, IObjectContainer database);
        void Configure(IEmbeddedConfiguration configuration);
        void RegisterEventOnContainer(IObjectContainer container);
    }
}