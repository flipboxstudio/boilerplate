const webpack = require('webpack')
const merge = require('webpack-merge')
const base = require('./webpack.config.base')
const UglifyJsPlugin = require('uglifyjs-webpack-plugin')
const ExtractTextPlugin = require('extract-text-webpack-plugin')
const cssLoader = {
  loader: 'css-loader',
  options: {
    minimize: true,
    sourceMap: false
  }
}

module.exports = merge(base, {
  devtool: false,
  module: {
    rules: [{
      test: /\.css$/,
      use: ExtractTextPlugin.extract({
        fallback: 'style-loader',
        use: [cssLoader, 'postcss-loader']
      })
    },
    {
      test: /\.scss$/,
      use: ExtractTextPlugin.extract({
        fallback: 'style-loader',
        use: [cssLoader, 'postcss-loader', 'sass-loader']
      })
    }]
  },
  plugins: [
    new webpack.DefinePlugin({
      'process.env': {
        NODE_ENV: '"production"'
      }
    }),
    new ExtractTextPlugin('client.css'),
    // keep module.id stable when vendor modules does not change
    new webpack.HashedModuleIdsPlugin(),
    // enable scope hoisting
    new webpack.optimize.ModuleConcatenationPlugin(),
    new UglifyJsPlugin({
      parallel: true
    })
  ]
})
