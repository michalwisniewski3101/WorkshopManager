// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  plugins: [
    '~/plugins/fontawesome.js',
    '~/plugins/pinia.js',
    '~/plugins/vuetify.ts',
  ], 
  router: {
    middleware: ['auth'],
  },
  build: {
    transpile: ['vuetify'],
  },
  modules: ['@nuxtjs/vuetify'], // Dodaj moduł Vuetify
  compatibilityDate: '2024-04-03',
  devtools: { enabled: true },
  runtimeConfig: {
    public: {
      apiBase: 'https://localhost:44347' // Domyślny URL API
    }
  },
  modules: [
    "vuetify-nuxt-module",
  ],
  ssr: false,
  vite: {
    server: {
      proxy: {
        '/api': {
          target: 'https://localhost:44347',  // Adres twojego backendu
          changeOrigin: true,
          secure: false, // Jeśli używasz samopodpisanego certyfikatu SSL
          rewrite: (path) => path.replace(/^\/api/, '/api') // Przepisz ścieżkę, aby zgadzała się z backendem
        }
      }
    },
    vue: {
      template: {
        transformAssetUrls: {
          base: null, // Dodaj konfigurację transformacji, jeśli jest potrzebna
        },
      },
    }

  }
})
