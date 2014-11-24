namespace Db4oTool.Tests.Core
{
	public delegate void ContextVariableBlock();

	public class ContextVariable<T>
	{
		private T _value;

		public ContextVariable(T initialValue)
		{
			_value = initialValue;
		}

		public T Value
		{
			get { return _value; }
		}

		public void Using(T value, ContextVariableBlock block)
		{
			T oldValue = _value;
			_value = value;
			try
			{
				block();
			}
			finally
			{
				_value = oldValue;
			}
		}
	}
}
