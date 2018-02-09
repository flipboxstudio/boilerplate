import 'bootstrap'
import Vue from 'vue'
import { VueRouter } from 'vue-router/types/router'
import { Vue as VueType } from 'vue/types/vue'
import { Store } from 'vuex'
import App from './components/App.vue'
import debugTool from './lib/debug'
import { createRouter } from './router'
import { createStore } from './store'
import { Kernel } from './typing'

Vue.prototype.$log = debugTool.log
Vue.prototype.$info = debugTool.info
Vue.prototype.$error = debugTool.error

export function createApp (): Kernel {
  const router: VueRouter = createRouter()
  const store: Store<any> = createStore()

  const app: VueType = new Vue({
    render: (h) => h(App),
    router,
    store
  })

  return { app, router, store }
}
