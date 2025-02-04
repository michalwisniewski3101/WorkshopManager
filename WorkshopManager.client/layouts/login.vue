<script setup>
import { onMounted } from 'vue'
import { useAuthStore } from '~/stores/auth'
import { useRouter } from '#app'
import axios from 'axios'

const authStore = useAuthStore()
const router = useRouter()

onMounted(() => {
  authStore.checkLoginStatus()  // Sprawdzamy stan logowania po załadowaniu
})

const logout = async () => {
  try {
    await axios.post('/api/auth/logout', {}, {
      /* headers: {
        Authorization: `Bearer ${authStore.jwtToken}`,
      },*/
    })
    authStore.logout()  // Zaktualizowanie stanu logowania w Pinia

    // Przekierowanie po wylogowaniu
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

