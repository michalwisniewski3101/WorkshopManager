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
    <!-- Sidebar Navigation -->
    <v-navigation-drawer app>
      <v-list>
        <v-list-item v-if="authStore.roles=='Administrator'||authStore.roles=='Starszy Mechanik'||authStore.roles=='Młodszy Mechanik'||authStore.roles=='Klient'">
          <v-list-item-title>
            <NuxtLink to="/">Strona główna</NuxtLink>
          </v-list-item-title>
        </v-list-item>

        <v-list-item v-if="authStore.roles=='Administrator'||authStore.roles=='Starszy Mechanik'||authStore.roles=='Młodszy Mechanik'">
          <v-list-item-title>
            <NuxtLink to="/vehicle/vehicle-list">Pojazdy</NuxtLink>
          </v-list-item-title>
        </v-list-item>

        <v-list-item v-if="authStore.roles=='Administrator'||authStore.roles=='Starszy Mechanik'">
          <v-list-item-title>
            <NuxtLink to="/warehouse/">Magazyn</NuxtLink>
          </v-list-item-title>
        </v-list-item>

        <v-list-item v-if="authStore.roles=='Administrator'||authStore.roles=='Starszy Mechanik'||authStore.roles=='Młodszy Mechanik'">
          <v-list-item-title>
            <NuxtLink to="/mechanic/mechanic-list">Pracownicy</NuxtLink>
          </v-list-item-title>
        </v-list-item>

        <v-list-item v-if="authStore.roles=='Administrator'||authStore.roles=='Starszy Mechanik'||authStore.roles=='Młodszy Mechanik'">
          <v-list-item-title>
            <NuxtLink to="/order">Zamówienia</NuxtLink>
          </v-list-item-title>
        </v-list-item>

        <v-list-item v-if="authStore.roles=='Administrator'||authStore.roles=='Starszy Mechanik'">
          <v-list-item-title>
            <NuxtLink to="/dictionaries/">Słowniki</NuxtLink>
          </v-list-item-title>
        </v-list-item>

        <v-list-item v-if="authStore.roles=='Administrator'||authStore.roles=='Klient'">
          <v-list-item-title>
            <NuxtLink to="/client/">Klient</NuxtLink>
          </v-list-item-title>
        </v-list-item>

        <!-- Wylogowanie -->
        <v-list-item v-if="authStore.isLoggedIn">
          <v-list-item-title>
            <span><font-awesome-icon icon="user" /> {{ authStore.username }}</span>
          </v-list-item-title>
          <v-btn @click="logout" color="red">
            <font-awesome-icon icon="sign-out-alt" /> Wyloguj
          </v-btn>
        </v-list-item>
      </v-list>
    </v-navigation-drawer>

    <!-- App Bar -->
    <v-app-bar app>
      <v-toolbar-title>Workshop Manager</v-toolbar-title>
    </v-app-bar>

    <!-- Main Content Area -->
    <v-main>
      <NuxtPage />
    </v-main>

    <!-- Footer -->
    <v-footer app>
      <span>&copy; 2024 Workshop Manager</span>
    </v-footer>
  </v-app>
</template>

<style scoped>
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
