// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  plugins: [
    '~/plugins/fontawesome.js',
    '~/plugins/pinia.js',
    '~/plugins/vuetify.ts',
  ],
  css: ["@/assets/styles/global.scss"],
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

  },
  vuetify: {
    vuetifyOptions: {
      theme: {
        defaultTheme: "dark",
        themes: {
          light: {
            dark: false,
            colors: {
              primary: "#c42222",
              secondary: "#8b8b8b",
              surface: "#fff",
              appBar: "#fff",
            },
          },
          dark: {
            dark: true,
            colors: {
              primary: "#c42222",
              surface: "#333",
              secondary: "#eee",
              appBar: "#333",
            },
          },
        },
      },
    },
  },
})
