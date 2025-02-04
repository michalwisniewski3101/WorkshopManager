<template>
    <v-card style="height: 100%; width: 100%;">
      <h1 class="text-center mb-4">Lista pojazdów</h1>
  
      <v-data-table
        v-if="vehicles.length"
        :headers="tableHeaders"
        :items="vehicles"
        item-key="id"
        class="elevation-1"
      >
        <template v-slot:item.make="{ item }">
          {{ item.make }}
        </template>
        <template v-slot:item.model="{ item }">
          {{ item.model }}
        </template>
        <template v-slot:item.year="{ item }">
          {{ item.year }}
        </template>
        <template v-slot:item.licensePlate="{ item }">
          {{ item.licensePlate }}
        </template>
        <template v-slot:item.actions="{ item }">
          <!-- Akcje, np. edytowanie, usuwanie pojazdu -->
          <v-btn color="primary" @click="editVehicle(item)">
            <v-icon>mdi-pencil</v-icon>
            Edytuj
          </v-btn>
          <v-btn color="error"  @click="deleteVehicle(item)">
            <v-icon>mdi-delete</v-icon>
            Usuń
          </v-btn>
        </template>
      </v-data-table>
  
  
      <AddVehicleForm  @close="closeModal" @refresh="fetchVehicles" />
    </v-card>
  </template>
  
  <script setup>
  import { ref, onMounted } from 'vue'
  import AddVehicleForm from '@/components/AddVehicleForm.vue'
  
  definePageMeta({
    middleware: 'auth',
    auth: {
      roles: ["Administrator", "Starszy Mechanik", "Młodszy Mechanik"]
    },
    layout: 'default'
  })
  
  const vehicles = ref([])
  const isModalOpen = ref(false)
  
  const tableHeaders = [
    { title: 'Marka', value: 'make' },
    { title: 'Model', value: 'model' },
    { title: 'Rok', value: 'year' },
    { title: 'Rejestracja', value: 'licensePlate' },
    { title: 'Akcje', value: 'actions', sortable: false }
  ]
  
  const openModal = () => {
    isModalOpen.value = true
  }
  
  const closeModal = () => {
    isModalOpen.value = false
  }
  
  const fetchVehicles = async () => {
    try {
      vehicles.value = await $fetch('/api/Vehicle')
    } catch (error) {
      alert('Błąd podczas ładowania listy pojazdów')
    }
  }
  
  const editVehicle = (vehicle) => {
    // Funkcja do edytowania pojazdu
    console.log('Edytuj pojazd:', vehicle)
  }
  
  const deleteVehicle = (vehicle) => {
    // Funkcja do usuwania pojazdu
    console.log('Usuń pojazd:', vehicle)
  }
  
  onMounted(fetchVehicles)
  </script>
  
  <style scoped>
  .v-btn {
    margin-top: 1rem;
  }
  
  h1 {
    color: var(--v-primary-base); /* Kolor z motywu Vuetify */
  }
  </style>
  