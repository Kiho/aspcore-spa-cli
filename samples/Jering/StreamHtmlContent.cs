using System.IO;
using System.Text;
using System.Text.Encodings.Web;

using Microsoft.AspNetCore.Html;

namespace Jering
{
	internal sealed class StreamHtmlContent : IHtmlContent
	{
		public MemoryStream HtmlEncodedStream { get; set; }

		public StreamHtmlContent(MemoryStream htmlEncodedStream)
		{
			HtmlEncodedStream = htmlEncodedStream;
		}

		public void WriteTo(TextWriter writer, HtmlEncoder encoder)
		{
			if (HtmlEncodedStream == null || HtmlEncodedStream.Position < 1)
				return;
			writer.Write(Encoding.UTF8.GetString(HtmlEncodedStream.GetBuffer(), 0, (int)HtmlEncodedStream.Position));
		}
	}
}
