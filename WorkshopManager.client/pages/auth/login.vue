<template>
  <v-card>
    <Login />
    <NuxtLink to="/client/">Strefa Klienta</NuxtLink>
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
<style>
a {
    text-decoration: none;
    color: inherit;
  }
</style>
  