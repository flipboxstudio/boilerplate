process.env.VUE_ENV = 'server'

const LRU = require('lru-cache')
const prerendering = require('aspnet-prerendering')
const { createBundleRenderer } = require('vue-server-renderer')

function createRenderer () {
  // https://github.com/vuejs/vue/blob/dev/packages/vue-server-renderer/README.md#why-use-bundlerenderer
  return createBundleRenderer(require('./dist/vue-ssr-server-bundle.json'), {
    // for component caching
    cache: LRU({
      max: 1000,
      maxAge: 1000 * 60 * 15
    }),
    // recommended for performance
    runInNewContext: false,
    // The client manifests are optional, but it allows the renderer
    // to automatically infer preload/prefetch links and directly add <script>
    // tags for any async chunks used during render, avoiding waterfall request
    clientManifest: require('./../wwwroot/dist/vue-ssr-client-manifest.json')
  })
}

module.exports = prerendering.createServerRenderer(function (params) {
  return new Promise(function (resolve) {
    createRenderer().renderToString(params, function (err, html) {
      resolve({
        globals: params.data,
        html: err ? err.message : html,
        statusCode: err ? 500 : 200
      })
    })
  })
})
