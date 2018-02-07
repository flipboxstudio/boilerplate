import Vue from 'vue'
import VueRouter from 'vue-router'
import { RouterMeta as iRouterMeta } from '../typing'
import About from './../components/pages/About/Component.vue'
import NotFound from './../components/pages/Errors/NotFound/Component.vue'
import Home from './../components/pages/Home/Component.vue'

Vue.use(VueRouter)

export class RouterMeta implements iRouterMeta {
  public statusCode: number

  constructor (statusCode: number = 200) {
    this.statusCode = statusCode
  }
}

export function createRouter (): VueRouter {
  return new VueRouter({
    mode: 'history',
    routes: [
      { path: '/', component: Home },
      { path: '/about', component: About },
      { path: '*', component: NotFound, meta: new RouterMeta(404) }
    ]
  })
}
