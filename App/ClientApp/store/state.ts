import { SpaResponse } from './../typing'
import { Auth } from './classes/auth/Auth'

export class DefaultState implements SpaResponse {
  public urlPath: string
  public auth: Auth

  constructor () {
    this.urlPath = ''
    this.auth = new Auth()
  }
}

export default new DefaultState()
