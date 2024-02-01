// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  // devServer: {
  //   https: true
  // },
  devtools: { enabled: true },
  ssr: false,
  // spaLoadingTemplate: true,
  modules: ['nuxt-primevue', 'nuxt-svgo', '@nuxtjs/tailwindcss'],
  runtimeConfig: {
    public: {
      baseURL: process.env.BASE_URL
    },
  },
  primevue: {
    options: {
      ripple: true,
      inputStyle: 'filled'
    }
  },
  css: ['primevue/resources/themes/aura-dark-noir/theme.css']
})
