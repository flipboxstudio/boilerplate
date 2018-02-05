const path = require('path');
const webpack = require('webpack');
const merge = require('webpack-merge');
const baseConfig = process.env.NODE_ENV === 'production'
    ? require('./webpack.config.prod')
    : require('./webpack.config.dev');

const webpackConfig = [
    // Client
    merge(baseConfig, {
        entry: {
            client: './ClientApp/client.ts'
        },
        output: {
            publicPath: '/dist/'
        },
        plugins: process.env.NODE_ENV === 'production'
            ? [
                // split vendor js into its own file
                new webpack.optimize.CommonsChunkPlugin({
                    name: 'vendor',
                    minChunks(module) {
                        // any required modules inside node_modules are extracted to vendor
                        return (
                            module.resource &&
                            /\.js$/.test(module.resource) &&
                            module.resource.indexOf(
                                path.join(__dirname, '../node_modules')
                            ) === 0
                        )
                    }
                }),
                // extract webpack runtime and module manifest to its own file in order to
                // prevent vendor hash from being updated whenever app bundle is updated
                new webpack.optimize.CommonsChunkPlugin({
                    name: 'manifest',
                    minChunks: Infinity
                }),
                // This instance extracts shared chunks from code splitted chunks and bundles them
                // in a separate chunk, similar to the vendor chunk
                // see: https://webpack.js.org/plugins/commons-chunk-plugin/#extra-async-commons-chunk
                new webpack.optimize.CommonsChunkPlugin({
                    name: 'client',
                    async: 'vendor-async',
                    children: true,
                    minChunks: 3
                }),
            ] : []
    }),
    // Renderer
    merge(baseConfig, {
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
