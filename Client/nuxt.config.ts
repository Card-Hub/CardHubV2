// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  // devServer: {
  //   https: true
  // },
  devtools: { enabled: true },
  ssr: false,
  // spaLoadingTemplate: true,
  modules: ['@pinia/nuxt', '@nuxtjs/tailwindcss', 'nuxt-primevue', 'nuxt-svgo'],
  runtimeConfig: {
    public: {
      baseURL: process.env.BASE_URL
    },
  },
  pinia: {
    storesDirs: ['./stores/**'],
  },
  primevue: {
    options: {
      ripple: true,
      inputStyle: 'filled'
    }
  },
  css: ['primevue/resources/themes/aura-dark-noir/theme.css']
})
