using System;

namespace UGMan6000
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.Title = "UGMan6000";
			new ConsoleView(new ApplicationController()).Interact();
		}
	}
}
