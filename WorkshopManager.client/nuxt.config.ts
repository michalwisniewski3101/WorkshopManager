// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  plugins: [
    '~/plugins/fontawesome.js',
    '~/plugins/pinia.js',
  ], 
  router: {
    middleware: ['auth'],
  },
  compatibilityDate: '2024-04-03',
  devtools: { enabled: true },
  runtimeConfig: {
    public: {
      apiBase: 'https://localhost:44347' // Domyślny URL API
    }
  },
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
    }
  }
})
