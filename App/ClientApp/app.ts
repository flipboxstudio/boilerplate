import Vue from 'vue';
import VueRouter from 'vue-router';
import { createRouter } from './router';
import { createStore } from './store';
import App from './components/App.vue';
import { Store } from 'vuex';

interface Kernel {
    app: Vue,
    router: VueRouter,
    store: Store<never[]>
}

export function createApp(): Kernel {
    const router = createRouter();
    const store = createStore();

    const app = new Vue({
        router: router,
        store: store,
        render: h => h(App)
    });

    return { app, router, store };
}
