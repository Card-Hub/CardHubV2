import { defineVitestConfig } from '@nuxt/test-utils/config'

export default defineVitestConfig({
    test: {
        environmentMatchGlobs: [
            // all tests in tests/ will run in jsdom
            ['tests/**', 'jsdom']
        ]
    }
})
