import { IApplicationUser } from './../../../typing'

export class ApplicationUserState implements IApplicationUser {
  public id: string
  public userName: string
  public email: string

  constructor () {
    this.id = ''
    this.userName = ''
    this.email = ''
  }
}
