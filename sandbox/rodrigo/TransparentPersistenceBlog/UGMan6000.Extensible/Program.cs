namespace UGMan6000.Extensible
{
	class Program
	{
		static void Main(string[] args)
		{
			using (ApplicationController controller = new ApplicationController())
			{
				new ConsoleView(controller).Interact();
			}
		}
	}
}
