import Vue from 'vue';
import Vuex, { Store } from 'vuex';

Vue.use(Vuex);

export function createStore(): Store<never[]> {
    return new Store({
        state: [],
        getters: {},
        mutations: {},
        actions: {},
        strict: true
    });
}
