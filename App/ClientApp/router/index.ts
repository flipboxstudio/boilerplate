import Vue from 'vue';
import VueRouter from 'vue-router';
import { RouterMeta } from '../typing';
import Home from '@/components/pages/Home/Component.vue';
import About from '@/components/pages/About/Component.vue';
import NotFound from '@/components/pages/Errors/NotFound/Component.vue';

Vue.use(VueRouter);

export function createRouter(): VueRouter {
    return new VueRouter({
        mode: 'history',
        routes: [
            { path: '/', component: Home },
            { path: '/about', component: About },
            { path: '*', component: NotFound, meta: { statusCode: 404 } as RouterMeta }
        ]
    });
}
