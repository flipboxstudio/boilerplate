import { DebugTool as iDebugTool } from '../typing'
const debug: any = require('debug')

if (process.env.NODE_ENV !== 'production') {
  debug.enable('app:*')
} else {
  debug.disable('app:*')
}

const log = debug('app:__LOG__')
const info = debug('app:__INFO__')
const error = debug('app:__ERROR__')

class DebugTool implements iDebugTool {
  public log = log
  public info = info
  public error = error
}

export default new DebugTool()
