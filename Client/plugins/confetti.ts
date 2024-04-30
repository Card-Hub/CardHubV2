import { defineNuxtPlugin } from '#app';
import Particles from "@tsparticles/vue3";
import { loadFull } from "tsparticles"; /* if you are going to use `loadFull`, install the "tsparticles" package too. */
declare module "@particles/vue3";

export default defineNuxtPlugin(nuxtApp => {
    nuxtApp.vueApp.use(Particles, {
        init: async engine => {
            await loadFull(engine); /* you can load the full tsParticles library from "tsparticles" if you need it */
        },
    })
})