using System;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Xsl;

namespace xslt_cli
{
	public class XslTransformer
	{
		public static XslTransformResult Transform(FileInfo source, FileInfo transform)
		{
			var start = DateTime.Now;
			try
			{
				var sourceXml = File.ReadAllText(source.FullName);
				var transformXsl = File.ReadAllText(transform.FullName);
				return Transform(sourceXml, transformXsl, startOrNull: start);
			}
			catch (Exception exception)
			{
				return XslTransformResult.Failure(DateTime.Now.Subtract(start), exception);
			}
		}

		public static XslTransformResult Transform(string sourceXml, string transformXsl, DateTime? startOrNull = null)
		{
			var start = startOrNull ?? DateTime.Now;
			try
			{
				var result = TransformInternal(sourceXml, transformXsl);
				return XslTransformResult.Success(DateTime.Now.Subtract(start), result);
			}
			catch (Exception exception)
			{
				return XslTransformResult.Failure(DateTime.Now.Subtract(start), exception);
			}
		}

		private static string TransformInternal(string source, string transform)
		{
			var dataSet = new DataSet();
			using (var stream = new MemoryStream(UTF8withoutBOM.Lazy.Value.GetBytes(source)))
			{
				dataSet.ReadXml(stream);
			}

			return TransformDataSet(dataSet, transform);
		}

		private static string TransformDataSet(DataSet dataSet, string xslt)
		{
			var encoding = UTF8withoutBOM.Lazy.Value;

			var xslTransform = new XslCompiledTransform();

			using (var xslStream = new MemoryStream(encoding.GetBytes(xslt)))
			using (var xslReader = new XmlTextReader(xslStream))
			{
				xslTransform.Load(xslReader);
			}

			using (var dataSetOutputStream = new MemoryStream())
			using (var dataSetXmlWriter = new XmlTextWriter(dataSetOutputStream, encoding))
			{
				dataSet.WriteXml(dataSetXmlWriter);
				dataSetOutputStream.Position = 0;

				var stringBuilder = new StringBuilder();

				var outputSettings = CreateOutputSettings(xslTransform.OutputSettings);

				using (var dataSetXml = new XmlTextReader(dataSetOutputStream))
				using (var resultWriter = XmlWriter.Create(stringBuilder, outputSettings))
				{
					xslTransform.Transform(dataSetXml, resultWriter);
					return stringBuilder.ToString();
				}
			}
		}

		private static XmlWriterSettings CreateOutputSettings(XmlWriterSettings settingsFromReader)
		{
			return new XmlWriterSettings
			{
				Async = settingsFromReader.Async,
				CheckCharacters = settingsFromReader.CheckCharacters,
				CloseOutput = settingsFromReader.CloseOutput,
				ConformanceLevel = settingsFromReader.ConformanceLevel,
				DoNotEscapeUriAttributes = settingsFromReader.DoNotEscapeUriAttributes,
				Encoding = settingsFromReader.Encoding,
				Indent = settingsFromReader.Indent,
				IndentChars = settingsFromReader.IndentChars,
				NamespaceHandling = settingsFromReader.NamespaceHandling,
				NewLineChars = settingsFromReader.NewLineChars,
				NewLineHandling = settingsFromReader.NewLineHandling,
				NewLineOnAttributes = settingsFromReader.NewLineOnAttributes,
				OmitXmlDeclaration = settingsFromReader.OutputMethod == XmlOutputMethod.AutoDetect || settingsFromReader.OmitXmlDeclaration,
				WriteEndDocumentOnClose = settingsFromReader.WriteEndDocumentOnClose
			};
		}
	}
}
