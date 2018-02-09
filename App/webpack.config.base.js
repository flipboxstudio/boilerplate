const path = require('path')
const webpack = require('webpack')

module.exports = {
  output: {
    path: path.resolve(__dirname, './wwwroot/dist'),
    filename: '[name].js'
  },
  resolve: {
    alias: {
      vue$: 'vue/dist/vue.esm.js',
      vuex$: 'vuex/dist/vuex.esm.js',
      'vue-router$': 'vue-router/dist/vue-router.esm.js'
    },
    extensions: [
      '.ts', '.js', '.vue', '.json',
      'css', 'styl'
    ]
  },
  module: {
    rules: [{
      test: /\.(js|vue)$/,
      loader: 'eslint-loader',
      enforce: 'pre',
      include: /ClientApp/
    },
    {
      test: /\.vue$/,
      include: /ClientApp/,
      exclude: /node_modules/,
      loader: 'vue-loader'
    },
    {
      test: /\.ts?$/,
      include: /ClientApp/,
      exclude: /node_modules/,
      loader: 'ts-loader',
      options: {
        silent: true,
        appendTsSuffixTo: [/\.vue$/]
      }
    }
    ]
  },
  plugins: [
    new webpack.EnvironmentPlugin({
      NODE_ENV: 'development' // use 'development' unless process.env.NODE_ENV is defined
    })
  ],
  devtool: 'cheap-module-eval-source-map'
}
