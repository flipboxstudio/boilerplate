import Vue from 'vue'

declare module 'vue/types/vue' {
    interface Vue {
        $log(message?: any, ...optionalParams: any[]): void
        $info(message?: any, ...optionalParams: any[]): void
        $error(message?: any, ...optionalParams: any[]): void
    }
}
