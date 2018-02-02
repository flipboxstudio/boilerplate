import { createApp } from './app'

export default function (context: any) {
    return new Promise((resolve: any, reject: any) => {
        const { app, router } = createApp();

        if (!context.url) {
            return resolve(app);
        }

        router.push(context.url);

        router.onReady(() => {
            const matchedComponents = router.getMatchedComponents();

            if (!matchedComponents.length) {
                return reject({ statusCode: 404, message: 'Not Found' });
            }

            resolve(app);
        });
    });
};
