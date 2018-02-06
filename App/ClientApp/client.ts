import { createApp } from './app';
import { Kernel } from './typing';
import { Vue } from 'vue/types/vue';
import { VueRouter } from 'vue-router/types/router';

const kernel: Kernel = createApp();
const app: Vue = kernel.app;
const router: VueRouter = kernel.router;

router.onReady(() => {
    app.$mount('#app');
});
