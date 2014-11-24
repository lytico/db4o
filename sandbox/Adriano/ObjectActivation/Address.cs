namespace ObjectActivationSample
{
    public class Address : SupportObjectId
    {
        private string street;
        public string Street
        {
            get { return street; }
            set { street = value; }
        }

        private int number;
        public int Number
        {
            get { return number; }
            set { number = value; }
        }

        private string city;
        public string City
        {
            get { return city; }
            set { city = value; }
        }

        public Address(string city, string street, int number)
        {
            this.city = city;
            this.street = street;
            this.number = number;
        }

        public override string ToString()
        {
            return "[" + (street ?? "**") + "(" + number + ") - " + (city ?? "**") + base.ToString() + "]" ;
        }
    }
}
