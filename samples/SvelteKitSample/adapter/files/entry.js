import { createWriteStream } from 'fs';
import { join } from 'path';
import { installPolyfills } from '@sveltejs/kit/node/polyfills';

import { getClientIPFromHeaders } from './headers';
import { Server } from 'SERVER';
import { manifest } from 'MANIFEST';

import { cleanup } from './cleanup';

// replaced at build time
const _isDebug = DEBUG;

installPolyfills();

const _server = new Server(manifest);
let initialized;
let _logger = null;

if (_isDebug) {
    const logPath = join(__dirname, 'debug.log');
    console.info(`log path : ${logPath}`);
    _logger = createWriteStream(logPath, {flags : 'w'});
}

/**
 * @returns {Request}
 * */
function toRequest(req) {
	const { method, headers, rawBody: body, url: originalUrl } = req;
  // console.log('originalUrl', originalUrl);
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

async function initServer() {
	if (!initialized) {
		await _server.init({ env: process.env });
		initialized = true;
		_logger.write(`svelte server initialized \r\n`)
	}
}

const HttpHandler =  (
  callback, 
  origRequest) => {
  try {
		initServer().then(() => {
			if (_isDebug) {
				_logger.write(`svelte request payload - ${JSON.stringify(origRequest)} \r\n`);
			}
	
			const req = toRequest(origRequest);
			const ipAddress = getClientIPFromHeaders(req.headers);
	
			_server.respond(req, {
				getClientAddress() {
					return ipAddress;
				}
			}).then((resp) => {
				if (resp.status == 404) {
					callback(null, null);
				} else {
					if (origRequest.bodyOnlyReply){
						resp.text().then((data) => {
							callback(null, data);
						});
						// this was working fine with old version of kit
						// callback(null, resp?.body);
					} else {
						resp.text().then((data) => {
							callback(null, {
								status: resp.status,
								headers: resp.headers,
								body: data
							})
						});
					}
				}
			})
			.catch((err) => callback(err, null));
		});
  } catch (err) {
    callback(err, null)
  }
};

cleanup(_logger);

export default HttpHandler;
