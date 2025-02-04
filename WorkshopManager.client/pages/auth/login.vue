<template>
  <v-card>
    <Login /></v-card>
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
  