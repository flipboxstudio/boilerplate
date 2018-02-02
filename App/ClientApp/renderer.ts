process.env.VUE_ENV;

const fs = require('fs');
const path = require('path');

import createApp from './server';
import { Vue } from 'vue/types/vue';
import { ServerContext, SpaResponse } from './interfaces';
import { createRenderer, Renderer } from 'vue-server-renderer';
import { createServerRenderer, RenderToStringResult } from 'aspnet-prerendering';

export default createServerRenderer((serverContext: ServerContext): Promise<RenderToStringResult> => {
    const spaResponse: SpaResponse = serverContext.data;
    const bundleRenderer: Renderer = createRenderer();

    return new Promise<RenderToStringResult>((resolve: any, reject: any) => {
        createApp(spaResponse).then((app: Vue) => {
            bundleRenderer.renderToString(app).then((html: string) => {
                resolve({
                    html: html,
                    globals: { __GLOBAL__: spaResponse || {} }
                });
            }).catch((error: any) => {
                reject(error.message);
            });
        })
    });
});
