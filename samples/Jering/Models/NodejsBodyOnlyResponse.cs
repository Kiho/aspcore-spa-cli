using System.IO;

namespace Jering
{
	internal class NodejsBodyOnlyResponse : NodejsResponse
	{
		public NodejsBodyOnlyResponse(Stream stream)
		{
			BodyStream = stream;
		}

		public override Stream BodyStream { get; }
	}
}
