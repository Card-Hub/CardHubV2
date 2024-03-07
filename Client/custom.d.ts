// Documentation: https://nuxt.com/modules/nuxt-svgo#usage-with-typescript
declare module '*.svg' {
    import type { DefineComponent } from 'vue'
    const component: DefineComponent
    export default component
}