const path = require('path')
const webpack = require('webpack')
const merge = require('webpack-merge')
const isProduction = process.env.NODE_ENV === 'production'
const baseConfig = isProduction ? require('./webpack.config.prod') : require('./webpack.config.dev')
const VueSSRServerPlugin = require('vue-server-renderer/server-plugin')
const VueSSRClientPlugin = require('vue-server-renderer/client-plugin')

const clientPlugins = isProduction ? [
  // split vendor js into its own file
  new webpack.optimize.CommonsChunkPlugin({
    name: 'vendor',
    minChunks (module) {
      // any required modules inside node_modules are extracted to vendor
      return (
        module.resource &&
        /\.js$/.test(module.resource) &&
        module.resource.indexOf(
          path.join(__dirname, 'node_modules')
        ) === 0
      )
    }
  }),
  // extract webpack runtime and module manifest to its own file in order to
  // prevent vendor hash from being updated whenever app bundle is updated
  new webpack.optimize.CommonsChunkPlugin({
    name: 'manifest',
    minChunks: Infinity
  })
] : []

const webpackConfig = [
  // Client
  merge(baseConfig, {
    entry: {
      base: ['babel-polyfill', './ClientApp/base.ts']
    },
    output: {
      path: path.resolve(__dirname, './wwwroot/dist/base'),
      publicPath: '/dist/base/'
    },
    plugins: clientPlugins
  }),
  // Client
  merge(baseConfig, {
    entry: {
      client: ['babel-polyfill', './ClientApp/client.ts']
    },
    output: {
      publicPath: '/dist/'
    },
    plugins: [
      // This plugins generates `vue-ssr-client-manifest.json` in the
      // output directory.
      new VueSSRClientPlugin()
    ].concat(clientPlugins)
  }),
  // Server
  merge(baseConfig, {
    target: 'node',
    entry: {
      server: './ClientApp/server.ts'
    },
    output: {
      path: path.resolve(__dirname, './ClientApp/dist'),
      libraryTarget: 'commonjs2'
    },
    plugins: [
      // This plugins generates `vue-ssr-server-bundle.json` in the
      // output directory.
      new VueSSRServerPlugin()
    ]
  })
]

module.exports = webpackConfig
