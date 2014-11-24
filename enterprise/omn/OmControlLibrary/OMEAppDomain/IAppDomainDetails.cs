using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OManager.BusinessLayer.Config;

namespace OMControlLibrary.OMEAppDomain
{
   public  interface IAppDomainDetails
   {
       bool LoadAppDomain(string searchPath);
       bool LoadAppDomain(ISearchPath searchPath);

    }
}
