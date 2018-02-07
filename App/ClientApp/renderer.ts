process.env.VUE_ENV = 'server'

import { createServerRenderer, RenderToStringResult } from 'aspnet-prerendering'
import { createRenderer as createVueServerRenderer, Renderer } from 'vue-server-renderer'
import { Vue } from 'vue/types/vue'
import { createApp } from './app'
const merge = require('webpack-merge')
import { BootFuncParameters, RouterMeta, ServerRendererKernel as iServerRendererKernel, SpaResponse } from './typing'

class ServerRendererKernel implements iServerRendererKernel {
  public app: Vue
  public meta: RouterMeta

  constructor (app: Vue, meta: RouterMeta) {
    this.app = app
    this.meta = meta
  }
}

const createServerApp = (spaResponse: SpaResponse): Promise<iServerRendererKernel> => {
  return new Promise<iServerRendererKernel>((resolve) => {
    const { app, router } = createApp()

    router.push(spaResponse.urlPath)

    router.onReady(() => {
      const route = router.currentRoute
      const meta: RouterMeta = merge({}, route.meta)

      resolve(new ServerRendererKernel(app, meta))
    })
  })
}

export default createServerRenderer((params: BootFuncParameters): Promise<RenderToStringResult> => {
  const { data } = params
  const bundleRenderer: Renderer = createVueServerRenderer()

  return new Promise<RenderToStringResult>((resolve) => {
    createServerApp(data).then((kernel) => {
      const { app, meta } = kernel

      bundleRenderer.renderToString(app).then((html: string) => {
        const result: RenderToStringResult = {
          globals: data,
          html,
          statusCode: meta.statusCode
        }

        resolve(result)
      }).catch((reason: any) => {
        const result: RenderToStringResult = {
          globals: data,
          html: reason.message,
          statusCode: 500
        }

        resolve(result)
      })
    }).catch((reason) => {
      const result: RenderToStringResult = {
        globals: {},
        html: reason.message,
        statusCode: 500
      }

      resolve(result)
    })
  })
})
