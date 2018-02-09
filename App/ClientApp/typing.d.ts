import { Store } from 'vuex';
import { Vue } from 'vue/types/vue';
import { VueRouter } from 'vue-router/types/router';
import { BootFuncParams } from 'aspnet-prerendering';

export interface IDebugTool {
  log(message?: any, ...optionalParams: any[]): void
  error(message?: any, ...optionalParams: any[]): void
  info(message?: any, ...optionalParams: any[]): void
}

export interface IKernel {
  app: Vue,
  router: VueRouter,
  store: Store<any>
}

export interface IApplicationUser {
  id: string,
  userName: string,
  email: string
}

export interface IAuth {
  user: IApplicationUser
}

export interface ISpaResponse {
  auth: IAuth
}

export interface IBootFuncParams extends BootFuncParams {
  data: ISpaResponse
}

declare global {
  interface Window {
    auth: IAuth
  }
}
