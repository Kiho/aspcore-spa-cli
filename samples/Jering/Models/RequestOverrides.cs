namespace Jering
{
	public class RequestOverrides
	{
		public RequestOverrides(string path)
		{
			Path = path;
		}

		public string Path { get; }
	}
}