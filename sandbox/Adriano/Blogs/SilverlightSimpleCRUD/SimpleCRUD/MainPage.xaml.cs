using Db4objects.Db4o;
using Db4objects.Db4o.Query;
using SimpleCRUD.Model;

namespace SimpleCRUD
{
	public partial class MainPage
	{
		public MainPage(IObjectContainer container)
		{
			InitializeComponent();
			_db = container;
		}

		private void RunQuery(object sender, System.Windows.RoutedEventArgs e)
		{
			IQuery query = NewQuery();
			query.AddConstraint("_firstName", FirstName.Text);
			query.AddConstraint("_lastName", LastName.Text);

			IObjectSet enumerable = query.Execute();
			foreach (Person person in enumerable)
			{
				System.Diagnostics.Debug.WriteLine(person.FirstName + " " + person.LastName);	
			}

			People.ItemsSource = enumerable;
		}

		private IQuery NewQuery()
		{
			IQuery query = _db.Query();
			query.Constrain(typeof (Person));
			
			return query;
		}

		private void AddPerson(object sender, System.Windows.RoutedEventArgs e)
		{
			_db.Store(new Person {FirstName = FirstName.Text, LastName = LastName.Text });
			_db.Commit();

			People.ItemsSource = NewQuery().Execute();
		}

		private void DeletePerson(object sender, System.Windows.RoutedEventArgs e)
		{
			if (People.SelectedItems.Count > 0)
			{
				IQuery query = NewQuery();
				query.Descend("_firstName").Constrain(SelectedPersonFistName());

				IObjectSet result = query.Execute();
				for(int i = 0; i < result.Count; i++)
				{
					_db.Delete(result[i]);
				}

				_db.Commit();
				People.ItemsSource = NewQuery().Execute();
			}
		}

		private string SelectedPersonFistName()
		{
			Person person = (Person) People.SelectedItems[0];
			return person.FirstName;
		}

		private readonly IObjectContainer _db;
	}
}
