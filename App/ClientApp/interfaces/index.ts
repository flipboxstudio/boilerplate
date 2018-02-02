import { Store } from 'vuex';
import { Vue } from 'vue/types/vue';
import { VueRouter } from 'vue-router/types/router';

export interface Kernel {
    app: Vue,
    router: VueRouter,
    store: Store<never[]>
}

export interface SpaResponse {
    urlPath: string
}

export interface ServerContext {
    data: SpaResponse
}
