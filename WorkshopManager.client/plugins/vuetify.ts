
import { createVuetify } from 'vuetify'
import 'vuetify/styles' 
import { aliases, mdi } from 'vuetify/iconsets/mdi'



export default defineNuxtPlugin((nuxtApp) => {
  const vuetify = createVuetify({
    icons: {
      defaultSet: 'mdi',
      aliases,
      sets: {
        mdi,
      },
    },

  })
  nuxtApp.vueApp.use(vuetify)
})