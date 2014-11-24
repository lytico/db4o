namespace Contacts
{
	public static class Console
	{
		public static string Prompt(string message)
		{
			System.Console.Write(message);
			return System.Console.ReadLine();
		}
	}
}