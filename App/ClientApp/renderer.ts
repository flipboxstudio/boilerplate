process.env.VUE_ENV = 'server';

import createApp from './server';
import { Vue } from 'vue/types/vue';
import { ServerContext } from './interfaces';
import { createRenderer } from 'vue-server-renderer';
import { createServerRenderer, RenderToStringResult } from 'aspnet-prerendering';

export default createServerRenderer((serverContext: ServerContext): Promise<RenderToStringResult> => {
    const spaResponse = serverContext.data;
    const bundleRenderer = createRenderer();

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
