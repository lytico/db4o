

using OManager.BusinessLayer.Config;

namespace OMCustomConfigImplementation.CustomConfigAssemblyInfo
{
	public interface ICustomConfigAssemblyInspector
	{
	    bool LoadAssembly(ISearchPath searchPath);
		bool LoadAssembly(string searchPath );
	}
}
	
