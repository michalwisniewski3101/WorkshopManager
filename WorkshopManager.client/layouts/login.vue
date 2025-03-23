<script setup>
import { onMounted } from 'vue'
import { useAuthStore } from '~/stores/auth'
import { useRouter } from '#app'
import axios from 'axios'

const authStore = useAuthStore()
const router = useRouter()

onMounted(() => {
  authStore.checkLoginStatus()  
})

const logout = async () => {
  try {
    await axios.post('/api/auth/logout', {}, {
    })
    authStore.logout()  


    router.push('/auth/login')
  } catch (error) {
    console.error("Błąd wylogowania:", error)
  }
}
</script>

<template>
  <v-app>
    <v-layout>
      <NuxtPage />
    </v-layout>
  </v-app>
</template>

<style scoped>
</style>

