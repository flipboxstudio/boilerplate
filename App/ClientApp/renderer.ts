process.env.VUE_ENV = 'server';

import { createApp } from './app';
import { Vue } from 'vue/types/vue';
const merge = require('webpack-merge');
import { Component } from 'vue-router/types/router';
import { createServerRenderer, RenderToStringResult } from 'aspnet-prerendering';
import { createRenderer as createVueServerRenderer, Renderer } from 'vue-server-renderer';
import { SpaResponse, ServerRendererKernel, BootFuncParameters, RouterMeta } from './typing';

const createServerApp = function (spaResponse: SpaResponse): Promise<ServerRendererKernel> {
    return new Promise<ServerRendererKernel>((resolve: Function, reject: Function) => {
        const { app, router } = createApp();

        router.push(spaResponse.urlPath);

        router.onReady(() => {
            const route = router.currentRoute;
            const meta = merge({}, route.meta) as RouterMeta;

            resolve({ app, meta } as ServerRendererKernel);
        });
    });
};

export default createServerRenderer((params: BootFuncParameters): Promise<RenderToStringResult> => {
    const { data } = params;
    const bundleRenderer: Renderer = createVueServerRenderer();

    return new Promise<RenderToStringResult>((resolve: Function, reject: Function) => {
        createServerApp(data).then((kernel) => {
            const { app, meta } = kernel;

            bundleRenderer.renderToString(app).then((html: string) => {
                const result: RenderToStringResult = {
                    html: html,
                    globals: data,
                    statusCode: meta.statusCode
                };

                resolve(result);
            }).catch((error: any) => {
                const result: RenderToStringResult = {
                    html: error.message,
                    globals: data,
                    statusCode: 500
                };

                resolve(result);
            });
        })
    });
});
