using System;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Xsl;

namespace CLI
{
	class Program
	{
		static void Main(string[] args)
		{
			var parsedArgs = new ArgsParser(args);
			if (parsedArgs.IsValid(File.Exists))
			{
				TryExecute(parsedArgs.Source, parsedArgs.Transform, parsedArgs.OutFile);
			}
			else
			{
				Console.WriteLine(parsedArgs.ErrorMessage);
			}
		}

		private static void TryExecute(string source, string transform, string outFile)
		{
			try
			{
				var start = DateTime.Now;
				Execute(source, transform, outFile);
				var duration = DateTime.Now.Subtract(start);
				Console.WriteLine($"Finished. Took {duration:ss} seconds {duration:fff} milliseconds.");
			}
			catch (Exception exception)
			{
				Console.WriteLine("An error occurred:");
				Console.ForegroundColor = ConsoleColor.Red;
				Console.BackgroundColor = ConsoleColor.Black;
				Console.WriteLine(exception.ToString());
			}
		}

		private static void Execute(string source, string transform, string outFile)
		{
			var dataSet = new DataSet();
			dataSet.ReadXml(source);

			var transformXslt = File.ReadAllText(transform);

			var output = TransformDataSet(dataSet, transformXslt);

			File.WriteAllText(outFile, output, Encoding.UTF8);
		}

		public static string TransformDataSet(DataSet dataSet, string xslt)
		{
			var encoding = Encoding.UTF8;

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

				using (var dataSetXml = new XmlTextReader(dataSetOutputStream))
				using (var resultOutputStream = new MemoryStream())
				using (var resultWriter = new XmlTextWriter(resultOutputStream, encoding))
				{
					xslTransform.Transform(dataSetXml, resultWriter);
					return encoding.GetString(resultOutputStream.ToArray());
				}
			}
		}
	}
}
