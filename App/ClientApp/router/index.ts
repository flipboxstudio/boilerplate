import Vue from 'vue';
import VueRouter from 'vue-router';

const Home = () => import('@/components/pages/Home.vue');
const About = () => import('@/components/pages/About.vue');

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
