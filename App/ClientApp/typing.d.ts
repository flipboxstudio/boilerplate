import { Store } from 'vuex';
import { Vue } from 'vue/types/vue';
import { VueRouter } from 'vue-router/types/router';
import { BootFuncParams as iBootFuncParams } from 'aspnet-prerendering';

export interface DebugTool {
  log(message?: any, ...optionalParams: any[]): void
  error(message?: any, ...optionalParams: any[]): void
  info(message?: any, ...optionalParams: any[]): void
}

export interface Kernel {
  app: Vue,
  router: VueRouter,
  store: Store<any>
}

export interface ApplicationUser {
  id: string,
  userName: string,
  email: string
}

export interface Auth {
  token: string,
  user: ApplicationUser
}

export interface SpaResponse {
  auth: Auth
}

export interface BootFuncParams extends iBootFuncParams {
  data: SpaResponse
}
