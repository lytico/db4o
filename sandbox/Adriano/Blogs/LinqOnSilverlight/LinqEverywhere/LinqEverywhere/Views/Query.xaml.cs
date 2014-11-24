using System;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Db4objects.Db4o.Linq;

namespace LinqEverywhere.Views
{
	public partial class Query
	{
		public Query()
		{
			InitializeComponent();
			
			LayoutRoot.DataContext = new Person();
		}

		// Executes when the user navigates to this page.
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
		}

		private void buttonCombinedQuery_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Expression<Func<Person, bool>> exp = null;
				switch (comboOperator.SelectionBoxItem.ToString())
				{
					case "Or":
						exp = p => p.Name == txtName.Text || p.Age == int.Parse(txtAge.Text);
						break;

					case "And":
						exp = p => p.Name == txtName.Text && p.Age == int.Parse(txtAge.Text);
						break;
						
					default:
						break;
				}


				var result = Persistence.Database.Cast<Person>().Where(exp);

				var dataContext = result;
				listResults.ItemsSource = dataContext;
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		private void listResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			InnerGrid.DataContext = listResults.SelectedItem;
		}

		private void buttonQueryAge_Click(object sender, RoutedEventArgs e)
		{
			listResults.ItemsSource = from Person p in Persistence.Database 
									  where p.Age == int.Parse(txtAge.Text) 
									  select p;
		}

		private void buttonQueryName_Click(object sender, RoutedEventArgs e)
		{
			listResults.ItemsSource = from Person p in Persistence.Database
									  where p.Name == txtName.Text
									  select p;
		}
	}
}
