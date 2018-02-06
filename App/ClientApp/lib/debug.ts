import { DebugTool as iDebugTool } from "../typing";

const debug = require('debug');

(process.env.NODE_ENV !== 'production') ? debug.enable('app:*') : debug.disable('app:*');

const log = debug('app:__LOG__');
const info = debug('app:__INFO__');
const error = debug('app:__ERROR__');

class DebugTool implements iDebugTool {
    log = log
    info = info
    error = error
}

export default new DebugTool();