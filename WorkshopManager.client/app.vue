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
  <nav>
    <ul>
      <li><NuxtLink to="/">Strona główna</NuxtLink></li>
      <li><NuxtLink to="/vehicle/add-vehicle">Dodaj pojazd</NuxtLink></li>
      <li><NuxtLink to="/vehicle/vehicle-list">Lista pojazdów</NuxtLink></li>
      <li><NuxtLink to="/mechanic/add-mechanic">Dodaj mechanika</NuxtLink></li>
      <li><NuxtLink to="/mechanic/mechanic-list">Lista mechaników</NuxtLink></li>
      <li><NuxtLink to="/dictionaries/">Słowniki</NuxtLink></li>

      <!-- Sekcja logowania i wylogowania -->
      <li v-if="authStore.isLoggedIn">
        <span><font-awesome-icon icon="user" /> {{ authStore.username }}</span>
        <button @click="logout">
          <font-awesome-icon icon="sign-out-alt" /> Wyloguj
        </button>
      </li>
      <li v-else>
        <NuxtLink to="/auth/login">
          <font-awesome-icon icon="sign-in-alt" /> Zaloguj
        </NuxtLink>
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
