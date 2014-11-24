using System;
using System.Collections.Generic;

namespace UGMan6000.UI
{
	public class ConsoleUI
	{
		public static void WithForegroundColor(ConsoleColor color, string text)
		{
			ConsoleColor saved = Console.ForegroundColor;
			Console.ForegroundColor = color;
			try
			{	
				Console.WriteLine(text);
			}
			finally
			{
				Console.ForegroundColor = saved;
			}
		}

		public static string Prompt(string text)
		{
			Console.Write(text);
			return Console.ReadLine();
		}

		public static T SelectObject<T>(IEnumerable<T> objects)
		{
			T selected = default(T);

			Menu menu = new Menu();
			foreach (T obj in objects)
			{
				T captured = obj;
				menu.AddItem(obj.ToString(), delegate { selected = captured; });
			}
			menu.Interact();

			return selected;
		}
	}
}
