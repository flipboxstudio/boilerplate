import Vue from 'vue';
import VueRouter from 'vue-router';
import Home from '@/components/pages/Home.vue';
import About from '@/components/pages/About.vue';

Vue.use(VueRouter);

export function createRouter(): VueRouter {
    return new VueRouter({
        mode: 'history',
        routes: [
            { path: '/', component: Home },
            { path: '/about', component: About }
        ]
    });
}
