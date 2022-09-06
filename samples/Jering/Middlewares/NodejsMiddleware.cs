using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

using Jering.Javascript.NodeJS;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jering
{
	/// <summary>
	/// Nodejs middleware with Jering lib.
	/// </summary>
	public class NodejsMiddleware
	{
		private readonly bool _ShouldGzipCompress = true;
		private readonly RequestDelegate _Next;
		private readonly ILogger<NodejsMiddleware> _Logger;
		private readonly INodeJSService _NodeJSService;
		private readonly IOptionsMonitor<NodejsOptions> _NodejsOptions;

		/// <summary>
		/// Initializes a new instance of the <see cref="NodejsMiddleware"/> class.
		/// </summary>
		/// <param name="next">Next http request task.</param>
		/// <param name="logger">Logger for middleware.</param>
		/// <param name="nodeJSService"><see cref="INodeJSService"/> and should not be disposed.</param>
		/// <param name="options"><see cref="NodejsOptions"/>.</param>
		public NodejsMiddleware(RequestDelegate next, ILogger<NodejsMiddleware> logger, INodeJSService nodeJSService, IOptionsMonitor<NodejsOptions> options)
		{
			_Next = next ?? throw new ArgumentNullException(nameof(next));
			_Logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_NodeJSService = nodeJSService
				?? throw new ArgumentNullException(nameof(nodeJSService));
			_NodejsOptions = options ?? throw new ArgumentNullException(nameof(options));
			_ShouldGzipCompress = _NodejsOptions.CurrentValue.GzipCompressResponse;
		}

		/// <summary>
		/// Invoke the middleware pipeline handler.
		/// </summary>
		/// <param name="context">HttpContext.</param>
		/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
		public async Task InvokeAsync(HttpContext context)
		{
			if (context == null || context.Request == null || context.Request.Path == null)
				return;

			if (Debugger.IsAttached)
				_Logger.LogInformation($"{nameof(NodejsMiddleware)} is invoked for {context.Request.Path}");

			NodejsResponse? result = await _NodeJSService
				.InvokeNodejsService(_NodejsOptions.CurrentValue, context, _ShouldGzipCompress, false, null, context.RequestAborted).ConfigureAwait(false);

			if (result == null || result.Status == 404)
			{
				await _Next(context).ConfigureAwait(false);
				return;
			}

			HttpResponse httpResp = context.Response;

			httpResp.StatusCode = result.Status;

			foreach (KeyValuePair<string, string> keyValuePair in result.Headers)
			{
				httpResp.Headers.Append(keyValuePair.Key, new Microsoft.Extensions.Primitives.StringValues(keyValuePair.Value));
			}

			if (result.BodyStream == null || result.BodyStream.Length == 0)
				return;

			if (_ShouldGzipCompress)
			{
				using Stream body = await result.BodyStream.CompressContentAsync().ConfigureAwait(false);

				IHeaderDictionary headers = context.Response.Headers;
				headers.Add("Content-Encoding", new Microsoft.Extensions.Primitives.StringValues("gzip"));
				headers.Add("Content-Length", new Microsoft.Extensions.Primitives.StringValues(body.Length.ToString(System.Globalization.CultureInfo.InvariantCulture)));
				await body.CopyToAsync(context.Response.Body, context.RequestAborted).ConfigureAwait(false);
				return;
			}
			else
			{
				await httpResp.WriteAsync(result.Body, context.RequestAborted).ConfigureAwait(false);
			}
		}
	}
}