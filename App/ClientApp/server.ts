import { Vue } from 'vue/types/vue'
import { createApp } from './app'
import { IBootFuncParams } from './typing'

export default (serverContext: IBootFuncParams): Promise<Vue> => {
  return new Promise<Vue>((resolve) => {
    const { app, router } = createApp(serverContext.data)

    router.push(serverContext.url)

    router.onReady(() => {
      resolve(app)
    })
  })
}
