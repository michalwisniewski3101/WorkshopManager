import { defineStore } from 'pinia'
import { ref } from 'vue'
import { jwtDecode } from 'jwt-decode'; 

export const useAuthStore = defineStore('auth', () => {
  const isLoggedIn = ref(false)
  const username = ref('')
  const jwtToken = ref('')
  const roles = ref([]) 

 
  const setLoginStatus = (token, user) => {
    jwtToken.value = token
    username.value = user
    isLoggedIn.value = true


    const decodedToken = jwtDecode(token)
    roles.value = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || []


    localStorage.setItem('jwtToken', token)
    localStorage.setItem('username', user)
    localStorage.setItem('roles', JSON.stringify(roles.value)); 
  }


  const checkLoginStatus = () => {
    const token = localStorage.getItem('jwtToken')
    if (token) {
      jwtToken.value = token
      isLoggedIn.value = true
      username.value = localStorage.getItem('username')


      const decodedToken = jwtDecode(token)
      roles.value = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || []
    } else {
      isLoggedIn.value = false
      roles.value = []
    }
  }


  const logout = () => {
    jwtToken.value = ''
    username.value = ''
    roles.value = []
    isLoggedIn.value = false
    localStorage.removeItem('jwtToken')
    localStorage.removeItem('username')
    localStorage.removeItem('roles')
  }

  return {
    isLoggedIn,
    username,
    jwtToken,
    roles,
    setLoginStatus,
    checkLoginStatus,
    logout
  }
})
