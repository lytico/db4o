using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using Db4objects.Db4o.Linq;

namespace LinqEverywhere.Views
{
	public partial class Insert
	{
		public Insert()
		{
			InitializeComponent();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			persons = (from Person p in Persistence.Database select p).ToList();
			PersonList.ItemsSource = persons;
		}

		private void InsertButtonClick(object sender, RoutedEventArgs e)
		{
			var person = new Person {Name = TextBoxName.Text, Age = Int32.Parse(TextBoxAge.Text)};
			
			persons.Add(person);
			PersonList.ItemsSource = null;
			PersonList.ItemsSource = persons;
			Persistence.Store(person);
		}

		private List<Person> persons;
	}
}
