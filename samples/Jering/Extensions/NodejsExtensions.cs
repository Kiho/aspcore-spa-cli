using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.IO.Pipelines;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Jering.Javascript.NodeJS;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace Jering
{
	/// <summary>
	/// Custom middleware to use the capability of Jering lib.
	/// </summary>
	public static class NodejsExtensions
	{
		/// <summary>
		/// AddJering method to registed in Startup.
		/// </summary>
		/// <param name="services">Services <see cref="IServiceCollection"/> collection.</param>
		/// <param name="config">Application <see cref="IConfiguration"/>.</param>
		/// <returns>Extension method return parameter.</returns>
		public static IServiceCollection ConfigureNodejsService(this IServiceCollection services)
		{
			services
				.AddNodeJS()
				.Configure<OutOfProcessNodeJSServiceOptions>(options =>
				{
					if (Debugger.IsAttached)
					{
						options.EnableFileWatching = true;
						options.NumRetries = 1;
						options.WatchPath = "./build/";
					}
					options.Concurrency = Concurrency.MultiProcess;
					options.ConcurrencyDegree = 2;
					options.TimeoutMS = -1; // -1 to wait forever (used for attaching debugger, which needs to be set in code)
				})
				.Configure<HttpNodeJSServiceOptions>(options => options.Version = HttpVersion.Version20)
				.Configure<NodeJSProcessOptions>(options =>
				{
#if DEBUG
					options.NodeAndV8Options = "--inspect --es-module-specifier-resolution=node --experimental-vm-modules";
#else
					options.NodeAndV8Options = "--es-module-specifier-resolution=node --experimental-vm-modules";
#endif
					options.EnvironmentVariables = new Dictionary<string, string>
					{
						{ "VITE_ForgePort", "5004"}, // this value needs to match the port # in launchSettings.json
						{ "NODE_ENV", "development"}
					};
				});

			return services;
		}

		/// <summary>
		/// UseJering method to start using in Startup.
		/// </summary>
		/// <param name="app">Application request pipeline.</param>
		/// <param name="hostEnvironment">Hosting environment.</param>
		/// <returns>Extension method return parameter.</returns>
		public static IApplicationBuilder UseNodejsService(this IApplicationBuilder app, IHostEnvironment hostEnvironment)
		{
			return app
				.UseStaticFiles(new StaticFileOptions()
				{
					HttpsCompression = HttpsCompressionMode.Compress,
					FileProvider = new PhysicalFileProvider(
						Path.Join(hostEnvironment?.ContentRootPath, "./build/assets"))
				});//.UseMiddleware<NodejsMiddleware>();
		}

		internal static async Task<NodejsResponse?> InvokeNodejsService(
			this INodeJSService nodeJSService,
			NodejsOptions options,
			HttpContext context,
			bool? overrideBodyOnlyReply = null,
			CancellationToken cancellationToken = default)
		{
			return await InvokeNodejsService(nodeJSService, options, context, false, overrideBodyOnlyReply, null, cancellationToken)
				.ConfigureAwait(false);
		}

		internal static async Task<NodejsResponse?> InvokeNodejsService(
			this INodeJSService nodeJSService,
			NodejsOptions options,
			HttpContext context,
			bool shouldGzipCompress,
			bool? overrideBodyOnlyReply = null,
			CancellationToken cancellationToken = default)
		{
			return await InvokeNodejsService(nodeJSService, options, context, shouldGzipCompress, overrideBodyOnlyReply, null, cancellationToken)
				.ConfigureAwait(false);
		}

		internal static async Task<NodejsResponse?> InvokeNodejsService(
			this INodeJSService nodeJSService,
			NodejsOptions options,
			HttpContext context,
			bool shouldGzipCompress,
			bool? overrideBodyOnlyReply = null,
			RequestOverrides? overrides = null,
			CancellationToken cancellationToken = default)
		{
			ArgumentNullException.ThrowIfNull(nameof(nodeJSService));
			ArgumentNullException.ThrowIfNull(nameof(options));

			bool bodyOnlyReply = overrideBodyOnlyReply ?? options.BodyOnlyReply;

			if (bodyOnlyReply == true)
			{
				Stream? streamResp = await nodeJSService.InvokeFromFileAsync<Stream>(
				   modulePath: options.ScriptPath,
				   args: new object[]
				   {
						await SetupRequest(
							context,
							bodyOnlyReply,
							overrides).ConfigureAwait(false)
				   },
				   cancellationToken: cancellationToken).ConfigureAwait(false);

				return streamResp == null ? null : new NodejsBodyOnlyResponse(streamResp);
			}

			return Debugger.IsAttached
				? await DebugNodejsInvokation(nodeJSService, options, context, bodyOnlyReply, overrides, cancellationToken).ConfigureAwait(false)
				: await nodeJSService.InvokeFromFileAsync<NodejsResponse>(
					modulePath: options.ScriptPath,
					args: new object[]
					{
						await SetupRequest(
							context,
							bodyOnlyReply,
							overrides).ConfigureAwait(false)
					},
					cancellationToken: cancellationToken).ConfigureAwait(false);
		}

		internal static async Task<Stream> CompressContentAsync(this Stream input)
		{
#pragma warning disable CA2000 // Dispose objects before losing scope
			GZipStream compressor = new(new MemoryStream(), CompressionLevel.Fastest);
#pragma warning restore CA2000 // Dispose objects before losing scope
			await input.CopyToAsync(compressor).ConfigureAwait(false);
			await compressor.FlushAsync().ConfigureAwait(false);
			input.Close();
			compressor.BaseStream.Position = 0;
			return compressor.BaseStream;
		}

		private static async Task<NodejsResponse?> DebugNodejsInvokation(
			INodeJSService nodeJSService,
			NodejsOptions options,
			HttpContext context,
			bool? overrideBodyOnlyReply = null,
			RequestOverrides? overrides = null,
			CancellationToken cancel = default)
		{
			bool bodyOnlyReply = overrideBodyOnlyReply ?? options.BodyOnlyReply;
			string? nodeResp;
			INodejsRequest? req = null;
			try
			{
				req = await SetupRequest(
							context,
							overrideBodyOnlyReply ?? options.BodyOnlyReply,
							overrides).ConfigureAwait(false);
				nodeResp = await nodeJSService.InvokeFromFileAsync<string>(
						modulePath: options.ScriptPath,
						args: new object[]
						{
							req
						},
						cancellationToken: cancel).ConfigureAwait(false);

				if (string.IsNullOrEmpty(nodeResp))
					return null;
			}
			catch (Exception ex)
			{
				string r = req != null ? JsonSerializer.Serialize(req) : string.Empty;
				throw new InvalidOperationException($"Nodejs invoke error {ex.Message} {r}", ex);
			}
			try
			{
				using Stream nodeRespStream = new MemoryStream(Encoding.UTF8.GetBytes(nodeResp));
				return await JsonSerializer.DeserializeAsync<NodejsResponse>(
					nodeRespStream,
					new JsonSerializerOptions
					{
						PropertyNameCaseInsensitive = true,
						IgnoreReadOnlyProperties = true,
					},
					cancel).ConfigureAwait(false);
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"NodejsResponse is invalid - ${ex.Message} - payload: ${nodeResp}", ex);
			}
		}

		private static async ValueTask<INodejsRequest> SetupRequest(HttpContext context, bool bodyOnlyReply, RequestOverrides? overrides = null)
		{
			HttpRequest request = context.Request;
			IDictionary<string, string> headers = context.Request.Headers
				.ToDictionary(k => k.Key.StartsWith(':') ? k.Key[1..] : k.Key, v => v.Value.ToString());
			NodejsDefaultRequest req = new(
				request.Method,
				headers,
				overrides == null ? request.Path : overrides.Path,
				request.QueryString.ToString(),
				request.Host.ToString());

			if (request.ContentLength > 0)
			{
				ReadResult bodyResult = await request.BodyReader.ReadAsync().ConfigureAwait(false);
				req.Body = Encoding.UTF8.GetString(bodyResult.Buffer);
			}
			req.BodyOnlyReply = bodyOnlyReply;

			return req;
		}
	}
}
