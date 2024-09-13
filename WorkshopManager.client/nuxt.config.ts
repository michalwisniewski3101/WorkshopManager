// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: '2024-04-03',
  devtools: { enabled: true },
  runtimeConfig: {
    public: {
      apiBase: 'https://localhost:7038/api' // Domyślny URL API
    }
  },
  vite: {
    server: {
      proxy: {
        '/api': {
          target: 'https://localhost:7038',  // Adres twojego backendu
          changeOrigin: true,
          secure: false, // Jeśli używasz samopodpisanego certyfikatu SSL
          rewrite: (path) => path.replace(/^\/api/, '/api') // Przepisz ścieżkę, aby zgadzała się z backendem
        }
      }
    }
  }
})
