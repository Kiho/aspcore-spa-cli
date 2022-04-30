import { WriteStream } from 'fs'

let _logger = null;

function exitHandler(options, exitCode) {
  if (options.cleanup) {
      if (_logger) _logger.close()
      console.log('clean')
  }
  if (exitCode || exitCode === 0) console.info('exit code', exitCode);
  if (options.exit) process.exit();
}

function cleanup(logger) {
  _logger = logger ? logger: null;

  //app is closing
  process.on('exit', exitHandler.bind(null,{cleanup:true}));

  //catches ctrl+c event
  process.on('SIGINT', exitHandler.bind(null, {exit:true}));

  // catches "kill pid"
  process.on('SIGUSR1', exitHandler.bind(null, {exit:true}));
  process.on('SIGUSR2', exitHandler.bind(null, {exit:true}));

  //catches uncaught exceptions
  process.on('uncaughtException', exitHandler.bind(null, {exit:true}));
}

export {
  cleanup
}