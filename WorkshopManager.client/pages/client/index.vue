<template>
    <div>
      <h1>cleint</h1>
      <p>posiadasz odpowiednich uprawnień, aby uzyskać dostęp do tej strony.</p>
      <nuxt-link to="/">Powrót do strony głównej</nuxt-link>
    </div>
  </template>
  
  <script setup>
    definePageMeta({
  middleware: 'auth',
  auth: {
    roles: ["Administrator", "Klient"]
  }
})
  </script>
  
  <style scoped>
  h1 {
    color: blue;
  }
  </style>
  