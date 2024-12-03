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
    <v-main class="d-flex align-center justify-center">
      <NuxtPage />
    </v-main>

    <!-- Footer -->
    <v-footer app>
      <span>&copy; 2024 Workshop Manager</span>
    </v-footer>
    </v-layout>
  </v-app>
</template>

<style scoped>
/* Nawigacja */
.v-navigation-drawer {
  background-color: var(--v-theme-surface); /* Kolor `surface` z motywu */
  border-right: 1px solid rgba(0, 0, 0, 0.1);
}

.v-list-item-title {
  font-weight: bold;
  font-size: 1rem;
  color: var(--v-theme-primary); /* Kolor `primary` z motywu */
  transition: color 0.3s ease;
}

.v-list-item-title a {
  text-decoration: none;
  color: inherit;
}

.v-list-item-title:hover {
  color: var(--v-theme-secondary); /* Kolor `secondary` z motywu */
}

/* Przyciski */
.v-btn {
  background-color: var(--v-theme-primary);
  color: white;
  font-weight: bold;
  text-transform: uppercase;
  transition: background-color 0.3s ease;
}

.v-btn:hover {
  background-color: rgba(196, 34, 34, 0.8); /* Przyciemnienie koloru przy najechaniu */
}

/* Górny pasek */
.v-app-bar {
  background-color: var(--v-theme-appBar); /* Kolor `appBar` z motywu */
  color: white;
}

.v-toolbar-title {
  font-size: 1.5rem;
  font-weight: bold;
}

/* Tło strony */
html,
body {
  background-color: var(--v-theme-surface); /* Kolor `surface` z motywu */
  color: var(--v-theme-secondary); /* Kolor `secondary` z motywu */
  font-family: "Roboto", sans-serif;
}





</style>

