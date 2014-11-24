using System.Linq;
using System.ServiceModel.Web;
using Db4objects.Db4o;
using Db4objects.Db4o.Data.Services;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Linq;

namespace Db4oDoc.WCFDataService
{
    // #example: An concrete context
    public class TeamDataContext : Db4oDataContext
    {
        // Provide access to your data via properties
        public IQueryable<Person> Persons
        {
            get { return Container.AsQueryable<Person>(); }
        }
        public IQueryable<Team> Teams
        {
            get { return Container.AsQueryable<Team>(); }
        }



        /// You need to implement the open-session and return a object container
        /// The best practise is to use a separate object-container per request.
        /// For example use the 
        /// <see cref="IObjectContainer"/>.<see cref="IObjectContainer.Ext"/>.<see cref="IExtObjectContainer.OpenSession"/>
        /// to open session-containers for each request.
        protected override IObjectContainer OpenSession()
        {
            return Db4oEmbedded.NewSession();
        }

    }
    // #end example
}