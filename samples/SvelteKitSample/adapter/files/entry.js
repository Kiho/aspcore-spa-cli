import { WriteStream, createWriteStream } from 'fs';
import { installFetch } from '@sveltejs/kit/install-fetch';
import { Server } from 'SERVER';
// export { Server } from '../output/server/index.js';
import { manifest } from 'MANIFEST';
// import { SSRManifest as manifest } from '../../node_modules/@sveltejs/kit';

// replaced at build time
const debug = true; // DEBUG;

installFetch();

const server = new Server(manifest);

/**
 * @typedef {import('@azure/functions').AzureFunction} AzureFunction
 * @typedef {import('@azure/functions').Context} Context
 * @typedef {import('@azure/functions').HttpRequest} HttpRequest
 */

/**
 * @param {Context} context
 */
async function index(context) {
	const request = toRequest(context);

	if (debug) {
		context.log(`Request: ${JSON.stringify(request)}`);
	}

	const rendered = await server.respond(request);
	const response = await toResponse(rendered);

	if (debug) {
		context.log(`Response: ${JSON.stringify(response)}`);
	}

	context.res = response;
}

/**
 * @param {Context} context
 * @returns {Request}
 * */
function toRequest(req) {
	const { method, headers, rawBody: body } = req;
	// because we proxy all requests to the render function, the original URL in the request is /api/__render
	// this header contains the URL the user requested
	const originalUrl = 'http://localhost:5004/'; // headers['x-ms-original-url'];

	/** @type {RequestInit} */
	const init = {
		method,
		headers: new Headers(headers)
	};

	if (method !== 'GET' && method !== 'HEAD') {
		init.body = typeof body === 'string' ? Buffer.from(body, 'utf-8') : body;
	}

	return new Request(originalUrl, init);
}

/**
 * @param {Response} rendered
 * @returns {Promise<Record<string, any>>}
 */
async function toResponse(rendered) {
	const { status } = rendered;
	const resBody = new Uint8Array(await rendered.arrayBuffer());

	/** @type {Record<string, string>} */
	const resHeaders = {};
	rendered.headers.forEach((value, key) => {
		resHeaders[key] = value;
	});

	return {
		status,
		body: resBody,
		headers: resHeaders,
		isRaw: true
	};
}

const _isDebug = process.env.NODE_ENV === 'development';
const _encoder = new TextEncoder();
let _logger = null;

if (_isDebug) {
    const logPath = __dirname + '/debug.log';
    console.info(`log path : ${logPath}`);
    _logger = createWriteStream(logPath, {flags : 'w'});
}

const HttpHandler = (
  callback, 
  origRequest) => {
    console.log('callback', callback);
    console.log('origRequest', origRequest);
  try {
    // init({ paths: { base: '', assets: '/.' }, prerendering: true })
    // origRequest.query = new URLSearchParams(origRequest.queryString)
    // origRequest.rawBody = _encoder.encode(origRequest.body)

    if (_isDebug) {
      _logger.write(`svelte request payload - ${JSON.stringify(origRequest)} \r\n`)
    }

    server.respond(toRequest(origRequest))
      .then((rendered) => toResponse(rendered))
      .then((resp) => {
          const body = new TextDecoder().decode(resp.body);
          console.log(body);
          if (_isDebug) {
              _logger.write(`svelte response - ${JSON.stringify(resp)} \r\n`)
          }
          if (origRequest.bodyOnlyReply)
              callback(null, body);
          else{            
              callback(null, {
                  status: resp.status,
                  headers: resp.headers,
                  body: body
              })
              // callback(null, body)
          }
      })
      .catch((err) => callback(err, null));

    // const rendered = await server.respond(origRequest);
    // const response = await toResponse(rendered);

    // if (_isDebug) {
    //   _logger.write(`svelte response - ${JSON.stringify(resp)} \r\n`);
    // }

    // if (origRequest.bodyOnlyReply) {      
    //   callback(null, (response).body);
    // }
    // else {      
    //   // callback(null, {
    //   //     status: resp.status,
    //   //     headers: resp.headers,
    //   //     body: resp.body
    //   // })
    //   callback(null, response)
    // }
  } catch (err) {
    callback(err, null)
  }
};

module.exports = HttpHandler;