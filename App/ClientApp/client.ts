import { createApp } from './app';
import { Vue } from 'vue/types/vue';
import { Kernel } from './interfaces/index';

const kernel: Kernel = createApp();
const app: Vue = kernel.app;

app.$mount('#app');
