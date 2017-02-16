using CLI;
using Xunit;

namespace Tests
{
	public class XslTransformerTests
	{
		public class Transform
		{
			[Fact]
			public void It_can_transform_valid_xml()
			{
				var output = XslTransformer.Transform(ValidXml, ValidXsl);
				Assert.True(output.IsSuccessful);
				Assert.Equal(output.Result, "Lorem Ipsum");
			}

			[Fact]
			public void It_fails_if_invalid_xml()
			{
				var output = XslTransformer.Transform(InvalidXml, ValidXsl);
				Assert.False(output.IsSuccessful);
			}

			[Fact]
			public void It_fails_if_invalid_xsl()
			{
				var output = XslTransformer.Transform(ValidXml, InvalidXsl);
				Assert.False(output.IsSuccessful);
			}

			private const string ValidXml = @"<?xml version=""1.0""?>
				<example-texts>
					<example-text>Lorem Ipsum</example-text>
				</example-texts>";

			private const string InvalidXml = @"<?xml version=""1.0""?>
				<example-texts
					<example-text>Lorem Ipsum</example-text>
				</error>";

			private const string ValidXsl = @"<?xml version=""1.0""?>
				<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" version=""1.0"">
					<xsl:template match = ""/"">
						<xsl:for-each select=""example-texts"">
							<xsl:value-of select=""example-text"" />
						</xsl:for-each>
					</xsl:template>
				</xsl:stylesheet>";

			private const string InvalidXsl = @"<?xml version=""1.0""?>
				<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" version=""1.0"">
				<xsl:template match = ""/""<";
		}
	}
}
