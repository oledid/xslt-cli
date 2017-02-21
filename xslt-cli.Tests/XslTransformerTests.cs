using Xunit;

namespace xslt_cli.Tests
{
	public class XslTransformerTests
	{
		public class Transform
		{
			[Fact]
			public void TransformsValidXml()
			{
				var output = XslTransformer.Transform(validXml, validXsl);
				Assert.True(output.IsSuccessful);
				Assert.Equal(output.Result, "Lorem Ipsum");
			}

			[Fact]
			public void FailsIfInvalidXml()
			{
				var output = XslTransformer.Transform(invalidXml, validXsl);
				Assert.False(output.IsSuccessful);
			}

			[Fact]
			public void FailsIfInvalidXsl()
			{
				var output = XslTransformer.Transform(validXml, invalidXsl);
				Assert.False(output.IsSuccessful);
			}

			private const string validXml = @"<?xml version=""1.0""?>
				<example-texts>
					<example-text>Lorem Ipsum</example-text>
				</example-texts>";

			private const string invalidXml = @"<?xml version=""1.0""?>
				<example-texts
					<example-text>Lorem Ipsum</example-text>
				</error>";

			private const string validXsl = @"<?xml version=""1.0""?>
				<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" version=""1.0"">
					<xsl:template match = ""/"">
						<xsl:for-each select=""example-texts"">
							<xsl:value-of select=""example-text"" />
						</xsl:for-each>
					</xsl:template>
				</xsl:stylesheet>";

			private const string invalidXsl = @"<?xml version=""1.0""?>
				<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" version=""1.0"">
				<xsl:template match = ""/""<";
		}
	}
}
