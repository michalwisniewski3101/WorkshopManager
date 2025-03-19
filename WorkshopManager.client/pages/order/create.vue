<template>
  <v-card style="height: 100%; width: 100%;">
    <v-breadcrumbs :items="items">
      </v-breadcrumbs>
      <v-row>
        <v-col cols="12" class="text-left">
          <h1>Dodaj Zamówienie</h1>
        </v-col>
      </v-row>
      <v-row>

<v-col>
    <v-card-text>

      <v-form @submit.prevent="addOrder">
        <!-- Dane klienta -->
        <v-text-field v-model="newOrder.clientName" label="Imię i nazwisko klienta" required></v-text-field>
        <v-text-field v-model="newOrder.clientPhoneNumber" label="Numer telefonu klienta" type="tel" required></v-text-field>
        <v-text-field v-model="newOrder.clientEmail" label="E-mail klienta" type="email"></v-text-field>
        <v-text-field v-model="newOrder.clientAddress" label="Adres klienta"></v-text-field>

        <!-- Dane pojazdu -->
        <v-text-field v-model="newOrder.vin" label="Numer VIN" required></v-text-field>
        <v-text-field v-model="newOrder.make" label="Marka pojazdu" required></v-text-field>
        <v-text-field v-model="newOrder.model" label="Model pojazdu" required></v-text-field>
        <v-text-field v-model="newOrder.year" label="Rok produkcji" type="number" required></v-text-field>
        <v-text-field v-model="newOrder.licensePlate" label="Tablica rejestracyjna" required></v-text-field>

        <!-- Opis zamówienia -->
        <v-textarea v-model="newOrder.description" label="Opis zamówienia"></v-textarea>
      </v-form>
    </v-card-text>
    <v-card-actions>
      <NuxtLink to="/order"><v-btn color="grey" type="button">Anuluj</v-btn></NuxtLink>
      <v-btn color="white" @click="addOrder">Dodaj Zamówienie</v-btn>
      
    </v-card-actions>
  </v-col>
        
       
          
       
      </v-row>
  </v-card>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'

const router = useRouter()
const newOrder = ref({
  services: [],
  description: '',
  clientName: '',
  clientPhoneNumber: '',
  clientEmail: '',
  clientAddress: '',
  vin: '',
  make: '',
  model: '',
  year: '',
  licensePlate: '',
})
const items = ref([
  {
    title: 'Dashboard',
    disabled: false,
    to: '/',
  },
  {
    title: 'Zamówienia',
    disabled: false,
    to: '/order',
  },
  {
    title: 'Nowe Zamówienie',
    disabled: true,
    to: '/order/create',
  },
]);
const addOrder = async () => {
  try {
    const response = await $fetch('/api/Order', {
      method: 'POST',
      body: JSON.stringify(newOrder.value),
      headers: { 'Content-Type': 'application/json' }
    })

    // Załóżmy, że odpowiedź zawiera obiekt zamówienia, w tym jego ID
    const orderId = response.id

    alert('Zamówienie dodane pomyślnie!')
    // Przekierowanie na stronę szczegółów zamówienia
    router.push(`/order/${orderId}`)
  } catch (error) {
    alert('Błąd podczas dodawania zamówienia')
  }
}

</script>

<style scoped>
/* Dostosowanie do stylu Vuetify */
.v-btn {
  margin-top: 1rem;
  transition: background 0.3s ease;
}

h1 {
  color: #000000;
  margin-bottom: 1rem;
  font-family: 'Arial', sans-serif;
}

.v-card {
  padding: 20px;
  background: #e0e0e0;
  border-radius: 10px;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1) !important;
}

.v-text-field {
  max-width: 300px;
  margin-left: auto;
  color: #000000;
}

.clickable-icon {
  color: #000000;
  transition: color 0.3s ease;
}

.clickable-icon:hover {
  color: #45a049;
}

:deep(.v-data-table) {
  background: rgb(185, 185, 185);
  border-radius: 5px;
  margin-top: 1rem;
  box-shadow: 0 2px 5px rgba(0, 0, 0, 0.05) !important;
}

:deep(.v-data-table th) {
  font-weight: bold;
  color: #000000;
  background: #a0a0a0;
}

:deep(.v-data-table td) {
  color: #000000;
}
.v-breadcrumbs:deep() {
  color: #000000
}
</style>
