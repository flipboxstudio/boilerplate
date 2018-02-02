import Vue from 'vue'
import { createApp } from './app'

interface SpaResponse {
    urlPath: string
}

export default function (spaResponse: SpaResponse) {
    return new Promise<Vue>((resolve: any, reject: any) => {
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
