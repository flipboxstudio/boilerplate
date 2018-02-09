import { IDebugTool } from '../typing'
const debug: any = require('debug')

if (process.env.NODE_ENV !== 'production') {
  debug.enable('app:*')
} else {
  debug.disable('app:*')
}

export const createLog = (name: string) => debug(`app:${name}`)
export const log = debug('app:__LOG__')
export const info = debug('app:__INFO__')
export const error = debug('app:__ERROR__')

class DebugTool implements IDebugTool {
  public log = log
  public info = info
  public error = error
}

export default new DebugTool()
