using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

using Jering.Javascript.NodeJS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Jering
{
	[HtmlTargetElement("nodejs")]
	public class NodejsTagHelper : TagHelper
	{
		private readonly INodeJSService _NodeJSService;
		private readonly NodejsOptions _NodejsOptions;
		private readonly MemoryStream _Buffer = new();

#pragma warning disable IDE0051 // Remove unused private members
		private HttpRequest Request => ViewContext.HttpContext.Request;
#pragma warning restore IDE0051 // Remove unused private members

		[ViewContext]
		[HtmlAttributeNotBound]
		public ViewContext ViewContext { get; set; }

		public bool ResetContent { get; set; }

		public string? RoutePath { get; set; } = "/";

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		public NodejsTagHelper(INodeJSService nodeJSService, IOptionsMonitor<NodejsOptions> options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		{
			_NodeJSService = nodeJSService ?? throw new ArgumentNullException(nameof(nodeJSService));
			_NodejsOptions = options != null ? options.CurrentValue : new NodejsOptions();
		}

		public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{
			Debug.Assert(context != null && output != null);

			output.TagMode = TagMode.StartTagAndEndTag;

			if (ResetContent)
				output.Content.Clear();

			RequestOverrides? overrides = RoutePath == null ? null : new(RoutePath);

			NodejsResponse? nodejsOutput = await _NodeJSService
				.InvokeNodejsService(_NodejsOptions, ViewContext.HttpContext, true, false, overrides)
				.ConfigureAwait(false);

			if (nodejsOutput != null)
			{
				await nodejsOutput.BodyStream.CopyToAsync(_Buffer).ConfigureAwait(false);
				output.Content.SetHtmlContent(new StreamHtmlContent(_Buffer));
			}
		}
	}
}
