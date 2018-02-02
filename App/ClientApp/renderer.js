const fs = require('fs');
const path = require('path');
const prerendering = require('aspnet-prerendering');
const filePath = path.join(__dirname, '../wwwroot/dist/server.js');
const code = fs.readFileSync(filePath, 'utf8');
const bundleRenderer = require('vue-server-renderer').createBundleRenderer(code, {
    runInNewContext: false
});

module.exports = prerendering.createServerRenderer(function (params) {
    return new Promise(function (resolve, reject) {
        bundleRenderer.renderToString(params.data || {}).then(function (html) {
            resolve({
                html: html,
                globals: {
                    __GLOBAL__: params.data || {}
                }
            });
        }).catch(function (error) {
            reject(error.message);
        });
    });
});