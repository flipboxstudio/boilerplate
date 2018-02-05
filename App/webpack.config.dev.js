const webpack = require('webpack');
const merge = require('webpack-merge');
const base = require('./webpack.config.base');

module.exports = merge(base, {
    plugins: [
        new webpack.NamedModulesPlugin(),
        new webpack.NoEmitOnErrorsPlugin()
    ]
});