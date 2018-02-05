const path = require('path');
const webpack = require('webpack');
const merge = require('webpack-merge');
const UglifyJsPlugin = require('uglifyjs-webpack-plugin');

const sharedConfig = function () {
    const config = {
        output: {
            path: path.resolve(__dirname, './wwwroot/dist'),
            filename: '[name].js'
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
                    test: /\.tsx?$/,
                    exclude: /node_modules/,
                    use: {
                        loader: 'ts-loader',
                        options: {
                            appendTsSuffixTo: [/\.vue$/]
                        }
                    }
                },
                {
                    test: /\.js$/,
                    exclude: /node_modules/,
                    use: {
                        loader: "babel-loader",
                        query: {
                            presets: [ 'env' ]
                        }
                    }
                }
            ]
        },
        devtool: 'cheap-module-eval-source-map'
    }

    if (process.env.NODE_ENV === 'production') {
        config.devtool = false;

        config.plugins = (module.exports.plugins || []).concat([
            new webpack.DefinePlugin({
                'process.env': {
                    NODE_ENV: '"production"'
                }
            }),
            new UglifyJsPlugin({ parallel: true })
        ]);
    }

    return config;
}

const webpackConfig = [
    // Client
    merge(sharedConfig(), {
        entry: {
            client: './ClientApp/client.ts'
        },
        output: {
            publicPath: '/dist/'
        }
    }),
    // Renderer
    merge(sharedConfig(), {
        target: 'node',
        entry: {
            renderer: './ClientApp/renderer.ts'
        },
        output: {
            path: path.resolve(__dirname, './ClientApp/dist'),
            libraryTarget: 'commonjs'
        }
    })
];

module.exports = webpackConfig;
