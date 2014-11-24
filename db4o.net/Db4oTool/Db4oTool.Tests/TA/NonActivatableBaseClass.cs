namespace Db4oTool.Tests.TA
{
	public class NonActivatableBaseClass
	{
		public string name;

		public override string ToString()
		{
			return "My name is " + name;
		}
	}
}
