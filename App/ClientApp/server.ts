import { createApp } from './app';
import { Vue } from 'vue/types/vue';
import { SpaResponse, Kernel } from './interfaces';
import { VueRouter } from 'vue-router/types/router';

export default (spaResponse: SpaResponse): Promise<Vue> => {
    return new Promise<Vue>((resolve: any, reject: any) => {
        const kernel: Kernel = createApp();
        const app: Vue = kernel.app;
        const router: VueRouter = kernel.router;

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
