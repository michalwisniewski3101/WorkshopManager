<template>
  <div>
    <h1>Lista Specjalności i Serwisów</h1>

    <!-- Sekcja Specjalności -->
    <section>
      <h2>Specjalności</h2>
      <ul>
        <li v-for="specialty in specialties" :key="specialty.id">
          {{ specialty.specialtyName }}
        </li>
      </ul>
      <button @click="openSpecialtyModal">Dodaj nową specjalność</button>
    </section>

    <!-- Sekcja Serwisów -->
    <section>
      <h2>Serwisy</h2>
      <ul>
        <li v-for="service in services" :key="service.id">
          {{ service.serviceDescription }} - {{ service.serviceCost.toFixed(2) }} PLN ({{ formatDuration(service.serviceDuration) }})
        </li>
      </ul>
      <button @click="openServiceModal">Dodaj nowy serwis</button>
    </section>

    <!-- Modal dodawania specjalności -->
    <div v-if="showSpecialtyModal" class="modal">
      <div class="modal-content">
        <h2>Dodaj Specjalność</h2>
        <form @submit.prevent="addSpecialty">
          <label for="specialtyName">Nazwa specjalności:</label>
          <input type="text" id="specialtyName" v-model="newSpecialty.specialtyName" required />
          <button type="submit">Dodaj</button>
          <button @click="closeSpecialtyModal" type="button">Anuluj</button>
        </form>
      </div>
    </div>

    <!-- Modal dodawania serwisu -->
    <div v-if="showServiceModal" class="modal">
      <div class="modal-content">
        <h2>Dodaj Serwis</h2>
        <form @submit.prevent="addService">
          <label for="serviceDescription">Opis serwisu:</label>
          <input type="text" id="serviceDescription" v-model="newService.serviceDescription" required />

          <label for="serviceCost">Koszt serwisu (PLN):</label>
          <input type="number" id="serviceCost" v-model.number="newService.serviceCost" step="0.01" required />

          <label for="serviceDuration">Czas trwania serwisu (minuty):</label>
          <input type="number" id="serviceDuration" v-model.number="newService.serviceDuration" required />

          <button type="submit">Dodaj</button>
          <button @click="closeServiceModal" type="button">Anuluj</button>
        </form>
      </div>
    </div>
  </div>
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

// Specjalności
const specialties = ref([])
const newSpecialty = ref({ specialtyName: '' })
const showSpecialtyModal = ref(false)

// Serwisy
const services = ref([])
const newService = ref({
  serviceDescription: '',
  serviceCost: 0,
  serviceDuration: 0
})
const showServiceModal = ref(false)

// Funkcje modali
const openSpecialtyModal = () => (showSpecialtyModal.value = true)
const closeSpecialtyModal = () => {
  showSpecialtyModal.value = false
  newSpecialty.value.specialtyName = ''
}

const openServiceModal = () => (showServiceModal.value = true)
const closeServiceModal = () => {
  showServiceModal.value = false
  newService.value = {
    serviceDescription: '',
    serviceCost: 0,
    serviceDuration: 0
  }
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
    await $fetch('/api/Service', {
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

// Formatowanie czasu trwania serwisu
const formatDuration = (duration) => {
  const match = duration.match(/PT(\d+)M/)
  return match ? `${match[1]} min` : 'N/A'
}

// Pobieranie danych przy zamontowaniu komponentu
onMounted(() => {
  fetchSpecialties()
  fetchServices()
})
</script>

<style>
/* Stylizacja jak wcześniej */
h1, h2 {
  color: #333;
}

.modal {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  display: flex;
  align-items: center;
  justify-content: center;
  background: rgba(0, 0, 0, 0.5);
}

.modal-content {
  background: white;
  padding: 20px;
  border-radius: 8px;
  max-width: 400px;
  width: 100%;
}

.modal-content h2 {
  margin-top: 0;
}

.modal-content button {
  margin-right: 10px;
}
</style>
