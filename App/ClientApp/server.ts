import { Vue } from 'vue/types/vue'
import { createApp } from './app'
import { BootFuncParams } from './typing'

export default (serverContext: BootFuncParams): Promise<Vue> => {
  return new Promise<Vue>((resolve) => {
    const { app, router } = createApp()

    router.push(serverContext.url)

    router.onReady(() => {
      resolve(app)
    })
  })
}
