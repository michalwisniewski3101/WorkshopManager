<template>
  <v-card>
    <Login />


    <div class="login-form">
      <NuxtLink to="/client/"><button to="/client/">Strefa Klienta</button></NuxtLink>
    <p v-if="errorMessage" class="error">{{ errorMessage }}</p>
  </div>

  </v-card>
  </template>
  
  <script setup>
  import Login from '~/components/Login.vue'
  definePageMeta({
  middleware: 'auth',
  auth: {
    roles: ['Administrator', 'Klient'],
  },
  layout: 'login'
})

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

button {
  width: 100%;
  padding: 10px;
  background-color: #4caf50;
  color: white;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  font-size: 16px;
  transition: background 0.3s ease;
}

button:hover {
  background-color: #45a049;
}

.error {
  color: red;
  text-align: center;
  margin-top: 10px;
}
</style>
  