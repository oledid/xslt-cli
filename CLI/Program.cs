using System;
using System.IO;

namespace CLI
{
	class Program
	{
		static void Main(string[] args)
		{
			var parsedArgs = new ArgsParser(args);
			if (parsedArgs.IsValid(File.Exists))
			{
				Execute(parsedArgs.Source, parsedArgs.Transform, parsedArgs.OutFile);
			}
			else
			{
				Console.WriteLine(parsedArgs.ErrorMessage);
			}
		}

		private static void Execute(string source, string transform, string outFile)
		{
			Exception writeException = null;
			var outFileInfo = new FileInfo(outFile);

			var output = XslTransformer.Transform(new FileInfo(source), new FileInfo(transform));

			if (output.IsSuccessful && TryWriteFile(output.Result, outFileInfo, out writeException))
			{
				Console.WriteLine($"Output written to: {outFileInfo.FullName}");
				Console.WriteLine($"Took {output.Duration:ss} seconds {output.Duration:fff} milliseconds.");
				return;
			}

			Console.WriteLine("An error occurred:");
			Console.ForegroundColor = ConsoleColor.Red;
			Console.BackgroundColor = ConsoleColor.Black;
			Console.WriteLine(output.Exception?.ToString() ?? writeException?.ToString() ?? "Please create an issue at https://github.com/oledid/dotnet-xslt-cli/issues");
		}

		private static bool TryWriteFile(string outputResult, FileInfo outFileInfo, out Exception writeException)
		{
			try
			{
				File.WriteAllText(outFileInfo.FullName, outputResult, UTF8withoutBOM.Lazy.Value);
				writeException = null;
				return true;
			}
			catch (Exception exception)
			{
				writeException = exception;
				return false;
			}
		}
	}
}
