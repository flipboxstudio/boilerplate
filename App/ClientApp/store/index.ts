import Vue from 'vue';
import state from './state';
import Vuex, { Store } from 'vuex';

Vue.use(Vuex);

export function createStore(): Store<any> {
    return new Store({
        state,
        strict: true
    });
}
