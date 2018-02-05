process.env.VUE_ENV = 'server';

import createApp from './server';
import { Vue } from 'vue/types/vue';
import { ServerContext } from './interfaces';
import { createRenderer as createVueServerRenderer } from 'vue-server-renderer';
import { createServerRenderer, RenderToStringResult } from 'aspnet-prerendering';

export default createServerRenderer(({ data }: ServerContext): Promise<RenderToStringResult> => {
    const bundleRenderer = createVueServerRenderer();

    return new Promise<RenderToStringResult>((resolve: Function, reject: Function) => {
        createApp(data).then((app: Vue) => {
            bundleRenderer.renderToString(app).then((html: string) => {
                resolve({
                    html: html,
                    globals: { __GLOBAL__: data || {} }
                });
            }).catch(({ message }: any) => {
                reject(message);
            });
        })
    });
});
