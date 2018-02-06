const path = require('path');
const webpack = require('webpack');
const WebpackCleanupPlugin = require('webpack-cleanup-plugin');

module.exports = {
    output: {
        path: path.resolve(__dirname, './wwwroot/dist'),
        filename: '[name].js',
    },
    resolve: {
        alias: {
            vue$: 'vue/dist/vue.esm.js',
            vuex$: 'vuex/dist/vuex.esm.js',
            'vue-router$': 'vue-router/dist/vue-router.esm.js',
            '@': path.resolve(__dirname, './ClientApp')
        },
        extensions: ['.ts', '.js', '.vue', '.json']
    },
    module: {
        rules: [{
            test: /\.vue$/,
            include: /ClientApp/,
            exclude: /node_modules/,
            loader: 'vue-loader'
        },
        {
            test: /\.ts?$/,
            include: /ClientApp/,
            exclude: /node_modules/,
            use: [
                {
                    loader: 'ts-loader',
                    options: {
                        silent: true,
                        appendTsSuffixTo: [/\.vue$/]
                    }
                }
            ]
        }
    ]},
    plugins: [
        new WebpackCleanupPlugin({
            exclude: [ '.gitignore' ]
        }),
        new webpack.EnvironmentPlugin(['NODE_ENV'])
    ],
    devtool: 'cheap-module-eval-source-map'
}