import 'bootstrap'
import { VueRouter } from 'vue-router/types/router'
import { Vue } from 'vue/types/vue'
import { createApp } from './app'
import './scss/main.scss'
import { IAuth, IKernel, ISpaResponse } from './typing'

class SpaResponse implements ISpaResponse {
  public auth: IAuth

  constructor (auth: IAuth) {
    this.auth = auth
  }
}

const kernel: IKernel = createApp(new SpaResponse(window.auth))
const app: Vue = kernel.app
const router: VueRouter = kernel.router

router.onReady(() => {
  app.$mount('#app')
})
