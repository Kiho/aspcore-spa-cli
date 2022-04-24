using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Jering
{
	/// <summary>
	/// Stores Nodejs options.
	/// </summary>
	public class NodejsOptions
	{
		/// <summary>
		/// Gets the default <see cref="JsonSerializerOptions"/>.
		/// </summary>
		public static JsonSerializerOptions DefaultSerializationOptions { get; } = new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true,
			DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
			PropertyNamingPolicy = null,
			Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
		};

		/// <summary>
		/// Gets or sets concurrency mode (MultiProcess or None).
		/// </summary>
		[Required]
		public Jering.Javascript.NodeJS.Concurrency Concurrency { get; set; }

		/// <summary>
		/// Gets or sets number of nodejs server instances.
		/// </summary>
		[Required]
		public int ConcurrencyDegree { get; set; }

		/// <summary>
		/// Gets or sets maximum time (millisecond) to connect with Nodejs.
		/// </summary>
		public int NodejsConnectionTimeout { get; set; } = 3000;

		/// <summary>
		/// Gets or sets Node options.
		/// </summary>
		public string NodeAndV8Options { get; set; } = "--es-module-specifier-resolution=node --experimental-vm-modules";

		/// <summary>
		/// Gets or sets the <see cref="JsonSerializerOptions"/> that will be used for serialization and deserialization of service requests. Default value: <see cref="DefaultSerializationOptions"/>.
		/// </summary>
		public JsonSerializerOptions? JsonOptions { get; set; }

		/// <summary>
		/// Gets or sets executable script path.
		/// </summary>
		public string ScriptPath { get; set; } = "./build/index.cjs";

		/// <summary>
		/// Gets or sets a value indicating whether gets or sets GzipCompressResponse.
		/// </summary>
		public bool GzipCompressResponse { get; set; } = true;

		/// <summary>
		/// Gets or sets the non-secured port used by forge.
		/// </summary>
		public int ForgePort { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether nodejs to reply only with body, which
		/// ignores status and headers. This is a default.
		/// </summary>
		public bool BodyOnlyReply { get; set; } = true;

		/// <summary>
		/// Gets or sets node environment.
		/// </summary>
		public string NodeEnv { get; set; } = "development";
	}
}
