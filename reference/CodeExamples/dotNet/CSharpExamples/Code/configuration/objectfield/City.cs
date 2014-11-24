using Db4objects.Db4o.Config.Attributes;

namespace Db4oDoc.Code.Configuration.Objectfield
{
    public class City
    {
        // #example: Index a field
        [Indexed]
        private string zipCode;
        // #end example
        private string name;

        public City(string zipCode)
        {
            this.zipCode = zipCode;
        }

        public string ZipCode
        {
            get { return zipCode; }
        }

        public string Name
        {
            get { return name; }
        }
    }
}