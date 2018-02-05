const path = require('path');

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
            exclude: /node_modules/,
            loader: 'vue-loader'
        },
        {
            test: /\.ts?$/,
            exclude: /node_modules/,
            use: [
                {
                    loader: 'ts-loader',
                    options: { appendTsSuffixTo: [/\.vue$/] }
                }
            ]
        },
        {
            test: /\.css$/,
            exclude: /node_modules/,
            use: [
                {
                    loader: 'style-loader',
                },
                {
                    loader: 'css-loader',
                    options: {
                        importLoaders: 1,
                    }
                },
                {
                    loader: 'postcss-loader'
                }
            ]
        }
    ]},
    devtool: 'cheap-module-eval-source-map'
}