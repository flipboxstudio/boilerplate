import Vue from 'vue'
import Vuex, { Store } from 'vuex'
import state from './state'

Vue.use(Vuex)

export function createStore (): Store<any> {
  return new Store({
    state,
    strict: true
  })
}
