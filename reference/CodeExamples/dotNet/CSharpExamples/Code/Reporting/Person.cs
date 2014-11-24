namespace Db4oDoc.Code.Reporting
{
    public class Person
    {
        public Person(string firstName, string sirName)
        {
            FirstName = firstName;
            SirName = sirName;
        }

        public string FirstName { get; set; } 
        public string SirName { get; set; }
        public Address Address { get; set; } 
    }
}