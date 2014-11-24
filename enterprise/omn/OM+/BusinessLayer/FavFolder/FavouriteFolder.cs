using System;
using System.Collections.Generic;
using System.Text;

namespace OManager.BusinessLayer.FavFolder
{
    public class FavouriteFolder
    {
        private string m_folderName;
        public string FolderName
        {
            get { return m_folderName; }
            set { m_folderName = value; }
        }

        private IList<string> m_ListClass;
        public IList<string> ListClass
        {
            get { return m_ListClass; }
            set { m_ListClass = value; }
        }

        public FavouriteFolder(IList<string> lstClass, string folderName)
        {
            m_folderName = folderName;            
            m_ListClass = lstClass; 
        }
    }


}
