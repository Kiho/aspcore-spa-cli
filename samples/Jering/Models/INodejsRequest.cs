using System.Collections.Generic;

namespace Jering
{
	/// <summary>
	/// Request object sent to nodejs server.
	/// </summary>
	public interface INodejsRequest
	{
		string Method { get; }

		/// <summary>
		/// Gets headers.
		/// </summary>
		IDictionary<string, string> Headers { get; }

		/// <summary>
		/// Gets path.
		/// </summary>
		string Path { get; }

		/// <summary>
		/// Gets queryString.
		/// </summary>
		string QueryString { get; }

		/// <summary>
		/// Gets Body.
		/// </summary>
		string? Body { get; }

		/// <summary>
		/// Gets host.
		/// </summary>
		string Host { get; }

		/// <summary>
		/// Gets a value indicating whether nodejs to reply only with body, which
		/// ignores status and headers. This is a default.
		/// </summary>
		bool BodyOnlyReply { get; }
	}
}
