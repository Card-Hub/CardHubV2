import { ofetch } from 'ofetch'

// Allows client-side fetching configured with a global header URL
// https://stackoverflow.com/a/75605263/18790415
export default defineNuxtPlugin(() => {
    const runtimeConfig = useRuntimeConfig();
    const instance = ofetch.create({
        baseURL: runtimeConfig.public.baseURL
    })

    return {
        provide: {
            api: instance
        }
    }
})
