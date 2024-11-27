// stores/auth.js
import { defineStore } from 'pinia'
import { ref } from 'vue'

export const useAuthStore = defineStore('auth', () => {
  const isLoggedIn = ref(false)
  const username = ref('')
  const jwtToken = ref('')

  // Funkcja do ustawienia stanu logowania
  const setLoginStatus = (token, user) => {
    jwtToken.value = token
    username.value = user
    isLoggedIn.value = true
    localStorage.setItem('jwtToken', token)
    localStorage.setItem('username', user)
  }

  // Funkcja do sprawdzenia, czy użytkownik jest zalogowany
  const checkLoginStatus = () => {
    const token = localStorage.getItem('jwtToken')
    if (token) {
      isLoggedIn.value = true
      username.value = localStorage.getItem('username')
    } else {
      isLoggedIn.value = false
    }
  }
  const logout = () => {
    jwtToken.value = ''
    username.value = ''
    isLoggedIn.value = false
    localStorage.removeItem('jwtToken')
    localStorage.removeItem('username')
  }

  return {
    isLoggedIn,
    username,
    setLoginStatus,
    checkLoginStatus,
    logout
  }
})
