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


authStore.roles=='Administrator'||authStore.roles=='Starszy Mechanik'||authStore.roles=='Młodszy Mechanik'||authStore.roles=='Klient'


<template>
  <nav>
    <ul >
      <li v-if="authStore.roles=='Administrator'||authStore.roles=='Starszy Mechanik'||authStore.roles=='Młodszy Mechanik'||authStore.roles=='Klient'"><NuxtLink to="/">Strona główna</NuxtLink></li>
      <li v-if="authStore.roles=='Administrator'||authStore.roles=='Starszy Mechanik'"><NuxtLink to="/vehicle/add-vehicle">Dodaj pojazd</NuxtLink></li>
      <li v-if="authStore.roles=='Administrator'||authStore.roles=='Starszy Mechanik'||authStore.roles=='Młodszy Mechanik'"><NuxtLink to="/vehicle/vehicle-list">Lista pojazdów</NuxtLink></li>
      <li v-if="authStore.roles=='Administrator'||authStore.roles=='Starszy Mechanik'"><NuxtLink to="/mechanic/add-mechanic">Dodaj mechanika</NuxtLink></li>
      <li v-if="authStore.roles=='Administrator'||authStore.roles=='Starszy Mechanik'||authStore.roles=='Młodszy Mechanik'"><NuxtLink to="/mechanic/mechanic-list">Lista mechaników</NuxtLink></li>
      <li v-if="authStore.roles=='Administrator'||authStore.roles=='Starszy Mechanik'||authStore.roles=='Młodszy Mechanik'"><NuxtLink to="/order">Zamówienia</NuxtLink></li>
      <li v-if="authStore.roles=='Administrator'||authStore.roles=='Starszy Mechanik'"><NuxtLink to="/dictionaries/">Słowniki</NuxtLink></li>
      <li v-if="authStore.roles=='Administrator'||authStore.roles=='Klient'"><NuxtLink to="/client/">Klient</NuxtLink></li>
      <!-- Sekcja logowania i wylogowania -->
      <li v-if="authStore.isLoggedIn">
        <span><font-awesome-icon icon="user" /> {{ authStore.username }}</span>
        <button @click="logout">
          <font-awesome-icon icon="sign-out-alt" /> Wyloguj
        </button>
      </li>
    </ul>
  </nav>
  <NuxtPage />
</template>

<style>
nav ul {
  display: flex;
  gap: 1rem;
  list-style-type: none;
}

nav ul li {
  display: inline;
}

nav ul li button {
  background: none;
  border: none;
  cursor: pointer;
  color: inherit;
}

nav ul li span {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}
</style>
