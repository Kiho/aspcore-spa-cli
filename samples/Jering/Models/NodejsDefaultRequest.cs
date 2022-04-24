using System.Collections.Generic;

namespace Jering
{
	internal class NodejsDefaultRequest : INodejsRequest
	{
		/// <summary>
		/// Gets or sets Http Method.
		/// </summary>
		public string Method { get; set; }

		/// <summary>
		/// Gets headers.
		/// </summary>
		public IDictionary<string, string> Headers { get; }

		/// <summary>
		/// Gets or sets path.
		/// </summary>
		public string Path { get; set; }

		/// <summary>
		/// Gets or sets queryString.
		/// </summary>
		public string QueryString { get; set; }

		/// <summary>
		/// Gets or sets Body.
		/// </summary>
		public string? Body { get; set; }

		/// <summary>
		/// Gets or sets host.
		/// </summary>
		public string Host { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether nodejs to reply only with body, which
		/// ignores status and headers. This is a default.
		/// </summary>
		public bool BodyOnlyReply { get; set; } = true;

		public NodejsDefaultRequest(string method, string path, string queryString, string host)
		{
			Method = method;
			Headers = new Dictionary<string, string>();
			Path = path;
			QueryString = queryString;
			Host = host;
		}

		public NodejsDefaultRequest(string method, IDictionary<string, string> headers, string path, string queryString, string host)
		{
			Method = method;
			Headers = headers;
			Path = path;
			QueryString = queryString;
			Host = host;
		}
	}
}
