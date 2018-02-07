import { ApplicationUser as iApplicationUser } from './../../../typing'

export class ApplicationUser implements iApplicationUser {
  public id: string
  public userName: string
  public email: string

  constructor () {
    this.id = ''
    this.userName = ''
    this.email = ''
  }
}
