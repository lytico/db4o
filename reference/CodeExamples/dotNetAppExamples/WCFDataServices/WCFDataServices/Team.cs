using System.Collections.Generic;
using System.Data.Services.Common;

namespace Db4oDoc.WCFDataService
{
    // #example: The Team has also a key
    [DataServiceKey("TeamName")]
    public class Team
    {
        private string teamName;
        private string motivation;
        private IList<Person> members = new List<Person>();

        public Team()
        {
        }

        public Team(string teamName)
        {
            this.teamName = teamName;
        }

        public string TeamName
        {
            get { return teamName; }
            set { teamName = value; }
        }

        public string Motivation
        {
            get { return motivation; }
            set { motivation = value; }
        }

        public IList<Person> Members
        {
            get { return members; }
            set { members = value; }
        }
    }
    //#end example
}