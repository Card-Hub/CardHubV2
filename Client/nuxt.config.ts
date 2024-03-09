// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
    css: ["primevue/resources/themes/aura-dark-noir/theme.css", "~/assets/css/cardhub.css"],
    devtools: { enabled: true },
    // spaLoadingTemplate: true,
    modules: ["@pinia/nuxt", "@nuxtjs/tailwindcss", "nuxt-primevue", "nuxt-svgo", "@nuxt/test-utils/module"],
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
            hubPath: "basehub", // SignalR hub path, must be same as one in Program.cs
            reconnectTimeout: 30 // Seconds
        }
    },
    ssr: false,
    svgo: {
        autoImportPath: "./assets/icons/"
    }
});
