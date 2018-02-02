const path = require('path');
const webpack = require('webpack');
const merge = require('webpack-merge');

const sharedConfig = function () {
    return {
        output: {
            path: path.resolve(__dirname, './wwwroot/dist'),
            publicPath: '/dist/',
            filename: '[name].js'
        },
        resolve: {
            alias: {
                vue$: 'vue/dist/vue.esm.js',
                '@': path.resolve(__dirname, './ClientApp')
            },
            extensions: ['.ts', '.js', '.vue', '.json']
        },
        module: {
            rules: [{
                    test: /\.vue$/,
                    loader: 'vue-loader',
                },
                {
                    test: /\.ts$/,
                    exclude: /node_modules/,
                    use: {
                        loader: 'ts-loader',
                        options: {
                            appendTsSuffixTo: [/\.vue$/]
                        }
                    }
                }
            ]
        },
        devtool: '#eval-source-map'
    }
}

module.exports = [
    merge(sharedConfig(), {
        target: 'node',
        entry: {
            server: './ClientApp/server.ts',
        },
        output: {
            libraryTarget: 'commonjs2',
        }
    }),
    merge(sharedConfig(), {
        entry: {
            client: './ClientApp/client.ts'
        },
    })
];

if (process.env.NODE_ENV === 'production') {
    module.exports.devtool = '#source-map';

    module.exports.plugins = (module.exports.plugins || []).concat([
        new webpack.DefinePlugin({
            'process.env': {
                NODE_ENV: '"production"'
            }
        }),
        new webpack.optimize.UglifyJsPlugin({
            sourceMap: true,
            compress: {
                warnings: false
            }
        }),
        new webpack.LoaderOptionsPlugin({
            minimize: true
        })
    ]);
}