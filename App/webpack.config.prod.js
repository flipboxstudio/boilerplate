const webpack = require('webpack')
const merge = require('webpack-merge')
const base = require('./webpack.config.base')
const UglifyJsPlugin = require('uglifyjs-webpack-plugin')

module.exports = merge(base, {
  devtool: false,
  plugins: [
    new webpack.DefinePlugin({
      'process.env': {
        NODE_ENV: '"production"'
      }
    }),
    // keep module.id stable when vendor modules does not change
    new webpack.HashedModuleIdsPlugin(),
    // enable scope hoisting
    new webpack.optimize.ModuleConcatenationPlugin(),
    new UglifyJsPlugin({
      parallel: true
    })
  ]
})
