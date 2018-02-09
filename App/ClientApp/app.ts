import Vue from 'vue'
import { VueRouter } from 'vue-router/types/router'
import { Vue as VueType } from 'vue/types/vue'
import { Store } from 'vuex'
import App from './components/App.vue'
import debugTool from './lib/debug'
import { createRouter } from './router'
import { createStore } from './store'
import { SET_USER } from './store/modules/auth/mutation-types'
import { IKernel, ISpaResponse } from './typing'

Vue.prototype.$log = debugTool.log
Vue.prototype.$info = debugTool.info
Vue.prototype.$error = debugTool.error

export function createApp (data: ISpaResponse): IKernel {
  const router: VueRouter = createRouter()
  const store: Store<any> = createStore()

  const app: VueType = new Vue({
    created () {
      this.$store.commit(`auth/${SET_USER}`, data.auth.user)
    },
    render: (h) => h(App),
    router,
    store
  })

  return { app, router, store }
}
