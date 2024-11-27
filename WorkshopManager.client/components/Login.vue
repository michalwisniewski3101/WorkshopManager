<template>
    <div class="login-form">
      <h2>Logowanie</h2>
      <form @submit.prevent="login">
        <div>
          <label for="username">Nazwa użytkownika</label>
          <input v-model="username" type="text" id="username" required />
        </div>
        <div>
          <label for="password">Hasło</label>
          <input v-model="password" type="password" id="password" required />
        </div>
        <div>
          <input v-model="rememberMe" type="checkbox" id="rememberMe" />
          <label for="rememberMe">Zapamiętaj mnie</label>
        </div>
        <button type="submit">Zaloguj się</button>
      </form>
      <p v-if="errorMessage" class="error">{{ errorMessage }}</p>
    </div>
  </template>
  
  <script setup>
  import { useRouter } from '#app'
  import { ref } from 'vue'
  import axios from 'axios'
  import { useAuthStore } from '~/stores/auth'
  
  const authStore = useAuthStore()  // Pobieramy store Pinia
  const username = ref('')
  const password = ref('')
  const rememberMe = ref(false)
  const errorMessage = ref('')
  const router = useRouter()
  
  const login = async () => {
    try {
      const response = await axios.post('/api/auth/login', {
        username: username.value,
        password: password.value,
        rememberMe: rememberMe.value,
      })
      const { token } = response.data
  
      authStore.setLoginStatus(token, username.value)  // Ustawiamy stan logowania
  
      // Przekierowanie po zalogowaniu
      router.push('/')
    } catch (error) {
      errorMessage.value = 'Nieprawidłowe dane logowania'
    }
  }
  </script>
  
  <style scoped>
  .login-form {
    max-width: 400px;
    margin: auto;
  }
  .error {
    color: red;
  }
  </style>
  