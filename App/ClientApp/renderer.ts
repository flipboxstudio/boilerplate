process.env.VUE_ENV = 'server';

import { createApp } from './app';
import { Vue } from 'vue/types/vue';
import { SpaResponse } from './interfaces/index';
import { createServerRenderer, RenderToStringResult } from 'aspnet-prerendering';
import { createRenderer as createVueServerRenderer, Renderer } from 'vue-server-renderer';

const createServerApp = function (spaResponse: SpaResponse): Promise<Vue> {
    return new Promise<Vue>((resolve: Function, reject: Function) => {
        const { app, router } = createApp();

        router.push(spaResponse.urlPath);

        router.onReady(() => {
            const matchedComponents = router.getMatchedComponents();

            if (!matchedComponents.length) {
                return reject({ statusCode: 404, message: 'Not Found' });
            }

            resolve(app);
        });
    });
};

export default createServerRenderer(({ data }) => {
    const bundleRenderer: Renderer = createVueServerRenderer();

    return new Promise<RenderToStringResult>((resolve: Function, reject: Function) => {
        createServerApp(data).then((app) => {
            bundleRenderer.renderToString(app).then((html: string) => {
                const result: RenderToStringResult = {
                    html: html,
                    globals: { __GLOBAL__: data || {} }
                };

                resolve(result);
            }).catch(({ message }: any) => {
                reject(message);
            });
        })
    });
});
