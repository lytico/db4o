using System;
using UGMan6000.Transparent.Model;
using UGMan6000.UI;

namespace UGMan6000.Transparent
{
	internal class ConsoleView
	{
		private ApplicationController _controller;

		public ConsoleView(ApplicationController controller)
		{
			_controller = controller;
		}

		public void Interact()
		{
			bool quit = false;
			
			Menu menu = new Menu();
			menu.AddItem("List Groups", ListGroups);
			menu.AddItem("List Users", ListUsers);
			menu.AddItem("New Group", NewGroup);
			menu.AddItem("New User", NewUser);
			menu.AddItem("Add User To Group", AddUserToGroup);
			menu.AddItem("Quit", delegate { quit = true; }); 

			while (!quit)
			{
				Console.Clear();
				try
				{
					menu.Interact();
				}
				catch (Exception e)
				{
					ConsoleUI.WithForegroundColor(ConsoleColor.Red, e.ToString());
					WaitForKey();
				}
			}
		}

		private void WaitForKey()
		{
			Console.Write("Press any key to continue... ");
			Console.ReadKey(true);
		}

		private void AddUserToGroup()
		{
			User user = SelectUser();
			Group group = SelectGroup();
			_controller.AddUserToGroup(user, group);
		}

		private Group SelectGroup()
		{
			return ConsoleUI.SelectObject(_controller.Groups);
		}

		private User SelectUser()
		{
			return ConsoleUI.SelectObject(_controller.Users);
		}

		private void NewUser()
		{
			string name = ConsoleUI.Prompt("User name: ");
			_controller.AddUser(new User(name));
		}

		private void NewGroup()
		{
			string name = ConsoleUI.Prompt("Group name: ");
			_controller.AddGroup(new Group(name));
		}

		private void ListUsers()
		{
			foreach (User user in _controller.Users)
			{
				Console.WriteLine(user.Name);
				foreach (Group group in user.Groups)
				{
					Console.WriteLine("\t" + group.Name);
				}
			}
			WaitForKey();
		}

		private void ListGroups()
		{
			foreach (Group group in _controller.Groups)
			{
				Console.WriteLine(group.Name);
				foreach (User user in group.Users)
				{
					Console.WriteLine("\t" + user.Name);
				}
			}
			WaitForKey();
		}
	}
}