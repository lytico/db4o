using System.Collections.Generic;
using OManager.BusinessLayer.Login;

namespace OManager.BusinessLayer.FavFolder
{
    public class FavouriteFolderList
    {
        List<FavouriteFolder> m_lstFavfolder;
        internal List<FavouriteFolder> lstFavFolder
        {
            get { return m_lstFavfolder; }
            set { m_lstFavfolder = value; }
        }

        ConnParams m_connParam;
        public ConnParams ConnParam
        {
            get { return m_connParam; }
            set { m_connParam = value; }
        }

        private long m_TimeOfCreation;
        public long TimeOfCreation
        {
            get { return m_TimeOfCreation; }
            set { m_TimeOfCreation = value; }
        }
        public FavouriteFolderList(ConnParams connParam)
        {
            m_connParam = connParam;           
            m_lstFavfolder = new List<FavouriteFolder>();
            
        }
    }

   
}
