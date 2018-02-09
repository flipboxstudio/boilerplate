import { IAuth } from './../../../typing'
import { ApplicationUserState } from './ApplicationUserState'

export class AuthState implements IAuth {
  public user: ApplicationUserState

  constructor () {
    this.user = new ApplicationUserState()
  }
}
