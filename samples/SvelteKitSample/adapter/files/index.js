import { WriteStream, createWriteStream } from 'fs'
import { URLSearchParams } from 'url'
// @ts-ignore
import { init, render } from '../output/server/index.js'
// import { Response } from '@sveltejs/kit/install-fetch'
// import { cleanup } from './cleanup'

const _isDebug = process.env.NODE_ENV === 'development'
const _encoder = new TextEncoder()
let _logger = null

if (_isDebug) {
    const logPath = __dirname + '/debug.log'
    console.info(`log path : ${logPath}`)
    _logger = createWriteStream(logPath, {flags : 'w'})
}

const HttpHandler = (
    callback, 
    origRequest) => {
    try {
        init({ paths: { base: '', assets: '/.' }, prerendering: true })
        origRequest.query = new URLSearchParams(origRequest.queryString)
        origRequest.rawBody = _encoder.encode(origRequest.body)

        if (_isDebug) {
            _logger.write(`svelte request payload - ${JSON.stringify(origRequest)} \r\n`)
        }

        render(origRequest)
            .then((resp) => {

                if (_isDebug) {
                    _logger.write(`svelte response - ${JSON.stringify(resp)} \r\n`)
                }
                if (origRequest.bodyOnlyReply)
                    callback(null, (resp).body )
                else
                    // callback(null, {
                    //     status: resp.status,
                    //     headers: resp.headers,
                    //     body: resp.body
                    // })
                    callback(null, resp)
            })
            .catch((err) => callback(err, null));
    } catch (err) {
        callback(err, null)
    }
};

// cleanup(_logger)

export default HttpHandler