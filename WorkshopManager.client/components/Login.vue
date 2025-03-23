<template>
  <div class="login-form">
    <h2>Logowanie</h2>
    <form @submit.prevent="login">
      <div class="form-group">
        <label for="username">Nazwa użytkownika</label>
        <input v-model="username" type="text" id="username" required />
      </div>
      <div class="form-group">
        <label for="password">Hasło</label>
        <input v-model="password" type="password" id="password" required />
      </div>
      <div class="remember-me">
        <input v-model="rememberMe" type="checkbox" id="rememberMe" />
        <label for="rememberMe">Zapamiętaj mnie</label>
      </div>
      <v-btn type="submit">
  Zaloguj się
</v-btn>
    </form>
    <p v-if="errorMessage" class="error">{{ errorMessage }}</p>
  </div>
</template>

<script setup>
import { useRouter } from '#app'
import { ref } from 'vue'
import axios from 'axios'
import { useAuthStore } from '~/stores/auth'

const authStore = useAuthStore()  
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

    authStore.setLoginStatus(token, username.value)  


    router.push('/')
  } catch (error) {
    errorMessage.value = 'Nieprawidłowe dane logowania'
  }
}
</script>

<style scoped>
.login-form {
  max-width: 400px;
  margin: 50px auto;
  padding: 20px;
  background: #f4f4f4;
  border-radius: 10px;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
  font-family: 'Arial', sans-serif;
}

h2 {
  text-align: center;
  color: #333;
}
.login-form label,
.login-form input {
  color: #333;
}
.form-group {
  margin-bottom: 15px;
}

label {
  display: block;
  margin-bottom: 5px;
  color: #555;
  font-weight: bold;
}

input[type="text"],
input[type="password"] {
  width: 100%;
  padding: 10px;
  border: 1px solid #ddd;
  border-radius: 5px;
  box-sizing: border-box;
}

.remember-me {
  display: flex;
  align-items: center;
  gap: 10px;
  margin-bottom: 20px;
}



.error {
  color: red;
  text-align: center;
  margin-top: 10px;
}
</style>
