import Vue from 'vue';
import { createRouter } from './router';
import App from './components/App.vue';

export function createApp() {
    const router = createRouter();

    const app = new Vue({
        router: router,
        render: h => h(App)
    });

    return { app, router };
}