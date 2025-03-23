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
  modules: ['@nuxtjs/vuetify',
    'vue3-carousel-nuxt'
  ], 
  compatibilityDate: '2024-04-03',
  devtools: { enabled: true },
  runtimeConfig: {
    public: {
      apiBase: 'https://localhost:44347' 
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
          target: 'https://localhost:44347',  
          changeOrigin: true,
          secure: false, 
          rewrite: (path) => path.replace(/^\/api/, '/api') 
        }
      }
    },
    vue: {
      template: {
        transformAssetUrls: {
          base: null, 
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
              surface: "#d7f0c2",
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
