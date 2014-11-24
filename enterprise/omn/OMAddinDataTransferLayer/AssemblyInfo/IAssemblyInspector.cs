using OManager.BusinessLayer.Config;

namespace OMAddinDataTransferLayer.AssemblyInfo
{
	public interface IAssemblyInspector
	{
		bool LoadAssembly(ISearchPath searchPath );
        bool LoadAssembly(string searchPath);
	}
}
	
