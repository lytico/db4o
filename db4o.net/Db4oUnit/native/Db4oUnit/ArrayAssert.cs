namespace Db4oUnit
{
	public partial class ArrayAssert
	{
		public static void AreEqual<T>(T[] expected, T[] actual)
		{
			AreEqualImpl(expected, actual);
		}

		static void AreEqualImpl<T>(T[] expected, T[] actual)
		{
			if (expected == actual)
			{
				return;
			}
			if (expected == null || actual == null)
			{
				Assert.AreSame(expected, actual);
				return;
			}
			Assert.AreEqual(expected.Length, actual.Length);
			Assert.AreSame(expected.GetType(), actual.GetType());
			for (int i = 0; i < expected.Length; i++)
			{
				Assert.AreEqual(expected[i], actual[i], IndexMessage(i));
			}
		}
	}
}
