using Db4objects.Db4o.CS.Config;
using Db4objects.Db4o.Config;

namespace OMCustomConfigurationforUser
{
    public interface IOMNEmbeddedCustomConfigurationProvider
    {
        IEmbeddedConfiguration NewEmbeddedCustomConfiguration();
    }

    public interface IOMNClientCustomConfigurationProvider
    {
        IClientConfiguration NewClientCustomConfiguration();
    }
}
