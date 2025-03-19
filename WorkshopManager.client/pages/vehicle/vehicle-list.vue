<template>
    <v-card style="height: 100%; width: 100%;">
      <v-breadcrumbs :items="items">
      </v-breadcrumbs>
      <v-row>
      <v-col cols="12" class="text-left">
        <h1>Lista pojazdów</h1>
      </v-col>
      <v-col cols="6" class="text-left">
        <AddVehicleForm  @close="closeModal" @refresh="fetchVehicles" />
      </v-col>
      <v-col cols="6" class="text-right">
        <v-text-field
        v-model="search"
        label="Szukaj"
        prepend-inner-icon="mdi-magnify"
        variant="outlined"
        hide-details
        single-line
      ></v-text-field>
      </v-col>
    </v-row>
      
      <v-data-table
        :headers="tableHeaders"
        :items="vehicles"
        item-key="id"
        class="elevation-1"
        :search="search"
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
        <NuxtLink :to="`/vehicle/${item.id}`" class="clickable-icon">

  <v-icon>mdi-eye</v-icon>

        </NuxtLink>
      </template>
      </v-data-table>
    </v-card>
  </template>
  
  <script setup>
  import { ref, onMounted } from 'vue'
  import AddVehicleForm from '@/components/AddVehicleForm.vue'
  
  definePageMeta({
    layout: 'default',
    middleware: 'auth',
    auth: {
      roles: ["Administrator", "Starszy Mechanik", "Młodszy Mechanik"]
    },
    
  })
  
  const vehicles = ref([])
  const isModalOpen = ref(false)
  const search = ref("")
  
  const tableHeaders = [
    { title: 'Marka', key: 'make' },
    { title: 'Model', key: 'model' },
    { title: 'Rok', key: 'year' },
    { title: 'Rejestracja', key: 'licensePlate' },
    { title: 'Akcje', key: 'actions', sortable: false }
  ]
  
  const openModal = () => {
    isModalOpen.value = true
  }
  const items = ref([
  {
    title: 'Dashboard',
    disabled: false,
    to: '/',
  },
  {
    title: 'Pojazdy',
    disabled: true,
    to: 'vehicle/vehicle-list',
  },
]);
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
  