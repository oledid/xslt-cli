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
			public void It_returns_false_if_invalid_number_of_arguments()
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
			public void It_returns_true_if_correct_number_of_arguments()
			{
				var argumentParser = new ArgsParser(new [] { "1", "2", "3" });
				Assert.True(argumentParser.IsValid(GetTrue));
			}

			[Fact]
			public void It_returns_true_if_the_files_exists()
			{
				var argumentParser = new ArgsParser(new[] { "1", "2", "3" });
				Assert.True(argumentParser.IsValid(str => str == "1" || str == "2"));
			}

			[Fact]
			public void It_returns_false_if_source_file_does_not_exist()
			{
				var argumentParser = new ArgsParser(new[] { "1", "2", "3" });
				Assert.False(argumentParser.IsValid(str => str != "1"));
				Assert.True(argumentParser.ErrorMessage.Contains(": 1"));
			}

			[Fact]
			public void It_returns_false_if_transform_file_does_not_exist()
			{
				var argumentParser = new ArgsParser(new[] { "1", "2", "3" });
				Assert.False(argumentParser.IsValid(str => str != "2"));
				Assert.True(argumentParser.ErrorMessage.Contains(": 2"));
			}

			[Fact]
			public void It_returns_false_if_outFile_is_invalid()
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
