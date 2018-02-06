const path = require('path');
const webpack = require('webpack');
const merge = require('webpack-merge');
const isProduction = process.env.NODE_ENV === 'production';
const baseConfig = isProduction ? require('./webpack.config.prod') : require('./webpack.config.dev');

const webpackConfig = [
    // Client
    merge(baseConfig, {
        entry: {
            client: './ClientApp/client.ts'
        },
        output: {
            publicPath: '/dist/'
        },
        plugins: isProduction
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
                                path.join(__dirname, 'node_modules')
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
