using System;
using Xunit;

namespace xslt_cli.Tests
{
	public class WriteDurationTests
	{
		[Fact]
		public void It_writes_correct_duration()
		{
			Assert.Equal("3 seconds 123 milliseconds", Program.WriteDuration(new TimeSpan(0, 0, 0, 3, 123)));
			Assert.Equal("4 minutes 3 seconds 123 milliseconds", Program.WriteDuration(new TimeSpan(0, 0, 4, 3, 123)));
			Assert.Equal("234 milliseconds", Program.WriteDuration(new TimeSpan(0, 0, 0, 0, 234)));
			Assert.Equal("1 days 2 hours 3 minutes 45 seconds 678 milliseconds", Program.WriteDuration(new TimeSpan(1, 2, 3, 45, 678)));
		}
	}
}
