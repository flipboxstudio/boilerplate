const path = require('path')
const webpack = require('webpack')
const merge = require('webpack-merge')
const base = require('./webpack.config.base')

module.exports = merge(base, {
  module: {
    rules: [{
      enforce: 'pre',
      test: /\.ts?$/,
      include: /ClientApp/,
      exclude: /node_modules/,
      loader: 'tslint-loader',
      options: {
        typeCheck: true,
        emitErrors: true,
        failOnHint: true,
        configFile: path.resolve(__dirname, './tslint.json'),
        tsConfigFile: path.resolve(__dirname, './tsconfig.json')
      }
    }, {
      test: /\.css$/,
      use: ['style-loader', 'css-loader', 'postcss-loader']
    },
    {
      test: /\.scss$/,
      use: ['style-loader', 'css-loader', 'postcss-loader', 'sass-loader']
    }]
  },
  plugins: [
    new webpack.NamedModulesPlugin(),
    new webpack.NoEmitOnErrorsPlugin()
  ]
})
