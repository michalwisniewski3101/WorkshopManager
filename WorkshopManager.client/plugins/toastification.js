import Toast, { POSITION } from 'vue-toastification'
import 'vue-toastification/dist/index.css'

export default defineNuxtPlugin((nuxtApp) => {
    nuxtApp.vueApp.use(Toast, {
        position: POSITION.TOP_RIGHT,
        timeout: 3000, // czas wyświetlania w ms
        closeOnClick: true,
        pauseOnHover: true,
        draggable: true
    })
})
