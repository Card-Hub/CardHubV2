import { defineVitestConfig } from '@nuxt/test-utils/config'

export default defineVitestConfig({
    test: {
        // Nuxt environment
        // https://nuxt.com/docs/getting-started/testing#using-a-nuxt-runtime-environment
        environment: 'nuxt',

        // All tests in tests/ will run in jsdom
        // https://vitest.dev/config/#environmentmatchglobs
        environmentMatchGlobs: [
            ["tests/**", "jsdom"]
        ],

        environmentOptions: {
            nuxt: {
                domEnvironment: 'jsdom',
                rootDir: __dirname,
            }
        }
    }
})