import Vue from 'vue';
import { Store } from 'vuex';
import { Kernel } from './interfaces';
import { createStore } from './store';
import App from './components/App.vue';
import { createRouter } from './router';
import { Vue as VueType } from 'vue/types/vue';
import { VueRouter } from 'vue-router/types/router';

export function createApp(): Kernel {
    const router: VueRouter = createRouter();
    const store: Store<never[]> = createStore();

    const app: VueType = new Vue({
        router: router,
        store: store,
        render: h => h(App)
    });

    return { app, router, store };
}
