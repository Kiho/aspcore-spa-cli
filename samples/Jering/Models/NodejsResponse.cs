using System.Collections.Generic;
using System.IO;

namespace Jering
{
	internal class NodejsResponse
	{
		public int Status { get; set; }

		public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

		public string Body { get; set; } = string.Empty;

		public virtual Stream BodyStream => new MemoryStream(System.Text.Encoding.UTF8.GetBytes(Body ?? string.Empty));
	}
}