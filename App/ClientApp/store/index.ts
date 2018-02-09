import Vue from 'vue'
import Vuex, { MutationPayload, Store } from 'vuex'
import { createLog } from './../lib/debug'
import modules from './modules'
import state from './state'
const log = createLog('__STATE__')

Vue.use(Vuex)

export function createStore (): Store<any> {
  return new Store({
    modules,
    plugins: process.env.NODE_ENV !== 'production' ? [
      (store) => {
        store.subscribe((mutation: MutationPayload) => {
          const { type, payload } = mutation

          log(type, payload)
        })
      }
    ] : [],
    state,
    strict: process.env.NODE_ENV !== 'production'
  })
}
