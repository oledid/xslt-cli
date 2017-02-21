using System;
using System.Text;

namespace xslt_cli
{
	public class UTF8withoutBOM
	{
		public static Lazy<Encoding> Lazy = new Lazy<Encoding>(() => new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
	}
}
