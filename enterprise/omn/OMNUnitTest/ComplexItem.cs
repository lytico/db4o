namespace OMNUnitTest
{
	internal class ComplexItem<T>
	{
		public ComplexItem(params T[] subItems)
		{
			_subItems = subItems;
		}

		public T[] SubItems
		{
			get { return _subItems; }
		}

		private readonly T[] _subItems;
	}

	internal class SubItem
	{
		public SubItem(int value)
		{
			_value = value;
		}

		public int Value
		{
			get { return _value; }
		}

		private readonly int _value;
	}
}
