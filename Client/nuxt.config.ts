// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
    // Fixes display sizing with Chrome DevTools
    // https://stackoverflow.com/a/40346515/18790415
    app: {
        head: {
            charset: 'utf-8',
            viewport: 'width=device-width, initial-scale=1, maximum-scale=1, user-scalable=0'
        }
    },
    css: ["primevue/resources/themes/aura-dark-noir/theme.css", "~/assets/css/cardhub.css"],
    devtools: { enabled: true },
    // spaLoadingTemplate: true,
    modules: ["@pinia/nuxt", "@vueuse/nuxt", "@nuxt/test-utils/module", "@nuxtjs/tailwindcss", "nuxt-primevue", "nuxt-svgo"],
    pinia: {
        storesDirs: ["./stores/**"]
    },
    primevue: {
        options: {
            ripple: true,
            inputStyle: "filled"
        }
    },
    runtimeConfig: {
        public: {
            baseURL: process.env.BASE_URL,
            baseHub: "basehub", // SignalR hub path, must be same as one in Program.cs
            reconnectTimeout: 30 // Seconds
        }
    },
    ssr: false,
    svgo: {
        autoImportPath: "./assets/icons/"
    }
});
