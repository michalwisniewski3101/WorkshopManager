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
          {{ service.serviceDescription }} - {{ service.serviceCost.toFixed(2) }} PLN ({{ service.serviceDuration }} min)
        </li>
      </ul>
      <button @click="openServiceModal">Dodaj nowy serwis</button>
    </section>
        <!-- Sekcja Magazyn -->
      <section>
      <h2>Elementy Magazynu</h2>
      <ul>
        <li v-for="item in inventoryItems" :key="item.id">
          {{ item.name }} - {{ item.description }} - {{ item.unitPrice.toFixed(2) }} PLN
        </li>
      </ul>
      <button @click="openInventoryModal">Dodaj nowy element magazynu</button>
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

<!-- Modal dodawania elementu magazynu -->
<div v-if="showInventoryModal" class="modal">
  <div class="modal-content">
    <h2>Dodaj Element Magazynu</h2>
    <form @submit.prevent="addInventoryItem">
      <label for="itemName">Nazwa:</label>
      <input type="text" id="itemName" v-model="newInventoryItem.name" required />

      <label for="itemDescription">Opis:</label>
      <input type="text" id="itemDescription" v-model="newInventoryItem.description" required />

      <label for="productNumber">Numer produktu:</label>
      <input type="text" id="productNumber" v-model="newInventoryItem.productNumber" required />


      <label for="unitPrice">Cena jednostkowa (PLN):</label>
      <input type="number" id="unitPrice" v-model.number="newInventoryItem.unitPrice" step="0.01" required />

      <label for="reorderLevel">Poziom zamawiania:</label>
      <input type="number" id="reorderLevel" v-model.number="newInventoryItem.reorderLevel" required />

      <label for="supplier">Dostawca:</label>
      <input type="text" id="supplier" v-model="newInventoryItem.supplier" required />

      <button type="submit">Dodaj</button>
      <button @click="closeInventoryModal" type="button">Anuluj</button>
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

const inventoryItems = ref([])
const newInventoryItem = ref({
  name: '',
  description: '',
  productNumber: '',
  unitPrice: 0,
  reorderLevel: 0,
  supplier: ''
})
const showInventoryModal = ref(false)


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
// Funkcje modali magazynu
const openInventoryModal = () => (showInventoryModal.value = true)
const closeInventoryModal = () => {
  showInventoryModal.value = false
  newInventoryItem.value = {
    name: '',
    description: '',
    quantityInStock: 0,
    unitPrice: 0
  }
}

// Funkcja pobierająca elementy magazynu z backendu
const fetchInventoryItems = async () => {
  try {
    inventoryItems.value = await $fetch('/api/InventoryItem')
  } catch (error) {
    alert('Błąd podczas ładowania elementów magazynu')
  }
}

// Funkcja dodawania elementu magazynu
const addInventoryItem = async () => {
  try {
    await $fetch('/api/InventoryItem', {
      method: 'POST',
      body: JSON.stringify(newInventoryItem.value),
      headers: { 'Content-Type': 'application/json' }
    })
    await fetchInventoryItems()
    closeInventoryModal()
  } catch (error) {
    alert('Błąd podczas dodawania elementu magazynu')
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
  fetchInventoryItems()
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
