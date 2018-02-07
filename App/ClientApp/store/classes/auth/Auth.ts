import { Auth as iAuth } from './../../../typing'
import { ApplicationUser } from './ApplicationUser'

export class Auth implements iAuth {
  public token: string
  public user: ApplicationUser

  constructor () {
    this.token = ''
    this.user = new ApplicationUser()
  }
}
