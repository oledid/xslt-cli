using System.Linq;
using CLI;
using Xunit;

namespace Tests
{
	public class ArgsParserTests
	{
		public class IsValid
		{
			[Fact]
			public void ReturnsFalseIfInvalidNumberOfArguments()
			{
				for (var i = 0; i < 5; ++i)
				{
					if (i == 3)
					{
						continue;
					}

					var argumentParser = new ArgsParser(Enumerable.Range(start: 0, count: i).Select(val => val.ToString()).ToArray());
					Assert.False(argumentParser.IsValid(GetTrue));
				}
			}

			[Fact]
			public void ReturnsTrueIfCorrectNumberOfArguments()
			{
				var argumentParser = new ArgsParser(new[] { "1", "2", "3" });
				Assert.True(argumentParser.IsValid(GetTrue));
			}

			[Fact]
			public void ReturnsTrueIfTheFilesExists()
			{
				var argumentParser = new ArgsParser(new[] { "1", "2", "3" });
				Assert.True(argumentParser.IsValid(str => str == "1" || str == "2"));
			}

			[Fact]
			public void ReturnsFalseIfSourceFileDoesNotExist()
			{
				var argumentParser = new ArgsParser(new[] { "1", "2", "3" });
				Assert.False(argumentParser.IsValid(str => str != "1"));
				Assert.True(argumentParser.ErrorMessage.Contains(": 1"));
			}

			[Fact]
			public void ReturnsFalseIfTransformFileDoesNotExist()
			{
				var argumentParser = new ArgsParser(new[] { "1", "2", "3" });
				Assert.False(argumentParser.IsValid(str => str != "2"));
				Assert.True(argumentParser.ErrorMessage.Contains(": 2"));
			}

			[Fact]
			public void ReturnsFalseIfOutFileIsInvalid()
			{
				var argumentParserNull = new ArgsParser(new[] { "1", "2", null });
				Assert.False(argumentParserNull.IsValid(GetTrue));

				var argumentParserWhitespace = new ArgsParser(new[] { "1", "2", " " });
				Assert.False(argumentParserWhitespace.IsValid(GetTrue));

				var argumentParserEmpty = new ArgsParser(new[] { "1", "2", string.Empty });
				Assert.False(argumentParserEmpty.IsValid(GetTrue));
			}

			private static bool GetTrue(string dummy = null)
			{
				return true;
			}
		}
	}
}
