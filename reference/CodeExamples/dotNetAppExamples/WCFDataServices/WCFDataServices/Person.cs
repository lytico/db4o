using System.Data.Services.Common;

namespace Db4oDoc.WCFDataService
{
    // #example: Add at least one key
    [DataServiceKey("Email")]
    public class Person
    {
        private string name;
        private string email;

        // Note that a default constructor is required for WCF
        public Person()
        {
        }

        public Person(string email, string name)
        {
            this.email = email;
            this.name = name;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

    }
    // #end example
}