namespace Db4oDoc.Code.Practises.Relations
{
    class Country
    {
        private readonly string name;

        public Country(string name)
        {
            this.name = name;
        }

        public string Name
        {
            get { return name; }
        }
    }
}