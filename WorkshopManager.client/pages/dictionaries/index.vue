<template>
  <v-card style="height: 100%; width: 100%;">
    <v-tabs v-model="tab" bg-color="primary">
      <v-tab value="specialties">Specjalności</v-tab>
      <v-tab value="services">Serwisy</v-tab>
    </v-tabs>

   
      <v-tabs-window v-model="tab">
        <!-- Tabela Specjalności -->
        <v-tabs-window-item value="specialties">
          <v-data-table
            :headers="specialtyHeaders"
            :items="specialties"
            item-value="id"
            item-key="id"
          >
            <template v-slot:[`item.specialtyName`]="{ item }">
              <span>{{ item.specialtyName }}</span>
            </template>
          </v-data-table>
          <!-- Przycisk do otwarcia modalnego okna dodawania specjalności -->
        <v-btn @click="showSpecialtyModal = true" color="primary">Dodaj Specjalność</v-btn>

<!-- Modal dodawania specjalności -->
<v-dialog v-model="showSpecialtyModal" persistent max-width="400px">
  <v-card>
    <v-card-title class="headline">Dodaj Specjalność</v-card-title>
    <v-card-text>
      <form @submit.prevent="addSpecialty">
        <v-text-field
          label="Nazwa specjalności"
          v-model="newSpecialty.specialtyName"
          required
        ></v-text-field>
      </form>
    </v-card-text>
    <v-card-actions>
      <v-btn @click="closeSpecialtyModal" color="secondary">Anuluj</v-btn>
      <v-btn @click="addSpecialty" color="primary">Dodaj</v-btn>
    </v-card-actions>
  </v-card>
</v-dialog>
        </v-tabs-window-item>

        <!-- Tabela Serwisów -->
        <v-tabs-window-item value="services">
          <v-data-table
            :headers="serviceHeaders"
            :items="services"
            item-value="id"
            item-key="id"
          >
            <template v-slot:[`item.serviceDescription`]="{ item }">
              <span>{{ item.serviceDescription }}</span>
            </template>
            <template v-slot:[`item.serviceCost`]="{ item }">
              <span>{{ item.serviceCost.toFixed(2) }} PLN</span>
            </template>
            <template v-slot:[`item.serviceDuration`]="{ item }">
              <span>{{ item.serviceDuration }} min</span>
            </template>
          </v-data-table>
           <!-- Przycisk do otwarcia modalnego okna dodawania serwisu -->
        <v-btn @click="showServiceModal = true" color="primary">Dodaj Serwis</v-btn>

<!-- Modal dodawania serwisu -->
<v-dialog v-model="showServiceModal" persistent max-width="400px">
  <v-card>
    <v-card-title class="headline">Dodaj Serwis</v-card-title>
    <v-card-text>
      <form @submit.prevent="addService">
        <v-text-field
          label="Opis serwisu"
          v-model="newService.serviceDescription"
          required
        ></v-text-field>
        <v-text-field
          label="Koszt serwisu (PLN)"
          v-model.number="newService.serviceCost"
          type="number"
          step="0.01"
          required
        ></v-text-field>
        <v-text-field
          label="Czas trwania serwisu (minuty)"
          v-model.number="newService.serviceDuration"
          type="number"
          required
        ></v-text-field>
      </form>
    </v-card-text>
    <v-card-actions>
      <v-btn @click="closeServiceModal" color="secondary">Anuluj</v-btn>
      <v-btn @click="addService" color="primary">Dodaj</v-btn>
    </v-card-actions>
  </v-card>
</v-dialog>
        </v-tabs-window-item>
      </v-tabs-window>
  
  </v-card>
</template>

<script setup>
import { ref, onMounted } from 'vue'

// Definiowanie meta danych
definePageMeta({
  middleware: 'auth',
  auth: {
    roles: ['Administrator', 'Starszy Mechanik']
  }
})
const showSpecialtyModal = ref(false)
const showServiceModal = ref(false)

// Specjalności
const specialties = ref([])
const newSpecialty = ref({ specialtyName: '' })
const tab = ref("specialties")
const specialtyHeaders = ref([
  { title: 'Specjalność', align: 'start', key: 'specialtyName', sortable: false }
])

// Serwisy
const services = ref([])
const newService = ref({
  serviceDescription: '',
  serviceCost: 0,
  serviceDuration: 0
})
const serviceHeaders = ref([
  { title: 'Opis Serwisu', align: 'start', key: 'serviceDescription', sortable: false },
  { title: 'Koszt (PLN)', align: 'start', key: 'serviceCost', sortable: true },
  { title: 'Czas trwania (min)', align: 'start', key: 'serviceDuration', sortable: true }
])
const closeSpecialtyModal = () => {
  showSpecialtyModal.value = false
}

const closeServiceModal = () => {
  showServiceModal.value = false
}







// Funkcja pobierająca specjalności z backendu
const fetchSpecialties = async () => {
  try {
    specialties.value = await $fetch('/api/MechanicSpecialty/GetSpecialties')
  } catch (error) {
    alert('Błąd podczas ładowania specjalności')
  }
}

// Funkcja pobierająca serwisy z backendu
const fetchServices = async () => {
  try {
    services.value = await $fetch('/api/Service')
  } catch (error) {
    alert('Błąd podczas ładowania serwisów')
  }
}

// Funkcja dodawania specjalności
const addSpecialty = async () => {
  try {
    await $fetch('/api/MechanicSpecialty/AddSpecialty', {
      method: 'POST',
      body: JSON.stringify(newSpecialty.value),
      headers: { 'Content-Type': 'application/json' }
    })
    await fetchSpecialties()
    closeSpecialtyModal()
  } catch (error) {
    alert('Błąd podczas dodawania specjalności')
  }
}
// Funkcja dodawania serwisu
const addService = async () => {
  try {
    const serviceData = {
      ...newService.value
    }
    await $fetch('/api/Service/AddService', {
      method: 'POST',
      body: JSON.stringify(serviceData),
      headers: { 'Content-Type': 'application/json' }
    })
    await fetchServices()
    closeServiceModal()
  } catch (error) {
    alert('Błąd podczas dodawania serwisu')
  }
}


// Pobieranie danych przy zamontowaniu komponentu
onMounted(() => {
  fetchSpecialties()
  fetchServices()
})

</script>

<style>
/* Można dodać dodatkową stylizację jeśli potrzebna */
</style>
