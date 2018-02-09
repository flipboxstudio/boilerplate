import { Auth as iAuth } from './../../../typing'
import { ApplicationUser } from './ApplicationUser'

export class Auth implements iAuth {
  public user: ApplicationUser

  constructor () {
    this.user = new ApplicationUser()
  }
}
