import { library } from '@fortawesome/fontawesome-svg-core'
import { faSignInAlt, faSignOutAlt, faUser } from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'

library.add(faSignInAlt, faSignOutAlt, faUser)

export default defineNuxtPlugin((nuxtApp) => {
  nuxtApp.vueApp.component('font-awesome-icon', FontAwesomeIcon)
})
