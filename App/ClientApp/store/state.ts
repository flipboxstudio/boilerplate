import { SpaResponse, Auth as iAuth, ApplicationUser as iApplicationUser } from './../typing';

export class ApplicationUser implements iApplicationUser {
    id: string
    userName: string
    email: string

    constructor() {
        this.id = '';
        this.userName = '';
        this.email = '';
    }
}

export class Auth implements iAuth {
    token: string
    user: ApplicationUser

    constructor() {
        this.token = '';
        this.user = new ApplicationUser();
    }
}

export class DefaultState implements SpaResponse {
    urlPath: string
    auth: Auth

    constructor() {
        this.urlPath = '';
        this.auth = new Auth();
    }
}

export default new DefaultState();