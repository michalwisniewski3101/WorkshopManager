<template>
    <div>
      <h1>Lista Specjalności</h1>
      <ul>
        <li v-for="specialty in specialties" :key="specialty.id">
          {{ specialty.specialtyName }}
        </li>
      </ul>
  
      <!-- Przycisk dodawania nowej specjalności -->
      <button @click="openModal">Dodaj nową specjalność</button>
  
      <!-- Okno modalne do dodawania specjalności -->
      <div v-if="showModal" class="modal">
        <div class="modal-content">
          <h2>Dodaj Specjalność</h2>
          <form @submit.prevent="addSpecialty">
            <label for="specialtyName">Nazwa specjalności:</label>
            <input
              type="text"
              id="specialtyName"
              v-model="newSpecialty.specialtyName"
              required
            />
            <button type="submit">Dodaj</button>
            <button @click="closeModal" type="button">Anuluj</button>
          </form>
        </div>
      </div>
    </div>
  </template>
  
  <script setup>
  import { ref, onMounted } from 'vue'
  definePageMeta({
  middleware: 'auth'
})
  // Lista specjalności
  const specialties = ref([])
  
  // Formularz nowej specjalności i kontrola modalu
  const newSpecialty = ref({ specialtyName: '' })
  const showModal = ref(false)
  
  // Funkcja otwierająca modal
  const openModal = () => {
    showModal.value = true
  }
  
  // Funkcja zamykająca modal
  const closeModal = () => {
    showModal.value = false
    newSpecialty.value.specialtyName = ''
  }
  
  // Funkcja pobierająca specjalności z backendu
  const fetchSpecialties = async () => {
    try {
      specialties.value = await $fetch('/api/MechanicSpecialty/GetSpecialties')
    } catch (error) {
      alert('Błąd podczas ładowania specjalności')
    }
  }
  
  // Funkcja dodawania specjalności
  const addSpecialty = async () => {
    try {
      const addedSpecialty = await $fetch('/api/MechanicSpecialty/AddSpecialty', {
        method: 'POST',
        body: JSON.stringify(newSpecialty.value),
        headers: { 'Content-Type': 'application/json' },
      })
      await fetchSpecialties()
      closeModal() // Zamykamy okno modalne po dodaniu
    } catch (error) {
      alert('Błąd podczas dodawania specjalności')
    }
  }
  
  // Pobieranie specjalności przy zamontowaniu komponentu
  onMounted(fetchSpecialties)
  </script>
  
  <style>
  h1 {
    color: #333;
  }
  
  /* Stylizacja modalu */
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
  