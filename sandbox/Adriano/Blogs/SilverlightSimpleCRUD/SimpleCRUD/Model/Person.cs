namespace SimpleCRUD.Model
{
	public class Person
	{
		public string _firstName;
		public string _lastName;

		public string FirstName
		{
			get { return _firstName; }
			set { _firstName = value; }
		}

		public string LastName
		{
			get { return _lastName; }
			set { _lastName = value; }
		}
	}
}
