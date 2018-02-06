import { Store } from 'vuex';
import { Vue } from 'vue/types/vue';
import { VueRouter, Component } from 'vue-router/types/router';
import { BootFuncParams } from 'aspnet-prerendering';

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
    urlPath: string,
    auth: Auth
}

export interface RouterMeta {
    statusCode: number
}

export interface ServerRendererKernel {
    app: Vue,
    meta: RouterMeta
}

export interface ServerContext {
    data: SpaResponse
}

export interface BootFuncParameters extends BootFuncParams {
    data: SpaResponse
}