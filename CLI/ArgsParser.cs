using System;

namespace CLI
{
	public class ArgsParser
	{
		public string ErrorMessage { get; private set; }

		public readonly string Source;
		public readonly string Transform;
		public readonly string OutFile;

		public ArgsParser(string[] args)
		{
			if (args.Length != 3)
			{
				SetErrorMessage("Usage: xslt-cli.exe [source.xml] [transform.xslt] [outFile.ext]");
				return;
			}

			Source = args[0];
			Transform = args[1];
			OutFile = args[2];
		}

		public bool IsValid(Func<string, bool> checkIfFileExists)
		{
			foreach (var filePath in new[] {Source, Transform})
			{
				if (checkIfFileExists.Invoke(filePath) == false)
				{
					SetErrorMessage($"Could not find file: {filePath}");
				}
			}

			if (string.IsNullOrWhiteSpace(OutFile))
			{
				SetErrorMessage($"Invalid out-file: {OutFile}");
			}

			return ErrorMessage == null;
		}

		private void SetErrorMessage(string error)
		{
			if (ErrorMessage == null)
			{
				ErrorMessage = error;
			}
		}
	}
}
