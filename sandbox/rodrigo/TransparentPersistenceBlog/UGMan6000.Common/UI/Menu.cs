using System;
using System.Collections.Generic;

namespace UGMan6000.UI
{
	public delegate void Command();

	public class Menu
	{
		class MenuItem
		{
			public readonly string Label;
			public readonly Command Command;

			public MenuItem(string label_, Command command_)
			{
				Label = label_;
				Command = command_;
			}
		}

		private List<MenuItem> _items = new List<MenuItem>();

		public void AddItem(string label, Command command)
		{
			_items.Add(new MenuItem(label, command));
		}

		public void Interact()
		{
			WriteMenuItems();
			int selected = GetSelectedIndex();
			_items[selected].Command();
		}

		private static int GetSelectedIndex()
		{
			string value = ConsoleUI.Prompt("Your choice: ");
			return int.Parse(value) - 1;
		}

		private void WriteMenuItems()
		{
			int index = 0;
			foreach (MenuItem item in _items)
			{
				Console.WriteLine("{0}) {1}", ++index, item.Label);
			}
		}
	}
}
