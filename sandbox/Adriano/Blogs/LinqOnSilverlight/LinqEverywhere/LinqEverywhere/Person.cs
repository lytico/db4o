namespace LinqEverywhere
{
	public class Person
	{
		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		public int Age
		{
			get { return age; }
			set { age = value; }
		}

		public override string ToString()
		{
			return Name + " - " + Age;
		}
		
		public string name;
		public int age;
	}
}