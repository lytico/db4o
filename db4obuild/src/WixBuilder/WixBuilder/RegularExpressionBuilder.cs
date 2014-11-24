using System.Linq;
using System.Text;

namespace WixBuilder
{
	public class RegularExpressionBuilder
	{
		public static string For(string pattern)
		{
			var builder = new StringBuilder();
			foreach (string part in pattern.Replace("**/", "!").Replace(@"**\", "!").Select(ch => MapCharToRegularExpression(ch)))
				builder.Append(part);
			return builder.ToString();
		}

		private static string MapCharToRegularExpression(char ch)
		{
			const string AnyChar = @"[^\\/]";
			const string AnySlash = @"(\\|/)";
			switch (ch)
			{
				case '/':
				case '\\':
					return AnySlash;
				case '.':
					return @"\.";
				case '*':
					return AnyChar + "*";
				case '!':
					return @"((" + AnyChar + @"|\.)*" + AnySlash + ")*";
				default:
					return ch.ToString();
			}
		}
	}
}