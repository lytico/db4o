namespace Db4oTool.Tests.Core.Resources
{
	public class CFInstrumentationSubject
	{
		public CFInstrumentationSubject(int value, CFInstrumentationSubject child)
		{
			Child = child;
			Value = value;
		}

		public int Value
		{
			get { return _value; }
			set { _value = value;}
		}

		public CFInstrumentationSubject Child
		{
			get { return _child; }
			set { _child = value;}
		}
		
		private int _value;
		private CFInstrumentationSubject _child;
	}
}
