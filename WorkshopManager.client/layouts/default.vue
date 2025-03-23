<script setup>
import { ref, onMounted, computed } from 'vue'
import { useAuthStore } from '~/stores/auth'
import { useRouter } from '#app'
import axios from 'axios'

const authStore = useAuthStore()
const router = useRouter()
const rail = ref(false)

onMounted(() => {
  authStore.checkLoginStatus() 
})


const logout = async () => {
  try {
    await axios.post('/api/auth/logout', {}, {})
    authStore.logout() 
    router.push('/auth/login') 
  } catch (error) {
    console.error('Błąd wylogowania:', error)
  }
}


const chevronIcon = computed(() => (rail.value ? 'mdi-chevron-right' : 'mdi-chevron-left'))


const menuItems = [
  { title: 'Strona główna', to: '/', icon: 'mdi-home', roles: ['Administrator', 'Starszy Mechanik', 'Młodszy Mechanik', 'Klient'] },
  { title: 'Pojazdy', to: '/vehicle/vehicle-list', icon: 'mdi-car', roles: ['Administrator', 'Starszy Mechanik', 'Młodszy Mechanik'] },
  { title: 'Magazyn', to: '/warehouse/', icon: 'mdi-warehouse', roles: ['Administrator', 'Starszy Mechanik'] },
  { title: 'Pracownicy', to: '/mechanic/mechanic-list', icon: 'mdi-account-group', roles: ['Administrator', 'Starszy Mechanik', 'Młodszy Mechanik'] },
  { title: 'Zamówienia', to: '/order', icon: 'mdi-cart', roles: ['Administrator', 'Starszy Mechanik', 'Młodszy Mechanik'] },
  { title: 'Słowniki', to: '/dictionaries/', icon: 'mdi-book', roles: ['Administrator', 'Starszy Mechanik'] },
  { title: 'Klient', to: '/client/', icon: 'mdi-account', roles: ['Administrator'] },
]
</script>


<template>
  <v-app>
    <v-layout>
      <!-- Menu boczne -->
      <v-navigation-drawer :rail="rail" permanent>
        <v-list>
          <!-- Przycisk rozwijania/zamykania menu -->
          <v-list-item>
            <v-list-item-title @click="rail = !rail">
 
                <v-icon>{{ chevronIcon }}</v-icon>

            </v-list-item-title>
          </v-list-item>

          <v-divider></v-divider>

          <!-- Dynamiczne generowanie menu z ikonami i kontrolą dostępu po rolach -->
          <template v-for="item in menuItems" :key="item.title">
            <v-list-item v-if="item.roles.includes(authStore.roles)">
              <v-list-item-title>
                <NuxtLink :to="item.to">
                  <v-icon left>{{ item.icon }}</v-icon>
                  {{ item.title }}
                </NuxtLink>
              </v-list-item-title>
            </v-list-item>
          </template>
        </v-list>
      </v-navigation-drawer>

      <!-- Pasek górny -->
      <v-app-bar app class="d-flex align-center justify-between">
  <v-toolbar-title class="d-flex align-center">
    <span class="title-text">Workshop Manager</span>
  </v-toolbar-title>
  <div class="d-flex align-center">
    <span class="mr-4"><font-awesome-icon icon="user" /> {{ authStore.username }}</span>
    <v-btn @click="logout" color="red">
      <font-awesome-icon icon="sign-out-alt" /> Wyloguj
    </v-btn>
  </div>
</v-app-bar>

      <!-- Główna zawartość -->
      <v-main class="d-flex align-center justify-center">
        <NuxtPage />
      </v-main>

      <!-- Stopka -->
      <v-footer app>
        <span>&copy; 2024 Workshop Manager</span>
      </v-footer>
    </v-layout>
  </v-app>
</template>

<style >
.v-card {
    padding: 20px;
    height: 100%; 
    width: 100%;
  }


  
  .v-list-item-title a {
    text-decoration: none;
    color: inherit;
  }
  
  .v-list-item:hover {
    background-color: rgba(100, 100, 100, 0.8); /* Kolor `secondary` z motywu */
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
  
  html,
  body {
    background-color: var(--v-theme-surface); /* Kolor `surface` z motywu */
    color: var(--v-theme-secondary); /* Kolor `secondary` z motywu */
    font-family: "Roboto", sans-serif;
  }
  .logo {
  height: 40px; /* Dopasuj wysokość */
  width: auto;
}

</style>

