import { VueRouter } from 'vue-router/types/router'
import { Vue } from 'vue/types/vue'
import { createApp } from './app'
import './scss/main.scss'
import { Kernel } from './typing'

const kernel: Kernel = createApp()
const app: Vue = kernel.app
const router: VueRouter = kernel.router

router.onReady(() => {
  app.$mount('#app')
})
