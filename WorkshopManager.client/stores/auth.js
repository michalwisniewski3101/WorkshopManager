import { defineStore } from 'pinia'
import { ref } from 'vue'
import { jwtDecode } from 'jwt-decode'; // Poprawiony import

export const useAuthStore = defineStore('auth', () => {
  const isLoggedIn = ref(false)
  const username = ref('')
  const jwtToken = ref('')
  const roles = ref([]) 

  // Funkcja do ustawienia stanu logowania
  const setLoginStatus = (token, user) => {
    jwtToken.value = token
    username.value = user
    isLoggedIn.value = true

    // Dekodowanie tokena i wyciąganie ról
    const decodedToken = jwtDecode(token)
    roles.value = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || []

    // Zapis w localStorage
    localStorage.setItem('jwtToken', token)
    localStorage.setItem('username', user)
    localStorage.setItem('roles', JSON.stringify(roles.value)); // Zapisujemy jako JSON
  }

  // Funkcja do sprawdzenia, czy użytkownik jest zalogowany
  const checkLoginStatus = () => {
    const token = localStorage.getItem('jwtToken')
    if (token) {
      jwtToken.value = token
      isLoggedIn.value = true
      username.value = localStorage.getItem('username')

      // Dekodowanie tokena, aby odświeżyć role
      const decodedToken = jwtDecode(token)
      roles.value = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || []
    } else {
      isLoggedIn.value = false
      roles.value = []
    }
  }

  // Funkcja wylogowania
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
