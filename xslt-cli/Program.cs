using System;
using System.Collections.Generic;
using System.IO;

namespace xslt_cli
{
	public class Program
	{
		public enum ExitCodes
		{
			Success = 0,
			Error = 1
		}

		public static int Main(string[] args)
		{
			var parsedArgs = new ArgsParser(args);
			if (parsedArgs.IsValid(File.Exists))
			{
				return (int)Execute(parsedArgs.Source, parsedArgs.Transform, parsedArgs.OutFile);
			}

			Console.WriteLine(parsedArgs.ErrorMessage);
			return (int)ExitCodes.Error;
		}

		public static ExitCodes Execute(string source, string transform, string outFile)
		{
			Exception writeException = null;
			var outFileInfo = new FileInfo(outFile);

			var output = XslTransformer.Transform(new FileInfo(source), new FileInfo(transform));

			if (output.IsSuccessful && TryWriteFile(output.Result, outFileInfo, out writeException))
			{
				Console.WriteLine($"Output written to: {outFileInfo.FullName}");
				Console.WriteLine($"Took {WriteDuration(output.Duration)}.");
				return ExitCodes.Success;
			}

			Console.WriteLine("An error occurred:");
			Console.ForegroundColor = ConsoleColor.Red;
			Console.BackgroundColor = ConsoleColor.Black;
			Console.WriteLine(output.Exception?.ToString() ?? writeException?.ToString() ?? "Please create an issue at https://github.com/oledid-dotnet/xslt-cli/issues");
			Console.ResetColor();

			return ExitCodes.Error;
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

		public static string WriteDuration(TimeSpan duration)
		{
			// http://metadataconsulting.blogspot.no/2017/09/Using-C-Sharp-Action-for-Human-Readable-TimeSpan-with-variable-length-formatting.html

			var parts = new List<string>();

			void AddActionToList(int val, string displayunit, int zeroplaces)
			{
				if (val > 0)
				{
					parts.Add(string.Format("{0:DZ}X".Replace("X", displayunit)
						.Replace("Z", zeroplaces.ToString()), val));
				}
			}

			AddActionToList(duration.Days, " days", 1);
			AddActionToList(duration.Hours, " hours", 1);
			AddActionToList(duration.Minutes, " minutes", 1);
			AddActionToList(duration.Seconds, " seconds", 1);
			AddActionToList(duration.Milliseconds, " milliseconds", 3);

			return string.Join(" ", parts);
		}
	}
}
