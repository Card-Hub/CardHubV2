import { ofetch } from 'ofetch'

interface PluginsInjections {
    $api: typeof ofetch
}

// Adds typing to the $api plugin
// https://stackoverflow.com/q/75046466/18790415
export { }

declare module '#app' {
    interface NuxtApp extends PluginsInjections { }
}

declare module 'nuxt/dist/app/nuxt' {
    interface NuxtApp extends PluginsInjections {}
}

declare module '@vue/runtime-core' {
    interface ComponentCustomProperties extends PluginsInjections { }
}
